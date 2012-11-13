package com.ankamagames.berilia
{

    public interface FinalizableUIComponent extends UIComponent
    {

        public function FinalizableUIComponent();

        function get finalized() : Boolean;

        function set finalized(param1:Boolean) : void;

        function finalize() : void;

    }
}
