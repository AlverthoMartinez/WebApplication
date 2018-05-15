using System.IO;
using Microsoft.Extensions.Configuration;

namespace WebApplication.Data.utils
{
    public class AppConfigurationBuilder
    {
        public static IConfigurationRoot Configuration { 
            get {
                return _configuration;
            }
        }

        private static IConfigurationRoot _configuration { get; set; }

        public static IConfigurationRoot GetBuilder() {
            if(Configuration != null) {
                return Configuration;
            }

            var directory = Directory.GetCurrentDirectory();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile("appsettings.json")
                .Build();

            _configuration = configuration;

            return configuration;
        }
    }
}