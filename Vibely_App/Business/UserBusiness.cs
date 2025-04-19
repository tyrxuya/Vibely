using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vibely_App.API;
using Vibely_App.Data;
using Vibely_App.Data.Models;

namespace Vibely_App.Business
{
    public class UserBusiness(VibelyDbContext dbContext) : IUserBusiness
    {
        VibelyDbContext _dbContext = dbContext;

        public List<User> GetAll()
        {
            return _dbContext.Users.ToList();
        }

        public void Add(User user)
        {
            if (_dbContext.Users.Find(user.Id) != null)
            {
                return;
            }

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        public User? Find(int id)
        {
            return _dbContext.Users.Find(id);
        }

        public void Remove(int id)
        {
            User? user = _dbContext.Users.Find(id);

            if (user != null)
            {
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
            }
        }

        public void Update(int id, User user)
        {
            User? userToUpdate = _dbContext.Users.Find(id);

            if (userToUpdate != null)
            {
                _dbContext.Entry(userToUpdate).State = EntityState.Detached;

                userToUpdate.Username = user.Username;
                userToUpdate.Password = user.Password;
                userToUpdate.Email = user.Email;
                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName = user.LastName;
                userToUpdate.PhoneNumber = user.Username;
                userToUpdate.IsPremium = user.IsPremium;
                userToUpdate.StartDate = user.StartDate;
                userToUpdate.EndDate = user.EndDate;
                userToUpdate.SubscriptionPrice = user.SubscriptionPrice;

                _dbContext.Attach(userToUpdate);
                _dbContext.Entry(userToUpdate).State = EntityState.Modified;

                _dbContext.SaveChanges();
            }
        }

        User? IUserBusiness.FindByCredentials(string username, string password)
        {
            IPasswordHasher passwordHasher = new PasswordHasher();
            password = passwordHasher.Hash(password);

            return _dbContext.Users.Where(u => u.Username == username && u.Password == password)
                .FirstOrDefault();
        }

        public bool IsUsernameTaken(string username)
        {
            return _dbContext.Users.Any(u => u.Username == username);
        }
    }
}
