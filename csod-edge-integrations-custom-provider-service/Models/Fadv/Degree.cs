using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class Degree
    {
        [XmlAttribute("degreeType")]
        public string DegreeType { get; set; }
        public DegreeMajor DegreeMajor { get; set; }
        public DegreeMeasure DegreeMeasure { get; set; }
        public string DegreeDate { get; set; }
        public string DegreeMinor { get; set; }
        public string Honors { get; set; }
        public string OtherHonors { get; set; }
    }
}
