using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public int HashCode { get { return this.GetHashCode(); } }

        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)
                //it's ok for this instance where we are referencing non readonly because we plan on ONLY using this DTO for write once and read multiple
                if (this.Username != null)
                    hash = hash * 59 + this.Username.GetHashCode();
                if (this.Password != null)
                    hash = hash * 59 + this.Password.GetHashCode();
                return hash;
            }
        }
    }
}
