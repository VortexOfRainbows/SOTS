using SOTS.NPCs.Inferno;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using SOTS.NPCs.Tide;
using SOTS.NPCs.AbandonedVillage;

namespace SOTS.Common.ItemDropConditions
{
	public class DownedSubspaceDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				Player player = info.player;
				if (SOTSWorld.downedSubspace && player.ZoneUnderworldHeight)// && !info.npc.CountsAsACritter)
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.DefeatSubspaceSerpent");
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.DefeatAdvisor");
		}
	}
	public class PreCurseDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				if (!SOTSWorld.downedCurse)
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.DefeatPharaohsCurseBefor");
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.DefeatPharaohsCurse");
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.DefeatEyeofCthulhuBefor");
		}
	}
	public class PostBoss1DropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				if (NPC.downedBoss1)
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.DefeatEyeofCthulhu");
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.Killedfully");
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.SansUndertale");
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.OrangeLesserWisps");
		}
	}
	public class PreBoss1DropConditionEoW : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				if (!NPC.downedBoss1 && info.npc.boss)
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.DefeatEyeofCthulhuBefor");
		}
	}
	public class PostBoss1DropConditionEoW : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation && info.npc.boss)
			{
				if (NPC.downedBoss1)
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
			return Language.GetTextValue("Mods.SOTS.DropCondition.DefeatEyeofCthulhu");
		}
    }
    public class IsCore : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
            {
                if (info.npc.ModNPC is PhantarayCore c)
                {
                    if (c.isCore)
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
            return Language.GetTextValue("Mods.SOTS.DropCondition.PhantarayCore1");
        }
    }
    public class IsNotCore : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
            {
                if (info.npc.ModNPC is PhantarayCore c)
                {
                    if (!c.isCore)
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
            return Language.GetTextValue("Mods.SOTS.DropCondition.PhantarayCore2");
        }
    }
}