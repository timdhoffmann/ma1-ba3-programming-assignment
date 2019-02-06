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

        public AvlNode<T> Find(T key)
        {
            var current = Root;

            // Key and current value have the same sort order.
            while (current.Value.CompareTo(key) != 0)
            {
                // Key precedes current value in sort order.
                if (current.Value.CompareTo(key) > 0)
                {
                    current = current.LeftChild;
                }
                // Key follows current value in sort order.
                else
                {
                    current = current.RightChild;
                }

                if (current == null)
                {
                    return null;
                }
            }

            return current;
        }

        #endregion

        #region Traversal
        /// <summary>
        /// Displays all nodes, starting from the given root, in order.
        /// </summary>
        /// <param name="rootNode"> The root node for the traversal. </param>
        public void DisplayInOrder(AvlNode<T> rootNode)
        {
            if (rootNode == null) return;

            DisplayInOrder(rootNode.LeftChild);
            rootNode.Display();
            DisplayInOrder(rootNode.RightChild);
        }

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

            root = ReBalanceIfNecessary(root);
            return root;
        }

        /// <summary>
        /// Re-balances a node, if necessary.
        /// </summary>
        /// <param name="node">The node to re-balance. </param>
        /// <returns> // TODO: what is returned? </returns>
        private static AvlNode<T> ReBalanceIfNecessary(AvlNode<T> node)
        {
            // Left tree is taller.
            if (node.Balance > 1)
            {
                return ReBalanceLeftSubTree(node);
            }

            // Right tree is taller.
            else if (node.Balance < -1)
            {
                return ReBalanceRightSubTree(node);
            }

            // No balancing needed.
            return node;
        }

        /// <summary>
        /// Performs the rotations necessary to balance a parent.
        /// </summary>
        /// <param name="parent"> The parent node to re-balance. </param>
        private static AvlNode<T> ReBalanceLeftSubTree(AvlNode<T> parent)
        {
            // Converts a Root -> Left -> Right sub tree
            // into a Root -> Left -> Left sub tree maintaining order
            // so that we can do the standard rotate.
            if (parent.LeftChild.Balance < 0)
                parent.LeftChild = parent.LeftChild.RotateLeft();

            return parent.RotateRight();
        }

        /// <summary>
        /// Performs the rotations necessary to balance a parent.
        /// </summary>
        /// <param name="parent"> The parent node to re-balance. </param>
        private static AvlNode<T> ReBalanceRightSubTree(AvlNode<T> parent)
        {
            // Converts a Root -> Right -> Left sub tree
            // Into a Root -> Right -> Right sub tree maintaining order
            // So that we can do the standard rotate
            if (parent.RightChild.Balance > 0)
                parent.RightChild = parent.RightChild.RotateRight();

            return parent.RotateLeft();
        }
        #endregion
    }
}