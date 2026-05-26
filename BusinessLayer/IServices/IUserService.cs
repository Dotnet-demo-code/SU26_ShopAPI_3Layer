using DataAccessLayer.DTOs.User;

namespace BusinessLayer.IServices
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllAsync();
        Task<UserDTO> GetByIdAsync(int id);
        Task<UserDTO> CreateAsync(UserRegisterDTO userRegisterDTO);
        Task<UserDTO> UpdateAsync(UserDTO userUpdateDTO);
        Task<bool> DeleteAsync(int id);
        Task<LoginResponseDTO> LoginAsync(UserLoginDTO userLoginDTO);
        Task<UserDTO> RegisterAsync(UserRegisterDTO userRegisterDTO);
    }
}
