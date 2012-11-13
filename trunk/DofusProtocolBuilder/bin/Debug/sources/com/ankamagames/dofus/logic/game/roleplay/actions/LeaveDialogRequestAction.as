package com.ankamagames.dofus.logic.game.roleplay.actions
{

    public class LeaveDialogRequestAction extends Object implements Action
    {

        public function LeaveDialogRequestAction()
        {
            return;
        }// end function

        public static function create() : LeaveDialogRequestAction
        {
            return new LeaveDialogRequestAction;
        }// end function

    }
}
