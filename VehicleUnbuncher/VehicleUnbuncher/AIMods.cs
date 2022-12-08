using Harmony;

namespace VehicleUnbuncher
{
    [HarmonyPatch(typeof(BusAI))]
    [HarmonyPatch(nameof(BusAI.CanLeave))]
    public class BusAIMod
    {
        public static bool Prefix(ushort vehicleID, ref Vehicle vehicleData, ref bool __result)
        {
            __result = VehicleUnbuncherManager.CanVehicleLeave(vehicleID, ref vehicleData);

            return false;
        }
    }

    /*[HarmonyPatch(typeof(CableCarAI))]
    [HarmonyPatch(nameof(CableCarAI.CanLeave))]
    public class CableCarAIMod
    {
        public static bool Prefix(ushort vehicleID, ref Vehicle vehicleData, ref bool __result)
        {
            float CurrentStoppedPosition;
            float MaximumPosition;

            //Helper.PrintError("GetPathProgress: " + CableCarAI.GetPathProgress(vehicleData.m_path, (int)vehicleData.m_pathPositionIndex, num, max2, out result););

            //__result = VehicleUnbuncherManager.CanVehicleLeave(vehicleID, ref vehicleData);

            return false;
        }
    }*/

    [HarmonyPatch(typeof(PassengerFerryAI))]
    [HarmonyPatch(nameof(PassengerFerryAI.CanLeave))]
    public class PassengerFerryAIMod
    {
        public static bool Prefix(ushort vehicleID, ref Vehicle vehicleData, ref bool __result)
        {
            __result = VehicleUnbuncherManager.CanVehicleLeave(vehicleID, ref vehicleData);

            return false;
        }
    }

    [HarmonyPatch(typeof(PassengerBlimpAI))]
    [HarmonyPatch(nameof(PassengerBlimpAI.CanLeave))]
    public class PassengerBlimpAIMod
    {
        public static bool Prefix(ushort vehicleID, ref Vehicle vehicleData, ref bool __result)
        {
            __result = VehicleUnbuncherManager.CanVehicleLeave(vehicleID, ref vehicleData);

            return false;
        }
    }

    [HarmonyPatch(typeof(PassengerTrainAI))]
    [HarmonyPatch(nameof(PassengerTrainAI.CanLeave))]
    public class PassengerTrainAIMod
    {
        public static bool Prefix(ushort vehicleID, ref Vehicle vehicleData, ref bool __result)
        {
            __result = VehicleUnbuncherManager.CanVehicleLeave(vehicleID, ref vehicleData);

            return false;
        }
    }

    [HarmonyPatch(typeof(TramAI))]
    [HarmonyPatch(nameof(TramAI.CanLeave))]
    public class TramAIMod
    {
        public static bool Prefix(ushort vehicleID, ref Vehicle vehicleData, ref bool __result)
        {
            __result = VehicleUnbuncherManager.CanVehicleLeave(vehicleID, ref vehicleData);

            return false;
        }
    }
}