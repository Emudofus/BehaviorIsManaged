package com.ankamagames.dofus.logic.game.approach.managers
{
    import __AS3__.vec.*;
    import com.ankamagames.berilia.managers.*;
    import com.ankamagames.dofus.datacenter.misc.*;
    import com.ankamagames.dofus.kernel.updater.*;
    import com.ankamagames.dofus.logic.game.approach.utils.*;
    import com.ankamagames.dofus.misc.lists.*;
    import com.ankamagames.dofus.network.enums.*;
    import com.ankamagames.dofus.network.messages.updater.parts.*;
    import com.ankamagames.dofus.network.types.updater.*;
    import com.ankamagames.jerakine.logger.*;
    import flash.utils.*;

    public class PartManager extends Object
    {
        private var _parts:Dictionary = null;
        private var _firstParts:Dictionary = null;
        private var _downloadList:Array;
        private var _downloadingPart:String = null;
        private var _downloadCount:int = 0;
        private var _downloadSuccess:int = 0;
        private var _state:int = 0;
        public static const STATE_WAITING:int = 0;
        public static const STATE_DOWNLOADING:int = 1;
        public static const STATE_FINISHED:int = 2;
        static const _log:Logger = Log.getLogger(getQualifiedClassName(PartManager));
        private static var _singleton:PartManager;

        public function PartManager() : void
        {
            this._downloadList = new Array();
            DownloadMonitoring.getInstance().initialize();
            return;
        }// end function

        public function initialize() : void
        {
            var _loc_1:* = new GetPartsListMessage();
            _loc_1.initGetPartsListMessage();
            UpdaterConnexionHandler.getConnection().send(_loc_1);
            return;
        }// end function

        public function receiveParts(param1:Vector.<ContentPart>) : void
        {
            var _loc_2:ContentPart = null;
            var _loc_3:String = null;
            this._parts = new Dictionary();
            for each (_loc_2 in param1)
            {
                
                this.updatePart(_loc_2);
            }
            if (!this._firstParts)
            {
                this._firstParts = new Dictionary();
                for (_loc_3 in this._parts)
                {
                    
                    this._firstParts[_loc_3] = this._parts[_loc_3];
                }
            }
            return;
        }// end function

        public function checkAndDownload(param1:String) : void
        {
            var _loc_2:String = null;
            if (!this._parts)
            {
                _log.warn("checkAndDownload \'" + param1 + "\' but can\'t got part list (updater is down ?)");
                return;
            }
            if (!this._parts.hasOwnProperty(param1))
            {
                _log.error("Unknow part id : " + param1);
                return;
            }
            if (this._parts[param1].state == PartStateEnum.PART_NOT_INSTALLED)
            {
                for each (_loc_2 in this._downloadList)
                {
                    
                    if (_loc_2 == param1)
                    {
                        return;
                    }
                }
                var _loc_3:String = this;
                var _loc_4:* = this._downloadCount + 1;
                _loc_3._downloadCount = _loc_4;
                this.download(param1);
            }
            return;
        }// end function

        public function updatePart(param1:ContentPart) : void
        {
            var _loc_3:Boolean = false;
            var _loc_4:ContentPart = null;
            var _loc_5:String = null;
            if (!this._parts)
            {
                _log.error("updatePart \'" + param1.id + "\' but can\'t got part liste (updater is down ?)");
                return;
            }
            var _loc_2:* = this._parts[param1.id];
            this._parts[param1.id] = param1;
            switch(param1.state)
            {
                case PartStateEnum.PART_BEING_UPDATER:
                {
                    DownloadMonitoring.getInstance().start();
                    if (param1.id != this._downloadingPart)
                    {
                        if (this._downloadingPart)
                        {
                            _log.error("On reçoit des informations de téléchargement d\'une partie de contenu " + param1.id + ", alors qu\'on a pour demande de récupérer " + this._downloadingPart + ". Ce téléchargement risque de provoquer un conflit (téléchargements simultanés");
                        }
                        else
                        {
                            this._downloadingPart = param1.id;
                        }
                    }
                    break;
                }
                case PartStateEnum.PART_UP_TO_DATE:
                {
                    if (param1.id == this._downloadingPart)
                    {
                        _loc_3 = false;
                        for each (_loc_4 in this._parts)
                        {
                            
                            if (_loc_4.state == PartStateEnum.PART_BEING_UPDATER)
                            {
                                _loc_3 = true;
                                _log.error(_loc_4.id + " en cours de téléchargement alors qu\'une autre part vient juste de se terminer...");
                                throw new Error(_loc_4.id + " en cours de téléchargement alors qu\'une autre part vient juste de se terminer...");
                            }
                        }
                        if (!_loc_3)
                        {
                            var _loc_6:String = this;
                            var _loc_7:* = this._downloadSuccess + 1;
                            _loc_6._downloadSuccess = _loc_7;
                            _log.info("Updater download is terminated.");
                            this._downloadingPart = null;
                            if (this._downloadList.length == 0)
                            {
                                DownloadMonitoring.getInstance().stop();
                                this._state = STATE_FINISHED;
                                KernelEventsManager.getInstance().processCallback(HookList.AllDownloadTerminated);
                            }
                            else
                            {
                                _loc_5 = this._downloadList.pop();
                                _log.info(_loc_5 + " found in download queue");
                                this.download(_loc_5);
                            }
                        }
                    }
                    break;
                }
                default:
                {
                    break;
                }
            }
            return;
        }// end function

        public function getServerPartList() : Vector.<uint>
        {
            var _loc_4:Pack = null;
            var _loc_5:Boolean = false;
            var _loc_6:ContentPart = null;
            if (this._firstParts == null)
            {
                return null;
            }
            var _loc_1:uint = 0;
            var _loc_2:* = Pack.getAllPacks();
            var _loc_3:* = new Vector.<uint>;
            for each (_loc_4 in _loc_2)
            {
                
                if (_loc_4.hasSubAreas)
                {
                    _loc_1 = _loc_1 + 1;
                    _loc_5 = false;
                    for each (_loc_6 in this._firstParts)
                    {
                        
                        if (_loc_6.id == _loc_4.name && _loc_6.state == 2)
                        {
                            _loc_5 = true;
                            break;
                        }
                    }
                    if (_loc_5)
                    {
                        _loc_3.push(_loc_4.id);
                    }
                }
            }
            if (_loc_3.length == _loc_1)
            {
                return null;
            }
            return _loc_3;
        }// end function

        public function getPart(param1:String) : ContentPart
        {
            var _loc_2:ContentPart = null;
            for each (_loc_2 in this._parts)
            {
                
                if (_loc_2.id == param1)
                {
                    return _loc_2;
                }
            }
            return null;
        }// end function

        public function createEmptyPartList() : void
        {
            this._parts = new Dictionary();
            return;
        }// end function

        public function getDownloadPercent(param1:int) : int
        {
            var _loc_2:* = 100 * this._downloadSuccess / this._downloadCount + param1 / this._downloadCount;
            if (_loc_2 < 0)
            {
                return 0;
            }
            if (_loc_2 > 100)
            {
                return 100;
            }
            return _loc_2;
        }// end function

        public function get isDownloading() : Boolean
        {
            return this._state == STATE_DOWNLOADING;
        }// end function

        public function get isFinished() : Boolean
        {
            return this._state == STATE_FINISHED;
        }// end function

        private function download(param1:String) : void
        {
            var _loc_2:DownloadPartMessage = null;
            this._state = STATE_DOWNLOADING;
            if (this._parts[param1].state == PartStateEnum.PART_NOT_INSTALLED)
            {
                if (!this._downloadingPart)
                {
                    _log.info("Send download request for " + param1 + " to updater");
                    _loc_2 = new DownloadPartMessage();
                    _loc_2.initDownloadPartMessage(param1);
                    UpdaterConnexionHandler.getConnection().send(_loc_2);
                    this._downloadingPart = param1;
                }
                else if (this._downloadList.indexOf(param1) == -1)
                {
                    _log.info("A download is running. Add " + param1 + " to download queue");
                    this._downloadList.push(param1);
                }
            }
            return;
        }// end function

        public static function getInstance() : PartManager
        {
            if (!_singleton)
            {
                _singleton = new PartManager;
            }
            return _singleton;
        }// end function

    }
}
