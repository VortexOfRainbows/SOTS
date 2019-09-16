using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.ChestItems
{
	public class TinyPlanet : ModItem
	{	int Probe = -1;
		int Probe2 = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiny Planet");
			Tooltip.SetDefault("Surrounds you with 2 orbital projectiles");
		}
		public override void SetDefaults()
		{
	
			
            item.width = 34;     
            item.height = 34;   
            item.value = Item.sellPrice(0, 0, 75, 0);
            item.rare = 3;
			item.accessory = true;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "TinyPlanetFish", 1);
			recipe.AddIngredient(ItemID.StoneBlock, 100);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
				if (Probe == -1)
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyPlanetTear"), 14, 0, player.whoAmI);
					Main.projectile[Probe].ai[1] = 180;
				}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != mod.ProjectileType("TinyPlanetTear"))
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyPlanetTear"), 14, 0, player.whoAmI);
					Main.projectile[Probe].ai[1] = 180;
				}
				Main.projectile[Probe].timeLeft = 6;
				if (Probe2 == -1)
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyPlanetTear"), 14, 0, player.whoAmI);
				}
				if (!Main.projectile[Probe2].active || Main.projectile[Probe2].type != mod.ProjectileType("TinyPlanetTear"))
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyPlanetTear"), 14, 0, player.whoAmI);
				}
				Main.projectile[Probe2].timeLeft = 6;
		}
	}
}