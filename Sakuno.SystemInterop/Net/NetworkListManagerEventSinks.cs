using System;

namespace Sakuno.SystemInterop.Net
{
    class NetworkListManagerEventSinks : NativeInterfaces.INetworkListManagerEvents, NativeInterfaces.INetworkEvents, NativeInterfaces.INetworkConnectionEvents
    {
        public uint NLMEventsCookie { get; set; }
        public uint NetworkEventsCookie { get; set; }
        public uint ConnectionEventsCookie { get; set; }

        public void ConnectivityChanged(ConnectivityStates rpConnectivity) => NetworkListManager.OnConnectivityChanged(rpConnectivity);

        public void NetworkAdded(Guid rpID) => NetworkListManager.OnNetworkAdded(rpID);
        public void NetworkDeleted(Guid rpID) => NetworkListManager.OnNetworkRemoved(rpID);

        public void NetworkConnectivityChanged(Guid rpID, ConnectivityStates rpConnectivity) => NetworkListManager.OnNetworkConnectivityChanged(rpID, rpConnectivity);
        public void NetworkPropertyChanged(Guid rpID, ChangedProperties rpProperties) => NetworkListManager.OnNetworkPropertyChanged(rpID, rpProperties);

        public void NetworkConnectionConnectivityChanged(Guid rpID, ConnectivityStates rpConnectivity) => NetworkListManager.OnNetworkConnectionConnectivityChanged(rpID, rpConnectivity);
        public void NetworkConnectionPropertyChanged(Guid rpID, ChangedProperties rpProperties) => NetworkListManager.OnNetworkConnectionPropertyChanged(rpID, rpProperties);
    }
}
