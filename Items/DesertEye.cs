using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class DesertEye : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Desert Eye");
			Tooltip.SetDefault("May the desert syphon shadows from the light");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 7));
		}
		public override void SetDefaults()
		{
      
            item.width = 48;     
            item.height = 48;   
            item.value = 250000;
            item.rare = 7;
			item.expert = true;
			item.accessory = true;
			item.defense = 3;

		}

		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			timer += 1;
		
		
					if(timer == 60)
					{
					Projectile.NewProjectile(player.Center.X, player.position.Y, -10, -10, 294, 50, 0, player.whoAmI);
					Projectile.NewProjectile(player.Center.X, player.position.Y, 10, 10, 294, 50, 0, player.whoAmI);
					Projectile.NewProjectile(player.Center.X, player.position.Y, 10, -10, 294, 50, 0, player.whoAmI);
					Projectile.NewProjectile(player.Center.X, player.position.Y, -10, 10, 294, 50, 0, player.whoAmI);
					timer = 0;
					}
				
		}
		
	}
}
