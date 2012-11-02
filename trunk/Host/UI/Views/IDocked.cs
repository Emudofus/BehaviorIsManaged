using AvalonDock.Layout;

namespace BiM.Host.UI.Views
{
    public interface IDocked
    {
        LayoutContent Parent
        {
            get;
            set;
        }
    }
}