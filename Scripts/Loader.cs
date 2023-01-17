using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;

namespace FN57_plugin
{
        [BepInDependency("pl.szikaka.receiver_2_modding_kit")]
        [BepInPlugin("Ciarencew.FN57", "FN57 Plugin", "2.1.0")]
        internal class MainPlugin : BaseUnityPlugin
        {
            public static MainPlugin instance
            {
                get;
                private set;
            }

            public static readonly string folder_name = "FN57";
            private void Awake()
            {
                Logger.LogInfo("FN57 Main Plugin loaded!");

                instance = this;
            }
        }
}
