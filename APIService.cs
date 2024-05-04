using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using RobloxObjects;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace APIService
{
    public class APICaller
    {
        internal static async Task<string> RetrieveData(string APIType, string Path)
        {
            using (HttpClient client = new HttpClient())
            {
                string URI = "https://" + APIType + ".roblox.com/v1" + Path;
                Console.WriteLine("Query to: " + URI);
                try
                {
                    HttpResponseMessage responseMessage = await client.GetAsync(URI);
                    responseMessage.EnsureSuccessStatusCode();

                    string responseJSON = await responseMessage.Content.ReadAsStringAsync();

                    return responseJSON;

                } catch (HttpRequestException)
                {
                    Console.WriteLine("HTTP Error! Please try again.");
                    return "error";
                }
            }
        }

        internal static Task<List<User>> JSON_GetUserList(string jsonData)
        {
            JObject jsonObject = JObject.Parse(jsonData);
            JToken dataToken = jsonObject["data"];
            List<User> users = dataToken?.ToObject<List<User>>();
            return Task.FromResult(users);
        }

        public async Task<List<User>> GetFriends(double UserId)
        {
            string jsonData = await RetrieveData("friends", "/users/" + UserId + "/friends");

            return await JSON_GetUserList(jsonData);
        }

        public async Task<List<User>> GetFollowers(double UserId)
        {
            string jsonData = await RetrieveData("friends", "/users/" + UserId + "/followers?limit=100");

            return await JSON_GetUserList(jsonData);
        }

        public async Task<List<User>> GetFollowing(double UserId)
        {
            string jsonData = await RetrieveData("friends", "/users/" + UserId + "/followings?limit=100");

            return await JSON_GetUserList(jsonData);
        }

        public async Task<User> GetUser(double UserId) {
            string jsonData = await RetrieveData("users", "/users/" + UserId);
            User user = JsonConvert.DeserializeObject<User>(jsonData);
            return user;
        }

        public async Task<List<UserGroup>> GetGroups(double userId)
        {
            string jsonData = await RetrieveData("groups", "/users/" + userId + "/groups/roles");
            JObject JsonObject = JObject.Parse(jsonData);
            JToken token = JsonObject["data"];
            List<UserGroup> groups = token?.ToObject<List<UserGroup>>();
            return groups;
        }

        public async Task<List<Collectible>> GetCollectibles(double userId)
        {
            string jsonData = await RetrieveData("inventory", "/users/" + userId + "/assets/collectibles");
            JObject JsonObject = JObject.Parse(jsonData);
            JToken token = JsonObject["data"];
            List<Collectible> collectibles = token?.ToObject<List<Collectible>>();
            return collectibles;
        }
    }
}
