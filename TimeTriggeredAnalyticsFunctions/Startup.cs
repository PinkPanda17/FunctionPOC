using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTriggeredAnalyticsFunctions
{
   public class Startup : IExtensionConfigProvider
    {   
        public void Initialize(ExtensionConfigContext context)
        {
            var extensionConfig = context; 
        }
    }
}