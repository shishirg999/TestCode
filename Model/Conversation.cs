using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace OnboardingApp.Model
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Conversation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserId { get; set; }
        public int ProviderId { get; set; }
        public string Summary { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Status { get; set; }
        public User User { get; set; }
        public Provider Provider { get; set; }
        public IEnumerable<ConversationHistory> ConversationHistories { get; set; }

    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ConversationHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ConvId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public DateTime CreatedOn { get; set; }
        public Conversation Conversation { get; set; }

    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ConversationInputs
    {
        public int providerId { get; set; }
        public List<ConversationQuestionAnswer> conversations { get; set; }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ConversationSummary
    {
        public int conversationId { get; set; }
        public string summary { get; set; }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ConversationDoctorSummary
    {
        public int ConversationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Summary { get; set; }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ConversationQuestionAnswer
    {
        public string question { get; set; }
        public string answer { get; set; }
    }
}