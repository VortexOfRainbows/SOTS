using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class PlanetariumHalo : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Planetary Halo");
			Tooltip.SetDefault("Grants immunity to Blood Tapped and Bleeding\nIncreases max health by 75");
		}
		public override void SetDefaults()
		{
      
            item.width = 32;     
            item.height = 18;   
            item.value = 150000;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.buffImmune[BuffID.Bleeding] = true;
			player.buffImmune[mod.BuffType("BloodTapped")] = true;
			
			player.statLifeMax2 += 75;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PlanetaryCore", 1);
			recipe.AddIngredient(null, "EmptyPlanetariumOrb", 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
