package com.ankamagames.dofus.logic.game.fight.actions
{

    public class ShowAllNamesAction extends Object implements Action
    {

        public function ShowAllNamesAction()
        {
            return;
        }// end function

        public static function create() : ShowAllNamesAction
        {
            var _loc_1:* = new ShowAllNamesAction;
            return _loc_1;
        }// end function

    }
}
