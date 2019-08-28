using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Settings
{
    public class TokenSettings : ITokenSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryMinutes { get; set; }
        public bool ValidateLifeTime { get; set; }
    }
    public interface ITokenSettings
    {
        string Secret { get; set; }
        string Issuer { get; set; }
        string Audience { get; set; }
        int ExpiryMinutes { get; set; }
        bool ValidateLifeTime { get; set; }
    }
}
