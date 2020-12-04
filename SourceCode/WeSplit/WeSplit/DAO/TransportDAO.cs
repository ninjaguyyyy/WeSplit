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
    class TransportDAO
    {
        public static List<Transport> Get()
        {
            var result = new List<Transport>();

            string jsonFilePath = "./Data/transports.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray transports = JArray.Parse(json);

                foreach (var item in transports)
                {
                    Transport transportObject = new Transport();
                    transportObject.Id = item["id"].ToString();
                    transportObject.Name = item["name"].ToString();
                    transportObject.NameImage = item["image_name"].ToString();

                    result.Add(transportObject);
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static Transport GetById(string id)
        {
            string jsonFilePath = "./Data/transports.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray transports = JArray.Parse(json);

                foreach (var item in transports)
                {
                    if(item["id"].ToString() == id)
                    {
                        Transport transportObject = new Transport();
                        transportObject.Id = item["id"].ToString();
                        transportObject.Name = item["name"].ToString();
                        transportObject.NameImage = item["image_name"].ToString();

                        return transportObject;
                    }
                    

                }

            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }
    }
}
