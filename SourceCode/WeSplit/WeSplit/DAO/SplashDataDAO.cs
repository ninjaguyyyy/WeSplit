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
    class SplashDataDAO
    {
        public static List<SplashData> GetAll()
        {
            var result = new List<SplashData>();

            string jsonFilePath = "./Data/splash-data.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray data = JArray.Parse(json);

                foreach (var item in data)
                {
                    SplashData dataObject = new SplashData();
                    dataObject.Id = item["id"].ToString();
                    dataObject.Name = item["name"].ToString();
                    dataObject.BackgroundNameFile = item["bg_name"].ToString();
                    dataObject.Description = item["description"].ToString();

                    result.Add(dataObject);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}
