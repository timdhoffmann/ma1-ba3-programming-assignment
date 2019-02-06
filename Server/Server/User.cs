using System;

namespace Server
{
    internal class User : IComparable
    {
        public int Id { get; private set; } = 0;
        public string Name { get; private set; } = string.Empty;

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

                default:
                    throw new ArgumentException("Other object is not a user.");
            }
        }

        public override string ToString()
        {
            return $"{Name} | Id: {Id}";
        }
    }
}