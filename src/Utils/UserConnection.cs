using System;

namespace WhitePlugin
{
    public class UserConnection
    {
        // The primary key used to identify each connection.
        public uint ConnectionId { get; set; }

        // The user's ID.
        public string UserId { get; set; } = "";

        // The user's type.
        public string UserType { get; set; } = "";

        // The date/time of this connection record.
        public DateTime ConnectionTime { get; set; }

        public UserConnection() {
            
        }
    }
}