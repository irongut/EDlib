using System.Collections.Generic;

namespace EDlib.Common
{
    /// <summary>Topic for GalNet News articles and Community Goals.</summary>
    public class Topic
    {
        #region Properties

        /// <summary>The topic name.</summary>
        /// <value>The topic name.</value>
        public string Name { get; }
        /// <summary>The words used to determine if the topic is relevant to the content.</summary>
        /// <value>The list of words.</value>
        public List<string> Terms { get; }
        /// <summary>The number of times the terms were found in the content.</summary>
        /// <value>The number of times the terms were found.</value>
        public int Count { get; set; }

        #endregion

        /// <summary>Initializes a new instance of the <see cref="Topic" /> class.</summary>
        /// <param name="name">The topic name.</param>
        /// <param name="terms">The words used to determine if the topic is relevant to the content.</param>
        public Topic(string name, List<string> terms)
        {
            Name = name;
            Terms = terms;
            Count = 0;
        }
    }
}
