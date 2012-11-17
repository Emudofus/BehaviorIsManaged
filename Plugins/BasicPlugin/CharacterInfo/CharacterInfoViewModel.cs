using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using BiM.Behaviors;
using BiM.Behaviors.Authentification;
using BiM.Behaviors.Frames;
using BiM.Behaviors.Game.Stats;
using BiM.Core.Cryptography;
using BiM.Core.Messages;
using BiM.Host.UI;
using BiM.Host.UI.Helpers;
using BiM.Host.UI.ViewModels;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;

namespace BasicPlugin.CharacterInfo
{
    public class CharacterStatsListRegister
    {
        [MessageHandler(typeof(CharacterStatsListMessage))]
        public static void HandleCharacterStatsListMessage(Bot bot, CharacterStatsListMessage message)
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
            get { return View; }
            set { View = (CharacterInfoView)value; }
        }

        public CharacterInfoView View
        {
            get;
            set;
        }
        
        #region UpgradeStats

        private DelegateCommand m_UpgradeStatCommand;

        public DelegateCommand UpgradeStatCommand
        {
            get { return m_UpgradeStatCommand ?? (m_UpgradeStatCommand = new DelegateCommand(OnUpgradeButtonClick)); }
        }

        private void OnUpgradeButtonClick(object Element)
        {
            if ((sbyte)Element < 0)
            return;
            Bot.SendToServer(new StatsUpgradeRequestMessage((sbyte)Element, 1));
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
            viewModel.RemoveDocument(View);
        }
    }
}
