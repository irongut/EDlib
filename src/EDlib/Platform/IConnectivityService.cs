namespace EDlib.Platform
{
    public interface IConnectivityService
    {
        bool IsConnected();

        bool IsMetered();
    }
}
