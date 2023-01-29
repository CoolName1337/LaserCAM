using LaserCAM.CAM.GTools;
using System.Drawing;
using System.Windows.Shapes;

namespace LaserCAM.CAM.GShapes
{
    public class GLine : GShape
    {
        public GLine(Line line = null)
        {
            if(line == null)
            {
                Shape = new Line() { Stroke = GTool.BlackBrush };
            }
            else
            {
                line.Stroke = GTool.BlackBrush;
                Shape = line;
            }
        }
        public override string ToGCode()
        {
            return $"G01 ";
        }
    }
}
