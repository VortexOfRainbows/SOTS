using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using SOTS.Projectiles;
using SOTS.Void;
using System.Runtime.Remoting.Messaging;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace SOTS.Items.Otherworld
{
	public class TwilightBeads : VoidItem	
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Beads");
			Tooltip.SetDefault("Increases void regen by 1\nGetting hit summons 5 Souls of Retaliation into the air\nEvery 10th void attack will release the souls in the form of a powerful laser");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 34;
			item.maxStack = 1;
            item.width = 30;     
            item.height = 26;   
            item.value = Item.sellPrice(0, 0, 80, 0);
            item.rare = 9;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = player.GetModPlayer<VoidPlayer>();
			voidPlayer.voidRegen += 0.1f;
			BeadPlayer modPlayer = player.GetModPlayer<BeadPlayer>();
			modPlayer.soulDamage += (int)(item.damage * (1f + (voidPlayer.voidDamage - 1f)));
			modPlayer.RetaliationSouls = true;
		}
	}
	public class BeadPlayer : ModPlayer
	{
		public int attackNum = 0;

		public int soulDamage = 0;
		public bool RetaliationSouls = false;
		public bool spawnSouls = false;
		public override void ResetEffects()
		{
			int currentSouls = 0;

			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (mod.ProjectileType("SoulofRetaliation") == proj.type && proj.active && proj.owner == player.whoAmI && proj.timeLeft > 748)
				{
					currentSouls++;
				}
			}
			if (currentSouls < 1)
			{
				spawnSouls = true;
			}
			else
			{
				spawnSouls = false;
			}
			if(attackNum >= 10)
			{
				attackNum++;
				if(attackNum > 12)
				{
					attackNum = 0;
				}
			}
			soulDamage = 0;
			RetaliationSouls = false;
		}
		public void SpawnSouls()
		{
			if (attackNum < 10 && player.whoAmI == Main.myPlayer && spawnSouls && RetaliationSouls)
			{
				for (int i = 0; i < 5; i++)
				{
					Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("SoulofRetaliation"), soulDamage, 1f, player.whoAmI);
				}
			}
		}
		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			SpawnSouls();
			return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
		}
	}
}