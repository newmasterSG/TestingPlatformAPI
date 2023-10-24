using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TestingPlatform.Application.DTO;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Domain.Entities;
using TestingPlatform.Domain.Interfaces;

namespace TestingPlatform.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public UserService(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }


        public async Task<bool> AddAsync(UserDTO dTO)
        {
            var user = new User { UserName = dTO.Email, Email = dTO.Email, DateRegistration = DateTime.UtcNow};
            
            var result = await _userManager.CreateAsync(user, dTO.Password);

            return result.Succeeded;
        }

        public async Task<List<UserDTO>> GetAllAsync(int pageNumber, int pageSize, string attribute = "", string order = "asc")
        {
            int skipElements = (pageNumber - 1) * pageSize;

            Expression<Func<User, object>> orderByExpression = null;

            switch (attribute.ToLower())
            {
                case "email":
                    orderByExpression = user => user.Email;
                    break;
                case "date registration":
                    orderByExpression = user => user.DateRegistration;
                    break;
                default:
                    orderByExpression = user => user.Id;
                    break;
            }

            bool ordering = order == "asc" ? true : false;

            var usersDb = await _unitOfWork.GetRepository<User>()
                .TakeAsync(skipElements, pageSize, (orderByExpression, ordering));

            if (usersDb == null)
            {
                return Enumerable.Empty<UserDTO>().ToList();
            }
            
            var usersDTO = _mapper.Map<List<UserDTO>>(usersDb);

            return usersDTO;
        }

        public Task<UserDTO> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, UserDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            
            return false;
        }

        public string GenerateToken(User user)
        {
            return _tokenService.GenerateToken(user);
        }

        public ClaimsPrincipal GetClaims(string token)
        {
            return _tokenService.GetPrincipalFromToken(token);
        }
    }
}
