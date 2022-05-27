using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;

namespace SOTS.Common.ItemDropConditions
{
	public class NatureFragmentDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				Player player = info.player;
				bool ZoneForest = !player.GetModPlayer<SOTSPlayer>().PyramidBiome && player.ZoneForest;
				if (ZoneForest || player.ZoneJungle)
                {
					return true;
                }
			}
			return false;
		}
		public bool CanShowItemDropInUI()
		{
			return true;
		}
		public string GetConditionDescription()
		{
			return "Drops in forest and jungle biomes";
		}
	}
	public class EarthFragmentDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				Player player = info.player;
				if (player.ZoneUndergroundDesert || player.ZoneDesert || player.GetModPlayer<SOTSPlayer>().PyramidBiome || player.ZoneRockLayerHeight)
				{
					return true;
				}
			}
			return false;
		}
		public bool CanShowItemDropInUI()
		{
			return true;
		}
		public string GetConditionDescription()
		{
			return "Drops in Pyramid, desert, and underground biomes";
		}
	}
	public class PermafrostFragmentDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				Player player = info.player;
				if (player.ZoneSnow)
				{
					return true;
				}
			}
			return false;
		}
		public bool CanShowItemDropInUI()
		{
			return true;
		}
		public string GetConditionDescription()
		{
			return "Drops in cold biomes";
		}
	}
	public class OtherworldFragmentDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				Player player = info.player;
				if (player.ZoneSkyHeight || player.ZoneMeteor)
				{
					return true;
				}
			}
			return false;
		}
		public bool CanShowItemDropInUI()
		{
			return true;
		}
		public string GetConditionDescription()
		{
			return "Drops in sky and meteor biomes";
		}
	}
	public class TideFragmentDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				Player player = info.player;
				if (player.ZoneBeach || player.ZoneDungeon)
				{
					return true;
				}
			}
			return false;
		}
		public bool CanShowItemDropInUI()
		{
			return true;
		}
		public string GetConditionDescription()
		{
			return "Drops in beach and dungeon biomes";
		}
	}
	public class InfernoFragmentDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				Player player = info.player;
				if (player.ZoneUnderworldHeight)
				{
					return true;
				}
			}
			return false;
		}
		public bool CanShowItemDropInUI()
		{
			return true;
		}
		public string GetConditionDescription()
		{
			return "Drops in the underworld";
		}
	}
	public class EvilFragmentDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				Player player = info.player;
				if (player.ZoneCorrupt || player.ZoneCrimson)
				{
					return true;
				}
			}
			return false;
		}
		public bool CanShowItemDropInUI()
		{
			return true;
		}
		public string GetConditionDescription()
		{
			return "Drops in the evil biome";
		}
	}
	public class ChaosFragmentDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				Player player = info.player;
				if (player.ZoneHallow)
				{
					return true;
				}
			}
			return false;
		}
		public bool CanShowItemDropInUI()
		{
			return true;
		}
		public string GetConditionDescription()
		{
			return "Drops in the hallow biome";
		}
	}
	public class BeachDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				Player player = info.player;
				if (player.ZoneBeach)
				{
					return true;
				}
			}
			return false;
		}
		public bool CanShowItemDropInUI()
		{
			return true;
		}
		public string GetConditionDescription()
		{
			return "Drops in the beach biome";
		}
	}
	public class DungeonDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				Player player = info.player;
				if (player.ZoneDungeon)
				{
					return true;
				}
			}
			return false;
		}
		public bool CanShowItemDropInUI()
		{
			return true;
		}
		public string GetConditionDescription()
		{
			return "Drops in the dungeon";
		}
	}
	public class PlanetariumDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				Player player = info.player;
				if (player.GetModPlayer<SOTSPlayer>().PlanetariumBiome)
				{
					return true;
				}
			}
			return false;
		}
		public bool CanShowItemDropInUI()
		{
			return true;
		}
		public string GetConditionDescription()
		{
			return "Drops in Planetarium biome";
		}
	}
	public class OnFireCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				if (info.npc.HasBuff(BuffID.OnFire))
				{
					return true;
				}
			}
			return false;
		}
		public bool CanShowItemDropInUI()
		{
			return true;
		}
		public string GetConditionDescription()
		{
			return "Drops if on fire";
		}

        public static implicit operator LeadingConditionRule(OnFireCondition v)
        {
            throw new NotImplementedException();
        }
    }
}