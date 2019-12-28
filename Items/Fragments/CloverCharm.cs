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
	public class CloverCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clover Charm");
			Tooltip.SetDefault("Critical strikes have a 50% chance to steal life\n2% increased crit chance");
		}
		public override void SetDefaults()
		{
            item.width = 30;     
            item.height = 32;  
            item.value = Item.sellPrice(0, 0, 25, 0);
            item.rare = 1;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			
			player.meleeCrit += 2;
			player.rangedCrit += 2;
			player.magicCrit += 2;
			player.thrownCrit += 2;
			
			if(Main.rand.Next(2) == 0)
				modPlayer.CritLifesteal += Main.rand.Next(3) + 2;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 20);
			recipe.AddIngredient(ItemID.SkyBlueFlower, 1);
			recipe.AddIngredient(null, "FragmentOfNature", 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
