package com.ankamagames.jerakine.cache
{

    public interface ICachable extends IDestroyable
    {

        public function ICachable();

        function set name(param1:String) : void;

        function get name() : String;

        function set inUse(param1:Boolean) : void;

        function get inUse() : Boolean;

    }
}
