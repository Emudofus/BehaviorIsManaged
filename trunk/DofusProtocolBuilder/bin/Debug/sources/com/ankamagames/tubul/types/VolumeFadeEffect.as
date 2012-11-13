package com.ankamagames.tubul.types
{
    import com.ankamagames.jerakine.logger.*;
    import com.ankamagames.tubul.events.*;
    import com.ankamagames.tubul.interfaces.*;
    import com.ankamagames.tubul.types.bus.*;
    import com.ankamagames.tubul.types.sounds.*;
    import flash.events.*;
    import flash.utils.*;
    import gs.*;
    import gs.easing.*;

    public class VolumeFadeEffect extends EventDispatcher
    {
        private var _running:Boolean = false;
        private var _beginningValue:Number;
        private var _endingValue:Number;
        private var _timeFade:Number;
        private var _soundSource:ISoundController;
        private var _tween:TweenMax;
        static const _log:Logger = Log.getLogger(getQualifiedClassName(VolumeFadeEffect));

        public function VolumeFadeEffect(param1:Number = 0, param2:Number = 1, param3:Number = 0)
        {
            this._beginningValue = param1;
            this._endingValue = param2;
            this._timeFade = param3;
            return;
        }// end function

        public function get running() : Boolean
        {
            return this._running;
        }// end function

        public function get beginningValue() : Number
        {
            return this._beginningValue;
        }// end function

        public function get endingValue() : Number
        {
            return this._endingValue;
        }// end function

        public function get timeFade() : Number
        {
            return this._timeFade;
        }// end function

        private function get soundSource() : ISoundController
        {
            return this._soundSource;
        }// end function

        public function attachToSoundSource(param1:ISoundController) : void
        {
            this._soundSource = param1;
            return;
        }// end function

        public function start(param1:Boolean = true) : void
        {
            var _loc_2:String = null;
            if (this.soundSource == null)
            {
                _log.warn("L\'effet de fade ne peut être lancé car le son auquel il est attaché ne peut être trouvé");
                return;
            }
            if (this._endingValue < 0 || this._endingValue > 1)
            {
                _log.warn("Le paramètre \'endingValue\' n\'est pas valide !");
                return;
            }
            if (this.soundSource is AudioBus)
            {
                _loc_2 = "Fade sur le bus " + (this.soundSource as AudioBus).name;
            }
            if (this.soundSource is MP3SoundDofus)
            {
                _loc_2 = "Fade sur le son " + (this.soundSource as MP3SoundDofus).id + "(" + (this.soundSource as MP3SoundDofus).uri.fileName + ")";
            }
            _log.warn(_loc_2 + " / => " + this._endingValue + " en " + this._timeFade + " sec.");
            this.clearTween();
            if (param1 && this._beginningValue >= 0)
            {
                this.soundSource.currentFadeVolume = this._beginningValue;
            }
            this._tween = new TweenMax(this.soundSource, this._timeFade, {currentFadeVolume:this._endingValue, onComplete:this.onFadeEnd, ease:Linear.easeNone});
            this._running = true;
            return;
        }// end function

        public function stop() : void
        {
            this.clearTween();
            this.onFadeEnd();
            return;
        }// end function

        public function reset(param1:Number, param2:Number, param3:Number) : void
        {
            this.clearTween();
            this._beginningValue = param1;
            this._endingValue = param2;
            this._timeFade = param3;
            return;
        }// end function

        public function clone() : VolumeFadeEffect
        {
            var _loc_1:* = new VolumeFadeEffect(this._beginningValue, this._endingValue, this._timeFade);
            return _loc_1;
        }// end function

        private function clearTween() : void
        {
            if (this._tween)
            {
                this._tween.clear();
                this._tween = null;
            }
            this._running = false;
            return;
        }// end function

        private function onFadeEnd() : void
        {
            var _loc_1:* = new FadeEvent(FadeEvent.COMPLETE);
            _loc_1.soundSource = this.soundSource;
            dispatchEvent(_loc_1);
            this.clearTween();
            return;
        }// end function

    }
}
