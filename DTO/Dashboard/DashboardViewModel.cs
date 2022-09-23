using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Dashboard
{
    public class DashboardViewModel
    {
        public int TotalOpportunitys { get; set; }
        public int TotalOpportunitysToday { get; set; }        
        public int TotalActiveOpportunitys { get; set; }
        public float Opportunitys => TotalActiveOpportunitys * 100 / TotalOpportunitys;

        public int TotalCompanys { get; set; }
        public int TotalCompanysToday { get; set; }
        public int TotalActiveCompanys { get; set; }
        public float Companys => TotalActiveCompanys * 100 / TotalCompanys;

        public int TotalCandidates { get; set; }
        public int TotalCandidatesToday { get; set; }
        public int TotalActiveCandidates { get; set; }
        public float Candidates => TotalActiveCandidates * 100 / TotalCandidates;

        public int TotalCandidateForOpportunitys { get; set; }
        public int TotalCandidateForOpportunitysToday { get; set; }

        public int TotalUser => TotalCompanys + TotalCandidates;
        public int TotalUserToday => TotalCompanysToday + TotalCandidatesToday;
        public int TotalActiveUser => TotalActiveCompanys + TotalActiveCandidates;
    }
}
