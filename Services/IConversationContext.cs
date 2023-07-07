
using OnboardingApp.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnboardingApp.Services
{
    public interface IConversationContext
    {
        Task<int> SaveConversationAsync(string token, int providerId, List<ConversationQuestionAnswer> data);

        Task<ConversationDoctorSummary> SaveConversationSummaryAsync(string token, int conversationId, string summary);
    }
}