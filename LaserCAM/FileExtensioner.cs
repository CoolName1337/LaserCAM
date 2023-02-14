using LaserCAM.CAM;
using LaserCAM.CAM.GShapes;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace LaserCAM
{
    /// <summary>
    /// Class that implement save 
    /// and open logic for .lcam file extension. 
    /// And implement G-code saving in file.
    /// </summary>
    public static class FileExtensioner
    {
        public static string PathForProjects { get; set; }
        public static string PathForGCodeFiles { get; set; }

        private static StringBuilder stringBuilder = new();

        public static async Task SaveProject(string path)
        {
            stringBuilder.AppendLine($"{GField.Sample.Width}|{GField.Sample.Height}");
            stringBuilder.AppendLine($"{GPoint.Position.X}|{GPoint.Position.Y}");
            foreach (GShape shape in GField.AllShapes)
                stringBuilder.Append(shape.ToSerialize()+";");

            using(FileStream fileStream = File.OpenWrite(path))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(stringBuilder.ToString());

                await fileStream.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        public static async Task OpenProject(string path)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.UnsetSample();
            string result;
            using (FileStream fileStream = File.OpenRead(path))
            {
                byte[] buffer = new byte[fileStream.Length];

                await fileStream.ReadAsync(buffer, 0, buffer.Length);

                result = Encoding.UTF8.GetString(buffer);
            }

            string[] resultArray = result.Split("\n", System.StringSplitOptions.RemoveEmptyEntries);

            string[] gSampleData = resultArray[0].Split("|");
            string[] gPointData = resultArray[1].Split("|");
            mainWindow.SetSample(
                double.Parse(gSampleData[0]), double.Parse(gSampleData[1]), 
                new Point(
                    double.Parse(gPointData[0]), double.Parse(gPointData[1])
                    )
                );

            GField.AllShapes = resultArray[2]
                .Split(";", System.StringSplitOptions.RemoveEmptyEntries)
                .Select(str => GShape.FromSerialize(str))
                .ToList();

            GField.AllShapes.ForEach(shape => shape.Create());
        }

        public static string GenerateGCode()
        {
            string res = string.Join("\n", GField.AllShapes.Select(s => s.ToGCode()));
            return GParams.GetResult(res);
        }

    }
}
