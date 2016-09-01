namespace Sakuno.SystemInterop
{
    class AudioManagerEventSink : NativeInterfaces.IMMNotificationClient, NativeInterfaces.IAudioSessionNotification
    {
        internal AudioManagerEventSink() { }

        public void OnDefaultDeviceChanged(NativeConstants.DataFlow rpFlow,  NativeConstants.Role rpRole, string rpDefaultDeviceID)
        {
        }

        public void OnSessionCreated(NativeInterfaces.IAudioSessionControl rpSession) => AudioManager.OnSessionCreated(rpSession);

        public void OnDeviceAdded(string rpDeviceID, AudioDeviceState rpState) { }
        public void OnDeviceRemoved(string rpDeviceID, AudioDeviceState rpState) { }
        public void OnDeviceStateChanged(string rpDeviceID, AudioDeviceState rpState) { }
        public void OnPropertyValueChanged(string rpDeviceID, NativeStructs.PROPERTYKEY rpKey) { }
    }
}
