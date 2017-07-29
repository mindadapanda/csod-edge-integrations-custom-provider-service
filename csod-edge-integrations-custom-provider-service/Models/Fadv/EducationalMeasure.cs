using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class EducationalMeasure
    {
        public string MeasureSystem { get; set; }
        public MeasureValue MeasureValue { get; set; }
        public string HighestPossibleValue { get; set; }
    }
}
