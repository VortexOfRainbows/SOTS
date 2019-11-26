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
	public class GraniteProtector : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Protector");
			Tooltip.SetDefault("Reduces damage taken by 6%");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 34;     
            item.height = 28;   
            item.value = Item.sellPrice(0, 0, 20, 0);
            item.rare = 2;
			item.defense = 1;
			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.endurance += 0.06f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(3087, 50); //smooth granite
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}