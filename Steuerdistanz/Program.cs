
// using Itinero;
// using Itinero.IO.Osm;
// using Itinero.LocalGeo;
// using Itinero.Osm.Vehicles;


namespace Steuerdistanz
{


    class Program
    {


        public static void Fetch(System.Decimal x, System.Decimal y)
        {
            Sbb.Locations.RootNode root = null;

            // http://transport.opendata.ch/v1/locations?query=haltestelle&type=station
            // http://transport.opendata.ch/v1/locations?x=lat&y=long&type=station

            string url = $"http://transport.opendata.ch/v1/locations?x={x}&y={y}&type=station";

            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                string response = wc.DownloadString(url);
                root = Sbb.Locations.RootNode.FromJson(response);
            } // End Using wc 

            GetNearest(root);
            System.Console.WriteLine(root);
        } // End Sub Fetch 


        public static void GetNearest(Sbb.Locations.RootNode root)
        {

            foreach (Sbb.Locations.Station station in root.Stations)
            {
                System.Console.WriteLine(station.Id.HasValue);
                System.Console.WriteLine(station.Id);
                System.Console.WriteLine(station.Name);
            } // Next station 

        } // End Sub GetNearest 


        private class MyWebClient : System.Net.WebClient
        {
            protected override System.Net.WebRequest GetWebRequest(System.Uri uri)
            {
                System.Net.WebRequest w = base.GetWebRequest(uri);
                w.Timeout = 20 * 60 * 1000;
                return w;
            }
        }

        public static Sbb.Connections.RootNode GetConnections(System.Decimal x, System.Decimal y, System.Decimal a, System.Decimal b)
        {
            Sbb.Connections.RootNode root = null;

            string url = $"http://transport.opendata.ch/v1/connections?from={x},{y}&to={a},{b}&date=2018-11-26&time=07:00";

            // https://hidemyna.me/en/proxy-list/
            // System.Net.WebProxy wp = new System.Net.WebProxy("194.186.162.254", 41282);

            using (System.Net.WebClient wc = new MyWebClient())
            {
                // wc.Proxy = wp;
                string response = wc.DownloadString(url);
                root = Sbb.Connections.RootNode.FromJson(response);
            } // End Using wc 

            System.Console.WriteLine(root);
            return root;
        } // End Function GetConnections 


        public static System.TimeSpan? GetFastestConnection(System.Decimal x, System.Decimal y, System.Decimal a, System.Decimal b)
        {
            Sbb.Connections.RootNode query = GetConnections(x, y, a, b);

            int min = -1;
            double minValue = double.MaxValue;

            for (int i = 0; i < query.Connections.Count; ++i)
            {
                if (query.Connections[i].Duration.Value.TotalHours < minValue)
                {
                    minValue = query.Connections[i].Duration.Value.TotalHours;
                    min = i;
                } // End if (query.Connections[i].Duration.Value.TotalHours < minValue) 

            } // Next i 

            if(min != -1)
                return query.Connections[min].Duration;

            return null;
        } // End Function GetFastestConnection 
        

        public class Wgs84Coordinates
        {

            public System.Decimal Latitude;
            public System.Decimal Longitude;


            public Wgs84Coordinates()
            { } // End Constructor 


            public Wgs84Coordinates(System.Decimal latitude, System.Decimal longitude)
            {
                this.Latitude = latitude;
                this.Longitude = longitude;
            } // End Constructor 


        } // End Class Wgs84Coordinates 


        static void Main(string[] args)
        {
            // RoutePlanner.Test();


            Wgs84Coordinates toLocation1 = new Wgs84Coordinates(47.552063m, 9.226030m); // Erlen 47.552063, 9.226030
            Wgs84Coordinates toLocation2 = new Wgs84Coordinates(47.377137m, 8.541654m); // Zürich HB 47.377137, 8.541654
            Wgs84Coordinates toLocation3 = new Wgs84Coordinates(47.422517m, 9.370708m); // St. Gallen HB 47.422517, 9.370708
            Wgs84Coordinates toLocation4 = new Wgs84Coordinates(47.548713m, 7.590845m); // Basel SBB 47.548713, 7.590845
            Wgs84Coordinates toLocation5 = new Wgs84Coordinates(47.501965m, 8.725905m); // Winterthur 47.501965, 8.725905

            string[] targetLocation = new string[] { "Erlen", "Zürich HB", "St. Gallen HB", "Basel SBB", "Winterthur" };
            Wgs84Coordinates[] toLocations = new Wgs84Coordinates[] { toLocation1, toLocation2, toLocation3, toLocation4, toLocation5 };


            for (int i = 0; i < toLocations.Length; ++i)
            {

                using (System.Data.IDbCommand cmdGemeinde = SQL.CreateCommand(@"SELECT * 
FROM __Steuern_2017 
WHERE gemeindenummer NOT IN 
(
    SELECT RL_GemeindeNummer FROM __Steuern_2017_ZO_Rail WHERE RL_Ort = @location 
) "))
                {
                    SQL.AddParameter(cmdGemeinde, "location", targetLocation[i]);

                    using (System.Data.DataTable dt = SQL.GetDataTable(cmdGemeinde))
                    {

                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            int gemeindenummer = (int)dr["gemeindenummer"];
                            System.Decimal lat = (System.Decimal)dr["latitude"]; // decimal(9,6) NOT NULL 
                            System.Decimal lon = (System.Decimal)dr["longitude"]; // decimal(9,6) NOT NULL 
                                                                                  // Fetch(lat, lon);

                            System.TimeSpan? duration = GetFastestConnection(lat, lon, toLocations[i].Latitude, toLocations[i].Longitude);

                            if (duration.HasValue)
                                System.Console.WriteLine(duration.Value.TotalHours);

                            string sql = @"
INSERT INTO __Steuern_2017_ZO_Rail
(
	 RL_UID
	,RL_GemeindeNummer
	,RL_Ort
	,RL_Dauer
)
SELECT 
	 NEWID() AS RL_UID --  uniqueidentifier
	,@gemeindenummer AS RL_GemeindeNummer -- int
	,@location AS RL_Ort -- nvarchar(255)
	,@duration AS RL_Dauer -- float
; 
";
                            using (System.Data.IDbCommand cmd = SQL.CreateCommand(sql))
                            {
                                SQL.AddParameter(cmd, "gemeindenummer", gemeindenummer);
                                SQL.AddParameter(cmd, "location", targetLocation[i]);

                                if (duration.HasValue)
                                    SQL.AddParameter(cmd, "duration", duration.Value.TotalHours);
                                else
                                    SQL.AddParameter(cmd, "duration", null);

                                SQL.ExecuteNonQuery(cmd);
                            } // End Using cmd 

                            System.Threading.Thread.Sleep(5000);
                        } // Next dr 

                    } // End Using dt 

                } // End Using cmdGemeinde 

            } // Next i 

        } // End Sub Main 


    } // End Class Program 


} // End Namespace Steuerdistanz 
