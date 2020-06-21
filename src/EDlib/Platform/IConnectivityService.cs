namespace EDlib.Platform
{
    /// <summary>Interface for a platform specific data connectivity service.</summary>
    public interface IConnectivityService
    {
        /// <summary>Determines whether a network is available.</summary>
        /// <returns><c>true</c> if a network connection is detected, else <c>false</c>.</returns>
        bool IsConnected();

        /// <summary>Determines whether the network connection is metered.</summary>
        /// <returns><c>true</c> if the connection is metered, else <c>false</c>.</returns>
        bool IsMetered();
    }
}
