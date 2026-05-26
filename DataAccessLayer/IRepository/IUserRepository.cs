using DataAccessLayer.Models;

namespace DataAccessLayer.IRepository
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllAsync();
        public Task<User> GetByIdAsync(int id);
        public Task<User> GetByUsernameAsync(string username);
        public Task<User> AddNewAsync(User user);
        public Task<User> UpdateAsync(User user);
        public Task<User> DeleteByIdAsync(int id);

        public Task<int> SaveChangeAsync();
    }
}
