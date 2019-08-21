using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Project.Helpers
{
    public class AppSettings : IAppSettings
    {
        public string Secret { get; set; }
    }
    public interface IAppSettings
    {
        string Secret { get; set; }
    }
}
