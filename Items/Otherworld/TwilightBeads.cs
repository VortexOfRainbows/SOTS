using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using SOTS.Projectiles;
using SOTS.Projectiles.Otherworld;
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
			Tooltip.SetDefault("Increases void gain by 1\nGetting hit summons 5 Souls of Retaliation into the air, assuming you have less than 5 Souls active already\nEvery 10th void attack will release the souls in the form of a powerful laser");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 34;
			item.maxStack = 1;
            item.width = 30;     
            item.height = 26;   
            item.value = Item.sellPrice(0, 0, 80, 0);
            item.rare = ItemRarityID.LightRed;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = player.GetModPlayer<VoidPlayer>();
			voidPlayer.bonusVoidGain += 1f;
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
				if (ModContent.ProjectileType<SoulofRetaliation>() == proj.type && proj.active && proj.owner == player.whoAmI && proj.timeLeft > 748)
				{
					currentSouls++;
				}
			}
			if (currentSouls < 5)
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
					Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, ModContent.ProjectileType<SoulofRetaliation>(), soulDamage, 1f, player.whoAmI);
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