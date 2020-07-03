using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace EDlib.Common
{
    /// <summary>The list of topics for GalNet News articles and Community Goals.</summary>
    public class TopicsList
    {
        private readonly string BoWFilename;

        #region Properties

        /// <summary>The list of topics.</summary>
        /// <value>The list of topics.</value>
        public List<Topic> Topics { get; set; }

        #endregion

        /// <summary>Initializes a new instance of the <see cref="TopicsList" /> class and loads the topics from a json resource file.</summary>
        /// <param name="filename">The filename of the json resource file.</param>
        public TopicsList(string filename)
        {
            BoWFilename = filename;
            Topics = new List<Topic>();
            GetBagOfWords();
        }

        private void GetBagOfWords()
        {
            try
            {
                Assembly assembly = GetType().GetTypeInfo().Assembly;
                using (Stream stream = assembly.GetManifestResourceStream(BoWFilename))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        Topics = (List<Topic>)serializer.Deserialize(reader, typeof(List<Topic>));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to load Bag of Words for linguistic analysis.", ex);
            }
        }
    }
}
