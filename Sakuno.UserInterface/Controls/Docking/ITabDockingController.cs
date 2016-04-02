namespace Sakuno.UserInterface.Controls.Docking
{
    public interface ITabDockingController
    {
        AdvancedTabControl CreateHost(AdvancedTabControl rpSourceTabControl, string rpSourcePartition);
    }
}
