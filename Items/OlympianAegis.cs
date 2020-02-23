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

namespace SOTS.Items
{	[AutoloadEquip(EquipType.Shield)]
	public class OlympianAegis : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Olympian Aegis");
			Tooltip.SetDefault("Increases life regen by 1 and void regen by 2\nReduces damage taken by 7% and increases crit chance by 4%");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 34;     
            item.height = 42;   
            item.value = Item.sellPrice(0, 4, 75, 0);
            item.rare = 6;
			item.defense = 3;
			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegen += 0.2f;
			player.lifeRegen += 1;
			player.endurance += 0.07f;
			player.meleeCrit += 4;
			player.rangedCrit += 4;
			player.magicCrit += 4;
			player.thrownCrit += 4;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GraniteProtector", 1);
			recipe.AddIngredient(null, "SpiritShield", 1);
			recipe.AddIngredient(null, "CrestofDasuver", 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}