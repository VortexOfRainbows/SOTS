using SOTS.Items.Furniture.Functional;
using System;
using static SOTS.Items.Furniture.Functional.MineralariumTE;
using Terraria.ModLoader;
using static SOTS.Items.Furniture.Functional.HydroponicsHerbSystem;

namespace SOTS
{
    public partial class SOTS
    {
        public override object Call(params object[] args)
        {
            try
            {
                if (args is null)
                    Logger.Error("SOTS.Call error: Arguments are null.");

                if (args.Length == 0)
                    Logger.Error("SOTS.Call error: Arguments are empty.");

                if (args[0] is not string context)
                    return null;

                switch (context)
                {
                    case "AddMineralariumOre":
                        return OreType.ParseNewOre(args[1..]);
                    case "AddHydroponicsHerb":
                        return HydroponicsHerbSystem.ParseNewHerb(args[1..]);
                }
            }
            catch (Exception e)
            {
                Logger.Error("SOTS.Call error: " + e.Message + "\n" + e.StackTrace);
            }

            return null;
        }
    }
}
