using System.Windows.Input;
using System.Windows.Media;

namespace LaserCAM.CAM.GTools
{
    public abstract class GTool
    {
        public static Brush GrayBrush { get => new SolidColorBrush(Color.FromRgb(100, 100, 100)); }
        public static Brush BlackBrush { get => new SolidColorBrush(Color.FromRgb(0, 0, 0)); }

        public virtual void OnMouseDown(object sender, MouseButtonEventArgs e) { }
        public virtual void OnMouseUp(object sender, MouseButtonEventArgs e) { }
        public virtual void OnMouseMove(object sender, MouseEventArgs e) { }
    }
}
