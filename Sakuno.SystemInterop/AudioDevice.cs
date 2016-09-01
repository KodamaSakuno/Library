using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop
{
    public class AudioDevice : DisposableObject
    {
        NativeInterfaces.IMMDevice r_Device;

        public string ID => r_Device.GetId();

        public AudioDeviceState State => r_Device.GetState();

        public string Name { get; }

        internal AudioDevice(NativeInterfaces.IMMDevice rpDevice)
        {
            r_Device = rpDevice;

            var rProperties = r_Device.OpenPropertyStore(NativeConstants.STGM.STGM_READ);
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
