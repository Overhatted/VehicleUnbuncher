using ColossalFramework;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleUnbuncher
{
    public static class VehicleUnbuncherManager
    {
        public const float MinimumDistanceBetweenStops = 1f;
        public static Dictionary<ushort, TransportLineUnbunched> TransportLines;
        public static List<ushort> DuplicatedStopsIDs;

        public static uint LoadFrame;

        public static void Init()
        {
            TransportLines = new Dictionary<ushort, TransportLineUnbunched>();
            DuplicatedStopsIDs = new List<ushort>();

            LoadFrame = Singleton<SimulationManager>.instance.m_currentFrameIndex;
        }

        public static void UnInit()
        {
            foreach(TransportLineUnbunched TransportLineUnbunched in TransportLines.Values)
            {
                TransportLineUnbunched.Vehicles.Clear();
            }

            TransportLines.Clear();
            DuplicatedStopsIDs.Clear();
        }

        /*public static bool CanCableCarLeave(ushort VehicleID, ref Vehicle VehicleData)
        {
            //m_transportLine is always 0 for cable cars

            bool CanLeave;

            if (VehicleData.m_waitCounter < 12)
            {
                CanLeave = false;
            }
            else if (VehicleData.m_waitCounter > Options.CurrentSettings.MaxWaitCounter)
            {
                CanLeave = true;
            }
            else
            {
                CanLeave = VehicleUnbuncherManager.IsCableCarUnbunched(VehicleID, ref VehicleData);
            }

            return CanLeave && VehicleAIMod.CanLeave(VehicleID, ref VehicleData);
        }

        public static bool IsCableCarUnbunched(ushort VehicleID, ref Vehicle VehicleData)
        {
            CableCarVehicleUnchunched CableCarVehicleUnchunchedInstance;
            if (CableCarLinesUnbunched.Vehicles.TryGetValue(VehicleID, out CableCarVehicleUnchunchedInstance))
            {
                if (CableCarVehicleUnchunchedInstance.IsVehicleUnbunched(VehicleID, ref VehicleData, CableCarVehicleUnchunchedInstance.NumberOfVehicles))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                CableCarLinesUnbunched.RefreshCableCarLine(VehicleID, ref VehicleData);
                return true;//Let the vehicle go and it will unbunch in next stop
            }
        }*/

        public static bool CanVehicleLeave(ushort VehicleID, ref Vehicle VehicleData)
        {
            bool CanLeave;
            //If m_leadingVehicle == 0 it's the leading Vehicle, thus, if m_leadingVehicle != 0 it's not the leading Vehicle
            //If m_transportLine == 0 it has no transport line (for example trains bringing tourists in)
            if (VehicleData.m_leadingVehicle != 0 || VehicleData.m_transportLine == 0)
            {
                CanLeave = true;
            }
            else
            {
                if (VehicleData.m_waitCounter < 12)
                {
                    CanLeave = false;
                }
                else if(VehicleData.m_waitCounter > Options.CurrentSettings.MaxWaitCounter)
                {
                    CanLeave = true;
                }
                else
                {
                    CanLeave = VehicleUnbuncherManager.IsVehicleUnbunched(VehicleID, ref VehicleData);
                }
            }

            return CanLeave && VehicleAIMod.CanLeave(VehicleID, ref VehicleData);
        }

        public static bool IsVehicleUnbunched(ushort VehicleID, ref Vehicle VehicleData)
        {
            TransportLineUnbunched TransportLineUnbunchedInstance;
            if (!VehicleUnbuncherManager.TransportLines.TryGetValue(VehicleData.m_transportLine, out TransportLineUnbunchedInstance))
            {
                TransportLineUnbunchedInstance = new TransportLineUnbunched();
                VehicleUnbuncherManager.TransportLines.Add(VehicleData.m_transportLine, TransportLineUnbunchedInstance);
            }

            int NumberOfVehicles = TransportLineUnbunchedInstance.Vehicles.Count;

            if (NumberOfVehicles == 0)
            {
                TransportLineUnbunchedInstance.RefreshTransportLine(VehicleData.m_transportLine);
                return true;//Let the vehicle go and it will unbunch in next stop
            }
            else
            {
                VehicleUnchunched VehicleUnchunchedInstance;
                if (TransportLineUnbunchedInstance.Vehicles.TryGetValue(VehicleID, out VehicleUnchunchedInstance))
                {
                    if (NumberOfVehicles == 1)
                    {
                        return true;
                    }
                    else
                    {
                        bool IsUnbunched;
                        if (VehicleUnchunchedInstance.IsVehicleUnbunched(VehicleID, ref VehicleData, NumberOfVehicles, out IsUnbunched))
                        {
                            return IsUnbunched;
                        }
                        else
                        {
                            TransportLineUnbunchedInstance.RefreshTransportLine(VehicleData.m_transportLine);

                            return true;
                        }
                    }
                }
                else
                {
                    TransportLineUnbunchedInstance.RefreshTransportLine(VehicleData.m_transportLine);
                    return true;//Let the vehicle go and it will unbunch in next stop
                }
            }
        }

        public static void RefreshDuplicatedPositions()
        {
#if DEBUG
            Helper.PrintError("RefreshDuplicatedPositions();");
#endif

            DuplicatedStopsIDs = new List<ushort>();

            //Temporary list that holds all stops positions
            Dictionary<ushort, Vector3> StopsPositions = new Dictionary<ushort, Vector3>();

            NetManager NetManagerInstance = Singleton<NetManager>.instance;

            foreach (TransportLine TransportLineInstance in Singleton<TransportManager>.instance.m_lines.m_buffer)
            {
                if (TransportLineInstance.Complete)
                {
#if DEBUG
                    string ToPrint = "LineID: " + Singleton<VehicleManager>.instance.m_vehicles.m_buffer[TransportLineInstance.m_vehicles].m_transportLine + "; ";
                    ToPrint += "Type: " + TransportLineInstance.Info.m_vehicleType + "; ";
#endif
                    ushort FirstStopID = TransportLineInstance.m_stops;
                    ushort CurrentStopID = FirstStopID;
                    while (CurrentStopID != 0)
                    {
                        Vector3 CurrentStopPosition = NetManagerInstance.m_nodes.m_buffer[CurrentStopID].m_position;

#if DEBUG
                        ToPrint += "(" + CurrentStopID + ": " + CurrentStopPosition.ToString() + "); ";
#endif

                        foreach (KeyValuePair<ushort, Vector3> AlreadyAddedStopKV in StopsPositions)
                        {
                            if (Vector3.Distance(CurrentStopPosition, AlreadyAddedStopKV.Value) < VehicleUnbuncherManager.MinimumDistanceBetweenStops)
                            {
                                if (!DuplicatedStopsIDs.Contains(AlreadyAddedStopKV.Key))
                                {
                                    DuplicatedStopsIDs.Add(AlreadyAddedStopKV.Key);
                                }

                                DuplicatedStopsIDs.Add(CurrentStopID);

                                break;
                            }
                        }

                        StopsPositions.Add(CurrentStopID, CurrentStopPosition);

                        CurrentStopID = TransportLine.GetNextStop(CurrentStopID);
                        if (CurrentStopID == FirstStopID)
                        {
                            break;
                        }
                    }

#if DEBUG
                    Helper.PrintError(ToPrint);
#endif

                }
            }
        }
    }
}