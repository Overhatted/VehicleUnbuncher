/*
using ColossalFramework;
using System.Collections.Generic;
using System.Reflection;

namespace VehicleUnbuncher
{
    public class CableCarVehicleUnchunched : VehicleUnchunched
    {
        public int NumberOfVehicles;

        public ushort FindAMainBuilding(ushort VehicleID, ref Vehicle VehicleData, List<ushort> PossibleMainBuildings)
        {
            NetManager NetManagerInstance = Singleton<NetManager>.instance;
            for (int i = 0; i < 8; i++)
            {
                ushort SegmentID = NetManagerInstance.m_nodes.m_buffer[(int)prevStop].GetSegment(i);
                if (SegmentID != 0 && NetManagerInstance.m_segments.m_buffer[SegmentID].m_startNode == prevStop)
                {
                    return NetManagerInstance.m_segments.m_buffer[SegmentID].m_endNode;
                }
            }
            ushort num = NetNode.FindOwnerBuilding(prevStop, 64f);
            if (num != 0)
            {
                ushort num2 = Singleton<BuildingManager>.instance.m_buildings.m_buffer[(int)num].m_netNode;
                int num3 = 0;
                while (num2 != 0)
                {
                    if (num2 != prevStop)
                    {
                        NetInfo info = NetManagerInstance.m_nodes.m_buffer[(int)num2].Info;
                        if (info.m_class.m_layer == ItemClass.Layer.PublicTransport && info.m_class.m_service == this.m_info.m_class.m_service && info.m_class.m_subService == this.m_info.m_class.m_subService)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                ushort segment2 = NetManagerInstance.m_nodes.m_buffer[(int)num2].GetSegment(j);
                                if (segment2 != 0 && NetManagerInstance.m_segments.m_buffer[(int)segment2].m_startNode == num2)
                                {
                                    return num2;
                                }
                            }
                        }
                    }
                    num2 = NetManagerInstance.m_nodes.m_buffer[(int)num2].m_nextBuildingNode;
                    if (++num3 > 32768)
                    {
                        CODebugBase<LogChannel>.Error(LogChannel.Core, "Invalid list detected!\n" + Environment.StackTrace);
                        break;
                    }
                }
            }
            return 0;
        }
    }
}
*/