package com.ankamagames.dofus.logic.game.common.actions.spectator
{

    public class StopToListenRunningFightAction extends Object implements Action
    {

        public function StopToListenRunningFightAction()
        {
            return;
        }// end function

        public static function create() : StopToListenRunningFightAction
        {
            return new StopToListenRunningFightAction;
        }// end function

    }
}
