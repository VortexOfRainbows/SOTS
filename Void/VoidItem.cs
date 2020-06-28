using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
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
			item.summon = false;
		}
		public sealed override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			float realDamageBoost = voidPlayer.voidDamage;
			GetVoid(player);
			item.mana = 1;
			voidManaAmount = (int)(voidMana * voidPlayer.voidCost);
			if(voidManaAmount < 1)
			{
				voidManaAmount = 1;
			}
			add += 1 - realDamageBoost;
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
		public sealed override bool CanUseItem(Player player) 
		{
			bool canUse = BeforeUseItem(player);
			if(!canUse || player.FindBuffIndex(mod.BuffType("VoidRecovery")) > -1 || item.useAnimation < 2)
			{
				return false;
			}
			item.mana = 0;
			if(item.useAmmo == 0 && BeforeDrainMana(player))
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