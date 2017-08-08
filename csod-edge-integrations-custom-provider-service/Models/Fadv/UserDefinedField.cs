﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class UserDefinedField
    {
        [XmlAttribute("type")]
        public string Name { get; set; }
        [XmlText]
        public string Value { get; set; }
        [XmlElement("UserDefinedField")]
        public string userDefinedField { get; set; }
    }
}
