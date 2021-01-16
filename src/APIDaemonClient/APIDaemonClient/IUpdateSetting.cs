using APIDaemonClient.Attributes;
using Newtonsoft.Json.Linq;

namespace APIDaemonClient
{
    public interface IUpdateSetting
    {
        JObject Settings { get;}
        void ChangeSettingValue(string settingName, string value);
        void ChangeSettingValue(string settingName, string[] newValue);

        [CLIMethod("Test this Method")]
        void Test();
    }
}