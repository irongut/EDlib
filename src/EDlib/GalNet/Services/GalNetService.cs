using EDlib.Common;
using EDlib.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace EDlib.GalNet
{
    /// <summary>
    ///   <para>
    ///     Gets the latest GalNet News from an API provided by Taranis Software.<br/>
    ///     Uses a Bag of Words technique to determine a Topic and content Tags for each article.
    ///   </para>
    ///   <para>Please cache for a minimum of 1 hour.</para>
    /// </summary>
    public sealed class GalNetService
    {
        private static readonly GalNetService instance = new GalNetService();

        private static IDownloadService dService;

        private const string GalNetURL = "https://api.taranissoftware.com/elite-dangerous/galnet-latest.json";

        private readonly List<NewsArticle> galnetNews;
        private DateTime lastUpdated;

        private GalNetService()
        {
            lastUpdated = DateTime.MinValue;
            galnetNews = new List<NewsArticle>();
        }

        /// <summary>Instantiates the GalNetService class.</summary>
        /// <param name="downloadService">IDownloadService instance used to download data.</param>
        /// <returns>GalNetService</returns>
        public static GalNetService Instance(IDownloadService downloadService)
        {
            dService = downloadService;
            return instance;
        }

        /// <summary>Gets the 20 most recent GalNet News articles.</summary>
        /// <param name="expiryHours">The number of hours to cache the data, minimum 1 hour.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="BoW">An optional alternative Bag of Words to use when classifying articles as a json string.</param>
        /// <param name="ignoreBoW">An optional alternative Bag of Words to ignore when classifying articles as a json string.</param>
        /// <param name="ignoreCache">Ignore any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(List&lt;NewsArticle&gt;, DateTime)&gt;</returns>
        public async Task<(List<NewsArticle> news, DateTime updated)> GetData(int expiryHours,
                                                                              CancellationTokenSource cancelToken,
                                                                              string BoW = null,
                                                                              string ignoreBoW = null,
                                                                              bool ignoreCache = false)
        {
            (List<NewsArticle> _, DateTime _) = await GetData(20, expiryHours, cancelToken, BoW, ignoreBoW, ignoreCache).ConfigureAwait(false);
            return (galnetNews.Take(20).ToList(), lastUpdated);
        }

        /// <summary>Gets the most recent GalNet News articles.</summary>
        /// <param name="articleCount">The number of articles to return.</param>
        /// <param name="expiryHours">The number of hours to cache the data, minimum 1 hour.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="BoW">An optional alternative Bag of Words to use when classifying articles as a json string.</param>
        /// <param name="ignoreBoW">An optional alternative Bag of Words to ignore when classifying articles as a json string.</param>
        /// <param name="ignoreCache">Ignore any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(List&lt;NewsArticle&gt;, DateTime)&gt;</returns>
        public async Task<(List<NewsArticle> news, DateTime updated)> GetData(int articleCount,
                                                                              int expiryHours,
                                                                              CancellationTokenSource cancelToken,
                                                                              string BoW = null,
                                                                              string ignoreBoW = null,
                                                                              bool ignoreCache = false)
        {
            if (expiryHours < 1) expiryHours = 1;

            TimeSpan expiry = TimeSpan.FromHours(expiryHours);
            if (galnetNews?.Any() == false || galnetNews.Count < articleCount || lastUpdated + expiry < DateTime.Now)
            {
                // download the json
                string json;
                DownloadOptions options = new DownloadOptions(cancelToken, expiry, ignoreCache);
                (json, lastUpdated) = await dService.GetData(GalNetURL, options).ConfigureAwait(false);

                // parse the news articles
                galnetNews.Clear();
                await Task.Run(() =>
                {
                    List<NewsArticle> fullNews = JsonConvert.DeserializeObject<List<NewsArticle>>(json, NewsArticleConverter.Instance());
                    List<Topic> topics = GetTopics(BoW);
                    List<Topic> ignoreTopics = GetIgnoreTopics(ignoreBoW);
                    foreach (NewsArticle item in fullNews.Where(o => !String.IsNullOrEmpty(o.Body)).OrderByDescending(o => o.PublishDateTime).Take(articleCount))
                    {
                        item.ClassifyArticle(topics, ignoreTopics);
                        galnetNews.Add(item);

                        foreach (Topic topic in topics)
                            topic.Count = 0;
                        foreach (Topic topic in ignoreTopics)
                            topic.Count = 0;
                    }
                }).ConfigureAwait(false);
            }
            return (galnetNews.Take(articleCount).ToList(), lastUpdated);
        }

        private List<Topic> GetTopics(string BoW)
        {
            return string.IsNullOrWhiteSpace(BoW)
                   ? LoadBoW("EDlib.GalNet.Resources.NewsBoW.json")
                   : JsonConvert.DeserializeObject<List<Topic>>(BoW);
        }

        private List<Topic> GetIgnoreTopics(string ignoreBoW)
        {
            return string.IsNullOrWhiteSpace(ignoreBoW)
                   ? LoadBoW("EDlib.GalNet.Resources.NewsFalseBoW.json")
                   : JsonConvert.DeserializeObject<List<Topic>>(ignoreBoW);
        }

        private List<Topic> LoadBoW(string filename)
        {
            List<Topic> topics;
            try
            {
                Assembly assembly = GetType().GetTypeInfo().Assembly;
                using (Stream stream = assembly.GetManifestResourceStream(filename))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        topics = (List<Topic>)serializer.Deserialize(reader, typeof(List<Topic>));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to load Bag of Words for linguistic analysis.", ex);
            }
            return topics;
        }
    }
}
