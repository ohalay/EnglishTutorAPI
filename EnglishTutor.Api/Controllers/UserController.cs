using System;
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
        public async Task<JsonResult> CreateUser([FromBody] UserModel userModel)
        {
            var savedUser = await _firebaseService.GetUser(UserId);

            if (savedUser == null)
            {
                var user = Mapper.Map<User>(userModel);
                savedUser = await _firebaseService.CreateUser(UserId, user);
            }

            return GenerateJsonResult(Mapper.Map<UserModel>(savedUser));
        }
    }
}
