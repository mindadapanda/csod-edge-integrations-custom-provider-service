using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Middleware
{
    public class BasicAuthenticationIdentity : IIdentity
    {
        readonly string _name;
        readonly int _id;
        public BasicAuthenticationIdentity(string name, int id)
        {
            _name = name;
            _id = id;
        }

        public string AuthenticationType
        {
            get { return "Basic"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get { return _name; }
        }

        public int Id
        {
            get { return _id; }
        }
    }
}
