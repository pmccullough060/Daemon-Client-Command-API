using Newtonsoft.Json.Linq;

namespace APIDaemonClient
{
    public interface IUpdateSetting
    {
        [CLIMethod("ChangeSettingValue","Change a setting value",":settingName :settingValue")]
        void ChangeSettingValue(string settingName, string value);

        [CLIMethod("Settings","Output all settings")]
        void OutputAllSettings();
    }
}