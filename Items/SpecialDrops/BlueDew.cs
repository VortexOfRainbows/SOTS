using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class BlueDew : ModItem
	{ int Probe = -1;
	int Probe2 = -1;
	int Probe3 = -1;
		public override void SetStaticDefaults()
		{	
		DisplayName.SetDefault("Blue Soul Dew");
			Tooltip.SetDefault("Summons Latios to protect you with Draco Meteors");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 250000;
			item.rare = 9;
			item.accessory = true;
		}
		//these wings use the same values as the solar wings
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.buffImmune[83] = true;
			
			if (Probe2 == -1)
			{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Latios"), 101, 0, player.whoAmI);
					}
				if (!Main.projectile[Probe2].active || Main.projectile[Probe2].type != mod.ProjectileType("Latios"))
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Latios"), 101, 0, player.whoAmI);
				}
				Main.projectile[Probe2].timeLeft = 6;
			
			

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"BoosterCore", 1);
			recipe.AddIngredient(null,"LusterBeam", 1);
			recipe.AddIngredient(ItemID.SoulofNight, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}