using System;

namespace Server
{
    /// <summary>
    /// A self-balancing binary search tree.
    /// </summary>
    /// <typeparam name="T"> The generic type. Must implement IComparable T. </typeparam>
    internal class AvlTree<T> where T : IComparable<T>
    {
        #region Properties
        /// <summary>
        /// The root of this tree.
        /// </summary>
        public AvlNode<T> Root { get; private set; }

        /// <summary>
        /// The number of nodes in this tree.
        /// </summary>
        public int Count { get; private set; }
        #endregion

        #region Constructors
        public AvlTree()
        {
            Root = null;
            Count = 0;
        }
        #endregion

        #region Public Methods
        #region Insertion
        /// <summary>
        /// Inserts value into the tree as a new newNode.
        /// </summary>
        /// <param name="value"> The value to insert. </param>
        public void Insert(T value)
        {
            Root = Insert(Root, new AvlNode<T>(value));
        }
        #endregion

        #region Finding
        public AvlNode<T> FindMin()
        {
            var current = Root;
            while (current.LeftChild != null)
            {
                current = current.LeftChild;
            }

            return current;
        }

        public AvlNode<T> FindMax()
        {
            var current = Root;
            while (current.RightChild != null)
            {
                current = current.RightChild;
            }

            return current;
        }

        //public User FindUserId(int key)
        //{
        //    var current = Root;

        //    if (current.Value)
        //    {
        //    }
        //}
        #endregion
        #endregion

        #region Private Methods
        /// <summary>
        /// Inserts a new node underneath the given root and re-balances the tree.
        /// </summary>
        /// <param name="root"> The root node. </param>
        /// <param name="newNode"> The new node to insert. </param>
        /// <returns> The new root of the tree after potential re-balancing. </returns>
        private AvlNode<T> Insert(AvlNode<T> root, AvlNode<T> newNode)
        {
            if (root == null)
            {
                root = newNode;
                Count++;
            }
            else
            {
                root.ResetHeight();
                int compareResult = root.Value.CompareTo(newNode.Value);
                if (compareResult > 0)
                {
                    root.LeftChild = Insert(root.LeftChild, newNode);
                }
                else if (compareResult < 0)
                {
                    root.RightChild = Insert(root.RightChild, newNode);
                }
                else
                {
                    throw new ArgumentException("Trying to insert duplicate node.");
                }
            }

            root = ReBalanceFrom(root);
            return root;
        }

        private AvlNode<T> ReBalanceFrom(AvlNode<T> node)
        {
            // TODO: Implementation.
            throw new NotImplementedException();
        }

        // TODO: Implement Insert().
        //private AvlNode Insert(IComparable item, AvlNode newNode)
        //{
        //    if (newNode == null)
        //    {
        //        newNode = new AvlNode(item, null, null);
        //    }
        //    else if (item.CompareTo(newNode.Value) < 0)
        //    {
        //        newNode.LeftChild = Insert(item, newNode.LeftChild);

        //        if (height(newNode.LeftChild) - height(newNode.RightChild) == 2)
        //        {
        //            newNode = RotateWithLeftChild(newNode);
        //        }
        //        else
        //        {
        //            newNode = DoubleWithLeftChild(newNode);
        //        }
        //    }
        //    else if (item.CompareTo(newNode.Value) > 0)
        //    {
        //        // TODO: Implement (s. p 267).
        //    }
        //}
        #endregion
    }
}