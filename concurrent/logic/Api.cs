namespace logic
{
    public sealed class Api
    {
        // singleton
        private static Api instance = null;
        private static readonly object padlock = new object();

        public static Api Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                        instance = new Api();
                    return instance;
                }
            }
        }

        
        public event Event.Handler onPosUpdated;

        private int marbleCount;
        public int MarbleCount { get { return marbleCount; } }

        Api()
        {
            marbleCount = 0;
        }

        public void CreateUnmovingMarble()
        {
            marbleCount++;
        }

        
    }
}