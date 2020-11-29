using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeSplit.DTO;

namespace WeSplit.DAO
{
    class TripDAO
    {
        public static List<Trip> GetTrips()
        {
            var result = new List<Trip>();

            string jsonFilePath = "./Data/trips.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray trips = JArray.Parse(json);
                foreach (var trip in trips)
                {
                    Trip tripObject = new Trip();
                    tripObject.Id = trip["id"].ToString();
                    tripObject.Name = trip["name"].ToString();
                    tripObject.MainImage = trip["image_main"].ToString();

                    result.Add(tripObject);
                }
            }
            catch(Exception)
            {
                throw;
            }
            return result;
        }
    }
}
