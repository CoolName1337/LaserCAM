using LaserCAM.CAM.GShapes;
using System.Collections.Generic;

namespace LaserCAM.CAM
{
    public class GChange
    {
        public List<GShape> GShapes { get; set; } = new();
        public bool IsCreated { get; set; }
        public GChange(List<GShape> gShapes, bool isCreated)
        {
            GShapes = gShapes;
            IsCreated = isCreated;
        }

        public void Return()
        {
            if (IsCreated)
            {
                GShapes.ForEach(shape => GField.RemoveShape(shape));
            }
            else
            {
                GShapes.ForEach(
                    shape =>
                    {
                        shape.Unselect();
                        if(shape is GImage gImage)
                            GField.Panel.Children.Add(gImage.Image);
                        else
                            GField.Panel.Children.Add(shape.Shape);
                        GField.AllShapes.Add(shape);
                    }
                );
            }
        }
    }
}
