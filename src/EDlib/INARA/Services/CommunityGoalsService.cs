using EDlib.Common;
using EDlib.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace EDlib.INARA
{
    /// <summary>
    ///   <para>Gets details of ongoing and recently completed Community Goals from the INARA API.</para>
    ///   <para>See INARA documentation for <a href="https://inara.cz/inara-api-docs/#event-37">getCommunityGoalsRecent</a>.</para>
    /// </summary>
    public sealed class CommunityGoalsService
    {
        private static readonly CommunityGoalsService instance = new CommunityGoalsService();

        private static IDownloadService dService;

        private const string eventName = "getCommunityGoalsRecent";

        private readonly List<CommunityGoal> communityGoals;
        private DateTime lastUpdated;
        private int lastDays;

        private CommunityGoalsService()
        {
            lastUpdated = DateTime.MinValue;
            communityGoals = new List<CommunityGoal>();
        }

        /// <summary>Instantiates the CommunityGoalsService class.</summary>
        /// <param name="downloadService">IDownloadService instance used to download data.</param>
        /// <returns>CommunityGoalsService</returns>
        public static CommunityGoalsService Instance(IDownloadService downloadService)
        {
            dService = downloadService;
            return instance;
        }

        /// <summary>Gets a number of recent Community Goals from INARA.</summary>
        /// <param name="goalCount">The number of Community Goals to return.</param>
        /// <param name="cacheMinutes">How long to cache the data in minutes, minimum 60 minutes.</param>
        /// <param name="identity">The credentials required to access the INARA API.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="BoW">An optional alternative Bag of Words to use when classifying Community Goals, passed as a json string.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(List&lt;CommunityGoal&gt;, DateTime)&gt;</returns>
        public async Task<(List<CommunityGoal> goals, DateTime updated)> GetData(int goalCount,
                                                                                 int cacheMinutes,
                                                                                 InaraIdentity identity,
                                                                                 CancellationTokenSource cancelToken,
                                                                                 string BoW = null,
                                                                                 bool ignoreCache = false)
        {
            if (cacheMinutes < 60) cacheMinutes = 60;

            if (communityGoals?.Any() == false || communityGoals.Count < goalCount || lastUpdated + TimeSpan.FromMinutes(cacheMinutes) < DateTime.Now)
            {
                await GetData(cacheMinutes, identity, cancelToken, BoW, ignoreCache).ConfigureAwait(false);
            }
            return (communityGoals.Take(goalCount).ToList(), lastUpdated);
        }

        /// <summary>Gets all the recent Community Goals from INARA; usually 15.</summary>
        /// <param name="cacheMinutes">How long to cache the data in minutes, minimum 60 minutes.</param>
        /// <param name="identity">The credentials required to access the INARA API.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="BoW">An optional alternative Bag of Words to use when classifying Community Goals, passed as a json string.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(List&lt;CommunityGoal&gt;, DateTime)&gt;</returns>
        public async Task<(List<CommunityGoal> goals, DateTime updated)> GetData(int cacheMinutes,
                                                                                 InaraIdentity identity,
                                                                                 CancellationTokenSource cancelToken,
                                                                                 string BoW = null,
                                                                                 bool ignoreCache = false)
        {
            if (cacheMinutes < 60) cacheMinutes = 60;
            TimeSpan expiry = TimeSpan.FromMinutes(cacheMinutes);

            if (communityGoals?.Any() == false || lastUpdated + expiry < DateTime.Now)
            {
                // request data
                string json;
                DownloadOptions options = new DownloadOptions(cancelToken, expiry, ignoreCache);
                InaraService inaraService = InaraService.Instance(dService);
                List<InaraEvent> input = new List<InaraEvent>
                {
                    new InaraEvent(eventName, new List<object>())
                };
                (json, lastUpdated) = await inaraService.GetData(new InaraHeader(identity), input, options).ConfigureAwait(false);

                // parse community goals
                communityGoals.Clear();
                await Task.Run(() =>
                {
                    InaraRequest outputData = JsonConvert.DeserializeObject<InaraRequest>(json);
                    List<Topic> topics = GetTopics(BoW);
                    foreach (InaraEvent item in outputData.Events)
                    {
                        if (item.EventData != null)
                        {
                            foreach (JObject cg in item.EventData)
                            {
                                CommunityGoal goal = cg.ToObject<CommunityGoal>();
                                goal.ClassifyCG(topics);
                                communityGoals.Add(goal);
                                foreach (Topic topic in topics)
                                    topic.Count = 0;
                            }
                        }
                    }
                }).ConfigureAwait(false);
            }
            return (communityGoals, lastUpdated);
        }

        /// <summary>Gets recent Community Goals from INARA over a specified number of days.</summary>
        /// <param name="requestDays">How many days back to look for Community Goals, minimum 7 days.</param>
        /// <param name="cacheMinutes">How long to cache the data in minutes, minimum 60 minutes.</param>
        /// <param name="identity">The credentials required to access the INARA API.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="BoW">An optional alternative Bag of Words to use when classifying Community Goals, passed as a json string.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(List&lt;CommunityGoal&gt;, DateTime)&gt;</returns>
        public async Task<(List<CommunityGoal> goals, DateTime updated)> GetDataByTime(int requestDays,
                                                                                       int cacheMinutes,
                                                                                       InaraIdentity identity,
                                                                                       CancellationTokenSource cancelToken,
                                                                                       string BoW = null,
                                                                                       bool ignoreCache = false)
        {
            if (requestDays < 7) requestDays = 7;
            if (cacheMinutes < 60) cacheMinutes = 60;

            if (communityGoals?.Any() == false || lastDays != requestDays || lastUpdated + TimeSpan.FromMinutes(cacheMinutes) < DateTime.Now)
            {
                await GetData(cacheMinutes, identity, cancelToken, BoW, ignoreCache).ConfigureAwait(false);
            }

            lastDays = requestDays;
            List<CommunityGoal> goals = new List<CommunityGoal>();
            foreach (CommunityGoal goal in communityGoals)
            {
                if (goal.GoalExpiry > DateTime.Today.AddDays(0 - requestDays))
                {
                    goals.Add(goal);
                }
            }
            return (goals, lastUpdated);
        }

        private List<Topic> GetTopics(string BoW)
        {
            return string.IsNullOrWhiteSpace(BoW)
                   ? LoadBoW("EDlib.INARA.Resources.CGBoW.json")
                   : JsonConvert.DeserializeObject<List<Topic>>(BoW);
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
