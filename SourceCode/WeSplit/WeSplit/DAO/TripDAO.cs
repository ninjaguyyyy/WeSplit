using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeSplit.DTO;
using WeSplit.Helpers;
using static WeSplit.Helpers.IEnum;

namespace WeSplit.DAO
{
    class TripDAO
    {

        public static List<Trip> GetTrips(int perPage, int pageCurrent, StatusFilter statusFilter, string searchKey)
        {
            var result = new List<Trip>();

            string jsonFilePath = "./Data/trips.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray trips = JArray.Parse(json);

                string key = searchKey.Trim().ToLower();
                if (key != "")
                {
                    trips = new JArray(trips.Where(trip => SearchHelper.ConvertToUnSign(trip["name"].ToString().ToLower()) == key));
                }

                if (statusFilter == StatusFilter.FINISH)
                {
                    trips = new JArray(trips.Where(trip => trip["status"].ToString() == "finish"));
                }

                if(statusFilter == StatusFilter.PROGRESS)
                {
                    trips = new JArray(trips.Where(trip => trip["status"].ToString() == "progress"));
                }

                int skipValue = pageCurrent == 1 ? 0 : (pageCurrent - 1) * perPage;
                trips = new JArray(trips.Skip(skipValue).Take(perPage));

                foreach (var trip in trips)
                {
                    Trip tripObject = new Trip();
                    tripObject.Id = trip["Id"].ToString();
                    tripObject.Name = trip["Name"].ToString();
                    tripObject.MainImage = trip["MainImage"].ToString();
                    tripObject.StartDate = trip["StartDate"].ToString();
                    tripObject.EndDate = trip["EndDate"].ToString();
                    tripObject.Transport = trip["Transport"].ToString();
                    tripObject.Status = trip["Status"].ToString();

                    tripObject.Members = new List<Member>();
                    JArray Members = JArray.Parse(trip["Members"].ToString());
                    foreach (var member in Members)
                    {
                        Member memberItem = new Member()
                        {
                            Name = member["Name"].ToString(),
                            Id = member["Id"].ToString(),
                            Donation = member["Donation"].ToString(),
                            Avatar = member["Avatar"].ToString()
                        };
                        tripObject.Members.Add(memberItem);
                    }

                    tripObject.Expenses = new List<Expense>();
                    JArray Expenses = JArray.Parse(trip["Expenses"].ToString());
                    foreach (var item in Expenses)
                    {
                        Expense expenseItem = new Expense()
                        {
                            Name = item["Name"].ToString(),
                            Id = item["Id"].ToString(),
                            Cost = item["Cost"].ToString()
                        };
                        tripObject.Expenses.Add(expenseItem);
                    }

                    tripObject.Places = new List<Place>();
                    JArray Places = JArray.Parse(trip["Places"].ToString());
                    foreach (var item in Places)
                    {
                        Place placeItem = new Place()
                        {
                            Name = item["Name"].ToString(),
                            Id = item["Id"].ToString(),
                            Avatar = item["Avatar"].ToString(),
                            Start = item["Start"].ToString(),
                            End = item["End"].ToString(),   
                            Address = item["Address"].ToString(),
                            Description = item["Description"].ToString(),
                            
                        };
                        tripObject.Places.Add(placeItem);
                    }

                    result.Add(tripObject);
                }
            }
            catch(Exception)
            {
                throw;
            }

            return result;
        }

        public static int CountTrips(StatusFilter statusFilter, string searchKey)
        {
            var result = 0;

            string jsonFilePath = "./Data/trips.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray trips = JArray.Parse(json);

                string key = searchKey.Trim();
                if(key != "")
                {
                    trips = new JArray(trips.Where(trip => trip["name"].ToString() == key));
                }

                if (statusFilter == StatusFilter.FINISH)
                {
                    trips = new JArray(trips.Where(trip => trip["status"].ToString() == "finish"));
                }
                if (statusFilter == StatusFilter.PROGRESS)
                {
                    trips = new JArray(trips.Where(trip => trip["status"].ToString() == "progress"));
                }

                result = trips.Count();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static bool InsertTrip(Trip tripEntered)
        {
            var result = true;

            string jsonFilePath = "./Data/trips.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray trips = JArray.Parse(json);

                var newTripString = Newtonsoft.Json.JsonConvert.SerializeObject(tripEntered);
                var newTripJObject = JObject.Parse(newTripString);
                trips.Add(newTripJObject);

                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(trips,
                               Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFilePath, newJsonResult);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static Trip GetById(string id)
        {
            string jsonFilePath = "./Data/trips.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray trips = JArray.Parse(json);

                foreach (var trip in trips)
                {
                    if (trip["Id"].ToString() == id)
                    {
                        Trip tripObject = new Trip();
                        tripObject.Id = trip["Id"].ToString();
                        tripObject.Name = trip["Name"].ToString();
                        tripObject.MainImage = trip["MainImage"].ToString();
                        tripObject.StartDate = trip["StartDate"].ToString();
                        tripObject.EndDate = trip["EndDate"].ToString();
                        tripObject.Transport = trip["Transport"].ToString();
                        tripObject.Status = trip["Status"].ToString();

                        tripObject.Members = new List<Member>();
                        JArray Members = JArray.Parse(trip["Members"].ToString());
                        foreach(var member in Members)
                        {
                            Member memberItem = new Member()
                            {
                                Name = member["Name"].ToString(),
                                Id = member["Id"].ToString(),
                                Donation = member["Donation"].ToString(),
                                Avatar = member["Avatar"].ToString()
                            };
                            tripObject.Members.Add(memberItem);
                        }

                        tripObject.Expenses = new List<Expense>();
                        JArray Expenses = JArray.Parse(trip["Expenses"].ToString());
                        foreach (var item in Expenses)
                        {
                            Expense expenseItem = new Expense()
                            {
                                Name = item["Name"].ToString(),
                                Id = item["Id"].ToString(),
                                Cost = item["Cost"].ToString()
                            };
                            tripObject.Expenses.Add(expenseItem);
                        }

                        tripObject.Places = new List<Place>();
                        JArray Places = JArray.Parse(trip["Places"].ToString());
                        foreach (var item in Places)
                        {
                            Place placeItem = new Place()
                            {
                                Name = item["Name"].ToString(),
                                Id = item["Id"].ToString(),
                                Avatar = item["Avatar"].ToString(),
                                Start = item["Start"].ToString(),
                                End = item["End"].ToString(),
                                Address = item["Address"].ToString(),
                                Description = item["Description"].ToString(),

                            };
                            tripObject.Places.Add(placeItem);
                        }

                        return tripObject;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }

        public static bool InsertPlaces(string idTrip, Place placeEntered)
        {
            var result = true;
            string jsonFilePath = "./Data/trips.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray trips = JArray.Parse(json);
                var tripDetail = trips.FirstOrDefault(obj => obj["Id"].Value<String>() == idTrip);
                JArray placesArrary = (JArray)tripDetail["Places"];

                var newPlaceString = Newtonsoft.Json.JsonConvert.SerializeObject(placeEntered);
                var newPlaceJObject = JObject.Parse(newPlaceString);
                placesArrary.Add(newPlaceJObject);

                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(trips,
                               Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFilePath, newJsonResult);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static bool RemovePlace(string idTrip, string idPlace)
        {
            var result = true;

            string jsonFilePath = "./Data/trips.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray trips = JArray.Parse(json);
                var tripDetail = trips.FirstOrDefault(obj => obj["Id"].Value<String>() == idTrip);
                JArray placessArray = (JArray)tripDetail["Places"];

                var placeToDelete = placessArray.FirstOrDefault(obj => obj["Id"].Value<string>() == idPlace);
                placessArray.Remove(placeToDelete);

                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(trips,
                               Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFilePath, newJsonResult);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static Place GetPlaceById(string idTrip, string idPlace)
        {
            var result = new Place();

            string jsonFilePath = "./Data/trips.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray trips = JArray.Parse(json);
                var tripDetail = trips.FirstOrDefault(obj => obj["Id"].Value<String>() == idTrip);
                JArray placesArray = (JArray)tripDetail["Places"];

                var placeToGet = placesArray.FirstOrDefault(obj => obj["Id"].Value<string>() == idPlace);
                result.Id = placeToGet["Id"].ToString();
                result.Name = placeToGet["Name"].ToString();
                result.Avatar = placeToGet["Avatar"].ToString();
                result.Address = placeToGet["Address"].ToString();
                result.Description = placeToGet["Description"].ToString();
                result.Start = placeToGet["Start"].ToString();
                result.End = placeToGet["End"].ToString();



            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static bool InsertMember(string idTrip, Member memberEntered)
        {
            var result = true;
            string jsonFilePath = "./Data/trips.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray trips = JArray.Parse(json);
                var tripDetail = trips.FirstOrDefault(obj => obj["Id"].Value<String>() == idTrip);
                JArray memberArrary = (JArray)tripDetail["Members"];

                var newMemberString = Newtonsoft.Json.JsonConvert.SerializeObject(memberEntered);
                var newMemberJObject = JObject.Parse(newMemberString);
                memberArrary.Add(newMemberJObject);

                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(trips,
                               Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFilePath, newJsonResult);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static bool RemoveMember(string idTrip, string idMember)
        {
            var result = true;

            string jsonFilePath = "./Data/trips.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray trips = JArray.Parse(json);
                var tripDetail = trips.FirstOrDefault(obj => obj["Id"].Value<String>() == idTrip);
                JArray membersArray = (JArray)tripDetail["Members"];

                var memberToDelete = membersArray.FirstOrDefault(obj => obj["Id"].Value<string>() == idMember);
                membersArray.Remove(memberToDelete);

                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(trips,
                               Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFilePath, newJsonResult);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static Member GetMemberById(string idTrip, string idMember)
        {
            var result = new Member();

            string jsonFilePath = "./Data/trips.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray trips = JArray.Parse(json);
                var tripDetail = trips.FirstOrDefault(obj => obj["Id"].Value<String>() == idTrip);
                JArray membersArray = (JArray)tripDetail["Members"];

                var memberToGet = membersArray.FirstOrDefault(obj => obj["Id"].Value<string>() == idMember);
                result.Id = memberToGet["Id"].ToString();
                result.Name = memberToGet["Name"].ToString();
                result.Avatar = memberToGet["Avatar"].ToString();
                result.Donation = memberToGet["Donation"].ToString();


                
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }


        public static bool InsertExpense(string idTrip, Expense expenseEntered)
        {
            var result = true;
            string jsonFilePath = "./Data/trips.json";
            var json = File.ReadAllText(jsonFilePath);

            try
            {
                JArray trips = JArray.Parse(json);
                var tripDetail = trips.FirstOrDefault(obj => obj["Id"].Value<String>() == idTrip);
                JArray expensesArrary = (JArray)tripDetail["Expenses"];

                var newExpenseString = Newtonsoft.Json.JsonConvert.SerializeObject(expenseEntered);
                var newExpenseJObject = JObject.Parse(newExpenseString);
                expensesArrary.Add(newExpenseJObject);

                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(trips,
                               Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFilePath, newJsonResult);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}
