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
using SOTS.Void;

namespace SOTS.Items.Celestial
{
	public class VoidspaceEmblem : ModItem
	{	int timer = 1;
		float boost = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Voidspace Emblem");
			Tooltip.SetDefault("Increases void damage and magic damage by 10%\nDecreases void cost by 10%\nIncreases void regen by 2.5 and max void by 50\nGenerates blood essence when hit");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 26;     
            item.height = 28;   
            item.value = Item.sellPrice(0, 7, 75, 0);
            item.rare = 8;
			item.accessory = true;

		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SanguiteBar", 15);
			recipe.AddIngredient(null, "LifeDevourer", 1);
			recipe.AddIngredient(null, "WormWoodParasite", 1);
			recipe.AddIngredient(null, "VoidenBracelet", 1);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidDamage += 0.1f;
			voidPlayer.voidCost -= 0.10f;
			player.magicDamage += 0.1f;
			voidPlayer.voidRegen += 0.25f;
			voidPlayer.voidMeterMax2 += 50;
			
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			if(modPlayer.onhit == 1)
			{
				player.QuickSpawnItem(mod.ItemType("BloodEssence"), 1 + (modPlayer.onhitdamage / 19));	
				voidPlayer.voidMeter += 5 + (modPlayer.onhitdamage / 9);
			}
		}
	}
}