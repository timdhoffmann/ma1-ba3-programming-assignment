using System;

namespace Server
{
    internal class AvlTree
    {
        public AvlNode Root { get; private set; }

        public AvlTree()
        {
            Root = null;
        }

        // TODO: Implement Insert().
        //private AvlNode Insert(IComparable item, AvlNode node)
        //{
        //    if (node == null)
        //    {
        //        node = new AvlNode(item, null, null);
        //    }
        //    else if (item.CompareTo(node.User) < 0)
        //    {
        //        node.LeftNode = Insert(item, node.LeftNode);

        //        if (height(node.LeftNode) - height(node.RightNode) == 2)
        //        {
        //            node = RotateWithLeftChild(node);
        //        }
        //        else
        //        {
        //            node = DoubleWithLeftChild(node);
        //        }
        //    }
        //    else if (item.CompareTo(node.User) > 0)
        //    {
        //        // TODO: Implement (s. p 267).
        //    }
        //}

        public User FindMin()
        {
            var current = Root;
            while (current.LeftNode != null)
            {
                current = current.LeftNode;
            }

            return current.User;
        }

        public User FindMax()
        {
            var current = Root;
            while (current.RightNode != null)
            {
                current = current.RightNode;
            }

            return current.User;
        }

        public User FindUserId(int key)
        {
            var current = Root;

            if (current.User)
            {
            }
        }
    }
}