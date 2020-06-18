using EDlib.Platform;

namespace EDlib.Mock.Platform
{
    public class UnmeteredConnection : IConnectivityService
    {
        public bool IsConnected() => true;

        public bool IsMetered() => false;
    }
}
