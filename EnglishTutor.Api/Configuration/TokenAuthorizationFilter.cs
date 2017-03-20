﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System;
using System.Security.Claims;
using EnglishTutor.Common.Exception;
using EnglishTutor.Common.Interfaces;

namespace EnglishTutor.Api.Configuration
{
    public class TokenAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly IAccountService _accountService;

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
                    var userId = await ValidateToken(token.Substring(BEARER.Length));
                    var principal = new TokenPrincipal(userId);
                    context.HttpContext.User = new ClaimsPrincipal(principal);
                    return;
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
            catch(ApiException)
            {
                throw new ApiException(ApiError.Unauthorized);
            }
        }
    }
}
