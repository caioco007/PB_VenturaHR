using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Dashboard
{
    public class DashboardViewModel
    {
        public int TotalOpportunitys { get; set; }
        public int TotalActiveOpportunitys { get; set; }
        public float Opportunitys => TotalActiveOpportunitys * 100 / TotalOpportunitys;

        public int TotalCompanys { get; set; }
        public int TotalActiveCompanys { get; set; }
        public float Companys => TotalActiveCompanys * 100 / TotalCompanys;

        public int TotalCandidates { get; set; }
        public int TotalActiveCandidates { get; set; }
        public float Candidates => TotalActiveCandidates * 100 / TotalCandidates;
    }
}
