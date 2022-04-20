using logic;
using logic.Event;

namespace model
{
    public class Model
    {
        //singleton
        private static Model instance = null;
        private static readonly object padlock = new object();

        //


        private Model() 
        {
            Api.Instance.PosUpdated += new MarbleEventHandler(OnPosUpdated);
        }

        public static Model Instance
        {
            get
            {
                lock (padlock) // critical section
                {
                    if (instance == null)
                        instance = new Model();
                    return instance;
                }
            }
        }

        public void OnPosUpdated(object source, MarbleArgs args)
        {
        }
    }
}