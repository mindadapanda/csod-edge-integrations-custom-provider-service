using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class PackageInformation
    {
        public string PackageId { get; set; }
        [XmlElement("ClientReferences")]
        public List<PackageInformationClientReferences> ClientReferences { get; set; }
        [XmlArrayItem("UserDefinedField", typeof(PackageInformationUserDefinedField))]
        public List<PackageInformationUserDefinedField> UserDefinedFields { get; set; }
        [XmlArrayItem("Quoteback", typeof(PackageInformationQuoteback))]
        public List<PackageInformationQuoteback> Quotebacks { get; set; }
        [XmlElement("OrderAccount")]
        public List<OrderAccount> OrderAccount { get; set; }

    }
}
