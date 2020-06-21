using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace EDlib.Common
{
    public class TopicsList
    {
        private readonly string BoWFilename;

        #region Properties

        public List<Topic> Topics { get; set; }

        #endregion

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
