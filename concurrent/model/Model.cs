using logic;
using logic.Event;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;

namespace model
{
    public class Model
    {
        //singleton
        private static Model instance = null;
        private static readonly object padlock = new object();

        //
        public static ObservableCollection<Vector2> Marbles;


        private Model() 
        {
            LogicApiImplementation.Instance.PosUpdated += new MarbleEventHandler(OnPosUpdated);
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