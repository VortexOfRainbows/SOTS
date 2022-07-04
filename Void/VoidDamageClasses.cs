using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SOTS.Void
{
	public class VoidGeneric : DamageClass
	{
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass == DamageClass.Generic)
				return StatInheritanceData.Full;
			return new StatInheritanceData(
				damageInheritance: 0f,
				critChanceInheritance: 0f,
				attackSpeedInheritance: 0f,
				armorPenInheritance: 0f,
				knockbackInheritance: 0f
			);
		}
		public override bool UseStandardCritCalcs => true;
	}
	public class VoidMelee : DamageClass
	{
		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass == DamageClass.Generic || damageClass == ModContent.GetInstance<VoidGeneric>() || damageClass == Melee)
				return StatInheritanceData.Full;
			return new StatInheritanceData(
				damageInheritance: 0f,
				critChanceInheritance: 0f,
				attackSpeedInheritance: 0f,
				armorPenInheritance: 0f,
				knockbackInheritance: 0f
			);
		}
		public override bool GetEffectInheritance(DamageClass damageClass)
		{
			if (damageClass == Melee || damageClass == ModContent.GetInstance<VoidGeneric>())
				return true;
			return false;
		}
		public override bool UseStandardCritCalcs => true;
	}
	public class VoidRanged : DamageClass
	{
		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass == DamageClass.Generic || damageClass == ModContent.GetInstance<VoidGeneric>() || damageClass == Ranged)
				return StatInheritanceData.Full;
			return new StatInheritanceData(
				damageInheritance: 0f,
				critChanceInheritance: 0f,
				attackSpeedInheritance: 0f,
				armorPenInheritance: 0f,
				knockbackInheritance: 0f
			);
		}
		public override bool GetEffectInheritance(DamageClass damageClass)
		{
			if (damageClass == Ranged || damageClass == ModContent.GetInstance<VoidGeneric>())
				return true;
			return false;
		}
		public override bool UseStandardCritCalcs => true;
	}
	public class VoidMagic : DamageClass
	{
		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass == DamageClass.Generic || damageClass == ModContent.GetInstance<VoidGeneric>() || damageClass == Magic)
				return StatInheritanceData.Full;
			return new StatInheritanceData(
				damageInheritance: 0f,
				critChanceInheritance: 0f,
				attackSpeedInheritance: 0f,
				armorPenInheritance: 0f,
				knockbackInheritance: 0f
			);
		}
		public override bool GetEffectInheritance(DamageClass damageClass)
		{
			if (damageClass == Magic || damageClass == ModContent.GetInstance<VoidGeneric>())
				return true;
			return false;
		}
		public override bool UseStandardCritCalcs => true;
	}
	public class VoidSummon : DamageClass
	{
		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass == DamageClass.Generic || damageClass == ModContent.GetInstance<VoidGeneric>() || damageClass == Summon)
				return StatInheritanceData.Full;
			return new StatInheritanceData(
				damageInheritance: 0f,
				critChanceInheritance: 0f,
				attackSpeedInheritance: 0f,
				armorPenInheritance: 0f,
				knockbackInheritance: 0f
			);
		}
		public override bool GetEffectInheritance(DamageClass damageClass)
		{
			if (damageClass == Summon || damageClass == ModContent.GetInstance<VoidGeneric>())
				return true;
			return false;
		}
		public override bool UseStandardCritCalcs => true;
	}
}