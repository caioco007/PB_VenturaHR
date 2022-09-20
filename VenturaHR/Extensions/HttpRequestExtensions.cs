using Microsoft.AspNetCore.Http;

namespace VenturaHR.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool IsAdministratorView(this HttpRequest me) => me.Query.ContainsKey("personType") && me.Query["personType"] == "1";
        public static bool IsCompanyView(this HttpRequest me) => me.Query.ContainsKey("personType") && me.Query["personType"] == "2";
        public static bool IsCandidateView(this HttpRequest me) => me.Query.ContainsKey("personType") && me.Query["personType"] == "3";
    }
}
