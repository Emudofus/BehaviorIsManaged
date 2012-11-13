package com.ankamagames.jerakine.messages
{

    public interface Frame extends MessageHandler, Prioritizable
    {

        public function Frame();

        function pushed() : Boolean;

        function pulled() : Boolean;

    }
}
