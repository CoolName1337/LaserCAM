using System.Linq;

namespace LaserCAM.CAM
{
    public static class GParams
    {
        public static bool UseSpacesInResult { get; set; } = false;
        public static bool UseDots { get; set; } = true;
        public static double Feed { get; set; } = 550;
        public static bool FullCode { get; set; } = true;

        public static string GetResult(string res)
        {
            var arr = res.Split("\n", System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (arr[i].StartsWith("G01") || arr[i].StartsWith("G02"))
                    if (arr[i].Substring(3) == arr[i + 1].Substring(3))
                        arr[i + 1] = "";

            }
            arr = arr.Where(el => !string.IsNullOrWhiteSpace(el)).ToArray();
            res = string.Join("\n", arr);
            if (FullCode)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i].StartsWith("G00"))
                        arr[i] += " M05";
                    if (arr[i].StartsWith("G01") || arr[i].StartsWith("G02"))
                        arr[i] += " M03";
                }
                res = $"G21 G49 G80 G90 F{Feed}\n" + string.Join("\n", arr) + "\nM5\nM30\n";
            }
            if (!UseSpacesInResult)
            {
                res = res.Replace(" ", "");
            }
            if (UseDots)
            {
                res = res.Replace(',', '.');
            }
            return res;
        }
    }
}
