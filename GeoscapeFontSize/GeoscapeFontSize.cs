using PhoenixPoint.Geoscape.View.ViewControllers;
using Newtonsoft.Json.Linq;
using System.Reflection;
using UnityEngine;
using System.IO;
using Harmony;
using System;

namespace PhoenixPointGeoscapeFontSize
{
    public class GeoscapeFontSize
    {
        public static int FONT_SIZE { get; set; } = 20;

        public static void Init()
        {
            var harmony = HarmonyInstance.Create("io.github.xebeth.geoscapefontsize");
            var optionFile = @"Mods/GeoscapeFontSize.json";

            if (File.Exists(optionFile))
            {
                var Options = JObject.Parse(File.ReadAllText(optionFile));

                if (int.TryParse((string)Options["size"], out int fontSize))
                {
                    FONT_SIZE = fontSize;
                }
            }
            else
            {
                var Options = new JObject(new JProperty("size", FONT_SIZE));

                File.WriteAllText(optionFile, Options.ToString());

            }

            harmony.PatchAll(Assembly.GetExecutingAssembly());            
        }

        [HarmonyPatch(typeof(GeoObjectiveElementController), "SetObjective")]
        public static class PhoenixPoint_GeoObjectiveElementController_SetObjective_Patch
        {
            // void SetObjective(Sprite icon, Color iconColor, string objectiveText, int objectiveIndex)
            static bool Prefix(GeoObjectiveElementController __instance, Sprite icon, Color iconColor, string objectiveText, int objectiveIndex)
            {
                if (__instance.ObjectiveText != null)
                {
                    __instance.ObjectiveText.fontSize = FONT_SIZE;
                }

                return true;
            }
        }
    }
}
