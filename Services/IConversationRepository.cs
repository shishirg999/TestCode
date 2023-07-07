using Model = OnboardingApp.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnboardingApp.Services
{
    public interface IConversationRepository
    {
        Task<Model.Conversation> GetByIdAsync(int id,int userId);

        Task<int> SaveAsync(Model.Conversation conversation);

        Task<int> GetConversationsCount();
        Task<int> GetConversationHistoryCount();

        Task SaveConversationHistoryAsync(Model.ConversationHistory conversationHistory);

        Task UpdateAsync(Model.Conversation conversation);
    }
}