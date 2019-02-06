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

        /// <summary>
        /// This node's height (max. height of sub-trees + 1).
        /// </summary>
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

        /// <summary>
        /// This node's balance (left child height - right child height).
        /// </summary>
        public int Balance => GetNodeBalance();

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

        /// <summary>
        /// Rotates this node's sub-tree counter-clockwise.
        /// </summary>
        /// <returns> The new root of the tree. </returns>
        internal AvlNode<T> RotateLeft()
        {
            var pivot = RightChild;

            RightChild = pivot.LeftChild;
            pivot.LeftChild = this;

            // Fixes heights.
            pivot.ResetHeight();
            ResetHeight();

            return pivot;
        }

        /// <summary>
        /// Rotates this node's sub-tree clockwise.
        /// </summary>
        /// <returns> The new root of the tree. </returns>
        internal AvlNode<T> RotateRight()
        {
            var pivot = LeftChild;

            LeftChild = pivot.RightChild;
            pivot.RightChild = this;

            // Fixes heights.
            pivot.ResetHeight();
            ResetHeight();

            return pivot;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the height of this node.
        /// </summary>
        /// <returns> This node's height or 0 for a tree with only a root. </returns>
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

        /// <summary>
        /// Gets the balance of this node (left child height - right child height).
        /// </summary>
        /// <returns> This node's balance. </returns>
        private int GetNodeBalance()
        {
            return GetChildNodeHeight(LeftChild) - GetChildNodeHeight(RightChild);
        }

        #endregion
    }
}