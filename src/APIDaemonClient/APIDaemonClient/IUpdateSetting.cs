using APIDaemonClient.Attributes;
using Newtonsoft.Json.Linq;

namespace APIDaemonClient
{
    public interface IUpdateSetting
    {
        JObject Settings { get; }

        [CLIMethod("ChangeSettingValue","-settingName -settingValue")]
        void ChangeSettingValue(string settingName, string value);

        [CLIMethod("Test this Method","-Number")]
        void Test(int value);

        [CLIMethod("Settings")]
        void OutputAllSettings();
    }
}