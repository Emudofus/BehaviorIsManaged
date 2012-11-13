package com.ankamagames.dofus.logic.game.common.actions.taxCollector
{

    public class GameRolePlayTaxCollectorFightRequestAction extends Object implements Action
    {
        public var taxCollectorId:uint;

        public function GameRolePlayTaxCollectorFightRequestAction()
        {
            return;
        }// end function

        public static function create(param1:uint) : GameRolePlayTaxCollectorFightRequestAction
        {
            var _loc_2:* = new GameRolePlayTaxCollectorFightRequestAction;
            _loc_2.taxCollectorId = param1;
            return _loc_2;
        }// end function

    }
}
