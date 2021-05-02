using EDlib.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EDlib.INARA
{
    public class CommunityGoal : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        [JsonProperty(PropertyName = "communitygoalName")]
        public string CommunityGoalName { get; internal set; }

        [JsonProperty(PropertyName = "communitygoalGameID")]
        public int CommunityGoalGameID { get; internal set; }

        [JsonProperty(PropertyName = "starsystemName")]
        public string StarsystemName { get; internal set; }

        [JsonProperty(PropertyName = "stationName")]
        public string StationName { get; internal set; }

        private DateTime _goalExpiry;
        [JsonProperty(PropertyName = "goalExpiry")]
        public DateTime GoalExpiry
        {
            get { return _goalExpiry; }
            internal set
            {
                if (_goalExpiry != value)
                {
                    _goalExpiry = value;
                    TimeRemaining = value - DateTime.UtcNow;
                }
            }
        }

        [JsonProperty(PropertyName = "tierReached")]
        public int TierReached { get; internal set; }

        private int _tierMax;
        [JsonProperty(PropertyName = "tierMax")]
        public int TierMax
        {
            get { return _tierMax; }
            internal set
            {
                if (_tierMax != value)
                {
                    _tierMax = value;
                    Progress = (double)TierReached / value;
                    ProgressText = $"Progress: {TierReached} / {value} Tiers";
                }
            }
        }

        [JsonProperty(PropertyName = "contributorsNum")]
        public int ContributorsNum { get; internal set; }

        [JsonProperty(PropertyName = "contributionsTotal")]
        public long ContributionsTotal { get; internal set; }

        public bool _isCompleted;
        [JsonProperty(PropertyName = "isCompleted")]
        public bool IsCompleted
        {
            get { return _isCompleted; }
            internal set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged(nameof(IsCompleted));
                }
            }
        }

        [JsonProperty(PropertyName = "lastUpdate")]
        public DateTime LastUpdate { get; internal set; }

        [JsonProperty(PropertyName = "goalObjectiveText")]
        public string GoalObjectiveText { get; internal set; }

        [JsonProperty(PropertyName = "goalRewardText")]
        public string GoalRewardText { get; internal set; }

        [JsonProperty(PropertyName = "goalDescriptionText")]
        public string GoalDescriptionText { get; internal set; }

        [JsonProperty(PropertyName = "inaraURL")]
        public string InaraURL { get; internal set; }

        private TimeSpan _timeRemaining;
        public TimeSpan TimeRemaining
        {
            get { return _timeRemaining; }
            internal set
            {
                if (_timeRemaining != value)
                {
                    if (value > TimeSpan.Zero)
                    {
                        _timeRemaining = value;
                    }
                    else
                    {
                        _timeRemaining = TimeSpan.Zero;
                        IsCompleted = true;
                    }
                    OnPropertyChanged(nameof(TimeRemaining));
                }
            }
        }

        public double Progress { get; private set; }

        public string ProgressText { get; internal set; }

        private string _topic;
        public string Topic
        {
            get { return _topic; }
            private set
            {
                if (_topic != value)
                {
                    _topic = value;
                    OnPropertyChanged(nameof(Topic));
                }
            }
        }

        #endregion

        internal void ClassifyCG(List<Topic> topics)
        {
            // analyse CG using Bag of Words technique
            AnalyseSentences(SplitSentences(), topics);

            // select topic
            Topic tempTopic = topics.OrderByDescending(o => o.Count).First();
            Topic = tempTopic.Count < 1 ? "Unclassified" : tempTopic.Name;
        }

        private void AnalyseSentences(List<string> sentences, List<Topic> topicsList)
        {
            foreach (string sentence in sentences)
            {
                Parallel.ForEach(topicsList, topic =>
                {
                    foreach (string term in topic.Terms)
                    {
                        if (sentence.Contains(term.ToLower()))
                        {
                            topic.Count++;
                        }
                    }
                });
            }
        }

        private List<string> SplitSentences()
        {
            List<string> sentences = new List<string>
            {
                CommunityGoalName.Trim().ToLower(),
                GoalObjectiveText.Trim().ToLower()
            };
            foreach (string sentence in Regex.Split(GoalDescriptionText, @"(?<=[\w\s](?:[\.\!\? ]+[\x20]*[\x22\xBB]*))(?:\s+(?![\x22\xBB](?!\w)))"))
            {
                sentences.Add(sentence.Trim().ToLower());
            }
            return sentences;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override string ToString()
        {
            return $"{CommunityGoalName} ({StarsystemName}): {GoalObjectiveText}";
        }
    }
}
