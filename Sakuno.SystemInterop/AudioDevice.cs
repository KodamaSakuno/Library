using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop
{
    public class AudioDevice : DisposableObject
    {
        NativeInterfaces.IMMDevice r_Device;

        public string ID
        {
            get
            {
                string rResult;
                Marshal.ThrowExceptionForHR(r_Device.GetId(out rResult));
                return rResult;
            }
        }

        public AudioDeviceState State
        {
            get
            {
                AudioDeviceState rResult;
                Marshal.ThrowExceptionForHR(r_Device.GetState(out rResult));
                return rResult;
            }
        }

        public string Name { get; }

        internal AudioDevice(NativeInterfaces.IMMDevice rpDevice)
        {
            r_Device = rpDevice;

            NativeInterfaces.IPropertyStore rProperties;
            Marshal.ThrowExceptionForHR(r_Device.OpenPropertyStore(NativeConstants.STGM.STGM_READ, out rProperties));

            var rPropertyKey = new NativeStructs.PROPERTYKEY(NativeGuids.PKEY_Device_FriendlyName, 14);
            using (var rPropertyVariant = new NativeStructs.PROPVARIANT())
            {
                rProperties.GetValue(ref rPropertyKey, rPropertyVariant);
                Name = rPropertyVariant.StringValue;
            }

            Marshal.ReleaseComObject(rProperties);
        }

        protected override void DisposeManagedResources()
        {
            Marshal.ReleaseComObject(r_Device);
        }
    }
}
