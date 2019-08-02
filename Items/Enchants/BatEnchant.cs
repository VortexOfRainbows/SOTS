using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class BatEnchant : ModItem
	{	int Probe = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vampiric Relic");
			Tooltip.SetDefault("Summons a buffed Vampire Probe to assist you");
		}
		public override void SetDefaults()
		{
      
            item.width = 28;     
            item.height = 22;   
            item.value = 0;
            item.rare = 5;
			item.accessory = true;
			item.defense = 3;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BatHat", 1);
			recipe.AddIngredient(null, "BatChest", 1);
			recipe.AddIngredient(null, "BatBoots", 1);
			recipe.AddIngredient(null, "AntimaterialMandible", 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (Probe == -1)
			{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("BVP"), 12, 0, player.whoAmI);
					}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != mod.ProjectileType("BVP"))
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("BVP"), 12, 0, player.whoAmI);
				}
				Main.projectile[Probe].timeLeft = 6;
				
			
		}
		
	}
}
