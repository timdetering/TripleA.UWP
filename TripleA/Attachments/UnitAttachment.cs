using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleA.Model;

namespace TripleA.Attachments
{
    public class UnitAttachment : IAttachment
    {
        public void Apply(string attachTo, Dictionary<string, string> options)
        {
            var unitType = Game.Instance.UnitTypes.Where(f => f.Name == attachTo).FirstOrDefault();
            if(unitType != null)
            {
                if (options.ContainsKey("movement"))
                {
                    unitType.Movement = int.Parse(options["movement"]);
                }
                if (options.ContainsKey("attack"))
                {
                    unitType.Attack = int.Parse(options["attack"]);
                }
                if (options.ContainsKey("defense"))
                {
                    unitType.Defense = int.Parse(options["defense"]);
                }
                if (options.ContainsKey("transportCost"))
                {
                    unitType.TransportCost = int.Parse(options["transportCost"]);
                }
                if (options.ContainsKey("transportCapacity"))
                {
                    unitType.TransportCost = int.Parse(options["transportCapacity"]);
                }
                if (options.ContainsKey("carrierCost"))
                {
                    unitType.CarrierCost = int.Parse(options["carrierCost"]);
                }
                if (options.ContainsKey("carrierCapacity"))
                {
                    unitType.CarrierCapacity = int.Parse(options["carrierCapacity"]);
                }

                if (options.ContainsKey("artillerySupportable"))
                {
                    unitType.IsArtillerySupportable = bool.Parse(options["artillerySupportable"]);
                }
                if (options.ContainsKey("isAir"))
                {
                    unitType.IsAir = bool.Parse(options["isAir"]);
                }
                if (options.ContainsKey("isStrategicBomber"))
                {
                    unitType.IsStrategicBomber = bool.Parse(options["isStrategicBomber"]);
                }
                if (options.ContainsKey("isTwoHit"))
                {
                    unitType.IsTwoHit = bool.Parse(options["isTwoHit"]);
                }
                if (options.ContainsKey("artillery"))
                {
                    unitType.IsArtillery = bool.Parse(options["artillery"]);
                }
                if (options.ContainsKey("isSea"))
                {
                    unitType.IsSea = bool.Parse(options["isSea"]);
                }
                if (options.ContainsKey("canBombard"))
                {
                    unitType.CanBombard = bool.Parse(options["canBombard"]);
                }
            }
        }
    }
}