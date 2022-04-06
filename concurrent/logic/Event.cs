namespace logic.Event
{
    public delegate void Handler(object src, Info info);

    public class MarbleInfo
    {
        private int index;
        private double x;
        private double y;

        public MarbleInfo(int idx, double x, double y)
        {
            this.index = idx;
            this.x = x; 
            this.y = y;
        }

        public int GetIndex()
        {
            return index;
        }

        public double GetX()
        {
            return x;
        }

        public double GetY()
        {
            return y;
        }

    }

    public class Info : EventArgs
    {
        private MarbleInfo[] infoArr;
        public Info(MarbleInfo[] marbles)
        {
            this.infoArr = marbles;
        }

        public MarbleInfo[] GetInfo()
        {
            return infoArr;
        }
    }
}