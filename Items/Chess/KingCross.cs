using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Chess
{
	public class KingCross : ModItem
	{int Probe = -1;
	int Probe2 = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Monarchy's Cross");
			Tooltip.SetDefault("Automatic stun beam");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 26;     
            item.height = 34;   
            item.value = 125000;
            item.rare = 7;
			item.expert = true;
			item.accessory = true;
			item.defense = 8;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (Probe == -1)
			{
					Probe = Projectile.NewProjectile(player.Center.X, player.Center.Y - 150, 0, 0, mod.ProjectileType("KingCrossProj"), 1, 1, 0);
					}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != mod.ProjectileType("KingCrossProj"))
				{
					Probe = Projectile.NewProjectile(player.Center.X, player.Center.Y - 150, 0, 0, mod.ProjectileType("KingCrossProj"), 1, 1, 0);
				}
				Main.projectile[Probe].timeLeft = 6;
		}
	}
}