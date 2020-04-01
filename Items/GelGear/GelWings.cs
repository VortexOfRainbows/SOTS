using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	[AutoloadEquip(EquipType.Wings)]
	public class GelWings : ModItem
	{
		public override void SetStaticDefaults()
		{	
		DisplayName.SetDefault("Gelatinous Wings");
			Tooltip.SetDefault("Allows flight and slow fall\n'It really shouldn't hold up well'");
		}

		public override void SetDefaults()
		{
			item.width = 19;
			item.height = 19;
            item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = 4;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.wingTimeMax = 30;
		}
		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.85f;
			ascentWhenRising = 0.15f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 1.35f;
			constantAscend = 0.195f;
		}
		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			speed = 8f;
			acceleration *= 1.01f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "WormWoodCore", 1);
			recipe.AddIngredient(null, "Wormwood", 24);
			recipe.AddIngredient(ItemID.Feather, 24);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}