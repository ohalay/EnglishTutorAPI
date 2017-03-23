using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnglishTutor.Common.Interfaces;
using EnglishTutor.Common.Dto;
using EnglishTutor.Api.Models;
using AutoMapper;

namespace EnglishTutor.Api.Controllers
{
    public class UserController : BaseController
    {
        private readonly IFirebaseService _firebaseService;

        public UserController(IFirebaseService firbaseService, IMapper mapper) : base(mapper)
        {
            _firebaseService = firbaseService;
        }

        [HttpPost]
        public async Task<JsonResult> CreateUserAsync([FromBody] UserModel userModel)
        {
            var userSettings = await _firebaseService.GetUserSettings(UserId);

            var user = Mapper.Map<User>(userModel);

            if (userSettings == null)
                user = await _firebaseService.CreateUser(UserId, user);

            return GenerateJsonResult(Mapper.Map<UserModel>(user));
        }

        [HttpGet]
        [Route("settings")]
        public async Task<JsonResult> GetUserSettingsAsync()
        {
            var settings = await _firebaseService.GetUserSettings(UserId);
            return GenerateJsonResult(Mapper.Map<SettingsModel>(settings));
        }
    }
}
