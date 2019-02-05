using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Server
{
    internal class AvlNode : IComparable
    {
        #region Properties
        public User User { get; set; }
        public AvlNode LeftNode { get; set; }
        public AvlNode RightNode { get; set; }
        public int Height { get; set; }
        #endregion

        #region Constructors
        public AvlNode(User user)
        {
            User = user;
            LeftNode = null;
            RightNode = null;
        }

        public AvlNode(User user, AvlNode leftNode, AvlNode rightNode)
        {
            User = user;
            LeftNode = leftNode;
            RightNode = rightNode;
            Height = 0;
        }
        #endregion

        public int CompareTo(object obj)
        {
            // TODO: Implement CompareTo().

            switch (obj)
            {
                case null:
                    return 1;

                case AvlNode otherNode:
                    return User.CompareTo(otherNode.User);

                default:
                    throw new ArgumentException("Object is not an AvlNode.");
            }
        }

        public void Display()
        {
            Console.WriteLine(User.ToString());
        }
    }
}