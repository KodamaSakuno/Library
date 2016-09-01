using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Sakuno.SystemInterop
{
    public static class ShellUtil
    {
        public static void InstallShortcutInStartScreen(string rpShortcutFilename, string rpShortcutTargetPath, string rpAppUserModelID)
        {
            if (!OS.IsWin8OrLater)
                return;

            if (rpShortcutFilename.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(rpShortcutFilename));
            if (rpShortcutTargetPath.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(rpShortcutTargetPath));
            if (rpAppUserModelID.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(rpAppUserModelID));

            NativeInterfaces.IShellLinkW rShellLink = null;
            IPersistFile rPersistFile = null;

            var rShortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), rpShortcutFilename);
            if (File.Exists(rShortcutPath))
            {
                rShellLink = (NativeInterfaces.IShellLinkW)new NativeInterfaces.CShellLink();
                rPersistFile = (IPersistFile)rShellLink;
                rPersistFile.Load(rShortcutPath, 0);

                var rBuffer = new StringBuilder(256);

                var rFindData = new NativeStructs.WIN32_FIND_DATAW();
                rShellLink.GetPath(rBuffer, rBuffer.Capacity, ref rFindData, NativeEnums.SLGP.SLGP_UNCPRIORITY);
                var rTargetPath = rBuffer.ToString();

                string rAppModelID;
                using (var rPropertyVariant = new NativeStructs.PROPVARIANT())
                {
                    var rPropertyStore = (NativeInterfaces.IPropertyStore)rShellLink;
                    var rAppUserModelIDPropertyKey = new NativeStructs.PROPERTYKEY(NativeGuids.PKEY_AppUserModel_ID, 5);
                    rPropertyStore.GetValue(ref rAppUserModelIDPropertyKey, rPropertyVariant);
                    rAppModelID = rPropertyVariant.StringValue;
                }

                Marshal.FinalReleaseComObject(rShellLink);

                if (rTargetPath.OICEquals(rpShortcutTargetPath) && rAppModelID.OICEquals(rpAppUserModelID))
                    return;
            }

            rShellLink = (NativeInterfaces.IShellLinkW)new NativeInterfaces.CShellLink();

            rShellLink.SetPath(rpShortcutTargetPath);

            using (var rPropertyVariant = new NativeStructs.PROPVARIANT(rpAppUserModelID))
            {
                var rPropertyStore = (NativeInterfaces.IPropertyStore)rShellLink;
                var rAppUserModelIDPropertyKey = new NativeStructs.PROPERTYKEY(NativeGuids.PKEY_AppUserModel_ID, 5);
                rPropertyStore.SetValue(ref rAppUserModelIDPropertyKey, rPropertyVariant);
                rPropertyStore.Commit();
            }

            rPersistFile = (IPersistFile)rShellLink;
            rPersistFile.Save(rShortcutPath, true);

            Marshal.FinalReleaseComObject(rShellLink);
        }
    }
}
