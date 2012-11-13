package com.ankamagames.dofus.logic.game.common.actions
{

    public class OpenMapAction extends Object implements Action
    {
        public var conquest:Boolean;

        public function OpenMapAction()
        {
            return;
        }// end function

        public static function create(param1:Boolean = false) : OpenMapAction
        {
            var _loc_2:* = new OpenMapAction;
            _loc_2.conquest = param1;
            return _loc_2;
        }// end function

    }
}
