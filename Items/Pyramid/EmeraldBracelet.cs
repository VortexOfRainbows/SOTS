using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Pyramid
{
	public class EmeraldBracelet : ModItem
	{	int timer = 1;
		float boost = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Emerald Bracelet");
			Tooltip.SetDefault("Periodically releases sand clouds");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 34;     
            item.height = 28;   
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.rare = 3;
			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			timer += 1;
		
		
					if(timer == 60)
					{
					Projectile.NewProjectile(player.Center.X, player.position.Y, -10, -10, mod.ProjectileType("SandyCloud"), 50, 0, player.whoAmI);
					Projectile.NewProjectile(player.Center.X, player.position.Y, 10, 10, mod.ProjectileType("SandyCloud"), 50, 0, player.whoAmI);
					Projectile.NewProjectile(player.Center.X, player.position.Y, 10, -10, mod.ProjectileType("SandyCloud"), 50, 0, player.whoAmI);
					Projectile.NewProjectile(player.Center.X, player.position.Y, -10, 10, mod.ProjectileType("SandyCloud"), 50, 0, player.whoAmI);
					timer = 0;
					}
				
		}
		
	}
}