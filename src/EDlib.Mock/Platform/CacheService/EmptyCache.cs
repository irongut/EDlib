using EDlib.Platform;
using System;
using System.Collections.Generic;

namespace EDlib.Mock.Platform
{
    public class EmptyCache : ICacheService
    {
        public void Add(string key, string data, TimeSpan expireIn) { }

        public void Delete(params string[] key) { }

        public void EmptyAll() { }

        public void EmptyExpired() { }

        public bool Exists(string key) => false;

        public string Get(string key) => string.Empty;

        public DateTime? GetExpiration(string key) => DateTime.Now.AddDays(-1);

        public IEnumerable<string> GetKeys(CacheState state = CacheState.Active) => null;

        public bool IsExpired(string key) => true;
    }
}
