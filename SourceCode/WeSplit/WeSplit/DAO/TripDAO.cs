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
        public static List<Trip> GetTrips(int perPage, int pageCurrent)
        {
            var result = new List<Trip>();

            string jsonFilePath = "./Data/trips.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray trips = JArray.Parse(json);

                int skipValue = pageCurrent == 1 ? 0 : (pageCurrent - 1) * perPage;
                trips = new JArray(trips.Skip(skipValue).Take(perPage));

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

        public static int CountTrips()
        {
            var result = 0;

            string jsonFilePath = "./Data/trips.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray trips = JArray.Parse(json);
                result = trips.Count();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}
