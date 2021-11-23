using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerSupport.Models
{
    public class MNotificationSettings
    {
        public MNotificationSettings()
        {
            LisMNotificationSettingsPriority = new List<MNotificationSettingsPriority>();
            LisNotificationSettingsStatus = new List<MNotificationSettingsStatus>();
        }

        public int IdSetting { get; set; }
        public bool SendResponsable { get; set; }
        public bool SendColaborator { get; set; }
        public bool SendFollower { get; set; }
        public bool SendAddComment { get; set; }
        public bool SendEditComment { get; set; }
        public int IdUser { get; set; }
        public string Email { get; set; }

        public List<MNotificationSettingsPriority> LisMNotificationSettingsPriority { get; set; }

        public List<MNotificationSettingsStatus> LisNotificationSettingsStatus { get; set; }

    }

    public class MNotificationSettingsPriority
    {
        public int IdSetting { get; set; }
        public int IdPriority { get; set; }
        public string Priority { get; set; }
    }

    public class MNotificationSettingsStatus
    {
        public int IdSetting { get; set; }
        public int IdStatus { get; set; }

        public string Status { get; set; }
    }

}
