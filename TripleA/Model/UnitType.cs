namespace TripleA.Model
{
    public class UnitType
    {
        public int Attack { get; internal set; }
        public bool CanBombard { get; internal set; }
        public int CarrierCapacity { get; internal set; }
        public int CarrierCost { get; internal set; }
        public int Defense { get; internal set; }
        public bool IsAir { get; internal set; }
        public bool IsArtillery { get; internal set; }
        public bool IsArtillerySupportable { get; internal set; }
        public bool IsSea { get; internal set; }
        public bool IsStrategicBomber { get; internal set; }
        public bool IsTwoHit { get; internal set; }
        public int Movement { get; internal set; }
        public string Name { get; internal set; }
        public int TransportCost { get; internal set; }

        public override bool Equals(object obj)
        {
            var isEqual = false;
            if(obj != null && obj is UnitType)
            {
                var otherUnitType = obj as UnitType;
                isEqual = Name.Equals(otherUnitType.Name);
            }
            return isEqual;
        }
    }
}