using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobloxObjects
{
    public class User
    {
        public string description { get; set; }
        public DateTime created { get; set; }
        public bool isBanned { get; set; }
        public string externalAppDisplayName { get; set; }
        public bool hasVerifiedBadge { get; set; }
        public double id { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public bool isOnline { get; set; }
    }

    public class UserGroup
    {
        public Group Group { get; set; }
        public Role Role { get; set; }
        public bool IsNotificationsEnabled { get; set; }
    }

    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MemberCount { get; set; }
        public bool HasVerifiedBadge { get; set; }
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }
    }

    public class Collectible
    {
        public int userAssetId { get; set; }
        public object serialNumber { get; set; }
        public int assetId { get; set; }
        public string name { get; set; }
        public int recentAveragePrice { get; set; }
        public int? originalPrice { get; set; }
        public object assetStock { get; set; }
        public int buildersClubMembershipType { get; set; }
        public bool isOnHold { get; set; }
    }
}
