package com.ankamagames.dofus.logic.game.common.actions.mount
{

    public class LeaveExchangeMountAction extends Object implements Action
    {

        public function LeaveExchangeMountAction()
        {
            return;
        }// end function

        public static function create() : LeaveExchangeMountAction
        {
            return new LeaveExchangeMountAction;
        }// end function

    }
}
