package com.ankamagames.dofus.logic.game.approach.actions
{

    public class CharacterDeselectionAction extends Object implements Action
    {

        public function CharacterDeselectionAction()
        {
            return;
        }// end function

        public static function create() : CharacterDeselectionAction
        {
            var _loc_1:* = new CharacterDeselectionAction;
            return _loc_1;
        }// end function

    }
}
