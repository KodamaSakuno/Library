using System;
using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop.Net
{
    public class Network : DisposableObject
    {
        NativeInterfaces.INetwork r_Network;

        public string Name
        {
            get { return r_Network.GetName(); }
            set { r_Network.SetName(value); }
        }
        public string Description
        {
            get { return r_Network.GetDescription(); }
            set { r_Network.SetDescription(value); }
        }

        public Guid ID => r_Network.GetNetworkId();

        public DomainType DomainType => r_Network.GetDomainType();

        public ConnectivityStates Connectivity => r_Network.GetConnectivity();

        public bool IsConnected => r_Network.IsConnected;
        public bool IsConnectedToInternet => r_Network.IsConnectedToInternet;

        public NetworkCategory Category
        {
            get { return r_Network.GetCategory(); }
            set { r_Network.SetCategory(value); }
        }

        public bool IsCaptivePortalDectected
        {
            get
            {
                string rPropertyName;

                var rConnectivity = Connectivity;
                if ((rConnectivity & ConnectivityStates.IPv4Internet) != 0 || (rConnectivity & ConnectivityStates.IPv4LocalNetwork) != 0)
                    rPropertyName = "NA_InternetConnectivityV4";
                else if ((rConnectivity & ConnectivityStates.IPv6Internet) != 0 || (rConnectivity & ConnectivityStates.IPv6LocalNetwork) != 0)
                    rPropertyName = "NA_InternetConnectivityV6";
                else
                    return false;

                using (var rVariant = new NativeStructs.PROPVARIANT())
                {
                    ((NativeInterfaces.IPropertyBag)r_Network).Read(rPropertyName, rVariant);
                    return ((NativeEnums.NLM_INTERNET_CONNECTIVITY)rVariant.Int32Value & NativeEnums.NLM_INTERNET_CONNECTIVITY.NLM_INTERNET_CONNECTIVITY_WEBHIJACK) != 0;
                }
            }
        }

        internal Network(NativeInterfaces.INetwork rpNetwork)
        {
            r_Network = rpNetwork;
        }

        protected override void DisposeNativeResources() => Marshal.ReleaseComObject(r_Network);
    }
}
