using CitiesHarmony.API;
using HarmonyLib;
using ICities;
using System;
using System.IO;
using UnityEngine;

namespace VehicleUnbuncher
{
    public class VehicleUnbuncher : IUserMod
    {
        public string Name
        {
            get
            {
                return "Vehicle Unbuncher";
            }
        }

        public string Description
        {
            get
            {
                return "Unbunches public transportation vehicles";
            }
        }
    }

    public static class Helper
    {
        public static void PrintError(string Message)
        {
#if DEBUG
            File.AppendAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
        "\\VULog.txt", Message + Environment.NewLine);
#else
            Debug.Log("[Vehicle Unbuncher] " + Message);
#endif
        }
    }

    public static class Patcher
    {
        private const string HarmonyId = "Overhatted.VehicleUnbuncher";
        private static bool patched = false;

        public static void PatchAll()
        {
            if (patched) return;

            patched = true;
            var harmony = new Harmony(HarmonyId);
            harmony.PatchAll(typeof(Patcher).Assembly);
        }

        public static void UnpatchAll()
        {
            if (!patched) return;

            var harmony = new Harmony(HarmonyId);
            harmony.UnpatchAll(HarmonyId);
            patched = false;
        }
    }

    public class Loader : LoadingExtensionBase
    {
        public override void OnCreated(ILoading loading)
        {
#if DEBUG
            Helper.PrintError("");
#endif
            VehicleUnbuncherManager.Init();

            if (HarmonyHelper.IsHarmonyInstalled) Patcher.PatchAll();

            Options.Load();
            Options.Save();
        }

        public override void OnReleased()
        {
            if (HarmonyHelper.IsHarmonyInstalled) Patcher.UnpatchAll();
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            VehicleUnbuncherManager.Init();
        }

        public override void OnLevelUnloading()
        {
            VehicleUnbuncherManager.UnInit();
        }
    }
}