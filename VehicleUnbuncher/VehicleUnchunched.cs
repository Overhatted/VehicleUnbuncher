using ColossalFramework;

namespace VehicleUnbuncher
{
    public class VehicleUnchunched
    {
        public ushort PreviousVehicleID;//The Vehicle in front of this one

        public float CurrentStoppedPosition;
        private ushort LastNextStop;
        private bool IgnoreCurrentStop;

        public bool IsVehicleUnbunched(ushort VehicleID, ref Vehicle VehicleData, int NumberOfVehicles, out bool IsUnbunched)
        {
            float MaximumPosition;//Line length

            //If its the first time the vehicle enters the station there are some calculations that can be done in advance
            if (this.LastNextStop != VehicleData.m_targetBuilding)
            {
                //Here this.LastNextStop is still this stop
                //m_targetBuilding changes to the next station right after the vehicle has arrived into this station

                //We already checked that the line has more than 1 vehicle so we don't need to check if
                //Except not because now there are cable cars that have no transport line so this is the only check
                if(this.PreviousVehicleID == VehicleID)
                {
                    this.IgnoreCurrentStop = true;
                }
                else if (VehicleUnbuncherManager.DuplicatedStopsIDs.Contains(this.LastNextStop))//If this station is used by more than one line it's not the right place to wait
                {
                    this.IgnoreCurrentStop = true;
                }
                else
                {
                    bool IsProgressStatusValid = VehicleData.Info.m_vehicleAI.GetProgressStatus(VehicleID, ref VehicleData, out this.CurrentStoppedPosition, out MaximumPosition);

                    if (IsProgressStatusValid)
                    {
                        this.IgnoreCurrentStop = false;
                    }
                    else
                    {
                        this.IgnoreCurrentStop = true;
                    }
                }

                this.LastNextStop = VehicleData.m_targetBuilding;
            }

            if (this.IgnoreCurrentStop)
            {
                IsUnbunched = true;
                return true;
            }

            //Find Previous vehicle position
            float PreviousVehiclePosition;

            Vehicle PreviousVehicleData = Singleton<VehicleManager>.instance.m_vehicles.m_buffer[this.PreviousVehicleID];

            if ((PreviousVehicleData.m_flags & Vehicle.Flags.Created) != Vehicle.Flags.Created)
            {
                IsUnbunched = true;
                return false;
            }

            bool IsPreviousVehicleProgressStatusValid = PreviousVehicleData.Info.m_vehicleAI.GetProgressStatus(this.PreviousVehicleID, ref PreviousVehicleData, out PreviousVehiclePosition, out MaximumPosition);

            if (!IsPreviousVehicleProgressStatusValid)
            {
#if DEBUG
                Helper.PrintError("LineID: " + VehicleData.m_transportLine + "; VehicleID: " + VehicleID + "; !IsPreviousVehicleProgressStatusValid");
#endif
                IsUnbunched = true;
                return true;
            }

            //Calculate the difference between this and the previous vehicle positions
            float CurrentDifference = PreviousVehiclePosition - this.CurrentStoppedPosition;
            if (CurrentDifference < 0)
            {
                CurrentDifference = CurrentDifference + MaximumPosition;
            }

            //Calculate distance to maintain
            float DistanceToMaintain = (MaximumPosition / NumberOfVehicles) - VehicleUnchunched.GetVehicleLength(VehicleData.Info.m_vehicleType);

            if (CurrentDifference > DistanceToMaintain)
            {
#if DEBUG
                Helper.PrintError("LineID: " + VehicleData.m_transportLine + "; VehicleID: " + VehicleID + "; CurrentDifference: " + CurrentDifference + "; DistanceToMaintain: " + DistanceToMaintain);
#endif
                this.IgnoreCurrentStop = true;

                IsUnbunched = true;

                return true;
            }
            else
            {
                IsUnbunched = false;
                return true;
            }
        }

        public static float GetVehicleLength(VehicleInfo.VehicleType VehicleType)
        {
            int NumberOfSquares;
            switch (VehicleType)
            {
                case VehicleInfo.VehicleType.Car:
                    NumberOfSquares = 8;
                    break;
                case VehicleInfo.VehicleType.CableCar:
                    NumberOfSquares = 1;
                    break;
                case VehicleInfo.VehicleType.Ferry:
                    NumberOfSquares = 6;
                    break;
                case VehicleInfo.VehicleType.Blimp:
                    NumberOfSquares = 6;
                    break;
                case VehicleInfo.VehicleType.Train:
                    NumberOfSquares = 16;
                    break;
                case VehicleInfo.VehicleType.Metro:
                    NumberOfSquares = 12;
                    break;
                case VehicleInfo.VehicleType.Monorail:
                    NumberOfSquares = 6;
                    break;
                case VehicleInfo.VehicleType.Tram:
                    NumberOfSquares = 9;
                    break;
                default://This is never supposed to happen
#if DEBUG
                    Helper.PrintError("TransportLineID: Unknown - Invalid TransportType: " + VehicleType);
#endif
                    NumberOfSquares = 8;
                    break;
            }

            return NumberOfSquares * 10;
        }
    }
}