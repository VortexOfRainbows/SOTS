using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Items.SpecialDrops;

namespace SOTS.Items.ChestItems
{
	public class TinyPlanet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiny Planet");
			Tooltip.SetDefault("Surrounds you with 2 orbital projectiles");
		}
		public override void SetDefaults()
		{
			item.damage = 10;
            item.width = 34;     
            item.height = 34;   
            item.value = Item.sellPrice(0, 0, 75, 0);
            item.rare = ItemRarityID.Blue;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TinyPlanetFish>(), 1);
			recipe.AddIngredient(ItemID.StoneBlock, 100);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (Main.myPlayer == player.whoAmI)
			{
				int damage = (int)(item.damage * (1f + (player.allDamage - 1f)));
				modPlayer.tPlanetDamage += damage;
				modPlayer.tPlanetNum += 2;
			}
		}
	}
}