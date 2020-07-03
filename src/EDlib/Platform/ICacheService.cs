using System;
using System.Collections.Generic;

namespace EDlib.Platform
{
    /// <summary>Current state of the item in the cache.</summary>
    [Flags]
    public enum CacheState
    {
        /// <summary>An unknown state for the cache item.</summary>
        None = 0,
        /// <summary>Expired cache item.</summary>
        Expired = 1,
        /// <summary>Active non-expired cache item.</summary>
        Active = 2
    }

    /// <summary>Interface for a platform specific data caching service.</summary>
    public interface ICacheService
    {
        /// <summary>Add an item to the cache.</summary>
        /// <param name="key">Key to identify the cached item.</param>
        /// <param name="data">String data to store in the cache.</param>
        /// <param name="expireIn">How long in the future the item should expire.</param>
        void Add(string key, string data, TimeSpan expireIn);

        /// <summary>Empty all items from the cache.</summary>
        void EmptyAll();

        /// <summary>Empty expired items from the cache.</summary>
        void EmptyExpired();

        /// <summary>Checks to see if an item exists in the cache.</summary>
        /// <param name="key">Key to identify the cached item.</param>
        /// <returns><c>true</c> if the item exists, else <c>false</c>.</returns>
        bool Exists(string key);

        /// <summary>Gets a cached item.</summary>
        /// <param name="key">Key to identify the cached item.</param>
        /// <returns>The cached data string if it exists, else <c>null</c>.</returns>
        string Get(string key);

        /// <summary>Gets the expiration date and time for a cached item.</summary>
        /// <param name="key">Key to identify the cached item.</param>
        /// <returns>The date if the cached item exists, else <c>null</c>.</returns>
        DateTime? GetExpiration(string key);

        /// <summary>Gets keys for any cached items with the specified state.</summary>
        /// <param name="state">State to get: Multiple with flags: CacheState.Active | CacheState.Expired</param>
        /// <returns>The relevant keys.</returns>
        IEnumerable<string> GetKeys(CacheState state = CacheState.Active);

        /// <summary>Checks if a cached item is expired.</summary>
        /// <param name="key">Key to identify the cached item.</param>
        /// <returns><c>true</c> if expired, else <c>false</c>.</returns>
        bool IsExpired(string key);
    }
}
