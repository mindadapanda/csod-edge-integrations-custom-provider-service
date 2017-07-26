using csod_edge_integrations_custom_provider_service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service
{
    public class CustomPrinciple : ClaimsPrincipal
    {
        IIdentity _identity;
        User _user;
        public CustomPrinciple(User user)
        {
            _identity = new GenericIdentity(user.Username);
            _user = user;
        }

        public override IIdentity Identity
        {
            get { return _identity; }
        }

        public User User
        {
            get { return _user; }
        }
    }
}
