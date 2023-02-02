namespace VehicleUnbuncher
{
    public class Settings
    {
        public byte MaxWaitCounter = 100;

        public void Validate()
        {
            if(this.MaxWaitCounter < 12)
            {
                this.MaxWaitCounter = 12;
            }
        }
    }
}
