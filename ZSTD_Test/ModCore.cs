using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using ZstdNet;

namespace ZSTD_Test
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class ZstdTestMod : BaseUnityPlugin
    {
        private const string ModName = "ZSTD_TestMod";
        private const string ModVersion = "1.0";
        private const string ModGUID = "some.new.guid";
        private static Harmony harmony = null!;

        public async void Awake()
        {
            await ReadManifestFiles().ConfigureAwait(false);
            
            Assembly assembly = Assembly.GetExecutingAssembly();
            harmony = new(ModGUID);
            harmony.PatchAll(assembly);
            var c = new Compressor();
        }
        
        public async Task ReadManifestFiles()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var s = assembly.GetManifestResourceStream("ZSTD_Test.Zstd_Lib.libzstd.dll");
            var path = Paths.PluginPath+ "\\" + "libzstd.dll";
            if (!File.Exists(path))
            {
                using var destinationStream = new FileStream(path, FileMode.OpenOrCreate);
                if (s != null) await s.CopyToAsync(destinationStream);
            }

            await Task.Yield();
        }
    }
}