using System;
using System.Windows;

namespace Sakuno.UserInterface.Controls.Docking
{
    public interface ITabTearOffController
    {
        Tuple<AdvancedTabControl, Window> CreateHost(AdvancedTabControl rpSourceTabControl, string rpSourcePartition);

        TabEmptiedAction OnTabEmptied(AdvancedTabControl rpTabControl, Window rpWindow);
    }
}
