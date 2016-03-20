using Sakuno.SystemInterop;
using System.Windows;

namespace Sakuno.UserInterface.Controls
{
    public interface IWindowPlacementPreference
    {
        NativeStructs.WINDOWPLACEMENT? Load(Window rpWindow);

        void Save(Window rpWindow, NativeStructs.WINDOWPLACEMENT rpData);
    }
}
