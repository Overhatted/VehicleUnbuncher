/*
using ColossalFramework;
using System;
using System.Collections.Generic;

namespace VehicleUnbuncher
{
    public static class CableCarLinesUnbunched
    {
        public static Dictionary<ushort, CableCarVehicleUnchunched> Vehicles = new Dictionary<ushort, CableCarVehicleUnchunched>();

        public static void RefreshCableCarLine(ushort VehicleID, ref Vehicle VehicleData)
        {
            if (Singleton<SimulationManager>.instance.m_currentFrameIndex - VehicleUnbuncherManager.LoadFrame < 4096)
            {
                return;
            }

            VehicleManager VehicleManagerInstance = Singleton<VehicleManager>.instance;
            Dictionary<ushort, float> VehiclePositions = new Dictionary<ushort, float>();

            //Get first Vehicle details
            ushort CheckingVehicleID = Singleton<TransportManager>.instance.m_lines.m_buffer[LineID].m_vehicles;
            Vehicle CheckingVehicleData = VehicleManagerInstance.m_vehicles.m_buffer[CheckingVehicleID];
            float CheckingVehiclePosition;
            float MaximumPosition = 0;

            while (CheckingVehicleID != 0)
            {
                bool IsProgressStatusValid = CheckingVehicleData.Info.m_vehicleAI.GetProgressStatus(CheckingVehicleID, ref CheckingVehicleData, out CheckingVehiclePosition, out MaximumPosition);

                if (!IsProgressStatusValid)
                {
                    return;
                }

                VehiclePositions.Add(CheckingVehicleID, CheckingVehiclePosition);

                //Get next Vehicle details
                CheckingVehicleID = CheckingVehicleData.m_nextLineVehicle;
                CheckingVehicleData = VehicleManagerInstance.m_vehicles.m_buffer[CheckingVehicleID];
            }

            //Add new Vehicles
            foreach (ushort VehicleID in VehiclePositions.Keys)
            {
                if (!this.Vehicles.ContainsKey(VehicleID))
                {
                    this.Vehicles.Add(VehicleID, new VehicleUnchunched());
                }
            }

            //Remove old Vehicles
            List<ushort> VehicleIDsToRemove = new List<ushort>();
            foreach (ushort VehicleID in this.Vehicles.Keys)
            {
                if (!VehiclePositions.ContainsKey(VehicleID))
                {
                    VehicleIDsToRemove.Add(VehicleID);
                }
            }
            foreach (ushort VehicleIDToRemove in VehicleIDsToRemove)
            {
                this.Vehicles.Remove(VehicleIDToRemove);
            }

            //Order Vehicles
            if (VehiclePositions.Count >= 2)
            {
                //This orders in ascending order of positions which means the last Vehicle is the one with the largest Position which means the first Vehicle is in front of the last one

                ushort[] OrderedVehiclesIDs = new ushort[VehiclePositions.Count];

                VehiclePositions.Keys.CopyTo(OrderedVehiclesIDs, 0);

                Array.Sort(OrderedVehiclesIDs, delegate (ushort X, ushort Y)
                {
                    if (VehiclePositions[X] > VehiclePositions[Y])
                    {
                        return 1;
                    }
                    else if (VehiclePositions[Y] > VehiclePositions[X])
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                });

#if DEBUG
                string ToPrint = "OrderedVehiclesIDs: " + VehiclePositions.Count + ". ";

                foreach (ushort VehicleID in OrderedVehiclesIDs)
                {
                    ToPrint += "(" + VehicleID + ": " + VehiclePositions[VehicleID] + "); ";
                }

                Helper.PrintError("LineID: " + LineID + ": " + ToPrint);
#endif

                for (int i = 0; i != OrderedVehiclesIDs.Length - 1; i++)
                {
                    this.Vehicles[OrderedVehiclesIDs[i]].PreviousVehicleID = OrderedVehiclesIDs[i + 1];
                }

                this.Vehicles[OrderedVehiclesIDs[OrderedVehiclesIDs.Length - 1]].PreviousVehicleID = OrderedVehiclesIDs[0];
            }
        }

        public static float GetVehicleLength()
        {
            int NumberOfSquares = 1;
            return NumberOfSquares * 10;
        }
    }
}
*/