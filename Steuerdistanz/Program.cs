using Itinero;
using Itinero.IO.Osm;
using Itinero.LocalGeo;
using Itinero.Osm.Vehicles;
using System;
using System.IO;

namespace Steuerdistanz
{
    class Program
    {
        static void Main(string[] args)
        {

            // enable logging.
            OsmSharp.Logging.Logger.LogAction = (o, level, message, parameters) =>
            {
                Console.WriteLine(string.Format("[{0}] {1} - {2}", o, level, message));
            };
            Itinero.Logging.Logger.LogAction = (o, level, message, parameters) =>
            {
                Console.WriteLine(string.Format("[{0}] {1} - {2}", o, level, message));
            };


            // https://download.geofabrik.de/index.html
            // https://download.geofabrik.de/europe.html
            // https://download.geofabrik.de/europe/switzerland.html

            // Download.ToFile("http://files.itinero.tech/data/OSM/planet/europe/luxembourg-latest.osm.pbf", "luxembourg-latest.osm.pbf").Wait();

            // load some routing data and create a router.
            var routerDb = new RouterDb();

            string filename = "luxembourg-latest.osm.pbf";
            filename = @"D:\username\Documents\Visual Studio 2017\Projects\routing\switzerland-latest.osm.pbf";


            System.Diagnostics.Stopwatch swLoad = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch routerCreation = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch routeCalculation = new System.Diagnostics.Stopwatch();


            swLoad.Start();
            using (var stream = File.OpenRead(filename))
            {
                routerDb.LoadOsmData(stream, Vehicle.Car);
            }
            swLoad.Stop();

            routerCreation.Start();
            // get the profile from the routerdb.
            // this is best-practice in Itinero, to prevent mis-matches.
            var car = routerDb.GetSupportedProfile("car");

            // add a contraction hierarchy.
            routerDb.AddContracted(car);

            // create router.
            var router = new Router(routerDb);
            routerCreation.Stop();

            // 47.373480, 9.558990 - Altstätten
            // 47.552063, 9.226030 - Erlen
            var from = new Coordinate(47.373480f, 9.558990f); // Altstätten
            var to = new Coordinate(47.552063f, 9.226030f); // Erlen


            routeCalculation.Start();
            // calculate route.
            // this should be the result: http://geojson.io/#id=gist:anonymous/c944cb9741f1fd511c8213b2dd83d58d&map=17/49.75454/6.09571
            Itinero.Route route = router.Calculate(car, from, to);

            float distance = route.TotalDistance;
            float time = route.TotalTime;
            routeCalculation.Stop();

            System.Console.WriteLine($"Distance: {distance}");
            System.Console.WriteLine($"Time: {time}");

            // Itinero:
            // Distance 56'315.09 = 56.1km
            // Time: 2'944.275 = 49.07 min

            // Google:
            // Distance: 55.1 km
            // Time:       46 min

            System.Console.WriteLine($"Load: {swLoad.Elapsed.TotalMinutes}"); // 4 min 42 s
            System.Console.WriteLine($"Router creation: {routerCreation.Elapsed.TotalMinutes}"); // 12min 39s
            System.Console.WriteLine($"Route calculation: {routeCalculation.Elapsed.TotalMinutes}"); // 0.12s




            Console.WriteLine(System.Environment.NewLine);
            Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        }
    }
}
