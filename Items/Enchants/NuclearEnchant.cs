using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class NuclearEnchant : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nuclear Relic");
			Tooltip.SetDefault("Summons nuclear aura to surround you\nDev Item");
		}
		public override void SetDefaults()
		{
      
            item.width = 34;     
            item.height = 34;   
            item.value = 0;
            item.rare = 7;
			item.expert = true;
			item.accessory = true;
			item.defense = 9;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CoreOfExpertise1", 1);
			recipe.AddIngredient(null, "CoreOfCreation", 3);
			recipe.AddIngredient(null, "TomeOfTheReaper", 1);
			recipe.AddIngredient(null, "DesertEye", 1);
			recipe.AddIngredient(ItemID.InfernoFork,1);
			recipe.AddIngredient(ItemID.Shackle,4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			timer += 1;
		
		
					if(timer == 35)
					{
					Projectile.NewProjectile(player.Center.X, player.position.Y, 0, 0, 296, 66, 0, player.whoAmI);
					Projectile.NewProjectile(player.Center.X, player.position.Y, 0, 0, 696, 66, 0, player.whoAmI);
					Projectile.NewProjectile(player.Center.X, player.position.Y, 0, 10, 294, 50, 0, player.whoAmI);
					Projectile.NewProjectile(player.Center.X, player.position.Y, 10, 0, 294, 50, 0, player.whoAmI);
					Projectile.NewProjectile(player.Center.X, player.position.Y, 0, -10, 294, 50, 0, player.whoAmI);
					Projectile.NewProjectile(player.Center.X, player.position.Y, -10, 0, 294, 50, 0, player.whoAmI);
					Projectile.NewProjectile(player.Center.X, player.position.Y, -10, -10, 294, 50, 0, player.whoAmI);
					Projectile.NewProjectile(player.Center.X, player.position.Y, 10, 10, 294, 50, 0, player.whoAmI);
					Projectile.NewProjectile(player.Center.X, player.position.Y, 10, -10, 294, 50, 0, player.whoAmI);
					Projectile.NewProjectile(player.Center.X, player.position.Y, -10, 10, 294, 50, 0, player.whoAmI);
					timer = 0;
					}
				
		}
		
	}
}
