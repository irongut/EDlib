using EDlib.Network;
using EDlib.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EDlib.GalNet
{
    /// <summary>Gets the GalNet News.</summary>
    public sealed class GalNetService
    {
        private static readonly GalNetService instance = new GalNetService();

        private static string agent;

        private static ICacheService cache;

        private static IConnectivityService connectivity;

        private const string GalNetURL = "https://elitedangerous-website-backend-production.elitedangerous.com/api/galnet?_format=json";
        private const string dataKey = "NewsFeed";
        private const string lastUpdatedKey = "NewsLastUpdated";

        private readonly List<NewsArticle> galnetNews;
        private DateTime lastUpdated;

        private GalNetService()
        {
            lastUpdated = DateTime.MinValue;
            galnetNews = new List<NewsArticle>();
        }

        /// <summary>Instances the GalNetService class singleton.</summary>
        /// <param name="userAgent">The user agent used for downloads.</param>
        /// <param name="cacheService">The platform specific cache for downloaded data.</param>
        /// <param name="connectivityService">The platform specific connectivity service.</param>
        /// <returns>GalNetService</returns>
        public static GalNetService Instance(string userAgent, ICacheService cacheService, IConnectivityService connectivityService)
        {
            agent = userAgent;
            cache = cacheService;
            connectivity = connectivityService;
            return instance;
        }

        /// <summary>Gets the 20 most recent GalNet News articles.</summary>
        /// <param name="expiryHours">The number of hours to cache the data.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="ignoreCache">Ignore any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(List&lt;NewsArticle&gt;, DateTime)&gt;</returns>
        public async Task<(List<NewsArticle> news, DateTime updated)> GetData(int expiryHours, CancellationTokenSource cancelToken, bool ignoreCache = false)
        {
            (List<NewsArticle> _, DateTime _) = await GetData(20, expiryHours, cancelToken, ignoreCache).ConfigureAwait(false);
            return (galnetNews.Take(20).ToList(), lastUpdated);
        }

        /// <summary>Gets the most recent GalNet News articles.</summary>
        /// <param name="articleCount">The number of articles to return.</param>
        /// <param name="expiryHours">The number of hours to cache the data.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="ignoreCache">Ignore any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(List&lt;NewsArticle&gt;, DateTime)&gt;</returns>
        public async Task<(List<NewsArticle> news, DateTime updated)> GetData(int articleCount, int expiryHours, CancellationTokenSource cancelToken, bool ignoreCache = false)
        {
            TimeSpan expiry = TimeSpan.FromHours(expiryHours);
            if (galnetNews?.Any() == false || galnetNews.Count < articleCount || lastUpdated + expiry < DateTime.Now)
            {
                // download the json
                string json;
                DownloadService downloadService = DownloadService.Instance(agent, cache, connectivity);
                (json, lastUpdated) = await downloadService.GetData(GalNetURL, dataKey, lastUpdatedKey, expiry, cancelToken, ignoreCache).ConfigureAwait(false);

                // parse the news articles
                galnetNews.Clear();
                await Task.Run(() =>
                {
                    List<NewsArticle> fullNews = JsonConvert.DeserializeObject<List<NewsArticle>>(json, NewsArticleConverter.Instance());
                    foreach (NewsArticle item in fullNews.Where(o => !String.IsNullOrEmpty(o.Body)).OrderByDescending(o => o.PublishDateTime).Take(articleCount))
                    {
                        item.ClassifyArticle();
                        galnetNews.Add(item);
                    }
                }).ConfigureAwait(false);
            }
            return (galnetNews.Take(articleCount).ToList(), lastUpdated);
        }
    }
}
