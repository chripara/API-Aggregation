using System.Net;
using API_Aggregation.Dto.News;
using API_Aggregation.Models.Spotify;
using Microsoft.AspNetCore.Mvc;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using Article = API_Aggregation.Models.News.Article;

namespace API_Aggregation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public NewsController(
        AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    [Route("GetNews")]
    public async Task<IActionResult> GetNews([FromQuery] RequestNewsDto dto)
    {
        var newsApiClient = new NewsApiClient(APIKeyConstants.NewsAPIKey);
        var everythingRequest = new EverythingRequest();
        
        everythingRequest.Q = dto.Q;
        everythingRequest.From = dto.From;
        everythingRequest.Domains = dto.Domains;
        
        if (dto.SortBy.HasValue)
            everythingRequest.SortBy = (SortBys)dto.SortBy.Value;
        
        if (dto.Language.HasValue)
            everythingRequest.Language = (Languages)dto.Language.Value;
        
        if (dto.Page.HasValue)
            everythingRequest.Page = dto.Page.Value;
        
        if (dto.PageSize.HasValue)
            everythingRequest.PageSize = dto.PageSize.Value;
        
        everythingRequest.Sources = dto.Sources;
        
        var articlesResponse = await newsApiClient.GetEverythingAsync(everythingRequest);
        
        if (articlesResponse.Status == Statuses.Error)
        {
            return Ok("Newsapi return an error. Please review your fetching data and try again.");
        }
        
        return Ok(articlesResponse);
    }

    [HttpGet]
    [Route("DownloadNews")]
    public async Task<IActionResult> DownloadNews([FromQuery] RequestNewsDto dto)
    {
        var newsApiClient = new NewsApiClient(APIKeyConstants.NewsAPIKey);
        var everythingRequest = new EverythingRequest();
        
        everythingRequest.Q = dto.Q;
        everythingRequest.From = dto.From;
        everythingRequest.Domains = dto.Domains;
        
        if (dto.SortBy.HasValue)
            everythingRequest.SortBy = (SortBys)dto.SortBy.Value;
        
        if (dto.Language.HasValue)
            everythingRequest.Language = (Languages)dto.Language.Value;
        
        if (dto.Page.HasValue)
            everythingRequest.Page = dto.Page.Value;
        
        if (dto.PageSize.HasValue)
            everythingRequest.PageSize = dto.PageSize.Value;
        
        everythingRequest.Sources = dto.Sources;
        
        var articlesResponse = await newsApiClient.GetEverythingAsync(everythingRequest);
        
        if(articlesResponse.Status == Statuses.Error)
        {
            return Ok("Newsapi return an error. Please review your fetching data and try again.");
        }
        
        var articles = new List<Article>();
        foreach (var article in articlesResponse.Articles)
        {
            articles.Add(new Article
            {
                SourceName = article.Source.Name,
                Author = article.Author,
                Title = article.Title,
                Description = article.Description,
                Url = article.Url,
                UrlToImage = article.UrlToImage,
                PublishedAt = article.PublishedAt,
                Content = article.Content
            });
        }

        await _context.Articles.AddRangeAsync(articles);
        await _context.SaveChangesAsync();
        
        return Ok(articlesResponse);
    }

    [HttpGet]
    [Route("GetNewsFromDb")]
    public async Task<IActionResult> GetNewsFromDb([FromQuery] RequestNewsFromDbDto dto)
    {
        var articles = new List<Article>();
        if (dto.SortBy && dto.SortByDesc)
        {
            articles.AddRange(
                _context.Articles
                    .Where(p => p.Content.Contains(dto.Q) 
                                || p.Description.Contains(dto.Q))
                    .OrderByDescending(p => p.PublishedAt).ToList());
        }
        else if (dto.SortBy && !dto.SortByDesc)
        {
            articles.AddRange(
                _context.Articles
                    .Where(p => p.Content.Contains(dto.Q) 
                                || p.Description.Contains(dto.Q))
                    .OrderBy(p => p.PublishedAt).ToList());
        }
        else
        {
            articles.AddRange(
                _context.Articles
                    .Where(p => p.Content.Contains(dto.Q) 
                                || p.Description.Contains(dto.Q))
                    .ToList());
        }

        var listedTracks = new List<Article>();
            
        if(dto.PageSize.HasValue && dto.Offset.HasValue)
        {
            listedTracks.AddRange(articles
                .Skip(dto.Offset.Value)
                .Take(dto.PageSize.Value)
                .ToList());
        }
        else if (dto.PageSize.HasValue)
        {
            listedTracks.AddRange(articles
                .Take(dto.PageSize.Value)
                .ToList());
        }
        else if (dto.Offset.HasValue)
        {
            listedTracks.AddRange(articles
                .Skip(dto.Offset.Value)
                .ToList());
        }
        else
        {
            listedTracks.AddRange(articles);
        }

        return Ok(listedTracks);
    }

}