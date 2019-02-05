using System;

namespace Server
{
    /// <summary>
    /// A self-balancing binary search tree.
    /// </summary>
    /// <typeparam name="T"> The generic type. Must implement IComparable T. </typeparam>
    internal class AvlTree<T> where T : IComparable
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
            // Common implementation.
            //Root = Insert(Root, new AvlNode<T>(value));

            // McMillan.
            Root = Insert(value, Root);
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
        #region Common
        /// <summary>
        /// Inserts a new node underneath the given root and re-balances the tree.
        /// </summary>
        /// <param name="root"> The root node. </param>
        /// <param name="newNode"> The new node to insert. </param>
        /// <returns> The new root of the tree after potential re-balancing. </returns>
        //private AvlNode<T> Insert(AvlNode<T> root, AvlNode<T> newNode)
        //{
        //    if (root == null)
        //    {
        //        root = newNode;
        //        Count++;
        //    }
        //    else
        //    {
        //        root.ResetHeight();
        //        int compareResult = root.Value.CompareTo(newNode.Value);
        //        if (compareResult > 0)
        //        {
        //            root.LeftChild = Insert(root.LeftChild, newNode);
        //        }
        //        else if (compareResult < 0)
        //        {
        //            root.RightChild = Insert(root.RightChild, newNode);
        //        }
        //        else
        //        {
        //            throw new ArgumentException("Trying to insert duplicate node.");
        //        }
        //    }

        //    root = ReBalanceFrom(root);
        //    return root;
        //}

        //private AvlNode<T> ReBalanceFrom(AvlNode<T> node)
        //{
        //    // TODO: Implementation.
        //    throw new NotImplementedException();
        //}

        #endregion

        #region McMillan
        // TODO: Implement Insert().
        private AvlNode<T> Insert(T item, AvlNode<T> root)
        {
            if (root == null)
            {
                root = new AvlNode<T>(item, null, null);
            }
            else if (item.CompareTo(root.Value) < 0)
            {
                root.LeftChild = Insert(item, root.LeftChild);

                if (root.LeftChild.Height - root.RightChild.Height == 2)
                {
                    root = RotateWithLeftChild(root);
                }
                else
                {
                    root = DoubleWithLeftChild(root);
                }
            }
            else if (item.CompareTo(root.Value) > 0)
            {
                root.RightChild = Insert(item, root.RightChild);

                if (root.RightChild.Height - root.LeftChild.Height == 2)
                {
                    if (item.CompareTo(root.RightChild.Value) > 0)
                    {
                        root = RotateWithRightChild(root);
                    }
                    else
                    {
                        root = DoubleWithRightChild(root);
                    }
                }
            }
            // Duplicate value.
            else
            {
                throw new ArgumentException("Trying to insert duplicate node.");
            }

            root.Height = Math.Max(root.LeftChild.Height, root.RightChild.Height) + 1;

            return root;
        }

        private AvlNode<T> RotateWithLeftChild(AvlNode<T> n2)
        {
            AvlNode<T> n1 = n2.LeftChild;
            n2.LeftChild = n1.RightChild;
            n1.RightChild = n2;
            n2.Height = Math.Max(n2.LeftChild.Height, n2.RightChild.Height) + 1;
            n1.Height = Math.Max(n1.LeftChild.Height, n2.Height) + 1;
            return n1;
        }
        private AvlNode<T> RotateWithRightChild(AvlNode<T> n1)
        {
            AvlNode<T> n2 = n1.RightChild;
            n1.RightChild = n2.LeftChild;
            n2.LeftChild = n1;
            n1.Height = Math.Max(n1.LeftChild.Height, n1.RightChild.Height) + 1;
            n2.Height = Math.Max(n2.RightChild.Height, n1.Height) + 1;
            return n2;
        }

        private AvlNode<T> DoubleWithLeftChild(AvlNode<T> n3)
        {
            n3.LeftChild = RotateWithRightChild(n3.LeftChild);
            return RotateWithLeftChild(n3);
        }
        private AvlNode<T> DoubleWithRightChild(AvlNode<T> n1)
        {
            n1.RightChild = RotateWithLeftChild(n1.RightChild);
            return RotateWithRightChild(n1);
        }
        #endregion
        #endregion
    }
}