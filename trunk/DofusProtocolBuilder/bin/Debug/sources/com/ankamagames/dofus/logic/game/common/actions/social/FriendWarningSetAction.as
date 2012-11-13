package com.ankamagames.dofus.logic.game.common.actions.social
{

    public class FriendWarningSetAction extends Object implements Action
    {
        public var enable:Boolean;

        public function FriendWarningSetAction()
        {
            return;
        }// end function

        public static function create(param1:Boolean) : FriendWarningSetAction
        {
            var _loc_2:* = new FriendWarningSetAction;
            _loc_2.enable = param1;
            return _loc_2;
        }// end function

    }
}
