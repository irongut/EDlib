using EDlib.Common;
using EDlib.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EDlib.INARA
{
    /// <summary>
    ///   <para>Represents a Community Goal returned by the INARA API.</para>
    ///   <para>See INARA documentation for <a href="https://inara.cz/inara-api-docs/#event-37">getCommunityGoalsRecent</a>.</para>
    /// </summary>
    public class CommunityGoal : INotifyPropertyChanged
    {
        /// <summary>Raised when some properties change.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        /// <summary>The name of the Community goal.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty(PropertyName = "communitygoalName")]
        public string CommunityGoalName { get; internal set; }

        /// <summary>The ID of the community goal.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty(PropertyName = "communitygoalGameID")]
        public int CommunityGoalGameID { get; internal set; }

        /// <summary>The name of the system where the Community Goal taking place.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty(PropertyName = "starsystemName")]
        public string StarsystemName { get; internal set; }

        /// <summary>The name of the station where the Community Goal taking place.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty(PropertyName = "stationName")]
        public string StationName { get; internal set; }

        private DateTime _goalExpiry;
        /// <summary>The date and time when the Community Goal ends.</summary>
        [Preserve(Conditional = true)]
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

        /// <summary>The current tier reached.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty(PropertyName = "tierReached")]
        public int TierReached { get; internal set; }

        private int _tierMax;
        /// <summary>The maximum number of tiers. If reached, the Community Goal ends.</summary>
        [Preserve(Conditional = true)]
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

        /// <summary>The number of contributors to the Community Goal.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty(PropertyName = "contributorsNum")]
        public int ContributorsNum { get; internal set; }

        /// <summary>The total contributions to the Community Goal.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty(PropertyName = "contributionsTotal")]
        public long ContributionsTotal { get; internal set; }

        private bool _isCompleted;
        /// <summary>Whether the Community Goal has ended.</summary>
        [Preserve(Conditional = true)]
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

        /// <summary>The date and time when INARA last updated this information.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty(PropertyName = "lastUpdate")]
        public DateTime LastUpdate { get; internal set; }

        /// <summary>The objective for the Community Goal.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty(PropertyName = "goalObjectiveText")]
        public string GoalObjectiveText { get; internal set; }

        /// <summary>The reward for the Community Goal.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty(PropertyName = "goalRewardText")]
        public string GoalRewardText { get; internal set; }

        /// <summary>The full description of the Community Goal.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty(PropertyName = "goalDescriptionText")]
        public string GoalDescriptionText { get; internal set; }

        /// <summary>The INARA URL for this Community Goal.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty(PropertyName = "inaraURL")]
        public string InaraURL { get; internal set; }

        private TimeSpan _timeRemaining;
        /// <summary>The time remaining until this Community Goal expires.</summary>
        [Preserve(Conditional = true)]
        public TimeSpan TimeRemaining
        {
            get { return _timeRemaining; }
            set
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

        /// <summary>The date and time when the Community Goal ends.</summary>
        [Preserve(Conditional = true)]
        public double Progress { get; private set; }

        /// <summary>The progress of the Community Goal. (Progress x/y Tiers)</summary>
        [Preserve(Conditional = true)]
        public string ProgressText { get; private set; }

        /// <summary>The topic for this Community Goal, generated using a Bag of Words technique.</summary>
        [Preserve(Conditional = true)]
        public string Topic { get; private set; }

        #endregion

        /// <summary>Initializes a new empty instance of the <see cref="CommunityGoal" /> class.</summary>
        [Preserve(Conditional = true)]
        public CommunityGoal() { }

        /// <summary>Determines the Topic for a Community Goal by analysing it using a Bag of Words technique.</summary>
        /// <param name="topics">List of <see cref="Topic" /> to use when classifying article.</param>
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

        /// <summary>
        ///   <para>Raised when TimeRemaining or IsCompleted properties change to handle System.ComponentModel.INotifyPropertyChanged.PropertyChanged.</para>
        ///   <para>Note: TimeRemaining is a public property, use a timer to update TimeRemaining and IsCompleted will update automatically.</para>
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        [Preserve(Conditional = true)]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>Returns the Name, System and Objective for a Community Goal as a string.</summary>
        [Preserve(Conditional = true)]
        public override string ToString()
        {
            return $"{CommunityGoalName} ({StarsystemName}): {GoalObjectiveText}";
        }
    }
}
