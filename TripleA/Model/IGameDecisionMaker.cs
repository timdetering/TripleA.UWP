namespace TripleA.Model
{
    public interface IGameDecisionMaker
    {
        void Decide(string step, Game data);
    }
}