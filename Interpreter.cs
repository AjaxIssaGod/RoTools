using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIService;
using RobloxObjects;

namespace RoTools
{
    public class Interpreter
    {
        internal static APICaller API = new APICaller();
        
        public static Task PrintUserList(List<User> list)
        {
            foreach (User user in list)
            {
                string onlineStatus = "";
                if (user.isOnline)
                {
                    onlineStatus = " [ONLINE]";
                }
                else if (user.isBanned)
                {
                    onlineStatus = " [BANNED]";
                } else 
                {
                    onlineStatus = "";
                }

                Console.WriteLine(user.id + " - " + user.displayName + " (" + user.name + ")" + onlineStatus);
            }

            return Task.CompletedTask;
        }

        public static async Task RcvCommand(string command)
        {
            string[] args = command.Split(' ');

            switch (args[0])
            {
                case "list":
                    if (args.Length != 3)
                    {
                        Console.WriteLine("Invalid number of arguments (3 required)!");
                        break;
                    }

                    if (args[1] == "friends")
                    {
                        var userId = Convert.ToDouble(args[2]);
                        var friendsList = await API.GetFriends(Convert.ToDouble(userId));
                        Console.WriteLine("Retrieving friends list for USERID (" + userId.ToString() + ") [Total Shown: " + friendsList.Count.ToString() + "]");
                        await PrintUserList(friendsList);
                    }
                    else if (args[1] == "followers")
                    {
                        var userId = Convert.ToDouble(args[2]);
                        var followersList = await API.GetFollowers(Convert.ToDouble(userId));
                        Console.WriteLine("Retrieving followers list for USERID (" + userId.ToString() + ") [Total Shown: " + followersList.Count.ToString() + "]");
                        await PrintUserList(followersList);
                        if (followersList.Count == 100)
                        {
                            Console.WriteLine("NOTE: Due to limits in the ROBLOX API, the maximum number of followers presentable is 100! Some users may not show on this list!");
                        }
                    }
                    else if (args[1] == "following")
                    {
                        var userId = Convert.ToDouble(args[2]);
                        var followingList = await API.GetFollowing(userId);
                        Console.WriteLine("Retrieving following list for USERID (" + userId.ToString() + ") [Total Shown: " + followingList.Count.ToString() + "]");
                        await PrintUserList(followingList);
                        if (followingList.Count == 100)
                        {
                            Console.WriteLine("NOTE: Due to limits in the ROBLOX API, the maximum number of following users presentable is 100! Some users may not show on this list!");
                        }
                    }
                    else if (args[1] == "groups")
                    {
                        var uid = Convert.ToDouble(args[2]);
                        var groups = await API.GetGroups(uid);
                        Console.WriteLine("Retrieving group information for USERID(" + uid.ToString() + ")");
                        foreach (var group in groups)
                        {
                            string verified = "";
                            string notifications = "Disabled";
                            if (group.Group.HasVerifiedBadge)
                            {
                                verified = "(Verified) ";
                            }

                            if (group.IsNotificationsEnabled)
                            {
                                notifications = "Enabled";
                            }

                            Console.WriteLine("======================================");
                            Console.WriteLine(verified + group.Group.Name + " - (ID: " + group.Group.Id.ToString() + ")");
                            Console.WriteLine("Member Count: " + group.Group.MemberCount.ToString());
                            Console.WriteLine("Notifications Enabled: " + notifications);
                            Console.WriteLine("\nRank: " + group.Role.Name + " (" + group.Role.Rank.ToString() + ")");
                        }
                    }
                    else if (args[1] == "collectibles")
                    {
                        var uid = Convert.ToDouble(args[2]);
                        var collectibles = await API.GetCollectibles(uid);
                        Console.WriteLine("Gathering collectibles information for USERID(" + uid.ToString() + ")");
                        if (collectibles.Count >= 100)
                        {
                            Console.WriteLine("Please note that collectibles queries are limited to 100 items! Some items may not apprear on this list!");
                        }

                        int totalValue = 0;

                        foreach (Collectible collectible in collectibles)
                        {
                            totalValue += collectible.recentAveragePrice;
                            Console.WriteLine("======================================");
                            Console.WriteLine(collectible.name + "(ID: " + collectible.assetId + ")");
                            Console.WriteLine("Serial Number: " + collectible.serialNumber);
                            Console.WriteLine("Recent Avg. Price: " + collectible.recentAveragePrice + " R$ (Original Price: " + collectible.originalPrice + " R$)");
                            Console.WriteLine("Catalog Link: https://www.roblox.com/catalog/" + collectible.assetId);
                        }

                        Console.WriteLine("\n\nCollectibles search completed, returned " + collectibles.Count().ToString() + " items with a total value of " + totalValue.ToString() + " R$ (based on recent average price, according to Roblox).");
                    }
                    else
                    {
                        Console.WriteLine("Invalid arg #2: Options <friends/followers/following/collectibles/groups>");
                    }
                    break;
                case "user":
                    if (args.Count() > 2)
                    {
                        Console.WriteLine("Invalid number of arguments (2 required)!");
                        break;
                    }

                    var UID = Convert.ToDouble(args[1]);
                    Console.WriteLine("Getting user information for USERID(" + UID.ToString() + ")");
                    User user = await API.GetUser(UID);
                    if (user != null && user.name != "")
                    {
                        Console.WriteLine("Printing user data for USER (" + user.name + ")");
                        Console.WriteLine("======================================");
                        Console.WriteLine(user.displayName + "(@" + user.name + ")");
                        Console.WriteLine("Date of Registration: " + user.created.ToString());
                        Console.WriteLine("ID #: " + user.id.ToString() + " | Profile Link: https://www.roblox.com/users/" + user.id + "/profile");

                        string verifiedStatus = "None";
                        string bannedStatus = "None";
                        string onlineStatus = "Offline";
                        if (user.hasVerifiedBadge)
                        {
                            verifiedStatus = "Verified Badge";
                        }

                        if (user.isBanned)
                        {
                            bannedStatus = "Banned";
                        }

                        if (user.isOnline)
                        {
                            onlineStatus = "Online";
                        }

                        Console.WriteLine("\nOnline Status: " + onlineStatus + " | Banned Status: " + bannedStatus + " | Verified Status: " + verifiedStatus);
                        Console.WriteLine("\nBio:" + user.description);
                        Console.WriteLine("Report concluded. Try other commands to learn more about this user.");
                        Console.WriteLine("======================================");
                    } else
                    {
                        Console.WriteLine("No user information found!");
                    }
                    break;
            }
        }
    }
}
