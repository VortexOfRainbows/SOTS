using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items
{
	public class MegaMelon : ModItem
	{int Probe = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Megamelon");
			Tooltip.SetDefault("Infinite melons!");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 34;     
            item.height = 54;   
            item.value = 155000;
            item.rare = 6;
			item.expert = true;
			item.accessory = true;
			item.defense = 3;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			Probe++;
			if(Probe == 4)
			{
				
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyWatermelonProj"), 25, 0, player.whoAmI); //staggered damage
			}
			if(Probe >= 8)
			{
				Probe = 0;
				
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("MegaMelon"), 21, 0, player.whoAmI);
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyWatermelonProj"), 45, 0, player.whoAmI); //damage staggered
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"TinyWatermelon", 1);
			recipe.AddIngredient(null,"CrimCruptPotion", 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}