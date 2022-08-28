using BepInEx;
using HarmonyLib;

namespace DecryptedSaves
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);

            // Patch save method
            harmony.Patch(
                AccessTools.Method(typeof(SaveAndLoad), "Saving"),
                null,
                new HarmonyMethod(AccessTools.Method(typeof(Plugin), "PreventEncryption"))
                );

            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        [HarmonyPostfix]
        public static void PreventEncryption(SaveAndLoad __instance)
        {
            COTLDataReadWriter<DataManager> _saveFileReadWriter = Traverse.Create(Singleton<SaveAndLoad>.Instance).Field("_saveFileReadWriter").GetValue() as COTLDataReadWriter<DataManager>;
            _saveFileReadWriter.Write(DataManager.Instance, SaveAndLoad.MakeSaveSlot(SaveAndLoad.SAVE_SLOT), false);
        }

    }

}
