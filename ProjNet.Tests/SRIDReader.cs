using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using GeoAPI.CoordinateSystems;
using ProjNet.CoordinateSystems;

namespace ProjNet.UnitTests
{
    internal class SRIDReader
    {
        public struct WktString {
            /// <summary>
            /// Well-known ID
            /// </summary>
            public int WktId;
            /// <summary>
            /// Well-known Text
            /// </summary>
            public string Wkt;
        }

        /// <summary>
        /// Enumerates all SRID's in the SRID.csv file.
        /// </summary>
        /// <returns>Enumerator</returns>
        public static IEnumerable<WktString> GetSrids(string filename = null)
        {
            Stream stream = string.IsNullOrWhiteSpace(filename)
                ? Assembly.GetExecutingAssembly().GetManifestResourceStream("ProjNET.Tests.SRID.csv")
                : File.OpenRead(filename);

            using (var sr = new StreamReader(stream, Encoding.UTF8))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;

                    var split = line.IndexOf(';');
                    if (split <= -1) continue;

                    var wkt = new WktString
                    {
                        WktId = int.Parse(line.Substring(0, split)),
                        Wkt = line.Substring(split + 1)
                    };
                    yield return wkt;
                }
            }
        }
        /// <summary>
        /// Gets a coordinate system from the SRID.csv file
        /// </summary>
        /// <param name="id">EPSG ID</param>
        /// <returns>Coordinate system, or null if SRID was not found.</returns>
        public static ICoordinateSystem GetCSbyID(int id)
        {
            ICoordinateSystemFactory factory = new CoordinateSystemFactory();
            foreach (var wkt in GetSrids(null))
                if (wkt.WktId == id)
                    return factory.CreateFromWkt(wkt.Wkt);
            return null;
        }
    }
}
