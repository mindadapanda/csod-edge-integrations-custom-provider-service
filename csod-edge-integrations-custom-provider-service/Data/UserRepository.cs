using csod_edge_integrations_custom_provider_service.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Data
{
    public class UserRepository
    {
        protected LiteRepository Repository;

        public UserRepository(LiteRepository repository)
        {
            Repository = repository;
        }
        
        public IEnumerable<User> GetAll()
        {
            var users = Repository.Fetch<User>();
            return users;
        }

        public User GetUser(int id)
        {
            var user = Repository.SingleOrDefault<User>(x => x.Id == id);
            return user;
        }

        public User GetUserByUsername(string username)
        {
            var user = Repository.SingleOrDefault<User>(x => x.Username.Equals(username));
            return user;
        }

        public User GetUserByCredentials(string username, string password)
        {
            var user = Repository.SingleOrDefault<User>(x => x.Username.Equals(username) && x.Password.Equals(password));
            return user;
        }

        public void CreateUser(User user)
        {
            //do not allow duplicate username and strings to be created
            var users = Repository.Fetch<User>();
            if(!users.Any(x => x.Username == user.Username && x.Password == user.Password))
            {
                Repository.Insert<User>(user);
            }
        }

        public bool UpdateUser(User user)
        {
            return Repository.Update<User>(user);
        }

        public void DeleteUser(int id)
        {
            Repository.Delete<User>(x => x.Id == id);
        }
    }
}
