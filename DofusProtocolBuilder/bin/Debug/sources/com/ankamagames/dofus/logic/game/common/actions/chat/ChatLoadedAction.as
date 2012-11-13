package com.ankamagames.dofus.logic.game.common.actions.chat
{

    public class ChatLoadedAction extends Object implements Action
    {

        public function ChatLoadedAction()
        {
            return;
        }// end function

        public static function create() : ChatLoadedAction
        {
            var _loc_1:* = new ChatLoadedAction;
            return _loc_1;
        }// end function

    }
}
