package com.ankamagames.dofus.logic.game.common.actions.mount
{

    public class MountReleaseRequestAction extends Object implements Action
    {

        public function MountReleaseRequestAction()
        {
            return;
        }// end function

        public static function create() : MountReleaseRequestAction
        {
            return new MountReleaseRequestAction;
        }// end function

    }
}
