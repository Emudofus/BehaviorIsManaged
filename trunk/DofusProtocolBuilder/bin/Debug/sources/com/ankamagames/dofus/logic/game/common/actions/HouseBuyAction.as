package com.ankamagames.dofus.logic.game.common.actions
{

    public class HouseBuyAction extends Object implements Action
    {
        public var proposedPrice:uint;

        public function HouseBuyAction()
        {
            return;
        }// end function

        public static function create(param1:uint) : HouseBuyAction
        {
            var _loc_2:* = new HouseBuyAction;
            _loc_2.proposedPrice = param1;
            return _loc_2;
        }// end function

    }
}
