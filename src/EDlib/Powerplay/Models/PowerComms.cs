namespace EDlib.Powerplay
{
    public class PowerComms
    {
        public int Id { get; set; }

        public string ShortName { get; set; }

        public string Reddit { get; set; }

        public string Comms { get; set; }

        public PowerComms(int id, string shortName, string reddit, string comms)
        {
            Id = id;
            ShortName = shortName;
            Reddit = reddit;
            Comms = comms;
        }

        public override string ToString()
        {
            return $"({Id}) {ShortName}, Reddit: {Reddit}, Comms: {Comms}";
        }
    }
}
