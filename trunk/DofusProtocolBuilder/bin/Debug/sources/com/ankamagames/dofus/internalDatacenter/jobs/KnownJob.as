package com.ankamagames.dofus.internalDatacenter.jobs
{
    import com.ankamagames.dofus.network.types.game.context.roleplay.job.*;

    public class KnownJob extends Object implements IDataCenter
    {
        public var jobDescription:JobDescription;
        public var jobExperience:JobExperience;
        public var jobPosition:int;

        public function KnownJob()
        {
            return;
        }// end function

    }
}
