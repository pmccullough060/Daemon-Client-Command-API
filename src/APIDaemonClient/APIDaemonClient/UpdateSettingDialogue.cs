using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace APIDaemonClient
{
    public class UpdateSettingDialogue : IUpdateSettingDialogue
    {
        private IList<JProperty> jPropIList;

        private readonly ILogger<UpdateSettingDialogue> _logger;
        private readonly IUpdateSetting _updateSetting;

        public UpdateSettingDialogue(ILogger<UpdateSettingDialogue> logger, IUpdateSetting updateSetting)
        {
            _logger = logger;
            _updateSetting = updateSetting;
        }

        public void RunDialogue()
        {
            Console.WriteLine("the Current settings are:");

            OutputSettingsToConsole();
        }

        private void OutputSettingsToConsole()
        {
            jPropIList = _updateSetting.Settings.Properties().ToList(); //since we are intending to Index the position of each JProperty we really should use an IList instead of IEnumerable which 

            for (int i = 0; i < jPropIList.Count; i++)
                Console.WriteLine( "[" + i + "] " + jPropIList[i].Name + " : " + jPropIList[i].Name);
        }
    }
}
