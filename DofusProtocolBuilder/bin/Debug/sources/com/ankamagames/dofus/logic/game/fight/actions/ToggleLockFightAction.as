package com.ankamagames.dofus.logic.game.fight.actions
{

    public class ToggleLockFightAction extends Object implements Action
    {

        public function ToggleLockFightAction()
        {
            return;
        }// end function

        public static function create() : ToggleLockFightAction
        {
            var _loc_1:* = new ToggleLockFightAction;
            return _loc_1;
        }// end function

    }
}
