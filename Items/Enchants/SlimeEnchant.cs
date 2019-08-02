using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class SlimeEnchant : ModItem
	{	int Probe = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slimey Relic");
			Tooltip.SetDefault("Summons a buffed Slime Probe to assist you");
		}
		public override void SetDefaults()
		{
      
            item.width = 24;     
            item.height = 26;   
            item.value = 50000;
            item.rare = 10;
			item.expert = true;
			item.accessory = true;
			item.defense = 3;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GelBar", 20);
			recipe.AddIngredient(null, "AntimaterialMandible", 5);
			recipe.AddIngredient(null, "GelledLeatherHelmet", 1);
			recipe.AddIngredient(null, "GelledLeatherJacket", 1);
			recipe.AddIngredient(null, "GelledLeatherLeggings", 1);
			recipe.AddIngredient(null, "CoreOfCreation", 1);
			recipe.AddIngredient(ItemID.Leather,30);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (Probe == -1)
			{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("SlimeProbe"), 21, 0, player.whoAmI);
					}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != mod.ProjectileType("SlimeProbe"))
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("SlimeProbe"), 21, 0, player.whoAmI);
				}
				Main.projectile[Probe].timeLeft = 6;
		}
		
	}
}
