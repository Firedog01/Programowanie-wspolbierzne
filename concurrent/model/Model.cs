using Logic;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Presentation.Model
{
    public abstract class ModelAbstractAPI
    {
        public static ModelAbstractAPI createApi(LogicAbstractAPI logicAbstractAPI = null)
        {
            return new ModelAPI();
        }

        public abstract void start(int number, float height, float width);

        public abstract ObservableCollection<IEllipse> getEllipses();

        public abstract void stop();

        internal sealed class ModelAPI : ModelAbstractAPI
        {
            internal ModelAPI(LogicAbstractAPI logicAbstractAPI = null)
            {
                if (logicAbstractAPI == null)
                {
                    this.logicApi = LogicAbstractAPI.createApi();
                }
                else
                {
                    this.logicApi = logicAbstractAPI;
                }
            }

            private LogicAbstractAPI logicApi = LogicAbstractAPI.createApi(null);

            private ObservableCollection<IEllipse> ellipses = new ObservableCollection<IEllipse>();

            internal ObservableCollection<IEllipse> Ellipses { get => ellipses; set => ellipses = value; }

            public override void start(int number, float height, float width)
            {
                logicApi.start(width, height, number, 20);
            }

            public override ObservableCollection<IEllipse> getEllipses()
            {
                List<LogicMarble> marbles = logicApi.getMarbles();
                Ellipses.Clear();
                foreach (LogicMarble b in marbles)
                {
                    Ellipses.Add(new Ellipse(b));
                }
                return Ellipses;
            }

            public override void stop()
            {
                logicApi.stop();
            }
        }
    }



}
