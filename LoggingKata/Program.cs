using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;
using System.Threading;

namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();
        const string csvPath = "TacoBell-US-AL.csv";

        public static double AskForDubs(string msg = "please enter something!", double limLower = -180, double limUpper = 180)
        {
            double dubs;
            bool gotDubs = false;
            bool dubsGood = false;
            do
            {
                logger.LogInfo(msg);
                gotDubs = double.TryParse(Console.ReadLine(), out dubs);
                if (gotDubs && limLower <= dubs && dubs <= limUpper)
                    dubsGood = true;
            } while (!dubsGood);
            return dubs;
        }

        public static int AskForInt(string msg = "please enter something!", int limLower = 0, int limUpper = 999)
        {
            int inty = 0;
            bool gotInt = false;
            bool intGood = false;
            do
            {
                Console.Clear();
                logger.LogInfo(msg);
                gotInt = int.TryParse(Console.ReadLine(), out inty);
                if (gotInt && limLower <= inty && inty <= limUpper)
                    intGood = true;
            } while (!intGood);
            return inty;
        }

        public static double? ConvertUnits(double? dist, int unit)
        {
            if (dist != null) 
            { 
                switch (unit)
                {
                    case 1: // kilometers
                        return dist * 0.001;
                    case 2: // feet
                        return dist * 3.28084;
                    case 3: // miles
                        return dist * 0.000621371;
                    case 0: // meters
                    default:
                        return dist;
                }
            } return dist;
        }

        public static string GetUnitLabel(int unit)
        {
            switch (unit)
            {
                case 1:
                    return "kilometers";
                case 2:
                    return "feet";
                case 3:
                    return "miles";
                case 0:
                default:
                    return "meters";
            }
        }

        static void Main(string[] args)
        {
            int unitChoice = AskForInt("Taco Parser defaults to meters, select which units you would like to use for distance" +
                "\n0: meters (default)" +
                "\n1: kilometers" +
                "\n2: feet" +
                "\n3: miles", 0, 4);
            string unitLabel = GetUnitLabel(unitChoice);
            logger.LogInfo("Log initialized");
            var lines = File.ReadAllLines(csvPath);
            logger.LogInfo($"Lines: {lines[0]}");
            var parser = new TacoParser();
            var locations = lines.Select(parser.Parse).ToArray();
            ITrackable bellUno = null;
            ITrackable bellDos = null;
            double? distanceChamp = null;
            double distance;
            var locA = new GeoCoordinate();
            var locB = new GeoCoordinate();
            for (int i = 0; i < locations.Length - 1; i++)
            {
                locA.Latitude = locations[i].Location.Latitude;
                locA.Longitude = locations[i].Location.Longitude;
                for (int j = i + 1; j < locations.Length; j++)
                {
                    locB.Latitude = locations[j].Location.Latitude;
                    locB.Longitude = locations[j].Location.Longitude;
                    distance = locA.GetDistanceTo(locB);
                    if (distance > distanceChamp || distanceChamp == null)
                    {
                        distanceChamp = distance;
                        bellUno = locations[i];
                        bellDos = locations[j];
                        logger.LogInfo($"found new distance champ! distance: {ConvertUnits(distanceChamp, unitChoice)} {unitLabel}: {bellUno.Name} & {bellDos.Name}");
                    }
                }
            }
            Console.WriteLine();
            logger.LogInfo($"found two taco bells farthest away from one another...");
            logger.LogInfo($"Total Distance: {ConvertUnits(distanceChamp, unitChoice)} {unitLabel}\nTacoBell Uno: \n\tName: {bellUno.Name}\n\tLatitude: {bellUno.Location.Latitude}\n\tLongitude: {bellUno.Location.Longitude}\nTacoBell Dos:\n\tName: {bellDos.Name}\n\tLatitude: {bellDos.Location.Latitude}\n\tLongitude: {bellDos.Location.Longitude}");
            Console.WriteLine();
            logger.LogInfo("want to see something cool?\ntype \"YES\" to continue");
            string userSayYes = Console.ReadLine();
            if (userSayYes == "YES")
            {
                double myLat = AskForDubs("please enter your latitude...");
                double myLong = AskForDubs("please enter your longitude...");
                double myDistance;
                double? myDistanceChamp = null;
                ITrackable myBell = null;
                var myLoc = new GeoCoordinate(myLat, myLong);
                var myBellLoc = new GeoCoordinate();
                Console.WriteLine();
                for (int i = 0; i < locations.Length; i++)
                {
                    myBellLoc.Latitude = locations[i].Location.Latitude;
                    myBellLoc.Longitude = locations[i].Location.Longitude;
                    myDistance = myLoc.GetDistanceTo(myBellLoc);
                    if ((myDistance < myDistanceChamp || myDistanceChamp == null) && myDistance != 0)
                    {
                        myDistanceChamp = myDistance;
                        myBell = locations[i];
                        logger.LogInfo($"found new closest distance champ! distance: {ConvertUnits(myDistanceChamp, unitChoice)} {unitLabel}: {myBell.Name}");
                    }
                }
                Console.WriteLine();
                logger.LogInfo($"found closest taco bell to your location. Total Distance: {ConvertUnits(myDistanceChamp, unitChoice)} {unitLabel}");
                logger.LogInfo($"current position: Latitude: {myLoc.Latitude}, Longitude: {myLoc.Longitude} \nClosest TacoBell: Latitude: {myBell.Location.Latitude}, Longitude: {myBell.Location.Longitude}");
                Console.WriteLine();
                logger.LogInfo("Interested in purchasing a home? how about smack dab in the middle of all these fine establishments?! finding most optimimal location!");
                double sumLats = 0;
                double sumLongs = 0;
                double centerLat;
                double centerLong;
                foreach (var location in locations)
                {
                    sumLats += location.Location.Latitude;
                    sumLongs += location.Location.Longitude;
                }
                centerLat = sumLats / locations.Length;
                centerLong = sumLongs / locations.Length;
                var centerLoc = new GeoCoordinate(centerLat, centerLong);
                var distToCenter = myLoc.GetDistanceTo(centerLoc);
                ITrackable centerBell = null;
                ITrackable centerBellFar = null;
                double centerDistance;
                double? centerDistanceChamp = null;
                double? centerDistanceChampFar = null;
                for (int i = 0; i < locations.Length; i++)
                {
                    var centerBellLoc = new GeoCoordinate(locations[i].Location.Latitude, locations[i].Location.Longitude);
                    centerDistance = centerLoc.GetDistanceTo(centerBellLoc);
                    if ((centerDistance < centerDistanceChamp || centerDistanceChamp == null) && centerDistance != 0)
                    {
                        centerDistanceChamp = centerDistance;
                        centerBell = locations[i];
                    }
                    else if ((centerDistance > centerDistanceChampFar || centerDistanceChampFar == null) && centerDistance != 0)
                    {
                        centerDistanceChampFar = centerDistance;
                        centerBellFar = locations[i];
                    }
                }
                Console.WriteLine();
                logger.LogInfo($"in order to be in the center of all tacobells, you must move {ConvertUnits(distToCenter, unitChoice)} {unitLabel} to Lat:{centerLoc.Latitude}, Long:{centerLoc.Longitude}. \nyoure welcome!");
                logger.LogInfo($"the closest taco bell at this location would be {ConvertUnits(centerDistanceChamp, unitChoice)} {unitLabel} away \n{centerBell.Name} @ ({centerBell.Location.Latitude},{centerBell.Location.Longitude})");
                logger.LogInfo($"the farthest taco bell from this location would be {ConvertUnits(centerDistanceChampFar, unitChoice)} {unitLabel} away \n{centerBellFar.Name} @ ({centerBellFar.Location.Latitude},{centerBellFar.Location.Longitude})");
                Console.WriteLine();
            }
        }
    }
}


