namespace CQRSCoreV2
{
    public sealed class NoCommandResult
    {
        private static readonly NoCommandResult Singleton = new NoCommandResult();

        private NoCommandResult()
        {
        }

        public static NoCommandResult Instance
        {
            get
            {
                return Singleton;
            }
        }
    }
}