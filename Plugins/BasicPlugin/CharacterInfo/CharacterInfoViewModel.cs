using System.ComponentModel;
using System.Collections.Generic;
using BiM.Behaviors;
using BiM.Behaviors.Frames;
using BiM.Core.Messages;
using BiM.Host.UI;
using BiM.Host.UI.Helpers;
using BiM.Host.UI.ViewModels;
using BiM.Protocol.Messages;

namespace BasicPlugin.CharacterInfo
{
    public class CharacterStatsListRegister
    {
        [MessageHandler(typeof(CharacterSelectedSuccessMessage))]
        public static void HandleCharacterSelectedSuccessMessage(Bot bot, CharacterSelectedSuccessMessage message)
        {
            bot.AddFrame(new CharacterInfoViewModel(bot));
        }
    }

    public class CharacterInfoViewModel : Frame<CharacterInfoViewModel>, IViewModel<CharacterInfoView>
    {
     

        public CharacterInfoViewModel(Bot bot)
            :base(bot)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        object IViewModel.View
        {
            get { return ViewModel; }
            set { ViewModel = (CharacterInfoView)value; }
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

        private void OnUpgradeButtonClick(object element)
        {
            if ((sbyte)element < 0)
            return;
            
            List<uint> threshold = StatsTreshold.GetThreshold((short)Bot.Character.Stats.StatsPoints, (sbyte)element, Bot.Character.Breed);
            Bot.SendToServer(new StatsUpgradeRequestMessage((sbyte)element, (short)threshold[1]));
        }

        #endregion

        public override void OnAttached()
        {
            base.OnAttached();
            var botViewModel = Bot.GetViewModel();
            var layout = botViewModel.AddDocument(this, () => new CharacterInfoView());
            layout.Title = "Character Info";
            layout.CanClose = false;
        }

        public override void OnDetached()
        {
            base.OnDetached();

            var viewModel = Bot.GetViewModel();
            viewModel.RemoveDocument(ViewModel);
        }

        public CharacterInfoView View
        {
            get;
            set;
        }
    }
}
