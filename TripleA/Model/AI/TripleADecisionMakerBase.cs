using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripleA.Model.AI
{
    public abstract class TripleADecisionMakerBase : IGameDecisionMaker
    {
        public void Decide(string stepTypeName, Game data)
        {
            switch (stepTypeName)
            {
                case "tech":
                    ResearchTechnology(data);
                    break;
                case "battle":
                    Battle(data);
                    break;
                case "move":
                    Move(data);
                    break;
                case "place":
                    PlaceUnits(data);
                    break;
                case "purchase":
                    PurchaseUnits(data);
                    break;
                case "endTurn":
                    EndTurn(data);
                    break;
                case "endRound":
                    EndRound(data);
                    break;
                case "placeBid":
                    PlaceBid(data);
                    break;
                case "bid":
                    Bid(data);
                    break;
            }
        }

        public abstract void ResearchTechnology(Game data);
        public abstract void Battle(Game data);
        public abstract void Move(Game data);
        public abstract void PurchaseUnits(Game data);
        public abstract void PlaceUnits(Game data);
        public abstract void EndTurn(Game data);
        public abstract void EndRound(Game data);
        public abstract void PlaceBid(Game data);
        public abstract void Bid(Game data);
    }
}