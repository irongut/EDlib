using System;
using System.Collections.Generic;

namespace EDlib.Platform
{
    [Flags]
    public enum CacheState
    {
        None = 0,
        Expired = 1,
        Active = 2
    }

    public interface ICacheService
    {
        void Add(string key, string data, TimeSpan expireIn);

        void EmptyAll();

        void EmptyExpired();

        bool Exists(string key);

        string Get(string key);

        DateTime? GetExpiration(string key);

        IEnumerable<string> GetKeys(CacheState state = CacheState.Active);

        bool IsExpired(string key);
    }
}
