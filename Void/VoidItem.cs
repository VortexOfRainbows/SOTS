using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.Items.Celestial;
using SOTS.Items.Otherworld;
using SOTS.Items.Permafrost;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Void
{
	public abstract class VoidItem : ModItem
	{
		//public int voidUsage = 0;
		public static int voidMana;
		public static int voidManaAmount;
		public virtual void SafeSetDefaults() 
		{
			
		}
		public sealed override void SetDefaults() {
			item.shoot = 10; 
			item.magic = false;
			item.melee = false;
			item.ranged = false; 
			SafeSetDefaults();
			item.mana = 1;
			item.thrown = false;
		}
		public sealed override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			float realDamageBoost = voidPlayer.voidDamage;
			GetVoid(player);
			item.mana = 1;
			if(!item.summon)
			{
				float voidCostMult = 1f;
				if (item.prefix == mod.GetPrefix("Famished").Type || item.prefix == mod.GetPrefix("Precarious").Type || item.prefix == mod.GetPrefix("Potent").Type || item.prefix == mod.GetPrefix("Omnipotent").Type)
				{
					voidCostMult = item.GetGlobalItem<PrefixItem>().voidCostMultiplier;
				}
				voidManaAmount = (int)(voidMana * voidPlayer.voidCost * voidCostMult);
				if (voidManaAmount < 1 && item.type != ModContent.ItemType<FrigidJavelin>())
				{
					voidManaAmount = 1;
				}
			}
			else
            {
				voidManaAmount = voidMana;
            }
			add += realDamageBoost - 1;
		}
		public override void GetWeaponKnockback(Player player, ref float knockback) 	
		{
			if(!item.magic && !item.thrown && !item.summon && !item.melee && !item.ranged)
			{
				knockback = knockback + VoidPlayer.ModPlayer(player).voidKnockback;
			}
		}
		public sealed override void GetWeaponCrit(Player player, ref int crit) 
		{
			crit = crit + VoidPlayer.ModPlayer(player).voidCrit;
		}
		public virtual void GetVoid(Player player)
		{
			voidMana = 1;
			if(item.summon)
            {
				voidMana = VoidPlayer.minionVoidCost(VoidPlayer.voidMinion(item.shoot));
				if (item.type == ModContent.ItemType<Lemegeton>())
					voidMana *= 3;
            }
		}
		public sealed override void ModifyTooltips(List<TooltipLine> tooltips) 
		{
			
			//Player player = Main.player[localPlayer];
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null) 
			{
				string[] splitText = tt.text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				
				tt.text = damageValue + " void " + damageWord;
				
				if(item.melee)
					tt.text = damageValue + " void + melee " + damageWord;
				
				if(item.ranged)
					tt.text = damageValue + " void + ranged " + damageWord;
			
				if(item.magic)
					tt.text = damageValue + " void + magic " + damageWord;

				if (item.summon)
					tt.text = damageValue + " void + summon " + damageWord;
			}
				
			string voidManaText = voidManaAmount.ToString();
			TooltipLine tt2 = tooltips.FirstOrDefault(x => x.Name == "UseMana" && x.mod == "Terraria");
			if (tt2 != null) 
			{
				string[] splitText = tt2.text.Split(' ');
				//string damageValue = splitText.First();
				//string damageWord = splitText.Last();
				if(item.accessory == false)
					tt2.text = "Consumes " + voidManaText + " void";
				else
				{
					tooltips.Remove(tt2);
				} 
			}
			
		}	
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) 
		{
			if(item.shoot != 10)
			{
				return true;
			}
			return false;
		}
		public sealed override bool ConsumeAmmo(Player player) ///this is the only way i have found to make void not be consumed when ammo is not present
		{
			if(item.useAmmo != 0 && BeforeDrainMana(player))
				DrainMana(player);
			bool canUse = BeforeConsumeAmmo(player);
			if(!canUse)
			{
				return false;
			}
			return true;
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
			bool cursed = player.HasBuff(BuffID.Cursed) || (player.HasBuff(BuffID.Silenced) && item.magic);
			if (cursed)
				return false;
			int currentVoid = voidPlayer.voidMeterMax2 - voidPlayer.lootingSouls - voidPlayer.VoidMinionConsumption;
			if (voidPlayer.safetySwitch && voidPlayer.voidMeter < voidMana && !item.summon && !voidPlayer.frozenVoid)
			{
				return false;
			}
			if(!canUse || player.FindBuffIndex(ModContent.BuffType<VoidRecovery>()) > -1 || item.useAnimation < 2 || (player.altFunctionUse != 2 && item.summon && currentVoid < voidMana))
			{
				return false;
			}
			OnUseEffects(player);
			item.mana = 0;
			if(item.useAmmo == 0 && BeforeDrainMana(player) && !item.summon)
				DrainMana(player);
			return true;
		}
		//<summary>
		// return false to not consume void
		//</summary>
		public virtual bool BeforeDrainMana(Player player)
		{
			return true;
		}
		public virtual bool BeforeUseItem(Player player) 
		{
			return true;
		}
		//<summary>
		// return false to not consume ammo
		//</summary>
		public virtual bool BeforeConsumeAmmo(Player player)
		{
			return true;
		}
		public static void DrainMana(Player player)
		{
			if(voidManaAmount > 0)
			{
				if(player.whoAmI == Main.myPlayer)
					VoidPlayer.ModPlayer(player).voidMeter -= voidManaAmount;
			}
		}
	}
}