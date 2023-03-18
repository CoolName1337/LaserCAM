using LaserCAM.CAM;
using LaserCAM.CAM.GShapes;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

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

        public static void SaveProject(string path)
        {
            stringBuilder.Clear();
            stringBuilder.AppendLine($"{GField.Sample.Width}|{GField.Sample.Height}");
            stringBuilder.AppendLine($"{GZeroPoint.Position.X}|{GZeroPoint.Position.Y}");
            foreach (GShape shape in GField.AllShapes)
            {
                var str = shape.ToSerialize() + ";";
                stringBuilder.Append(str);
            }

            using(FileStream fileStream = File.Open(path, FileMode.Create))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(stringBuilder.ToString());

                fileStream.Write(buffer, 0, buffer.Length);
            }
        }

        public static void OpenProject(string path)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.UnsetSample();
            string result;
            using (FileStream fileStream = File.OpenRead(path))
            {
                byte[] buffer = new byte[fileStream.Length];

                fileStream.Read(buffer, 0, buffer.Length);

                result = Encoding.UTF8.GetString(buffer);
            }

            string[] resultArray = result.Split("\n", System.StringSplitOptions.RemoveEmptyEntries);

            string[] gSampleData = resultArray[0].Split("|");
            string[] GZeroPointData = resultArray[1].Split("|");
            mainWindow.SetSample(
                double.Parse(gSampleData[0]), double.Parse(gSampleData[1]), 
                new Point(
                    double.Parse(GZeroPointData[0]), double.Parse(GZeroPointData[1])
                    )
                );

            var listOfSerializedShapes = resultArray[2]
                .Split(";", System.StringSplitOptions.RemoveEmptyEntries);
            var listOfShapes = listOfSerializedShapes
                .Select(str => GShape.FromSerialize(str))
                .ToList();
            listOfShapes.ForEach(shape => shape.Create());
        }

        public static string GenerateGCode()
        {
            string res = string.Join("\n", GField.AllShapes.Select(s => s.ToGCode()));
            return GParams.GetResult(res);
        }

    }
}
