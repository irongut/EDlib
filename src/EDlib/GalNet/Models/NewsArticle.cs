using EDlib.Common;
using EDlib.Platform;
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
        /// <value>The article title.</value>
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
        /// <summary>The content of the News article.</summary>
        /// <value>The article content.</value>
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

        /// <summary>The publish date and time of the News article.</summary>
        /// <value>When the article was published.</value>
        [JsonProperty(PropertyName = "date")]
        public DateTime PublishDateTime { get; internal set; }

        /// <summary>Frontier Development's Id for the News article.</summary>
        /// <value>Article ID.</value>
        [JsonProperty(PropertyName = "nid")]
        public int Id { get; internal set; }

        /// <summary>
        ///   <para>
        ///  A value used by Frontier Developments to display an image with each News article.
        ///  Note: This is not a url or filename.
        ///   </para>
        /// </summary>
        /// <value>Article image name.</value>
        [JsonProperty(PropertyName = "image")]
        public string FDImageName { get; internal set; }

        /// <summary>A normalised version of the article title used as a unique identifier.</summary>
        /// <value>The article slug.</value>
        [JsonProperty(PropertyName = "slug")]
        public string Slug { get; internal set; }

        /// <summary>The topic of the News article, generated using a Bag of Words technique.</summary>
        /// <value>The article topic.</value>
        public string Topic { get; private set; }

        /// <summary>Content tags for the News article, generated using a Bag of Words technique.</summary>
        /// <value>Content tags for the article.</value>
        public List<string> Tags { get; private set; }

        /// <summary>The publish date and time of the News article as a string.</summary>
        /// <value>When the article was published.</value>
        [Preserve(Conditional = true)]
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
        /// <param name="topics">List of <see cref="Topic" /> to use when classifying article.</param>
        /// <param name="ignoreTopics">List of <see cref="Topic" /> to ignore when classifying article.</param>
        public void ClassifyArticle(List<Topic> topics, List<Topic> ignoreTopics)
        {
            Tags = new List<string>();
            List<string> sentences = SplitSentences();

            // analyse article using Bag of Words technique
            AnalyseSentences(sentences, topics);

            // analyse article again to identify false positives
            AnalyseSentences(sentences, ignoreTopics);

            // subtract false positives from topics list
            foreach (Topic falseTopic in ignoreTopics)
            {
                Topic topic = topics.Find(x => x.Name.Equals(falseTopic.Name));
                topic.Count -= falseTopic.Count;
            }

            // select topic + tags
            Topic tempTopic = topics.OrderByDescending(o => o.Count).First();
            Topic = (tempTopic.Count < 2 || string.Equals(Title, "week in review", StringComparison.OrdinalIgnoreCase)) ? "Unclassified" : tempTopic.Name;
            foreach (Topic topic in topics.OrderByDescending(o => o.Count).Take(4))
            {
                if (topic.Count > 0)
                {
                    Tags.Add(topic.Name);
                }
            }
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
        [Preserve(Conditional = true)]
        public override string ToString()
        {
            return String.Format("{0}: {1}", Title, Body);
        }
    }
}
