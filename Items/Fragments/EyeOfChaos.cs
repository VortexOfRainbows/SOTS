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


namespace SOTS.Items.Fragments
{
	public class EyeOfChaos : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eye of Chaos");
			Tooltip.SetDefault("Increases crit chance by 20%");
		}
		public override void SetDefaults()
		{
            item.width = 34;     
            item.height = 34;     
            item.value = Item.sellPrice(0, 7, 25, 0);
            item.rare = 8;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeCrit += 20;
			player.rangedCrit += 20;
			player.magicCrit += 20;
			player.thrownCrit += 20;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SnakeEyes", 1);
			recipe.AddIngredient(null, "ChaosBadge", 1);
			recipe.AddIngredient(1248, 1); // "EyeoftheGolem"
			//recipe.AddIngredient(ItemID.SpookyWood, 1); //To be replaced later (dissolving chaos)
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
