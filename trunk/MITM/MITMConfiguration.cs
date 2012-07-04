namespace BiM.MITM
{
    public class MITMConfiguration
    {
        public string FakeAuthHost
        {
            get;
            set;
        }

        public int FakeAuthPort
        {
            get;
            set;
        }

        public string FakeWorldHost
        {
            get;
            set;
        }

        public int FakeWorldPort
        {
            get;
            set;
        }

        public string RealAuthHost
        {
            get;
            set;
        }

        public int RealAuthPort
        {
            get;
            set;
        }
    }
}