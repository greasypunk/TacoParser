namespace LoggingKata
{
    /// <summary>
    /// Parses a POI file to locate all the Taco Bells
    /// </summary>
    public class TacoParser
    {

        public TacoParser()
        {
        }

        readonly ILog logger = new TacoLogger();
        
        public ITrackable Parse(string line)
        {
            logger.LogInfo("Begin parsing");
            var cells = line.Split(',');
            if (cells.Length < 3) {
                logger.LogWarning($"uh oh, cells.Length < 3. line = {line}");
                return null; }
            Point point = new Point();
            bool latitudeGood = double.TryParse(cells[0], out double latitude);
            if (!latitudeGood)
                logger.LogError("latitude is all whacky!");
            bool longitudeGood = double.TryParse(cells[1], out double longitude);
            if (!longitudeGood)
                logger.LogError("longitude is all whacky!");
            point.Latitude = latitude;
            point.Longitude = longitude;
            string name = cells[2];
            if (name == null || name.Length == 0)
                logger.LogError("the name is all whacky!");
            var taco = new TacoBell(name, point);
            return taco;
        }
    }
}