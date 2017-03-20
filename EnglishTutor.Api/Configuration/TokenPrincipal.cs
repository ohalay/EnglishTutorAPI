using System;
using System.Security.Principal;

namespace EnglishTutor.Api.Configuration
{

    public interface ITokenPrincipal
    {
    }

    public class TokenPrincipal: ITokenPrincipal, IPrincipal
    {
        public TokenPrincipal(string name)
        {
            Identity = new GenericIdentity(name);
        }

        public IIdentity Identity { get; }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}