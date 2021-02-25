using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;

namespace AnonymousImpostor
{
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class AnonymousImpostor : BasePlugin
    {
        public const string Id = "com.tomatan515.anonymousImpostor";
        public static BepInEx.Logging.ManualLogSource log;
        public Harmony Harmony { get; } = new Harmony(Id);

        public override void Load()
        {
            log = Log;
            Harmony.PatchAll();
        }
    }
}
