
// using Itinero;
// using Itinero.IO.Osm;
// using Itinero.LocalGeo;
// using Itinero.Osm.Vehicles;


namespace Steuerdistanz
{


    class Program
    {


        static void Main(string[] args)
        {
            // enable logging.
            OsmSharp.Logging.Logger.LogAction = (o, level, message, parameters) =>
            {
                System.Console.WriteLine(string.Format("[{0}] {1} - {2}", o, level, message));
            };
            Itinero.Logging.Logger.LogAction = (o, level, message, parameters) =>
            {
                System.Console.WriteLine(string.Format("[{0}] {1} - {2}", o, level, message));
            };


            // https://download.geofabrik.de/index.html
            // https://download.geofabrik.de/europe.html
            // https://download.geofabrik.de/europe/switzerland.html

            // Download.ToFile("http://files.itinero.tech/data/OSM/planet/europe/luxembourg-latest.osm.pbf", "luxembourg-latest.osm.pbf").Wait();

            // load some routing data and create a router.
            Itinero.RouterDb routerDb = new Itinero.RouterDb();

            string filename = "luxembourg-latest.osm.pbf";
            filename = @"D:\username\Documents\Visual Studio 2017\Projects\routing\switzerland-latest.osm.pbf";


            System.Diagnostics.Stopwatch swLoad = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch swRouterCreation = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch swRouteCalculation = new System.Diagnostics.Stopwatch();


            swLoad.Start();
            using (System.IO.FileStream stream = System.IO.File.OpenRead(filename))
            {
                Itinero.IO.Osm.RouterDbExtensions.LoadOsmData(routerDb, stream, Itinero.Osm.Vehicles.Vehicle.Car);
            } // End Using stream 
            swLoad.Stop();


            swRouterCreation.Start();
            // get the profile from the routerdb.
            // this is best-practice in Itinero, to prevent mis-matches.
            Itinero.Profiles.Profile car = routerDb.GetSupportedProfile("car");

            // add a contraction hierarchy.
            Itinero.RouterDbExtensions.AddContracted(routerDb, car);

            // create router.
            Itinero.Router router = new Itinero.Router(routerDb);
            swRouterCreation.Stop();


            // 47.373480, 9.558990 - Altstätten
            // 47.552063, 9.226030 - Erlen
            // Coordinate from = new Coordinate(47.373480f, 9.558990f); // Altstätten
            // Coordinate to = new Coordinate(47.552063f, 9.226030f); // Erlen
            // calculate route.
            // Itinero.Route route = router.Calculate(car, from, to);
            // this should be the result: http://geojson.io/#id=gist:anonymous/c944cb9741f1fd511c8213b2dd83d58d&map=17/49.75454/6.09571

            // Itinero:
            // Distance 56'315.09 = 56.1km
            // Time: 2'944.275 = 49.07 min

            // Google:
            // Distance: 55.1 km
            // Time:       46 min


            Itinero.LocalGeo.Coordinate toLocation1 = new Itinero.LocalGeo.Coordinate(47.552063f, 9.226030f); // Erlen 47.552063, 9.226030
            Itinero.LocalGeo.Coordinate toLocation2 = new Itinero.LocalGeo.Coordinate(47.377137f, 8.541654f); // Zürich HB 47.377137, 8.541654
            Itinero.LocalGeo.Coordinate toLocation3 = new Itinero.LocalGeo.Coordinate(47.422517f, 9.370708f); // St. Gallen HB 47.422517, 9.370708
            Itinero.LocalGeo.Coordinate toLocation4 = new Itinero.LocalGeo.Coordinate(47.548713f, 7.590845f); // Basel SBB 47.548713, 7.590845
            Itinero.LocalGeo.Coordinate toLocation5 = new Itinero.LocalGeo.Coordinate(47.501965f, 8.725905f); // Winterthur 47.501965, 8.725905


            string[] targetLocation = new string[] { "Erlen", "Zürich HB", "St. Gallen HB", "Basel SBB", "Winterthur" };
            Itinero.LocalGeo.Coordinate[] toLocations = new Itinero.LocalGeo.Coordinate[] { toLocation1, toLocation2, toLocation3, toLocation4, toLocation5 };


            using (System.Data.DataTable dt = SQL.GetDataTable("SELECT * FROM __Steuern_2017"))
            {
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    int gemeindenummer = (int)dr["gemeindenummer"];
                    System.Decimal lat = (System.Decimal)dr["latitude"]; // decimal(9,6) NOT NULL 
                    System.Decimal lon = (System.Decimal)dr["longitude"]; // decimal(9,6) NOT NULL 

                    Itinero.LocalGeo.Coordinate fromLocation = new Itinero.LocalGeo.Coordinate((float)lat, (float)lon); // Altstätten


                    Itinero.RouterPoint a = null;

                    try
                    {
                        a = ResolveConnected(router, car, fromLocation.Latitude, fromLocation.Longitude);
                        System.Console.WriteLine(a);
                    }
                    catch (Itinero.Exceptions.ResolveFailedException resolveFail)
                    {
                        System.IO.File.AppendAllText(@"A_ResolveFailureLog.txt", gemeindenummer.ToString() + ": " + resolveFail.Message + System.Environment.NewLine);
                        // https://stackoverflow.com/questions/5910817/does-a-break-leave-just-the-try-catch-or-the-surrounding-loop
                        // A break statement always applies to the innermost while, do, or switch, regardless of other intervening statements. 
                        // However, there is one case where the break will not cause the loop to exit:
                        continue;
                    }
                    catch (System.Exception exx)
                    {
                        System.IO.File.AppendAllText(@"A_FatalFailureLog.txt", gemeindenummer.ToString() + ": " + exx.Message + System.Environment.NewLine);
                        continue;
                    }




                    for (int i = 0; i < toLocations.Length; ++i)
                    {

                        string sql = @"
-- DECLARE @gemeindenummer AS int 
-- DECLARE @location AS nvarchar(255)
-- DECLARE @distance AS float
-- DECLARE @duration AS float

-- SET @gemeindenummer = 123 
-- SET @location = N''
-- SET @distance = 1.0e
-- SET @duration = 2.0e 



INSERT INTO __Steuern_2017_ZO_Road
(
	 RD_UID
	,RD_GemeindeNummer
	,RD_Ort
	,RD_Entfernung
	,RD_Dauer
)
SELECT 
	 NEWID() AS RD_UID --  uniqueidentifier
	,@gemeindenummer AS RD_GemeindeNummer -- int
	,@location AS RD_Ort -- nvarchar(255)
	,@distance AS RD_Entfernung -- float
	,@duration AS RD_Dauer -- float
; 
";

                        try
                        {
                            swRouteCalculation.Start();

                            Itinero.RouterPoint b = ResolveConnected(router, car, toLocations[i].Latitude, toLocations[i].Longitude);

                            // Itinero.Route route = Itinero.RouterBaseExtensions.Calculate(router, car, fromLocation, toLocations[i]);
                            Itinero.Route route = Itinero.RouterBaseExtensions.Calculate(router, car, a, b);
                            swRouteCalculation.Stop();

                            float distance = route.TotalDistance;
                            float time = route.TotalTime;


                            using (System.Data.IDbCommand cmd = SQL.CreateCommand(sql))
                            {
                                SQL.AddParameter(cmd, "gemeindenummer", gemeindenummer);
                                SQL.AddParameter(cmd, "location", targetLocation[i]);
                                SQL.AddParameter(cmd, "distance", distance);
                                SQL.AddParameter(cmd, "duration", time);

                                SQL.ExecuteNonQuery(cmd);
                            } // End Using cmd 

                            System.Console.WriteLine($"Route calculation: {swRouteCalculation.Elapsed.TotalMinutes}"); // 0.12s
                            System.Console.WriteLine($"Distance: {distance}");
                            System.Console.WriteLine($"Time: {time}");
                        }
                        catch (Itinero.Exceptions.ResolveFailedException resolveFail)
                        {
                            System.IO.File.AppendAllText(@"B_ResolveFailureLog.txt", gemeindenummer.ToString() + ": " + resolveFail.Message + System.Environment.NewLine);
                            // https://stackoverflow.com/questions/5910817/does-a-break-leave-just-the-try-catch-or-the-surrounding-loop
                            // A break statement always applies to the innermost while, do, or switch, regardless of other intervening statements. 
                            // However, there is one case where the break will not cause the loop to exit:
                            continue;
                        }
                        catch (System.Exception ex)
                        {
                            System.Console.WriteLine(ex.Message);
                            System.Console.WriteLine(ex.StackTrace);
                            System.IO.File.AppendAllText(@"B_FatalFailureLog.txt", gemeindenummer.ToString() + ": " + ex.Message + System.Environment.NewLine);
                            continue;

                            // TRUNCATE TABLE __Steuern_2017_ZO_Road 
                            // Plateau de Diesse - 47.11667, 7.116667 
                        }

                    } // Next i 

                } // Next dr 

            } // End Using dt 

            System.Console.WriteLine($"Load: {swLoad.Elapsed.TotalMinutes}"); // 4 min 42 s 
            System.Console.WriteLine($"Router creation: {swRouterCreation.Elapsed.TotalMinutes}"); // 12min 39s 

            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub Main 


        /// <summary>
        /// Resolves a location but also checks if it's connected to the rest of the network.
        /// </summary>
        //public static RouterPoint ResolveConnected(this RouterBase router, IProfileInstance profileInstance, float latitude, float longitude, float radiusInMeter = 1000,
        public static Itinero.RouterPoint ResolveConnected(Itinero.RouterBase router, Itinero.Profiles.IProfileInstance profileInstance, float latitude, float longitude, float radiusInMeter = 1000,
            float maxSearchDistance = 4 * Itinero.Constants.SearchDistanceInMeter)
        {
            return router.TryResolve(new Itinero.Profiles.IProfileInstance[] { profileInstance }, latitude, longitude, (edge) =>
            {
                // create a temp resolved point in the middle of this edge.
                var tempRouterPoint = new Itinero.RouterPoint(0, 0, edge.Id, ushort.MaxValue / 2);
                var connectivityResult = router.TryCheckConnectivity(profileInstance, tempRouterPoint, radiusInMeter);
                if (connectivityResult.IsError)
                { // if there is an error checking connectivity, choose not report it, just don't choose this point.
                    return false;
                }
                return connectivityResult.Value;
            }, maxSearchDistance).Value;
        }


    } // End Class Program 


} // End Namespace Steuerdistanz 
