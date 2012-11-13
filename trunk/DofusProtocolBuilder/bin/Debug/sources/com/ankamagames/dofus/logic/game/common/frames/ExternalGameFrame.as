package com.ankamagames.dofus.logic.game.common.frames
{
    import com.ankamagames.berilia.managers.*;
    import com.ankamagames.dofus.kernel.net.*;
    import com.ankamagames.dofus.logic.game.common.actions.externalGame.*;
    import com.ankamagames.dofus.misc.lists.*;
    import com.ankamagames.dofus.network.messages.web.krosmaster.*;
    import com.ankamagames.jerakine.logger.*;
    import com.ankamagames.jerakine.messages.*;
    import com.ankamagames.jerakine.types.enums.*;
    import flash.utils.*;

    public class ExternalGameFrame extends Object implements Frame
    {
        static const _log:Logger = Log.getLogger(getQualifiedClassName(ExternalGameFrame));

        public function ExternalGameFrame()
        {
            return;
        }// end function

        public function get priority() : int
        {
            return Priority.NORMAL;
        }// end function

        public function pushed() : Boolean
        {
            return true;
        }// end function

        public function pulled() : Boolean
        {
            return true;
        }// end function

        public function process(param1:Message) : Boolean
        {
            var _loc_2:KrosmasterTokenRequestAction = null;
            var _loc_3:KrosmasterAuthTokenRequestMessage = null;
            var _loc_4:KrosmasterAuthTokenErrorMessage = null;
            var _loc_5:KrosmasterAuthTokenMessage = null;
            var _loc_6:KrosmasterInventoryRequestAction = null;
            var _loc_7:KrosmasterInventoryRequestMessage = null;
            var _loc_8:KrosmasterInventoryErrorMessage = null;
            var _loc_9:KrosmasterInventoryMessage = null;
            var _loc_10:KrosmasterTransferRequestAction = null;
            var _loc_11:KrosmasterTransferRequestMessage = null;
            var _loc_12:KrosmasterTransferMessage = null;
            var _loc_13:KrosmasterPlayingStatusAction = null;
            var _loc_14:KrosmasterPlayingStatusMessage = null;
            switch(true)
            {
                case param1 is KrosmasterTokenRequestAction:
                {
                    _loc_2 = param1 as KrosmasterTokenRequestAction;
                    _loc_3 = new KrosmasterAuthTokenRequestMessage();
                    _loc_3.initKrosmasterAuthTokenRequestMessage();
                    ConnectionsHandler.getConnection().send(_loc_3);
                    return true;
                }
                case param1 is KrosmasterAuthTokenErrorMessage:
                {
                    _loc_4 = param1 as KrosmasterAuthTokenErrorMessage;
                    KernelEventsManager.getInstance().processCallback(HookList.KrosmasterAuthTokenError, _loc_4.reason);
                    return true;
                }
                case param1 is KrosmasterAuthTokenMessage:
                {
                    _loc_5 = param1 as KrosmasterAuthTokenMessage;
                    KernelEventsManager.getInstance().processCallback(HookList.KrosmasterAuthToken, _loc_5.token);
                    return true;
                }
                case param1 is KrosmasterInventoryRequestAction:
                {
                    _loc_6 = param1 as KrosmasterInventoryRequestAction;
                    _loc_7 = new KrosmasterInventoryRequestMessage();
                    _loc_7.initKrosmasterInventoryRequestMessage();
                    ConnectionsHandler.getConnection().send(_loc_7);
                    return true;
                }
                case param1 is KrosmasterInventoryErrorMessage:
                {
                    _loc_8 = param1 as KrosmasterInventoryErrorMessage;
                    KernelEventsManager.getInstance().processCallback(HookList.KrosmasterInventoryError, _loc_8.reason);
                    return true;
                }
                case param1 is KrosmasterInventoryMessage:
                {
                    _loc_9 = param1 as KrosmasterInventoryMessage;
                    KernelEventsManager.getInstance().processCallback(HookList.KrosmasterInventory, _loc_9.figures);
                    return true;
                }
                case param1 is KrosmasterTransferRequestAction:
                {
                    _loc_10 = param1 as KrosmasterTransferRequestAction;
                    _loc_11 = new KrosmasterTransferRequestMessage();
                    _loc_11.initKrosmasterTransferRequestMessage(_loc_10.figureId);
                    ConnectionsHandler.getConnection().send(_loc_11);
                    return true;
                }
                case param1 is KrosmasterTransferMessage:
                {
                    _loc_12 = param1 as KrosmasterTransferMessage;
                    KernelEventsManager.getInstance().processCallback(HookList.KrosmasterTransfer, _loc_12.uid, _loc_12.failure);
                    return true;
                }
                case param1 is KrosmasterPlayingStatusAction:
                {
                    _loc_13 = param1 as KrosmasterPlayingStatusAction;
                    _loc_14 = new KrosmasterPlayingStatusMessage();
                    _loc_14.initKrosmasterPlayingStatusMessage(_loc_13.playing);
                    ConnectionsHandler.getConnection().send(_loc_14);
                    return true;
                }
                default:
                {
                    break;
                }
            }
            return false;
        }// end function

    }
}
