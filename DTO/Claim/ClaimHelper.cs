using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DTO.Claim
{
    public static class ClaimHelper
    {
        public static readonly List<string> CandidateRoles = new List<string> { "Candidate" };
        public static readonly List<string> CompanyRoles = new List<string> { "Company" };
        public static readonly List<string> AdministratorRoles = new List<string> { "Administrator" };

        public const string AuthorizationCandidateRoles = "Candidate";
        public const string AuthorizationCompanyRoles = "Company";
        public const string AuthorizationAdministratorRoles = "Administrator";

        public static bool IsCandidate(this ClaimsPrincipal claimsPrincipal) => CandidateRoles.Any(x => claimsPrincipal.IsInRole(x));
        public static bool IsCompany(this ClaimsPrincipal claimsPrincipal) => CompanyRoles.Any(x => claimsPrincipal.IsInRole(x));
        public static bool IsAdministrator(this ClaimsPrincipal claimsPrincipal) => AdministratorRoles.Any(x => claimsPrincipal.IsInRole(x));
    }
}
