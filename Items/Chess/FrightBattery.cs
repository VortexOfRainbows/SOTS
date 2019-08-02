using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Chess
{
	public class FrightBattery : ModItem
	{int Probe = -1;
	int Probe2 = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fright Battery");
			Tooltip.SetDefault("Randomly discharge\nGrants unlimited rocket boot flight\nCanceled by some other types of rocket boots\nStacks with wings");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 26;     
            item.height = 36;   
            item.value = 125000;
            item.rare = 6;

			item.accessory = true;
			item.defense = 2;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.canRocket = true;
			player.rocketBoots = 1; 
			player.rocketTimeMax += 2; 
			if(Main.rand.Next(3600) == 0)
			{
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 295, 40, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 295, 40, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 295, 40, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 295, 40, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 295, 40, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 295, 40, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 295, 40, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 295, 40, 1, 0);
			}
		}
	}
}