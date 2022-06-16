using SOTS.NPCs.Inferno;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace SOTS.Common.ItemDropConditions
{
	public class DownedSubspaceDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				Player player = info.player;
				if (SOTSWorld.downedSubspace && player.ZoneUnderworldHeight)
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
			return "Drops if Subspace Serpent has been defeated";
		}
	}
	public class DownedAdvisorDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				if (SOTSWorld.downedAdvisor)
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
			return "Drops if Advisor has been defeated";
		}
	}
	public class DownedCurseDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				if (SOTSWorld.downedCurse)
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
			return "Drops if Pharaoh's Curse has been defeated";
		}
	}
	public class PreBoss1DropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				if (!NPC.downedBoss1)
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
			return "Drops before the Eye of Cthulhu has been defeated";
		}
	}
	public class OtherworldSpiritAlternateCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				if (info.npc.localAI[1] == -1)
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
			return "Drops when killed fully";
		}
	}
	public class IsSansWisp : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				if (info.npc.ModNPC is LesserWisp wisp)
				{
					if(wisp.sans)
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
			return "Sans Undertale";
		}
	}
	public class NotSansWisp : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				if (info.npc.ModNPC is LesserWisp wisp)
				{
					if (!wisp.sans)
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
			return "Orange Lesser Wisps";
		}
	}
}