namespace LaserCAM.CAM
{
    public static class GParams
    {
        public static bool UseSpacesInResult { get; set; } = false;
        public static bool UseDots { get; set; } = true;

        public static string GetResult(string res)
        {
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
