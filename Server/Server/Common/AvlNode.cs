using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Server
{
    internal class AvlNode<T>
    {
        #region Properties

        /// <summary>
        /// The data of type T stored in this node.
        /// </summary>
        public T Value { get; private set; }
        public AvlNode<T> LeftChild { get; set; } = null;
        public AvlNode<T> RightChild { get; set; } = null;

        public int Height
        {
            get
            {
                if (_height == int.MinValue)
                {
                    _height = GetNodeHeight();
                }

                return _height;
            }
            set => _height = value;
        }

        #endregion

        #region Fields
        private int _height = int.MinValue;
        #endregion

        #region Constructors

        public AvlNode(T value)
        {
            Value = value;
            Height = int.MinValue;
        }

        public AvlNode(T value, AvlNode<T> leftChild, AvlNode<T> rightChild)
        {
            Value = value;
            Height = int.MinValue;
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

        #region Private Methods
        /// <summary>
        /// Gets the height of the node.
        /// </summary>
        /// <returns> The height of the node or 0 for a tree with only a root. </returns>
        private int GetNodeHeight()
        {
            return System.Math.Max(GetChildNodeHeight(LeftChild), GetChildNodeHeight(RightChild)) + 1;
        }

        /// <summary>
        /// Returns the height of a child node.
        /// </summary>
        /// <typeparam name="T"> The data type stored in the node. </typeparam>
        /// <param name="childNode"> The child node. </param>
        /// <returns> The cached node height or -1 if the node is null. </returns>
        private static int GetChildNodeHeight(AvlNode<T> childNode)
        {
            return (childNode == null) ? -1 : childNode.Height;
        }
        #endregion
    }
}