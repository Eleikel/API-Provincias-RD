using API_InfoProvinciasRD.Models;
using API_InfoProvinciasRD.Models.Dto;
using API_InfoProvinciasRD.Repository.IConfiguration;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API_InfoProvinciasRD.Controllers
{
    [Authorize]
    [Route("api/User")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiUser")] //SwagerEndPoint
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public class UserController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config; // Generar Token


        public UserController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
        }

        /// <summary>
        /// Get all the users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<UserDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUsers()
        {
            var listUsers = await _unitOfWork.User.GetUsers();

            var listUsersDto = new List<UserDto>();

            foreach (var user in listUsers)
            {
                listUsersDto.Add(_mapper.Map<UserDto>(user));
            }

            return Ok(listUsersDto);
        }

        /// <summary>
        /// Get the user by it's id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId:int}", Name = "GetUser")]
        [ProducesResponseType(200, Type = typeof(UserDto))]  // El 'ProducesResponseType' es importante ponerlo
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetUser(int userId)
        {
            var itemUser = await _unitOfWork.User.GetUser(userId);

            if (itemUser == null)
            {
                return NotFound();
            }

            var itemUserDto = _mapper.Map<UserDto>(itemUser);
            return Ok(itemUserDto);
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="userAuthDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        [ProducesResponseType(201, Type = typeof(UserAuthDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterUser(UserAuthDto userAuthDto)
        {
            userAuthDto.User = userAuthDto.User.ToLower();

            if (await _unitOfWork.User.ExistUser(userAuthDto.User))
            {
                return BadRequest("The User already exist");
            }

            var userToCreate = new User
            {
                UserA = userAuthDto.User
            };

            var userRegistered = await _unitOfWork.User.Register(userToCreate, userAuthDto.Password);
            await _unitOfWork.CompleteAsync();

            return Ok(userRegistered);
        }


        /// <summary>
        /// Login/Access with an user account
        /// </summary>
        /// <param name="userLoginDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(200, Type = typeof(UserLoginDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var userLogin = await _unitOfWork.User.Login(userLoginDto.User, userLoginDto.Password);

            if (userLogin == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userLogin.Id.ToString()),
                new Claim(ClaimTypes.Name, userLogin.UserA.ToString())
            };

            //Token Generation

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credenciales
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }

    }

}