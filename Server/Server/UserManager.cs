using System;
using System.Collections.Generic;

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
        public List<User> UserList { get; private set; } = null;
        public AvlTree<User> UserTree { get; private set; } = new AvlTree<User>();
        #endregion

        private readonly int _testUserCount = 10;

        #region Public Methods

        public void DisplayRegisteredUsers()
        {
            Console.WriteLine("Registered users:");
            UserTree.DisplayInOrder(UserTree.Root);
            Console.WriteLine();
        }

        public User FindUserById(int id)
        {
            var userNode = UserTree.Find(new User(id, string.Empty));

            // Return value if exists, null otherwise.
            return userNode?.Value;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Gets all registered users from a persistent source.
        /// </summary>
        private void GetRegisteredUsers()
        {
            UserList = CreateTestUsers(_testUserCount);

            foreach (var user in UserList)
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