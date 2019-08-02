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


namespace SOTS.Items.ChestItems
{
	public class TridentFlare : ModItem
	{ 	int firerate = 0;
		bool overheated = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Trident Flare");
			Tooltip.SetDefault("Replaces all red flares shot with trident flares\nTrident flares pierce defense, do 10 extra damage, and shoot fire upon hitting an enemy");
		}
		public override void SetDefaults()
		{
            
            item.width = 26;     
            item.height = 32;     
            item.value = 52500;
            item.rare = 3;
			item.accessory = true;
			item.defense = 1;
			
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
     
			for(int j = 0; j < 1000; j++)
			{	
					Projectile projectile = Main.projectile[j];
					
					if(projectile.friendly == true && projectile.hostile == false && player == Main.player[projectile.owner] && projectile.type == 163)
					{
						
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("TridentFlare"), projectile.damage, projectile.knockBack, player.whoAmI);

							projectile.Kill();
						
						
					}
			}
			
		}
	}
}
