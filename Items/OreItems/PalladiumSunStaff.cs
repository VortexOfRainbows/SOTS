using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class PalladiumSunStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Palladium Sun");
			Tooltip.SetDefault("Fires an bolt of destruction");
		}
		public override void SetDefaults()
		{
            item.damage = 34;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 50;     //gun image width
            item.height = 50;   //gun image  height
            item.useTime = 17;  //how fast 
            item.useAnimation = 17;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 100000;
            item.rare = 5;
            item.UseSound = SoundID.Item28;
            item.autoReuse = true;
            item.shoot = 424; 
            item.shootSpeed = 9;
			item.mana = 9;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PalladiumBar, 14);
			recipe.AddIngredient(null, "BrassBar", 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
