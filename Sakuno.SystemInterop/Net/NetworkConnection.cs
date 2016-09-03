using System;
using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop.Net
{
    public class NetworkConnection : DisposableObject
    {
        NativeInterfaces.INetworkConnection r_Connection;

        public Network Network => new Network(r_Connection.GetNetwork());

        public Guid ID => r_Connection.GetConnectionId();
        public Guid AdapterID => r_Connection.GetAdapterId();

        public DomainType DomainType => r_Connection.GetDomainType();

        public ConnectivityStates Connectivity => r_Connection.GetConnectivity();

        public bool IsConnected => r_Connection.IsConnected;
        public bool IsConnectedToInternet => r_Connection.IsConnectedToInternet;

        internal NetworkConnection(NativeInterfaces.INetworkConnection rpConnection)
        {
            r_Connection = rpConnection;
        }

        protected override void DisposeNativeResources() => Marshal.ReleaseComObject(r_Connection);
    }
}
