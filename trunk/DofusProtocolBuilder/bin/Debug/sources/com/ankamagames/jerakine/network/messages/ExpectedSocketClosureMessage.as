package com.ankamagames.jerakine.network.messages
{

    public class ExpectedSocketClosureMessage extends Object implements Message, ILogableMessage
    {
        public var reason:uint;

        public function ExpectedSocketClosureMessage(param1:uint = 0)
        {
            this.reason = param1;
            return;
        }// end function

    }
}
