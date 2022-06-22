using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Data
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUserNamePasswordAsync(string username, string password);
    }

    public class UserRepository : IUserRepository
    {
        //dummy data
        //TODO: need to think where to store real data
        private async static Task<List<User>> Users() =>
            await Task.FromResult(new List<User>
            {
                new User { Id = 1, FirstName = "Test", LastName = "User", UserName = "test", Password = "test" }

            });

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await Users();
        }
        public async Task<User> GetByIdAsync(int id)
        {
            var users = await Users();
            return users.FirstOrDefault(user => user.Id == id);
        }
        public async Task<User> GetByUserNamePasswordAsync(string username, string password)
        {
            var users = await Users();
            return users.FirstOrDefault(user => user.UserName == username &&
                                                user.Password == password);
        }
    }
}
