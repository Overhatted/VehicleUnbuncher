using ColossalFramework;
using System;

namespace VehicleUnbuncher
{
    public static class VehicleAIMod
    {
        public static bool CanLeave(ushort VehicleID, ref Vehicle VehicleData)
        {
            CitizenManager CitizenManagerInstance = Singleton<CitizenManager>.instance;
            uint CitizenUnitID = VehicleData.m_citizenUnits;
            int ListLimiter = 0;
            while (CitizenUnitID != 0u)
            {
                uint NextUnitID = CitizenManagerInstance.m_units.m_buffer[(int)((UIntPtr)CitizenUnitID)].m_nextUnit;
                for (int i = 0; i < 5; i++)
                {
                    uint CitizenID = CitizenManagerInstance.m_units.m_buffer[(int)((UIntPtr)CitizenUnitID)].GetCitizen(i);
                    if (CitizenID != 0u)
                    {
                        ushort CitizenInstanceID = CitizenManagerInstance.m_citizens.m_buffer[(int)((UIntPtr)CitizenID)].m_instance;
                        if (CitizenInstanceID != 0 && (CitizenManagerInstance.m_instances.m_buffer[CitizenInstanceID].m_flags & CitizenInstance.Flags.EnteringVehicle) != CitizenInstance.Flags.None)
                        {
                            return false;
                        }
                    }
                }
                CitizenUnitID = NextUnitID;
                if (++ListLimiter > 524288)
                {
                    CODebugBase<LogChannel>.Error(LogChannel.Core, "Invalid list detected!\n" + Environment.StackTrace);
                    break;
                }
            }
            return true;
        }
    }
}