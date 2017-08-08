using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    [XmlType(AnonymousType = true, Namespace = "http://www.cpscreen.com/schemas")]
    public class BackgroundReportPackage
    {
        public string ProviderReferenceId { get; set; }
        [XmlElement("PackageInformation")]
        public List<PackageInformation> PackageInformation { get; set; }
        [XmlElement("PersonalData")]
        public List<BackgroundReportPackagePersonalData> PersonalData { get; set; }
        [XmlElement("ScreeningStatus")]
        public ScreeningStatus ScreeningStatus { get; set; }
        [XmlElement("ScreeningResults")]
        public List<ScreeningResults> ScreeningResults { get; set; }
        [XmlArrayItem("Screening", typeof(Screening), IsNullable = false)]
        public List<Screening> Screenings { get; set; }
        [XmlAttribute("type")]
        public string Type { get; set; }
    }
}
