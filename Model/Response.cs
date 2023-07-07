using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnboardingApp.Model
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Response
    {
        public Response()
        {

        }
        public Response(int code,string message,string token)
        {
            StatusCode = code;
            StatusMessage = message;
            Token = token;
        }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string Token { get; set; }
    }
}