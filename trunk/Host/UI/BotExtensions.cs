using BiM.Behaviors;
using BiM.Host.UI.ViewModels;

namespace BiM.Host.UI
{
    public static class BotExtensions
    {
        public static BotViewModel GetViewModel(this Bot bot)
        {
            return UIManager.Instance.GetBotViewModel(bot);
        }
    }
}