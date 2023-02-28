using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;

namespace SOTS.Common.ItemDropConditions
{
	public class DontDropOnFriendlyCondition : IItemDropRuleCondition, IProvideItemConditionDescription //Thank you Nalydddd!!! CODE FROM AEQUUS
	{
		public DontDropOnFriendlyCondition()
		{
		}
		public virtual bool CanDrop(DropAttemptInfo info)
		{
			return info.npc?.friendly != true && info.npc.lifeMax > 5 && !NPCID.Sets.CountsAsCritter[info.npc.type];
		}
		public virtual bool CanShowItemDropInUI()
		{
			return true;
		}
		public virtual string GetConditionDescription()
		{
			return null;
		}
	}
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.ForestAndJungleBiomes");
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.PyramidAndDesertAndUndergroundBiomes");
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.ColdBiomes");
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.SkyAndMeteorBiomes");
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.BeachAndDungeonBiomes");
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.Underworld");
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.EvilBiome");
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.HallowBiome");
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.BeachBiome");
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.Dungeon");
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.PlanetariumBiome");
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.OnFire");
		}
    }
}