using Microsoft.VisualBasic.CompilerServices;
namespace OnboardingApp.Services
{
    using OnboardingApp.Model;
    // Purpose: Class for handling onboarding requests
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ConversationContext : IConversationContext
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly ITokenValidator _tokenValidator;

        public ConversationContext(IConversationRepository conversationRepository, ITokenValidator tokenValidator)
        {
            _conversationRepository = conversationRepository;
            _tokenValidator = tokenValidator;
        }

        public async Task<int> SaveConversationAsync(string token, int providerId, List<ConversationQuestionAnswer> data)
        {
                var user = _tokenValidator.ValidateToken(token);

                if (user == null)
                    return 0;

                var ConversationsCount = await _conversationRepository.GetConversationsCount();
                var conversation = new Conversation
                {
                    ID = ConversationsCount + 1,
                    UserId = user.ID,
                    ProviderId = providerId,
                    Summary = " ",
                    CreatedOn = DateTime.Now
                };

                var conversationID = await _conversationRepository.SaveAsync(conversation);

                if (conversationID > 0)
                {
                    List<Task> tasks = new List<Task>();
                    foreach (var item in data)
                    {
                        var ConversationHistoryCount = await _conversationRepository.GetConversationHistoryCount();
                        var conversationHistory = new ConversationHistory
                        {
                            ID = ConversationHistoryCount + 1,
                            ConvId = conversationID,
                            Question = item.question,
                            Answer = item.answer,
                            CreatedOn = DateTime.Now
                        };

                        tasks.Add(_conversationRepository.SaveConversationHistoryAsync(conversationHistory));
                    }

                    await Task.WhenAll(tasks);

                }

                return conversationID;
        }

        public async Task<ConversationDoctorSummary> SaveConversationSummaryAsync(string token, int conversationId, string summary)
        {
            ConversationDoctorSummary response = new ConversationDoctorSummary();

            var user = _tokenValidator.ValidateToken(token);

            var conversation = await _conversationRepository.GetByIdAsync(conversationId, user.ID);

            conversation.Summary = response.Summary = summary;

            await _conversationRepository.UpdateAsync(conversation);

            response.ConversationId = conversationId;
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.Email = user.Email;

            return response;

        }
    }
}