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

namespace SOTS.Items.Pyramid
{	[AutoloadEquip(EquipType.Shield)]
	public class SpiritShield : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Shield");
			Tooltip.SetDefault("Increases void regen by 1, life regen by 1, and reduces damage taken by 1%");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 26;     
            item.height = 28;   
            item.value = Item.sellPrice(0, 3, 50, 0);
            item.rare = 6;
			item.defense = 1;
			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegen += 0.1f;
			player.lifeRegen += 1;
			player.endurance += 0.01f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SoulResidue", 25);
			recipe.AddIngredient(null, "EmeraldBracelet", 1);
			recipe.AddIngredient(49, 1); //band of regen
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}