using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnglishTutor.Common.Interfaces;
using EnglishTutor.Common.Dto;
using EnglishTutor.Api.Models;
using AutoMapper;

namespace EnglishTutor.Api.Controllers
{
    //[Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IFirebaseService _firebaseService;
        private readonly IMapper _mapper;

        public UserController(IFirebaseService firbaseService, IMapper mapper)
        {
            _firebaseService = firbaseService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<JsonResult> CreateUser([FromBody] UserModel userModel)
        {
            var savedUser = await _firebaseService.GetUser(UserId);

            if (savedUser == null)
            {
                var user = _mapper.Map<User>(userModel);
                savedUser = await _firebaseService.CreateUser(UserId, user);
            }

            return GenerateJsonResult(_mapper.Map<UserModel>(savedUser));
        }
    }
}
