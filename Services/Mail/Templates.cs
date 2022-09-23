using DTO.Opportunity;

namespace Services.Mail
{
    public class Templates
    {
        public static string OpportunityExpired(string companyName, OpportunityViewModel model)
        {
            return "Olá <strong>" + companyName + "</strong>,<br/><br/>" +
                    "Sua Vaga #" + model.OpportunityId + ", expirou<br/><br/>";
        }
        public static string Assunto(OpportunityViewModel model)
        {
            string assunto = "Vaga #"+ model.OpportunityId+" expirada";
            return assunto;
        }

    }
}
