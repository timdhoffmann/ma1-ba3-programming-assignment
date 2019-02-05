using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Server
{
    internal class AvlNode<T>
    {
        #region Properties
        public T Value { get; private set; }
        public AvlNode<T> LeftChild { get; set; }
        public AvlNode<T> RightChild { get; set; }
        #endregion

        #region Fields
        private int _height = 0;
        #endregion

        #region Constructors
        public AvlNode(T value)
        {
            Value = value;
            LeftChild = null;
            RightChild = null;
            _height = int.MinValue; // TODO: Check if necessary.
        }

        // TODO: Check if this constructor is necessary.
        //public AvlNode(T value, AvlNode<T> leftNode, AvlNode<T> rightNode)
        //{
        //    Value = value;
        //    LeftChild = leftNode;
        //    RightChild = rightNode;
        //    _height = 0;
        //}
        #endregion

        //public int CompareTo(object obj)
        //{
        //    // TODO: Implement CompareTo().

        //    switch (obj)
        //    {
        //        case null:
        //            return 1;

        //        case AvlNode otherNode:
        //            return Value.CompareTo(otherNode.Value);

        //        default:
        //            throw new ArgumentException("Object is not an AvlNode.");
        //    }
        //}

        public void Display()
        {
            Console.WriteLine(Value.ToString());
        }
    }
}