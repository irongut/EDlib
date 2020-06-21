using EDlib.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EDlib.GalNet
{
    /// <summary>A GalNet News article.</summary>
    public class NewsArticle
    {
        #region Properties

        private string _title;
        /// <summary>The title of the News article.</summary>
        [JsonProperty(PropertyName = "title")]
        public string Title
        {
            get { return _title; }
            internal set
            {
                if (_title != value)
                {
                    _title = value.Trim();
                }
            }
        }

        private string _body;
        /// <summary>The body of the News article.</summary>
        [JsonProperty(PropertyName = "body")]
        public string Body
        {
            get { return _body; }
            internal set
            {
                if (_body != value)
                {
                    _body = value.Replace("<p>", "").Replace("</p>", "\n").Replace("<br />", "\n").Replace("&#039;", "'").Replace("&quot;", "\u201d").Trim();
                }
            }
        }

        [JsonProperty(PropertyName = "date")]
        /// <summary>The publish date and time of the News article.</summary>
        public DateTime PublishDateTime { get; internal set; }

        [JsonProperty(PropertyName = "nid")]
        /// <summary>Frontier Development's Id for the News article.</summary>
        public int Id { get; internal set; }

        /// <summary>
        ///   <para>
        ///  A key used by Frontier Development's to specify the image displayed with a News article.
        ///  Note: This is not an image url or filename.
        ///   </para>
        /// </summary>
        [JsonProperty(PropertyName = "image")]
        public string FDImageName { get; internal set; }

        [JsonProperty(PropertyName = "slug")]
        /// <summary>A normalised version of the article title used as a unique identifier.</summary>
        public string Slug { get; internal set; }

        /// <summary>The topic of the News article, generated using a Bag of Words technique.</summary>
        public string Topic { get; private set; }
        /// <summary>Content tags for the News article, generated using a Bag of Words technique.</summary>
        public List<string> Tags { get; private set; }

        /// <summary>The publish date and time of the News article as a string.</summary>
        public string PublishDate
        {
            get
            {
                string date = PublishDateTime.ToString();
                return date.Substring(0, date.IndexOf(" "));
            }
        }

        #endregion

        /// <summary>Determines the Topic and content Tags for an article by analysing it using a Bag of Words technique.</summary>
        public void ClassifyArticle()
        {
            Tags = new List<string>();
            List<string> sentences = SplitSentences();

            // analyse article using Bag of Words technique
            TopicsList topicsList = new TopicsList("EDlib.GalNet.Resources.NewsBoW.json");
            AnalyseSentences(sentences, topicsList);

            // analyse article again to identify false positives
            TopicsList falseTopicsList = new TopicsList("EDlib.GalNet.Resources.NewsFalseBoW.json");
            AnalyseSentences(sentences, falseTopicsList);

            // subtract false positives from topics list
            foreach (Topic falseTopic in falseTopicsList.Topics)
            {
                Topic topic = topicsList.Topics.Find(x => x.Name.Equals(falseTopic.Name));
                topic.Count -= falseTopic.Count;
            }

            // select topic + tags
            Topic tempTopic = topicsList.Topics.OrderByDescending(o => o.Count).First();
            Topic = (tempTopic.Count < 2 || string.Equals(Title, "week in review", StringComparison.OrdinalIgnoreCase)) ? "Unclassified" : tempTopic.Name;
            foreach (Topic topic in topicsList.Topics.OrderByDescending(o => o.Count).Take(4))
            {
                if (topic.Count > 0)
                {
                    Tags.Add(topic.Name);
                }
            }
        }

        private void AnalyseSentences(List<string> sentences, TopicsList topicsList)
        {
            foreach (string sentence in sentences)
            {
                Parallel.ForEach(topicsList.Topics, topic =>
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
                Title.Trim().ToLower(),
                Slug.Replace("-", " ").Trim().ToLower()
            };
            foreach (string sentence in Regex.Split(Body, @"(?<=[\w\s](?:[\.\!\? ]+[\x20]*[\x22\xBB]*))(?:\s+(?![\x22\xBB](?!\w)))"))
            {
                sentences.Add(sentence.Trim().ToLower());
            }
            return sentences;
        }

        /// <summary>Returns the Title and Body of an article as a string.</summary>
        public override string ToString()
        {
            return String.Format("{0}: {1}", Title, Body);
        }
    }
}
