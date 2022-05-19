using System.ComponentModel;

namespace model
{
    public interface IEllipse : INotifyPropertyChanged
    {
        double X { get; set; }
        double Y { get; set; }
    }
}