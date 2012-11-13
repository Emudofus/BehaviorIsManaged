package com.ankamagames.dofus.logic.game.common.actions.guild
{

    public class GuildCharacsUpgradeRequestAction extends Object implements Action
    {
        public var charaTypeTarget:uint;

        public function GuildCharacsUpgradeRequestAction()
        {
            return;
        }// end function

        public static function create(param1:uint) : GuildCharacsUpgradeRequestAction
        {
            var _loc_2:* = new GuildCharacsUpgradeRequestAction;
            _loc_2.charaTypeTarget = param1;
            return _loc_2;
        }// end function

    }
}
