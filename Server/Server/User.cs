using System;
using System.Net.Sockets;

namespace Server
{
    internal class User : IComparable
    {
        public int Id { get; private set; } = 0;
        public string Name { get; private set; }
        public TcpClient TcpClient { get; set; } = null;

        public bool IsConnected => (TcpClient != null);

        #region Constructors

        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }

        #endregion

        public int CompareTo(object obj)
        {
            switch (obj)
            {
                case null:
                    return 1;

                case User otherUser:
                    return Id.CompareTo(otherUser.Id);

                case int id:
                    return Id.CompareTo(id);

                default:
                    throw new ArgumentException("Other object is not a user.");
            }
        }

        public override string ToString()
        {
            var onlineStatus = (IsConnected) ? "logged in" : "-";
            return $"ID: {Id} | NAME: {Name} | STATUS: {onlineStatus}";
        }
    }
}