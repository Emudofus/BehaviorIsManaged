using System.Collections.Generic;
using System.ComponentModel;
using AvalonDock.Layout;
using BiM.Behaviors;
using BiM.Behaviors.Frames;
using BiM.Behaviors.Game.Stats;
using BiM.Core.Messages;
using BiM.Host.UI;
using BiM.Host.UI.Helpers;
using BiM.Host.UI.ViewModels;
using BiM.Protocol.Messages;

namespace BasicPlugin.CharacterInfo
{
    public class CharacterStatsListRegister
    {
        [MessageHandler(typeof (CharacterSelectedSuccessMessage))]
        public static void HandleCharacterSelectedSuccessMessage(Bot bot, CharacterSelectedSuccessMessage message)
        {
            bot.AddFrame(new CharacterInfoViewModel(bot));
        }
    }

    public class CharacterInfoViewModel : Frame<CharacterInfoViewModel>, IViewModel<CharacterInfoView>
    {
        public CharacterInfoViewModel(Bot bot)
            : base(bot)
        {
        }

        public CharacterInfoView ViewModel
        {
            get;
            set;
        }

        #region UpgradeStats

        private DelegateCommand m_upgradeStatCommand;

        public DelegateCommand UpgradeStatCommand
        {
            get { return m_upgradeStatCommand ?? (m_upgradeStatCommand = new DelegateCommand(OnUpgradeButtonClick)); }
        }

        private void OnUpgradeButtonClick(object stat)
        {
            if (stat == null)
                return;

            Bot.Character.SpendStatsPoints((BoostableStat)stat, Bot.Character.GetPointsForBoostAmount((BoostableStat) stat, 1));
        }

        #endregion

        #region IViewModel<CharacterInfoView> Members

        public event PropertyChangedEventHandler PropertyChanged;

        object IViewModel.View
        {
            get { return ViewModel; }
            set { ViewModel = (CharacterInfoView) value; }
        }

        public CharacterInfoView View
        {
            get;
            set;
        }

        #endregion

        public override void OnAttached()
        {
            base.OnAttached();
            BotViewModel botViewModel = Bot.GetViewModel();
            LayoutDocument layout = botViewModel.AddDocument(this, () => new CharacterInfoView());
            layout.Title = "Character Info";
            layout.CanClose = false;
        }

        public override void OnDetached()
        {
            base.OnDetached();

            BotViewModel viewModel = Bot.GetViewModel();
            viewModel.RemoveDocument(ViewModel);
        }
    }
}