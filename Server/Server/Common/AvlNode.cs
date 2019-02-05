using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Server
{
    internal class AvlNode<T>
    {
        #region Properties
        public T Value { get; private set; }
        public AvlNode<T> LeftChild { get; set; } = null;
        public AvlNode<T> RightChild { get; set; } = null;
        public int Height { get; set; } = int.MinValue;
        #endregion

        #region Constructors
        public AvlNode(T value)
        {
            Value = value;
        }

        public AvlNode(T value, AvlNode<T> leftChild, AvlNode<T> rightChild)
        {
            Value = value;
            LeftChild = leftChild;
            RightChild = rightChild;
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

        #region Public Methods
        /// <summary>
        /// Writes this node's value to the console.
        /// </summary>
        public void Display()
        {
            Console.WriteLine(Value.ToString());
        }

        /// <summary>
        /// Resets the height value of this node.
        /// </summary>
        public void ResetHeight()
        {
            Height = int.MinValue;
        }
        #endregion
    }
}