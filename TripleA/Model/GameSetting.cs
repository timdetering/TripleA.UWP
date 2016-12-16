namespace TripleA.Model
{
    public class GameSetting
    {
        public string FieldType { get; internal set; }
        public bool IsEditable { get; internal set; }
        public string Name { get; internal set; }
        public string Value { get; internal set; }
    }
}