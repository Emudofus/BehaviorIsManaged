package com.ankamagames.dofus.logic.common.actions
{

    public class EmptyStackAction extends Object implements Action
    {

        public function EmptyStackAction()
        {
            return;
        }// end function

        public static function create() : EmptyStackAction
        {
            return new EmptyStackAction;
        }// end function

    }
}
