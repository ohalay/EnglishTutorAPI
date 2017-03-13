using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnglishTutor.Common.Interfaces;
using EnglishTutor.Common.Dto;

namespace EnglishTutor.Api.Controllers
{
    [Route("api/[controller]")]
    public class VocabularyController : Controller
    {
        private IFirebaseService _service;

        public VocabularyController(IFirebaseService service)
        {
            _service = service;
        }
    }
}
