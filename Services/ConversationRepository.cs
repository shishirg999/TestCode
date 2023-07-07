namespace OnboardingApp.Services
{
    using Microsoft.EntityFrameworkCore;
    using OnboardingApp.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ConversationRepository : IConversationRepository
    {
        private readonly AppDbContext _context;

        public ConversationRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<Conversation> GetByIdAsync(int id,int userId)
        {
            return Task.FromResult(_context.Conversations.FirstOrDefault(p => p.ID == id && p.UserId == userId));
        }

        public Task<int> SaveAsync(Conversation conversation)
        {
            try
            {
                _context.Conversations.Add(conversation);
                _context.SaveChanges();
                return Task.FromResult(conversation.ID);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public Task<int> GetConversationsCount()
        {
            return Task.FromResult(_context.Conversations.Count());
        }

        public Task<int> GetConversationHistoryCount()
        {
            return Task.FromResult(_context.ConversationHistories.Count());
        }

        public Task SaveConversationHistoryAsync(ConversationHistory conversationHistory)
        {
            _context.ConversationHistories.Add(conversationHistory);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Conversation conversation)
        {
            _context.Conversations.Update(conversation);
            _context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}