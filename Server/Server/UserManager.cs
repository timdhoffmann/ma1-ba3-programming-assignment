using System.Collections.Generic;

namespace Server
{
    internal class UserManager
    {
        public List<User> UserList { get; private set; } = null;
        public AvlTree<User> UserTree { get; private set; } = new AvlTree<User>();

        #region Constructors
        public UserManager(int testUsers)
        {
            UserList = CreateTestUsers(testUsers);

            foreach (var user in UserList)
            {
                UserTree.Insert(user);
            }
        }
        #endregion

        internal List<User> CreateTestUsers(int amount)
        {
            var testUsers = new List<User>();

            for (int i = 0; i < amount; i++)
            {
                testUsers.Add(new User(i, $"TestUser{i}"));
            }

            return testUsers;
        }
    }
}