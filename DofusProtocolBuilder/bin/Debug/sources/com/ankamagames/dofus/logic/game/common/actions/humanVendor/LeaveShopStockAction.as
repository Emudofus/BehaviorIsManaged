package com.ankamagames.dofus.logic.game.common.actions.humanVendor
{

    public class LeaveShopStockAction extends Object implements Action
    {

        public function LeaveShopStockAction()
        {
            return;
        }// end function

        public static function create() : LeaveShopStockAction
        {
            var _loc_1:* = new LeaveShopStockAction;
            return _loc_1;
        }// end function

    }
}
