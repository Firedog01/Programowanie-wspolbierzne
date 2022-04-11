namespace logic.Event
{
    public delegate void MarbleEventHandler(object src, MarbleArgs info);
    // https://stackoverflow.com/questions/623451/how-can-i-make-my-own-event-in-c

    public class MarbleInfo
    {
        private int index;
        public int Index { get { return index; } }
        private double x;
        public double X { get { return x; } }

        private double y;
        public double Y { get { return y; } }


        public MarbleInfo(int idx, double x, double y)
        {
            this.index = idx;
            this.x = x; 
            this.y = y;
        }
    }

    public class MarbleArgs : EventArgs
    {
        private MarbleInfo[] infoArr;
        public MarbleArgs(MarbleInfo[] marbles) { infoArr = marbles; }

        public MarbleInfo[] InfoArr { get { return infoArr; } }
    }
}