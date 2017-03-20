using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System;
using EnglishTutor.Common.Exception;
using EnglishTutor.Common.Interfaces;
using System.Security.Principal;

namespace EnglishTutor.Api.Configuration
{
    public class TokenAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private IAccountService _accountService;

        public TokenAuthorizationFilter(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            const string HEADER = "Authorization"
                ,BEARER = "Bearer";
            StringValues values;
            if (context.HttpContext.Request.Headers.TryGetValue(HEADER, out values))
            {
                var token = values.FirstOrDefault(s => s.StartsWith(BEARER));
                if (token != null)
                {
                    var userId = await ValidateToken(token.Split(' ')[1]);
                    //context.HttpContext.User = new System.Security.Claims.ClaimsPrincipal()
                }
            }
            
            throw new ApiException(ApiError.Unauthorized);
        }

        

        private async Task<string> ValidateToken(string token)
        {
            try
            {
                return await _accountService.ValidateTokenAsync(token);
            
            }
            catch(Exception e)
            {
                throw new ApiException(ApiError.Unauthorized);
            }
        }
    }
}
