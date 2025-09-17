using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using Receiver2;
using UnityEngine.Events;

namespace FN57_plugin
{
    [BepInDependency("pl.szikaka.receiver_2_modding_kit")]
    [BepInPlugin("Ciarencew.FN57", "FN57 Plugin", "3.0.0")]
    class MainPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("FN57 Main Plugin loaded!");
        }
    }
}
