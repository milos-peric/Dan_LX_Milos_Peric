using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dan_LX_Milos_Peric.Utility
{
    class LocationLoader
    {
        private const string pathLocation = @"..\..\Locations.txt";
        private static List<tblLocation> listOfLocationsFromFile = new List<tblLocation>();
        public static List<tblLocation> LoadLocations()
        {
            string line;
            try
            {
                using (StreamReader streamReader = new StreamReader(pathLocation))
                {
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        tblLocation location = new tblLocation();
                        string[] lines = line.Split(',');
                        location.Address = lines.ElementAt(0).ToString();
                        location.City = lines.ElementAt(1).ToString();
                        location.Country = lines.ElementAt(2).ToString();
                        listOfLocationsFromFile.Add(location);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Not possible to read from file {0} or file doesn't exist.", pathLocation);
                Console.WriteLine(e.Message);
            }
            return listOfLocationsFromFile;
        }
    }
}
