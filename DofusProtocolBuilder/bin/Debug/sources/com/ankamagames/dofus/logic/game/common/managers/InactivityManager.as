package com.ankamagames.dofus.logic.game.common.managers
{
    import com.ankamagames.berilia.managers.*;
    import com.ankamagames.dofus.kernel.net.*;
    import com.ankamagames.dofus.misc.lists.*;
    import com.ankamagames.dofus.network.messages.common.basic.*;
    import com.ankamagames.jerakine.logger.*;
    import com.ankamagames.jerakine.utils.display.*;
    import flash.events.*;
    import flash.utils.*;

    public class InactivityManager extends Object
    {
        private var _isAfk:Boolean;
        private var _activityTimer:Timer;
        private var _serverActivityTimer:Timer;
        private var _hasActivity:Boolean = false;
        private static var _self:InactivityManager;
        static const _log:Logger = Log.getLogger(getQualifiedClassName(InactivityManager));
        private static const SERVER_INACTIVITY_DELAY:int = 600000;
        private static const INACTIVITY_DELAY:int = 1.2e+006;

        public function InactivityManager()
        {
            _log.info("Initialisation du manager d\'inactivité");
            this._activityTimer = new Timer(INACTIVITY_DELAY);
            this._activityTimer.addEventListener(TimerEvent.TIMER, this.onActivityTimerUp);
            this._serverActivityTimer = new Timer(SERVER_INACTIVITY_DELAY, 0);
            this._serverActivityTimer.addEventListener(TimerEvent.TIMER, this.onServerActivityTimerUp);
            this.resetActivity();
            this.resetServerActivity();
            return;
        }// end function

        public function start() : void
        {
            _log.info("Démarage du manager d\'inactivité");
            this.resetActivity();
            this.resetServerActivity();
            StageShareManager.stage.addEventListener(KeyboardEvent.KEY_DOWN, this.onActivity);
            StageShareManager.stage.addEventListener(MouseEvent.CLICK, this.onActivity);
            StageShareManager.stage.addEventListener(MouseEvent.MOUSE_MOVE, this.onActivity);
            this._isAfk = false;
            return;
        }// end function

        public function stop() : void
        {
            _log.info("Arret du manager d\'inactivité");
            this._activityTimer.stop();
            this._serverActivityTimer.stop();
            StageShareManager.stage.removeEventListener(KeyboardEvent.KEY_DOWN, this.onActivity);
            StageShareManager.stage.removeEventListener(MouseEvent.CLICK, this.onActivity);
            StageShareManager.stage.removeEventListener(MouseEvent.MOUSE_MOVE, this.onActivity);
            return;
        }// end function

        public function resetActivity() : void
        {
            this._activityTimer.reset();
            this._activityTimer.start();
            return;
        }// end function

        public function resetServerActivity() : void
        {
            this._serverActivityTimer.reset();
            this._serverActivityTimer.start();
            return;
        }// end function

        public function activity() : void
        {
            this.resetActivity();
            this._hasActivity = true;
            if (this._isAfk)
            {
                this._isAfk = false;
                KernelEventsManager.getInstance().processCallback(HookList.InactivityNotification, false);
            }
            return;
        }// end function

        private function onActivity(event:Event) : void
        {
            this.activity();
            return;
        }// end function

        private function onActivityTimerUp(event:Event) : void
        {
            this._isAfk = true;
            _log.info("Le timer d\'activité à expiré. Dispatch de callback de notification d\'inactivité");
            KernelEventsManager.getInstance().processCallback(HookList.InactivityNotification, true);
            return;
        }// end function

        private function onServerActivityTimerUp(event:Event) : void
        {
            if (this._hasActivity)
            {
                this._hasActivity = false;
                serverNotification();
            }
            return;
        }// end function

        public static function getInstance() : InactivityManager
        {
            if (!_self)
            {
                _self = new InactivityManager;
            }
            return _self;
        }// end function

        private static function serverNotification() : void
        {
            var _loc_1:BasicPingMessage = null;
            if (ConnectionsHandler.getConnection().connected)
            {
                _loc_1 = new BasicPingMessage();
                _loc_1.quiet = true;
                ConnectionsHandler.getConnection().send(_loc_1);
            }
            return;
        }// end function

    }
}
