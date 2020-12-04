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
    class TypeTripDAO
    {
        public static List<TypeTrip> Get()
        {
            var result = new List<TypeTrip>();

            string jsonFilePath = "./Data/type-trip.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray typeTrips = JArray.Parse(json);

                foreach (var item in typeTrips)
                {
                    TypeTrip typeObject = new TypeTrip();
                    typeObject.Id = item["id"].ToString();
                    typeObject.Name = item["name"].ToString();
                    typeObject.NameImage = item["image_name"].ToString();

                    result.Add(typeObject);
                }

            } catch(Exception)
            {
                throw;
            }

            return result;
        }
    }
}
