using System;

namespace BiM.Host.Plugins
{
    public static class PluginExtensions
    {
        public static string GetDefaultDescription(this IPlugin plugin)
        {
            return string.Format("'{0}' v{1} by {2}", plugin.Name, plugin.GetType().Assembly.GetName().Version, plugin.Author);
        }
    }

    public interface IPlugin
    {
        PluginContext Context
        {
            get;
        }

        string Name
        {
            get;
        }

        string Description
        {
            get;
        }

        string Author
        {
            get;
        }

        Version Version
        {
            get;
        }

        void Initialize();
        void Shutdown();

        void Dispose();
    }
}