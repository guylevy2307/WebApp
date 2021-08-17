
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace IpWebApp.Models
{
    public static class Settings
    {
        private static int? daysToAlertBeforeDeadline = null;

        public static int DaysToAlertBeforeDeadline
        {
            get 
            {
                if (!daysToAlertBeforeDeadline.HasValue)
                {
                    daysToAlertBeforeDeadline = Int32.Parse(ConfigurationManager.AppSettings["settings:daysToAlertBeforeDeadline"]);
                }
                return daysToAlertBeforeDeadline.Value;

            }
            set 
            { 
                if(daysToAlertBeforeDeadline != value)
                {
                    Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                    config.AppSettings.Settings["settings:daysToAlertBeforeDeadline"].Value = value.ToString();
                    config.Save();
                }
                daysToAlertBeforeDeadline = value; 
            }
        }

    }
}