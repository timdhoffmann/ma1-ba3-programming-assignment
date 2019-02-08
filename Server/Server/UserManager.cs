using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Server
{
    internal class UserManager
    {
        #region Constructors

        public UserManager()
        {
            GetRegisteredUsers();
        }

        #endregion

        #region Properties

        public AvlTree<User> UserTree { get; private set; } = new AvlTree<User>();

        #endregion

        #region Fields

        /// <summary>
        /// Lock object for the user list.
        /// </summary>
        private readonly object _userListLock = new object();

        /// <summary>
        /// Lock object for a user.
        /// </summary>
        private readonly object _userLock = new object();

        /// <summary>
        /// Lock object for the user tree.
        /// </summary>
        private readonly object _userTreeLock = new object();

        /// <summary>
        /// The amount of test users to create.
        /// </summary>
        private readonly int _testUserCount = 10;

        /// <summary>
        /// Simulates a persistent data base outside the application.
        /// </summary>
        private List<User> _testUserDataBase = null;

        /// <summary>
        /// All connected users by TCP Client.
        /// </summary>
        private readonly Dictionary<TcpClient, User> _connectedUsersByTcpClient = new Dictionary<TcpClient, User>();

        /// <summary>
        /// Lock object for the user tree.
        /// </summary>
        private readonly object _connectedUsersByTcpClientLock = new object();

        #endregion

        #region Public Methods

        /// <summary>
        /// Displays all registered users in ascending order.
        /// </summary>
        public void DisplayRegisteredUsers()
        {
            lock (_userTreeLock)
            {
                Console.WriteLine("Registered users [InOrder from AVL Tree]:");
                UserTree.DisplayInOrder(UserTree.Root);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Finds a user by id.
        /// Thread safe.
        /// </summary>
        /// <param name="id"> The id to look for. </param>
        /// <returns> The user, if it exists, or null. </returns>
        public User FindUserById(int id)
        {
            lock (_userTreeLock)
            {
                var userNode = UserTree.Find(new User(id, string.Empty));

                // Return value if userNode exists, null otherwise.
                return userNode?.Value;
            }
        }

        public void SetUserTcpClient(User user, TcpClient tcpClient)
        {
            lock (_userLock)
            {
                user.TcpClient = tcpClient;
            }

            lock (_connectedUsersByTcpClientLock)
            {
                _connectedUsersByTcpClient.Add(tcpClient, user);
            }
        }

        public void RemoveConnectedUserByClient(TcpClient client)
        {
            User user = null;

            lock (_connectedUsersByTcpClientLock)
            {
                if (_connectedUsersByTcpClient.ContainsKey(client))
                {
                    user = _connectedUsersByTcpClient[client];

                    _connectedUsersByTcpClient.Remove(client);
                }
            }

            if (user == null) return;

            lock (_userLock)
            {
                user.TcpClient = null;
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Gets all registered users from a persistent source
        /// outside of the application.
        /// </summary>
        private void GetRegisteredUsers()
        {
            _testUserDataBase = CreateTestUsers(_testUserCount);

            foreach (var user in _testUserDataBase)
            {
                UserTree.Insert(user);
            }
        }

        /// <summary>
        /// Creates a set of test users.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private static List<User> CreateTestUsers(int amount)
        {
            var testUsers = new List<User>();

            for (int i = 0; i < amount; i++)
            {
                testUsers.Add(new User(i, $"TestUser{i}"));
            }

            return testUsers;
        }
        #endregion
    }
}