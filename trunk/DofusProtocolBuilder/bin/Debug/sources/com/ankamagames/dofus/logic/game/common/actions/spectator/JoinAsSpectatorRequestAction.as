package com.ankamagames.dofus.logic.game.common.actions.spectator
{

    public class JoinAsSpectatorRequestAction extends Object implements Action
    {
        public var fightId:uint;

        public function JoinAsSpectatorRequestAction()
        {
            return;
        }// end function

        public static function create(param1:uint) : JoinAsSpectatorRequestAction
        {
            var _loc_2:* = new JoinAsSpectatorRequestAction;
            _loc_2.fightId = param1;
            return _loc_2;
        }// end function

    }
}
