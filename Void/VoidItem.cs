using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using SOTS.Void;

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
			item.shoot = 10; //placeholder projectile 
			item.magic = false;
			item.melee = false;
			item.ranged = false; //placeholder damage setting
			
			SafeSetDefaults();
			item.mana = 1;
			item.thrown = false;
			item.summon = false;
		}
		public sealed override void GetWeaponDamage(Player player, ref int damage) 
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			float realDamageBoost = voidPlayer.voidDamage;
			GetVoid(player);
			item.mana = 1;
			voidManaAmount = (int)(voidMana * voidPlayer.voidCost);
			float totalBuff = 0;
			/*
			totalBuff = (float)((player.meleeDamage + player.magicDamage + player.minionDamage + player.rangedDamage + player.thrownDamage)/5f - 1f);
				//Finding the universalDamage upgrade
				if(player.meleeDamage <= player.magicDamage && player.meleeDamage <= player.minionDamage && player.meleeDamage <= player.rangedDamage && player.meleeDamage <= player.thrownDamage)
				{
					totalBuff = player.meleeDamage - 1;
				}
				if(player.magicDamage <= player.meleeDamage && player.magicDamage <= player.minionDamage && player.magicDamage <= player.rangedDamage && player.magicDamage <= player.thrownDamage)
				{
					totalBuff = player.magicDamage - 1;
				}	
				if(player.minionDamage <= player.magicDamage && player.minionDamage <= player.meleeDamage && player.minionDamage <= player.rangedDamage && player.minionDamage <= player.thrownDamage)
				{
					totalBuff = player.minionDamage - 1;
				}
				if(player.rangedDamage <= player.meleeDamage && player.rangedDamage <= player.minionDamage && player.rangedDamage <= player.magicDamage && player.rangedDamage <= player.thrownDamage)
				{
					totalBuff = player.rangedDamage - 1;
				}	
				if(player.thrownDamage <= player.meleeDamage && player.thrownDamage <= player.minionDamage && player.thrownDamage <= player.magicDamage && player.thrownDamage <= player.rangedDamage)
				{
					totalBuff = player.thrownDamage - 1;
				}

				if(!item.magic && !item.thrown && !item.summon && !item.melee && !item.ranged)
				{
					realDamageBoost += totalBuff;
				}				
			*/
			
				damage = (int)(damage * realDamageBoost + 5E-06f);
			
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
			if(!item.magic && !item.thrown && !item.summon && !item.melee && !item.ranged)
			{
				crit = crit + VoidPlayer.ModPlayer(player).voidCrit + 4;
			}
		}
		public virtual void GetVoid(Player player)
		{
			voidMana = 1;
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
			}
				
			string voidManaText = voidManaAmount.ToString();
			TooltipLine tt2 = tooltips.FirstOrDefault(x => x.Name == "UseMana" && x.mod == "Terraria");
			if (tt2 != null) 
			{
			string[] splitText = tt2.text.Split(' ');
				//string damageValue = splitText.First();
				//string damageWord = splitText.Last();
				tt2.text = "Consumes " + voidManaText + " void";
				
			}
			
		}	
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) 
		{
				if(item.shoot != 10)
				{
				return true;
				}
				
				return false;
			return true;
		}
		public sealed override bool CanUseItem(Player player) 
		{
			bool canUse = BeforeUseItem(player);
			if(!canUse || player.FindBuffIndex(mod.BuffType("VoidRecovery")) > -1)
			{
				return false;
			}
			item.mana = 0;
			DrainMana(player);
			return true;
		}
		public virtual bool BeforeUseItem(Player player) 
		{
			return true;
		}
		public void DrainMana(Player player)
		{
			if(voidManaAmount > 0)
			{
				VoidPlayer.ModPlayer(player).voidMeter -= voidManaAmount;
			}
		}
	}
}