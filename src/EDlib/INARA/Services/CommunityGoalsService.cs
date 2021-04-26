﻿using EDlib.Common;
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
    public sealed class CommunityGoalsService
    {
        private static readonly CommunityGoalsService instance = new CommunityGoalsService();

        private static IDownloadService dService;

        private const string eventName = "getCommunityGoalsRecent";

        private readonly List<CommunityGoal> communityGoals;
        private DateTime lastUpdated;

        private CommunityGoalsService()
        {
            lastUpdated = DateTime.MinValue;
            communityGoals = new List<CommunityGoal>();
        }

        public static CommunityGoalsService Instance(IDownloadService downloadService)
        {
            dService = downloadService;
            return instance;
        }

        public async Task<(List<CommunityGoal> goals, DateTime updated)> GetData(int cacheMinutes,
                                                                                 int requestDays,
                                                                                 InaraIdentity identity,
                                                                                 CancellationTokenSource cancelToken,
                                                                                 string BoW = null,
                                                                                 bool ignoreCache = false)
        {
            if (cacheMinutes < 60) cacheMinutes = 60;
            TimeSpan expiry = TimeSpan.FromMinutes(cacheMinutes);
            if (requestDays < 7) requestDays = 7;

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
                                if (goal.GoalExpiry > DateTime.Today.AddDays(0 - requestDays))
                                {
                                    goal.ClassifyCG(topics);
                                    communityGoals.Add(goal);
                                    foreach (Topic topic in topics)
                                        topic.Count = 0;
                                }
                            }
                        }
                    }
                }).ConfigureAwait(false);
            }
            return (communityGoals, lastUpdated);
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