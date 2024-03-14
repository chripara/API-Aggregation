using System.Net;
using System.Text;
using System.Text.Json;
using API_Aggregation.Dto.Spotify;
using API_Aggregation.Models.Spotify;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API_Aggregation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpotifyController : ControllerBase
{
    private readonly AppDbContext _context;
    private HttpClient client = new HttpClient();

    public SpotifyController(
        AppDbContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    [Route("DownloadAlbum")]
    public async Task<IActionResult> DownloadAlbum([FromQuery] RequestSpotifyDto dto)
    {
        var accessToken = JsonDocument.Parse(await GetSpotifyAccessTokenAsync())
            .RootElement.ToString().Split('"')[3];

        var request = new HttpRequestMessage() {
            Method = HttpMethod.Get
        };
        
        if (string.IsNullOrEmpty(dto.Market))
            request.RequestUri = new Uri(string.Format("https://api.spotify.com/v1/albums/{0}", dto.ID));
        else
            request.RequestUri = new Uri(string.Format("https://api.spotify.com/v1/albums/{0}?market={1}", dto.ID, dto.Market));

        request.Headers.Add("Authorization", string.Format("Bearer {0}", accessToken));
        
        var response = await client.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return Ok("Api Spotify return BadRequest. Please review your fetching data and try again.");
        
        if (response.StatusCode == HttpStatusCode.Forbidden)
            return Ok("Api Spotify return Forbidden. Something went wrong maybe try later.");
            
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return Ok("Api Spotify return Unauthorized. Something went wrong maybe try later.");

        if (!response.IsSuccessStatusCode)
            return Ok("Api Spotify return an unknown error. Something went wrong maybe try later.");

        var responseContent = await response.Content.ReadAsStringAsync();
        
        ResponseAlbumDto responseAlbumDto = JsonConvert
            .DeserializeObject<ResponseAlbumDto>(responseContent
                .Replace("\r\n", String.Empty));
        Album album;
        if (!_context.Albums.Any(a => a.SpotifyAlbumId == responseAlbumDto.Id))
        {
            album = new Album{
                SpotifyAlbumId = responseAlbumDto.Id,
                AlbumType = responseAlbumDto.AlbumType,
                TotalTracks= int.Parse(responseAlbumDto.TotalTracks),
                externalUrls=responseAlbumDto.ExternalUrls.Spotify,
                Href=responseAlbumDto.Href,
                Name=responseAlbumDto.Name,
                ReleaseDate= responseAlbumDto.ReleaseDate,
                Type=responseAlbumDto.Type
            };

            _context.Albums.Add(album);
            await _context.SaveChangesAsync();   
            var images = new List<Image>();
            foreach (var img in responseAlbumDto.Images)
            {
                images.Add(new Image
                {
                    Url = img.Url,
                    Height = img.Height,
                    Width = img.Width,
                    AlbumId = album.Id
                });
            }
            await _context.Images.AddRangeAsync(images);
        }
        else
        {
            album = await _context.Albums.FirstOrDefaultAsync(a => a.SpotifyAlbumId == responseAlbumDto.Id);
        }
        
        var artists = new List<Artist>();
        
        foreach (var artistDto in responseAlbumDto.ArtistDtos)
        {
            if(!_context.Artists.Any(a => a.ArtistSpotifyId == artistDto.Id))
                artists.Add(new Artist
                {
                    Type = artistDto.Type,
                    ArtistSpotifyId = artistDto.Id,
                    Alias = artistDto.Name,
                    Href = artistDto.Href,
                    SpotifyUrl = artistDto.ExternalUrls.Spotify
                });
            
            else
            {
                var artist = await _context.Artists
                    .FirstOrDefaultAsync(p => p.ArtistSpotifyId == artistDto.Id);

                if (!_context.ArtistAlbums.Any(p =>
                        p.ArtistId == artist.Id || p.AlbumId == album.Id))
                    _context.ArtistAlbums.Add(new ArtistAlbum
                    {
                        AlbumId = album.Id,
                        ArtistId = artist.Id
                    });
            }
        }

        if (artists.Count > 0)
        {
            _context.Artists.AddRange(artists);
            await _context.SaveChangesAsync();
            foreach (var artist in artists)
            {
                if (!_context.ArtistAlbums.Any(p =>
                        p.ArtistId == artist.Id || p.AlbumId == album.Id))
                    _context.ArtistAlbums.Add(new ArtistAlbum
                    {
                        AlbumId = album.Id,
                        ArtistId = artist.Id
                    });
            }
        }
        
        var tracks = new List<Track>();
        foreach (var trackDto in responseAlbumDto.TrackDtos.TrackDtos)
        {
            if(!_context.Tracks.Any(a => a.SpotifyTrackID == trackDto.id))
                tracks.Add(new Track
                {
                    Name = trackDto.Name,
                    Href = trackDto.Href,
                    Type = trackDto.Type,
                    AlbumId = album.Id,
                    ExternalUrl = trackDto.ExternalUrl.Spotify,
                    SpotifyTrackID = trackDto.id,
                    AvailableMarkets = string.Join(',',trackDto.AvailableMarkets)
                });
        }
        
        await _context.Tracks.AddRangeAsync(tracks);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost]
    [Route("DownloadArtist")]
    public async Task<IActionResult> DownloadArtist([FromQuery] RequestArtistSpotifyDto dto)
    {
        var accessToken = JsonDocument.Parse(await GetSpotifyAccessTokenAsync())
            .RootElement.ToString().Split('"')[3];

        var request = new HttpRequestMessage() {
            Method = HttpMethod.Get
        };
        
        request.RequestUri = new Uri(string.Format("https://api.spotify.com/v1/artists/{0}", dto.ID));
        
        request.Headers.Add("Authorization", string.Format("Bearer {0}", accessToken));
        
        var response = await client.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return Ok("Api Spotify return BadRequest. Please review your fetching data and try again.");
        
        if (response.StatusCode == HttpStatusCode.Forbidden)
            return Ok("Api Spotify return Forbidden. Something went wrong maybe try later.");
            
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return Ok("Api Spotify return Unauthorized. Something went wrong maybe try later.");

        if (!response.IsSuccessStatusCode)
            return Ok("Api Spotify return an unknown error. Something went wrong maybe try later.");

        var responseContent = await response.Content.ReadAsStringAsync();
        
        var json = JObject.Parse(responseContent);

        ResponseArtistDto responseArtistDto = JsonConvert
            .DeserializeObject<ResponseArtistDto>(responseContent
                .Replace("\r\n", String.Empty));

        var artist = new Artist();
        if(!_context.Artists.Any(a => a.ArtistSpotifyId == responseArtistDto.Id))
        {
            artist = new Artist
            {
                Type = responseArtistDto.Type,
                ArtistSpotifyId = responseArtistDto.Id,
                Alias = responseArtistDto.Name,
                Href = responseArtistDto.Href,
                SpotifyUrl = responseArtistDto.ExternalUrls.Spotify,
                Popularity = responseArtistDto.Popularity,
                TotalFollowers = responseArtistDto.Followers.Total
            };
            
            await _context.Artists.AddAsync(artist);
        }
        else
        {
            artist = await _context.Artists
                .FirstOrDefaultAsync(p => p.ArtistSpotifyId == responseArtistDto.Id);

            artist.Type = responseArtistDto.Type;
            artist.ArtistSpotifyId = responseArtistDto.Id;
            artist.Alias = responseArtistDto.Name;
            artist.Href = responseArtistDto.Href;
            artist.SpotifyUrl = responseArtistDto.ExternalUrls.Spotify;
            artist.Popularity = responseArtistDto.Popularity;
            artist.TotalFollowers = responseArtistDto.Followers.Total;
        }

        await _context.SaveChangesAsync();
        
        var images = new List<Image>();
        foreach (var img in responseArtistDto.Images)
        {
            if(!_context.Images.Any(a => a.Url == img.Url || a.ArtistId == artist.Id))
            images.Add(new Image
            {
                Url = img.Url,
                Height = img.Height,
                Width = img.Width,
                ArtistId = artist.Id
            });
            else
            {
                var image = await _context.Images
                    .FirstOrDefaultAsync(a => a.Url == img.Url
                                              || a.ArtistId == artist.Id);
                
                image.ArtistId = artist.Id;
                _context.Update(image);
            }
        }
        
        await _context.Images.AddRangeAsync(images);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    [Route("DownloadTrack")]
    public async Task<IActionResult> DownloadTrack([FromQuery] RequestSpotifyDto dto)
    {
        var accessToken = JsonDocument.Parse(await GetSpotifyAccessTokenAsync())
            .RootElement.ToString().Split('"')[3];

        var request = new HttpRequestMessage() {
            Method = HttpMethod.Get
        };
        
        if (string.IsNullOrEmpty(dto.Market))
            request.RequestUri = new Uri(string.Format("https://api.spotify.com/v1/tracks/{0}", dto.ID));
        else
            request.RequestUri = new Uri(string.Format("https://api.spotify.com/v1/tracks/{0}?market={1}", dto.ID, dto.Market));

        request.Headers.Add("Authorization", string.Format("Bearer {0}", accessToken));
        
        var response = await client.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return Ok("Api Spotify return BadRequest. Please review your fetching data and try again.");
        
        if (response.StatusCode == HttpStatusCode.Forbidden)
            return Ok("Api Spotify return Forbidden. Something went wrong maybe try later.");
            
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return Ok("Api Spotify return Unauthorized. Something went wrong maybe try later.");

        if (!response.IsSuccessStatusCode)
            return Ok("Api Spotify return an unknown error. Something went wrong maybe try later.");

        var responseContent = await response.Content.ReadAsStringAsync();
        
        ResponseTrackDto responseTrackDto = JsonConvert
            .DeserializeObject<ResponseTrackDto>(responseContent
                .Replace("\r\n", String.Empty));
        
        Album album = await _context.Albums.FirstOrDefaultAsync(a => a.SpotifyAlbumId == responseTrackDto.AlbumDto.Id);;
        if (album == null)
        {
            album = new Album{
                SpotifyAlbumId = responseTrackDto.AlbumDto.Id,
                AlbumType = responseTrackDto.AlbumDto.AlbumType,
                TotalTracks= int.Parse(responseTrackDto.AlbumDto.TotalTracks),
                externalUrls=responseTrackDto.AlbumDto.ExternalUrls.Spotify,
                Href=responseTrackDto.AlbumDto.Href,
                Name=responseTrackDto.AlbumDto.Name,
                ReleaseDate=responseTrackDto.AlbumDto.ReleaseDate,
                Type=responseTrackDto.AlbumDto.Type
            };

            _context.Albums.Add(album);
            await _context.SaveChangesAsync();   
            var images = new List<Image>();
            foreach (var img in responseTrackDto.AlbumDto.Images)
            {
                images.Add(new Image
                {
                    Url = img.Url,
                    Height = img.Height,
                    Width = img.Width,
                    AlbumId = album.Id
                });
            }
            await _context.Images.AddRangeAsync(images);
        }
        else
        {
            album = await _context.Albums.FirstOrDefaultAsync(a => a.SpotifyAlbumId == responseTrackDto.AlbumDto.Id);
        }
        
        var artists = new List<Artist>();
        
        foreach (var artistDto in responseTrackDto.AlbumDto.ArtistDtos)
        {
            if(!_context.Artists.Any(a => a.ArtistSpotifyId == artistDto.Id))
                artists.Add(new Artist
                {
                    Type = artistDto.Type,
                    ArtistSpotifyId = artistDto.Id,
                    Alias = artistDto.Name,
                    Href = artistDto.Href,
                    SpotifyUrl = artistDto.ExternalUrls.Spotify
                });
            
            else
            {
                var artist = await _context.Artists
                    .FirstOrDefaultAsync(p => p.ArtistSpotifyId == artistDto.Id);

                if (!_context.ArtistAlbums.Any(p =>
                        p.ArtistId == artist.Id || p.AlbumId == album.Id))
                    _context.ArtistAlbums.Add(new ArtistAlbum
                    {
                        AlbumId = album.Id,
                        ArtistId = artist.Id
                    });
            }
        }

        if (artists.Count > 0)
        {
            _context.Artists.AddRange(artists);
            await _context.SaveChangesAsync();
            foreach (var artist in artists)
            {
                if (!_context.ArtistAlbums.Any(p =>
                        p.ArtistId == artist.Id || p.AlbumId == album.Id))
                _context.ArtistAlbums.Add(new ArtistAlbum
                {
                    AlbumId = album.Id,
                    ArtistId = artist.Id
                });
            }
        }
        
        var track = await _context.Tracks.FirstOrDefaultAsync(a => a.SpotifyTrackID == responseTrackDto.id);
        
        if(track == null)
            _context.Tracks.Add(new Track
                {
                    Name = responseTrackDto.Name,
                    Href = responseTrackDto.Href,
                    Type = responseTrackDto.Type,
                    AlbumId = album.Id,
                    ExternalUrl = responseTrackDto.ExternalUrl.Spotify,
                    SpotifyTrackID = responseTrackDto.id,
                    AvailableMarkets = string.Join(',',responseTrackDto.AvailableMarkets),
                    Popularity = responseTrackDto.Popularity
                });
        else if(track.Popularity != null)
        {
            track.Popularity = responseTrackDto.Popularity;
            track.ExternalUrl = responseTrackDto.ExternalUrl.Spotify;
            _context.Update(track);
        }
        
        await _context.SaveChangesAsync();
        
        return Ok();
    }

    [HttpGet]
    [Route("GetAlbum")]
    public async Task<IActionResult> GetAlbumFromSite([FromQuery] RequestSpotifyDto dto)
    {
        var accessToken = JsonDocument.Parse(await GetSpotifyAccessTokenAsync())
            .RootElement.ToString().Split('"')[3];

        var request = new HttpRequestMessage() {
            Method = HttpMethod.Get
        };
        
        if (string.IsNullOrEmpty(dto.Market))
            request.RequestUri = new Uri(string.Format("https://api.spotify.com/v1/albums/{0}", dto.ID));
        else
            request.RequestUri = new Uri(string.Format("https://api.spotify.com/v1/albums/{0}?market={1}", dto.ID, dto.Market));

        request.Headers.Add("Authorization", string.Format("Bearer {0}", accessToken));
        
        var response = await client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();
        
        ResponseAlbumDto jsonString = JsonConvert
            .DeserializeObject<ResponseAlbumDto>(responseContent
                .Replace("\r\n", String.Empty));
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return Ok("Api Spotify return BadRequest. Please review your fetching data and try again.");
        
        if (response.StatusCode == HttpStatusCode.Forbidden)
            return Ok("Api Spotify return Forbidden. Something went wrong maybe try later.");
            
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return Ok("Api Spotify return Unauthorized. Something went wrong maybe try later.");

        if (!response.IsSuccessStatusCode)
            return Ok("Api Spotify return an unknown error. Something went wrong maybe try later.");
        
        return Ok(responseContent);
    }

    [HttpGet]
    [Route("GetArtist")]
    public async Task<IActionResult> GetArtist([FromQuery] RequestArtistSpotifyDto dto)
    {
        var accessToken = JsonDocument.Parse(await GetSpotifyAccessTokenAsync())
            .RootElement.ToString().Split('"')[3];

        var request = new HttpRequestMessage() {
            RequestUri = new Uri(string.Format("https://api.spotify.com/v1/artists/{0}", dto.ID)),
            Method = HttpMethod.Get
        };
        
        request.Headers.Add("Authorization", string.Format("Bearer {0}", accessToken));
        
        var response = await client.SendAsync(request);
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return Ok("Api Spotify return BadRequest. Please review your fetching data and try again.");
        
        if (response.StatusCode == HttpStatusCode.Forbidden)
            return Ok("Api Spotify return Forbidden. Something went wrong maybe try later.");
            
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return Ok("Api Spotify return Unauthorized. Something went wrong maybe try later.");

        if (!response.IsSuccessStatusCode)
            return Ok("Api Spotify return an unknown error. Something went wrong maybe try later.");
        
        return Ok(await response.Content.ReadAsStringAsync());
    }

    [HttpGet]
    [Route("GetTrack")]
    public async Task<IActionResult> GetTrack([FromQuery] RequestSpotifyDto dto)
    {
        var accessToken = JsonDocument.Parse(await GetSpotifyAccessTokenAsync())
            .RootElement.ToString().Split('"')[3];
        
        var request = new HttpRequestMessage() {
            Method = HttpMethod.Get
        };

        if (string.IsNullOrEmpty(dto.Market))
            request.RequestUri = new Uri(string.Format("https://api.spotify.com/v1/tracks/{0}", dto.ID));
        else
            request.RequestUri = new Uri(string.Format("https://api.spotify.com/v1/tracks/{0}?market={1}", dto.ID, dto.Market));
        
        request.Headers.Add("Authorization", string.Format("Bearer {0}", accessToken));
        
        var response = await client.SendAsync(request);
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return Ok("Api Spotify return BadRequest. Please review your fetching data and try again.");
        
        if (response.StatusCode == HttpStatusCode.Forbidden)
            return Ok("Api Spotify return Forbidden. Something went wrong maybe try later.");
            
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return Ok("Api Spotify return Unauthorized. Something went wrong maybe try later.");

        if (!response.IsSuccessStatusCode)
            return Ok("Api Spotify return an unknown error. Something went wrong maybe try later.");

        return Ok(await response.Content.ReadAsStringAsync());
    }

    [HttpGet]
    [Route("GetAlbumFromDb")]
    public async Task<IActionResult> GetAlbumFromDb([FromQuery] RequestAlbumFromDbDto dto)
    {
        List<Album> albums = null;
        
        if(!string.IsNullOrEmpty(dto.Name) 
           && !string.IsNullOrEmpty(dto.SpotifyAlbumId))
            albums = _context.Albums
            .Where(f => f.SpotifyAlbumId.Equals(dto.SpotifyAlbumId) 
                && f.Name.Contains(dto.Name)
                && f.ReleaseDate > dto.MinReleaseDate 
                && f.ReleaseDate < dto.MaxReleaseDate)
            .ToList();
        else if (!string.IsNullOrEmpty(dto.Name))
        {
            albums = _context.Albums
                .Where(f => f.Name.Contains(dto.Name)
                             && (f.ReleaseDate > dto.MinReleaseDate 
                                 && f.ReleaseDate < dto.MaxReleaseDate))
                .ToList();
        }
        else if (!string.IsNullOrEmpty(dto.SpotifyAlbumId))
        {
            albums = _context.Albums
                .Where(f => f.SpotifyAlbumId.Equals(dto.SpotifyAlbumId) 
                    && (f.ReleaseDate > dto.MinReleaseDate 
                        && f.ReleaseDate < dto.MaxReleaseDate))
                .ToList();
        }
        
        if (albums == null)
            return Ok("No albums Found.");

        var listedAlbums = (await PaginationAlbumAsync(albums, dto))
            .Skip(dto.Offset)
            .Take(dto.MaxPerPage);

        return Ok(listedAlbums);
    }

    [HttpGet]
    [Route("GetArtistFromDb")]
    public async Task<IActionResult> GetArtistFromDb([FromQuery] RequestArtistFromDbDto dto)
    {
        List<Artist> artists = null;
        if(!string.IsNullOrEmpty(dto.Name) 
           && !string.IsNullOrEmpty(dto.SpotifyArtistId))
            artists = _context.Artists.Where(
                f => f.ArtistSpotifyId == dto.SpotifyArtistId
                 && f.Alias.Contains(dto.Name)).ToList();
        else if (!string.IsNullOrEmpty(dto.Name))
        {
            artists = _context.Artists.Where(
                f => f.Alias.Contains(dto.Name)).ToList();
        }
        else if (!string.IsNullOrEmpty(dto.SpotifyArtistId))
        {
            artists = _context.Artists.Where(
                f => f.ArtistSpotifyId == dto.SpotifyArtistId).ToList();
        }

        if (artists == null)
            return Ok("No artists Found.");

        var listedArtists = (await PaginationArtistAsync(artists, dto))
            .Skip(dto.Offset)
            .Take(dto.MaxPerPage).ToList();

        return Ok(listedArtists);
    }

    [HttpGet]
    [Route("GetTrackFromDb")]
    public async Task<IActionResult> GetTrackFromDb([FromQuery] RequestTrackFromDbDto dto)
    {
        List<Track> tracks = null;
        if(!string.IsNullOrEmpty(dto.Name) 
           && !string.IsNullOrEmpty(dto.SpotifyTrackId))
            tracks = _context.Tracks.Where(
                f => f.SpotifyTrackID == dto.SpotifyTrackId
                     && f.Name.Contains(dto.Name)).ToList();
        else if (!string.IsNullOrEmpty(dto.Name))
        {
            tracks = _context.Tracks.Where(
                f => f.Name.Contains(dto.Name)).ToList();
        }
        else if (!string.IsNullOrEmpty(dto.SpotifyTrackId))
        {
            tracks = _context.Tracks.Where(
                f => f.SpotifyTrackID == dto.SpotifyTrackId)
                .ToList();
        }

        if (tracks == null)
            return Ok("No Tracks Found.");
            
        var listedTracks = (await PaginationTrackAsync(tracks, dto))
            .Skip(dto.Offset)
            .Take(dto.MaxPerPage)
            .ToList();
        
        return Ok(listedTracks);
    }
    
    private async Task<String> GetSpotifyAccessTokenAsync()
    {
        var request = new HttpRequestMessage() {
            RequestUri = new Uri("https://accounts.spotify.com/api/token"),
            Method = HttpMethod.Post
        };
        
        request.Content = new StringContent("grant_type=client_credentials&client_id=ac7cd01a1c5e4aeea77f864a3a4d43d2" +
                                            "&client_secret=92125ac9287c4ffdb9f536652cd49817", 
            Encoding.UTF8, "application/x-www-form-urlencoded"); 
            
        var response = await client.SendAsync(request);

        if ((int)response.StatusCode > 299)
            return "Error";
        
        return await response.Content.ReadAsStringAsync();
    }

    private async Task<List<Album>> PaginationAlbumAsync(List<Album> albums, RequestAlbumFromDbDto dto)
    {
        var orderedList = albums;
        if (dto.IsASC)
        {
            if (dto.OrderByReleaseDate)
                orderedList = albums.OrderBy(o => o.ReleaseDate).ToList();

            if (dto.OrderByName)
                orderedList = albums.OrderBy(o => o.Name).ToList();
        
            if (dto.OrderByTotalTracks)
                orderedList = albums.OrderBy(o => o.TotalTracks).ToList();
        }
        else
        {
            if (dto.OrderByReleaseDate)
                orderedList = albums.OrderByDescending(o => o.ReleaseDate).ToList();

            if (dto.OrderByName)
                orderedList = albums.OrderByDescending(o => o.Name).ToList();
        
            if (dto.OrderByTotalTracks)
                orderedList = albums.OrderByDescending(o => o.TotalTracks).ToList();
        }
        
        return orderedList;
    }
    
    private async Task<List<Artist>> PaginationArtistAsync(List<Artist> artists, RequestArtistFromDbDto dto)
    {
        var orderedList = artists;
        if (dto.IsASC)
        {
            if (dto.OrderByPopularity)
                orderedList = artists.OrderBy(o => o.Popularity).ToList();

            if (dto.OrderByAlias)
                orderedList = artists.OrderBy(o => o.Alias).ToList();
        
            if (dto.OrderByTotalFollowers)
                orderedList = artists.OrderBy(o => o.TotalFollowers).ToList();
        }
        else
        {
            if (dto.OrderByPopularity)
                orderedList = artists.OrderByDescending(o => o.Popularity).ToList();

            if (dto.OrderByAlias)
                orderedList = artists.OrderByDescending(o => o.Alias).ToList();
        
            if (dto.OrderByTotalFollowers)
                orderedList = artists.OrderByDescending(o => o.TotalFollowers).ToList();
        }
        
        return orderedList;
    }
    
    private async Task<List<Track>> PaginationTrackAsync(List<Track> tracks, RequestTrackFromDbDto dto)
    {
        var orderedList = tracks;
        if (dto.IsASC)
        {
            if (dto.OrderByName)
                orderedList = tracks.OrderBy(o => o.Name).ToList();
        }
        else
        {
            if (dto.OrderByName)
                orderedList = tracks.OrderByDescending(o => o.Name).ToList();
        }
        
        return orderedList;
    }

}