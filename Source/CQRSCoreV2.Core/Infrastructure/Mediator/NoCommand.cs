namespace CQRSCoreV2.Core
{ 
    public class NoCommand
    {
        private static readonly NoCommand Singleton = new NoCommand();

        public NoCommand()
        {
        }

        public static NoCommand Instance
        {
            get
            {
                return Singleton;
            }
        }
    }
}