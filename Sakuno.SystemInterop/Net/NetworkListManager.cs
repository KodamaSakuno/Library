using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop.Net
{
    public static class NetworkListManager
    {
        static NativeInterfaces.INetworkListManager r_Manager;

        public static ConnectivityStates Connectivity => r_Manager.GetConnectivity();

        public static bool IsConnected => r_Manager.IsConnected;
        public static bool IsConnectedToInternet => r_Manager.IsConnectedToInternet;

        static NetworkListManagerEventSinks r_EventSinks;

        public static event Action<ConnectivityStates> ConnectivityChanged;

        public static event Action<Guid> NetworkAdded;
        public static event Action<Guid> NetworkRemoved;
        public static event Action<Guid, ConnectivityStates> NetworkConnectivityChanged;
        public static event Action<Guid, ChangedProperties> NetworkPropertyChanged;

        public static event Action<Guid, ConnectivityStates> NetworkConnectionConnectivityChanged;
        public static event Action<Guid, ChangedProperties> NetworkConnectionPropertyChanged;

        static NetworkListManager()
        {
            r_Manager = (NativeInterfaces.INetworkListManager)new NativeInterfaces.NetworkListManager();

            r_EventSinks = new NetworkListManagerEventSinks();

            var rConnectionPointContainer = (NativeInterfaces.IConnectionPointContainer)r_Manager;

            var rGuid = typeof(NativeInterfaces.INetworkListManagerEvents).GUID;
            r_EventSinks.NLMEventsCookie = rConnectionPointContainer.FindConnectionPoint(ref rGuid).Advise(r_EventSinks);

            rGuid = typeof(NativeInterfaces.INetworkEvents).GUID;
            r_EventSinks.NetworkEventsCookie = rConnectionPointContainer.FindConnectionPoint(ref rGuid).Advise(r_EventSinks);

            rGuid = typeof(NativeInterfaces.INetworkConnectionEvents).GUID;
            r_EventSinks.ConnectionEventsCookie = rConnectionPointContainer.FindConnectionPoint(ref rGuid).Advise(r_EventSinks);
        }

        public static Network GetNetwork(Guid rpID) => new Network(r_Manager.GetNetwork(rpID));
        public static NetworkConnection GetNetworkConnection(Guid rpID)
        {
            var rConnection = r_Manager.GetNetworkConnection(rpID);

            return rConnection != null ? new NetworkConnection(rConnection) : null;
        }

        public static IEnumerable<Network> GetNetworks(NetworkTypes rpTypes)
        {
            foreach (NativeInterfaces.INetwork rNetwork in r_Manager.GetNetworks(rpTypes))
                yield return new Network(rNetwork);
        }
        public static IEnumerable<NetworkConnection> GetNetworkConnections()
        {
            foreach (NativeInterfaces.INetworkConnection rConnection in r_Manager.GetNetworkConnections())
                yield return new NetworkConnection(rConnection);
        }

        internal static void OnConnectivityChanged(ConnectivityStates rpConnectivity) => ConnectivityChanged?.Invoke(rpConnectivity);

        internal static void OnNetworkAdded(Guid rpID) => NetworkAdded?.Invoke(rpID);
        internal static void OnNetworkRemoved(Guid rpID) => NetworkRemoved?.Invoke(rpID);
        internal static void OnNetworkConnectivityChanged(Guid rpID, ConnectivityStates rpConnectivity) => NetworkConnectivityChanged?.Invoke(rpID, rpConnectivity);
        internal static void OnNetworkPropertyChanged(Guid rpID, ChangedProperties rpProperties) => NetworkPropertyChanged?.Invoke(rpID, rpProperties);

        internal static void OnNetworkConnectionConnectivityChanged(Guid rpID, ConnectivityStates rpConnectivity) => NetworkConnectionConnectivityChanged?.Invoke(rpID, rpConnectivity);
        internal static void OnNetworkConnectionPropertyChanged(Guid rpID, ChangedProperties rpProperties) => NetworkConnectionPropertyChanged?.Invoke(rpID, rpProperties);
    }
}
