using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.Items.Celestial;
using SOTS.Items.Earth;
using SOTS.Items.Planetarium;
using SOTS.Items.Permafrost;
using SOTS.Items.Secrets;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using SOTS.Items.Tide;
using SOTS.Items;

namespace SOTS.Void
{
	public abstract class VoidItem : ModItem
	{
		//public int voidUsage = 0;
		public virtual void SafeSetDefaults() 
		{

		}
		public sealed override void SetDefaults() {
			Item.shoot = ProjectileID.PurificationPowder; 
			SafeSetDefaults();
			if (Item.DamageType == DamageClass.Melee)
				Item.DamageType = ModContent.GetInstance<VoidMelee>();
			else if (Item.DamageType == DamageClass.Ranged)
				Item.DamageType = ModContent.GetInstance<VoidRanged>();
			else if (Item.DamageType == DamageClass.Magic)
				Item.DamageType = ModContent.GetInstance<VoidMagic>();
			else if (Item.DamageType == DamageClass.Summon)
				Item.DamageType = ModContent.GetInstance<VoidSummon>();
			else if (Item.DamageType != DamageClass.Default) //For items that cost void, but have no damage type
				Item.DamageType = ModContent.GetInstance<VoidGeneric>();
			Item.mana = 1;
		}
		/*public sealed override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			float realDamageBoost = player.GetDamage<VoidGeneric>();
			int baseVoidCost = VoidCost(player);
			Item.mana = 1;
			add += realDamageBoost - 1;
		}
		public sealed override void GetWeaponCrit(Player player, ref int crit) 
		{
			crit = crit + VoidPlayer.ModPlayer(player).voidCrit;
		}*/
		public int VoidCost(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			int baseCost = GetVoid(player);
			int finalCost;
			float voidCostMult = 1f;
			if (!Item.CountsAsClass<VoidSummon>())
			{
				if (Item.prefix == ModContent.PrefixType<Famished>() || Item.prefix == ModContent.PrefixType<Precarious>() || Item.prefix == ModContent.PrefixType<Potent>() || Item.prefix == ModContent.PrefixType<Omnipotent>() || Item.prefix == ModContent.PrefixType<Chthonic>())
				{
					voidCostMult = Item.GetGlobalItem<PrefixItem>().voidCostMultiplier;
				}
				finalCost = (int)(baseCost * voidPlayer.voidCost * voidCostMult);
				if (finalCost < 1 && Item.type != ModContent.ItemType<FrigidJavelin>() && Item.type != ModContent.ItemType<Items.Temple.Revolution>() && Item.type != ModContent.ItemType<PixelBlaster>() && Item.type != ModContent.ItemType<Atlantis>())
				{
					finalCost = 1;
				}
			}
			else
			{
				finalCost = baseCost;
			}
			return finalCost;
        }
		public virtual int GetVoid(Player player)
		{
			int cost = 1;
			if(Item.CountsAsClass<VoidSummon>())
            {
				cost = VoidPlayer.minionVoidCost(VoidPlayer.voidMinion(Item.shoot));
				if (Item.type == ModContent.ItemType<Lemegeton>())
					cost *= 3;
            }
			return cost;
		}
		public sealed override void ModifyTooltips(List<TooltipLine> tooltips) 
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(Main.LocalPlayer); //only the local player will see the tooltip, afterall
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null) 
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = Language.GetTextValue("Mods.SOTS.Common.Damage");
				
				tt.Text = Language.GetTextValue("Mods.SOTS.Common.Void2", damageValue, damageWord);
				
				if(Item.CountsAsClass(DamageClass.Melee))
					tt.Text = Language.GetTextValue("Mods.SOTS.Common.VoidM", damageValue, damageWord);
				
				if(Item.CountsAsClass(DamageClass.Ranged))
					tt.Text = Language.GetTextValue("Mods.SOTS.Common.VoidR", damageValue, damageWord);
			
				if(Item.CountsAsClass(DamageClass.Magic))
					tt.Text = Language.GetTextValue("Mods.SOTS.Common.VoidM2", damageValue, damageWord);

				if (Item.CountsAsClass(DamageClass.Summon))
				{
					if(Item.type == ModContent.ItemType<Tesseract>())
                    {
                        tt.Text = Language.GetTextValue("Mods.SOTS.Common.VoidSPercent", damageValue, damageWord);
                    }
					else
                    {
                        tt.Text = Language.GetTextValue("Mods.SOTS.Common.VoidS", damageValue, damageWord);
                    }
                }
			}
			string voidCostText = VoidCost(Main.LocalPlayer).ToString();
			TooltipLine tt2 = tooltips.FirstOrDefault(x => x.Name == "UseMana" && x.Mod == "Terraria");
			if (tt2 != null) 
			{
				string[] splitText = tt2.Text.Split(' ');
				//string damageValue = splitText.First();
				//string damageWord = splitText.Last();
				if(Item.accessory)
					tooltips.Remove(tt2);
				else
				{
					tt2.Text = Language.GetTextValue("Mods.SOTS.Common.CV", voidCostText);
				} 
			}
			ModifyTooltip(tooltips);
		}
		public virtual void ModifyTooltip(List<TooltipLine> tooltips)
        {

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if(type != 10)
			{
				return true;
			}
			return false;
		}
        public sealed override bool CanConsumeAmmo(Item ammo, Player player)
        {
			//if(Item.useAmmo != 0 && BeforeDrainMana(player))
			//	DrainMana(player);
			bool canUse = BeforeConsumeAmmo(player);
			return canUse;
		}
        public sealed override bool CanBeConsumedAsAmmo(Item weapon, Player player)
        {
			//if(Item.useAmmo != 0 && BeforeDrainMana(player))
			//	DrainMana(player);
			bool canUse = BeforeConsumeAmmo(player);
			return canUse;
		}
		public void OnUseEffects(Player player)
		{
			BeadPlayer modPlayer = player.GetModPlayer<BeadPlayer>();
			modPlayer.attackNum++;
		}
		public sealed override bool CanUseItem(Player player) 
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			bool canUse = BeforeUseItem(player);
			bool cursed = player.HasBuff(BuffID.Cursed) || (player.HasBuff(BuffID.Silenced) && Item.CountsAsClass(DamageClass.Magic));
			if (cursed)
				return false;
			int currentVoid = voidPlayer.voidMeterMax2 - voidPlayer.lootingSouls - voidPlayer.VoidMinionConsumption;
			int finalCost = VoidCost(player);
			bool canDrainMana = BeforeDrainMana(player);
			if ((voidPlayer.safetySwitch && canDrainMana) && voidPlayer.voidMeter < finalCost && !Item.CountsAsClass(DamageClass.Summon) && !voidPlayer.frozenVoid)
			{
				return false;
			}
			if(!canUse || player.FindBuffIndex(ModContent.BuffType<VoidRecovery>()) > -1 || Item.useAnimation < 2 || (player.altFunctionUse != 2 && Item.CountsAsClass(DamageClass.Summon) && currentVoid < finalCost))
			{
				return false;
			}
			OnUseEffects(player);
			//Item.mana = 0;
			if(Item.useAmmo == 0 && canDrainMana && !Item.CountsAsClass(DamageClass.Summon))
				DrainMana(player);
			if (Item.mana > 0)
				player.statMana += Item.mana;
			return true;
		}
        public sealed override bool? UseItem(Player player)
		{
			if (Item.createTile > -1)
			{
				return base.UseItem(player);
			}
			if (Item.useAmmo != 0 && BeforeDrainMana(player) && !Item.CountsAsClass(DamageClass.Summon))
				DrainMana(player);
			return true;
        }
        ///<summary>
        /// return false to not consume void
        ///</summary>
        public virtual bool BeforeDrainMana(Player player)
		{
			return true;
		}
		public virtual bool BeforeUseItem(Player player) 
		{
			return true;
		}
		///<summary>
		/// return false to not consume ammo
		///</summary>
		public virtual bool BeforeConsumeAmmo(Player player)
		{
			return true;
		}
		private float StoredLifeHeals = 0f;
		public void DrainMana(Player player)
		{
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			int finalCost = VoidCost(player);
			if (finalCost > 0)
			{
				if(player.whoAmI == Main.myPlayer)
					vPlayer.voidMeter -= finalCost;
			}
			if(vPlayer.GainHealthOnVoidUse > 0)
			{
				float healAmount = finalCost * vPlayer.GainHealthOnVoidUse + StoredLifeHeals;
				if(healAmount >= 1)
                {
                    player.statLife += (int)healAmount;
                    player.HealEffect((int)healAmount);
                }
				StoredLifeHeals = healAmount % 1f;
			}
		}
	}
}