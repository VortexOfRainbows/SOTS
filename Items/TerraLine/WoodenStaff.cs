using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.TerraLine
{
	public class WoodenStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wood Staff");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            item.damage = 7;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 40;     //gun image width
            item.height = 40;   //gun image  height
            item.useTime = 24;  //how fast 
            item.useAnimation = 24;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 0;
            item.rare = 0;
            item.UseSound = SoundID.Item8;
            item.autoReuse = false;
            item.shoot = 206; 
            item.shootSpeed = 12;
			item.mana = 2;
			item.reuseDelay = 6;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 24);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
