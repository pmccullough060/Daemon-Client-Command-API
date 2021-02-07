using APIDaemonClient.Attributes;
using Newtonsoft.Json.Linq;

namespace APIDaemonClient
{
    public interface IUpdateSetting
    {
        JObject Settings { get; }

        [CLIMethod("ChangeSettingValue","Change a setting value","-settingName -settingValue")]
        void ChangeSettingValue(string settingName, string value);

        [CLIMethod("Test","A Test Method","-Number")]
        void Test(int value);

        [CLIMethod("Settings","Output all settings")]
        void OutputAllSettings();
    }
}