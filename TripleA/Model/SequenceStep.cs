namespace TripleA.Model
{
    public class SequenceStep
    {
        public SequenceStep()
        {
        }

        public SequenceStepType Delegate { get; set; }
        public string Display { get; internal set; }
        public int? MaxRunCount { get; internal set; }
        public string Name { get; set; }
        public Player Player { get;  set; }
    }
}