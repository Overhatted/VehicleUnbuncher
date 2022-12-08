using Harmony;
using ICities;
using System;
using System.IO;
using System.Reflection;
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

    public class Loader : LoadingExtensionBase
    {
        public override void OnCreated(ILoading loading)
        {
#if DEBUG
            Helper.PrintError("");
#endif
            VehicleUnbuncherManager.Init();

            var harmony = HarmonyInstance.Create("com.overhatted.vehicleunbuncher");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Options.Load();
            Options.Save();
        }

        public override void OnReleased()
        {

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