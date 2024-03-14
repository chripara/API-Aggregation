using NewsAPI.Constants;

namespace API_Aggregation.Dto.News;

public class RequestNewsDto
{
    /// <summary>
    /// Keywords or a phrase to search for.
    /// </summary>
    public string Q { get; set; } = "";
    /// <summary>
    /// A comma-seperated string of identifiers for the news sources or blogs you want headlines from.
    /// Use the /top-headlines/sources endpoint to locate these programmatically or look at the sources index.
    /// Note: you can't mix this param with the country or category params.
    /// </summary>
    public List<string> Sources { get; set; } = new List<string>();
    /// <summary>
    /// A comma-seperated string of domains (eg bbc.co.uk, techcrunch.com, engadget.com) to restrict the search to.
    /// </summary>
    public List<string> Domains { get; set; } = new List<string>();
    /// <summary>
    /// A date and optional time for the oldest article allowed. This should be in ISO 8601 format
    /// (e.g. 2024-03-13 or 2024-03-13T10:52:27)
    /// Default: the oldest according to your plan.
    /// </summary>
    public DateTime? From { get; set; } = null;
    /// <summary>
    /// A date and optional time for the newest article allowed. This should be in ISO 8601 format
    /// (e.g. 2024-03-13 or 2024-03-13T10:52:27)
    /// Default: the newest according to your plan.
    /// </summary>
    public DateTime? To { get; set; } = null;
    /// <summary>
    /// Languages:
    /// AF = 0, AN = 1,  AR = 2, AZ = 3, BG = 4, BN = 5, BR = 6, BS = 7, CA = 8, CS = 9, CY = 10, DA = 11, DE = 12, EL = 13, EN = 14,
    /// EO = 15, ES = 16, ET = 17, EU = 18, FA = 19, FI = 20, FR = 21, GL = 22, HE = 23, HI = 24, HR = 25, HT = 26, HU = 27, HY = 28,
    /// ID = 29, IS = 30, IT = 31, JP = 32, JV = 33, KK = 34, KO = 35, LA = 36, LB = 37, LT = 38, LV = 39, MG = 40, MK = 41, ML = 42,
    /// MR = 43, MS = 44, NL = 45, NN = 46, NO = 47, OC = 48, PL = 49, PT = 50, RO = 51, RU = 52, SH = 53, SK = 54, SL = 55, SQ = 56,
    /// SR = 57, SV = 58, SW = 59, TA = 60, TE = 61, TH = 62, TL = 63, TR = 64, UK = 65, UR = 66, VI = 67, VO = 68, ZH = 69
    /// </summary>
    public int? Language { get; set; } = null;
    /// <summary>
    /// Popularity = 0, PublishedAt = 1, Relevancy = 2
    /// </summary>
    public int? SortBy { get; set; } = null;
    /// <summary>
    /// Use this to page through the results.
    /// Default: 1.
    /// </summary>
    public int? Page { get; set; } = null;
    /// <summary>
    /// The number of results to return per page.
    /// Default: 100. Maximum: 100.
    /// </summary>
    public int? PageSize { get; set; } = null;
}