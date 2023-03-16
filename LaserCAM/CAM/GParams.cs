namespace LaserCAM.CAM
{
    public static class GParams
    {
        public static bool UseSpacesInResult { get; set; } = false;
        public static bool UseDots { get; set; } = true;

        public static bool FullCode { get; set; } = true;

        public static string GetResult(string res)
        {
            if (FullCode)
            {
                var arr = res.Split("\n", System.StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i].StartsWith("G00"))
                        arr[i] += " M05";
                    if (arr[i].StartsWith("G01") || arr[i].StartsWith("G02"))
                        arr[i] += " M03";
                }
                res = "G21 G49 G80 G90\n" + string.Join("\n", arr) + "\nM5\nM30\n";
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
