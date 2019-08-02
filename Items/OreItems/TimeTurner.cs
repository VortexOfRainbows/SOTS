using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class TimeTurner : ModItem
	{ 
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Time Turner");
			Tooltip.SetDefault("Freezes enemies in their place\nDoesn't work on some enemies");
		}
		public override void SetDefaults()
		{
            item.damage = 7;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 54;     //gun image width
            item.height = 30;   //gun image  height
            item.useTime = 23;  //how fast 
            item.useAnimation = 23;
			item.rare = 7;
			item.shoot = mod.ProjectileType("TurnTime");
			item.shootSpeed = 23;
            item.value = 245000;
			item.expert = true;
			item.mana = 43;
            item.UseSound = SoundID.Item36;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
		
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"BrassBar", 7);
			recipe.AddIngredient(null,"CoreOfExpertise", 1);
			recipe.AddIngredient(ItemID.GoldWatch, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
