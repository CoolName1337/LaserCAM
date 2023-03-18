using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LaserCAM.CAM
{
    public static class GBindingParams
    {
        public static Dictionary<string, bool> Params = new Dictionary<string, bool>()
        {
            {"UseBinding" , false},
            {"Center", false},
            {"Edge", false},
            {"Vertex", false},
        };
    }
}
