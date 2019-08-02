using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Chess
{
	public class SightChalice : ModItem
	{int Probe = -1;
	int Probe2 = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Chalice");
			Tooltip.SetDefault("Summons Cursed Twins to protect you");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 22;     
            item.height = 36;   
            item.value = 125000;
            item.rare = 5;

			item.accessory = true;
			item.defense = 0;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.maxMinions++;
			if (Probe == -1)
			{
					Probe = Projectile.NewProjectile(player.position.X + 80, player.position.Y, 0, 0, mod.ProjectileType("SightRetanimini"), 37, 0, player.whoAmI);
					}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != mod.ProjectileType("SightRetanimini"))
				{
					Probe = Projectile.NewProjectile(player.position.X + 80, player.position.Y, 0, 0, mod.ProjectileType("SightRetanimini"), 37, 0, player.whoAmI);
				}
				Main.projectile[Probe].timeLeft = 6;
				
			player.maxMinions++;
			if (Probe2 == -1)
			{
					Probe2 = Projectile.NewProjectile(player.position.X - 80, player.position.Y, 0, 0, mod.ProjectileType("SightSpazmamini"), 37, 0, player.whoAmI);
					}
				if (!Main.projectile[Probe2].active || Main.projectile[Probe2].type != mod.ProjectileType("SightSpazmamini"))
				{
					Probe2 = Projectile.NewProjectile(player.position.X - 80, player.position.Y, 0, 0, mod.ProjectileType("SightSpazmamini"), 37, 0, player.whoAmI);
				}
				Main.projectile[Probe2].timeLeft = 6;
		}
	}
}