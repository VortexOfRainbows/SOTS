using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.TerraLine
{
	public class ConcentraitionStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystilliarium");
			Tooltip.SetDefault("Transforms mana into pure death");
		}
		public override void SetDefaults()
		{
            item.damage = 22;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 44;     //gun image width
            item.height = 44;   //gun image  height
            item.useTime = 1;  //how fast 
            item.useAnimation = 7;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 100000;
            item.rare = 5;
            item.UseSound = SoundID.Item8;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("BlueLaser"); 
            item.shootSpeed = 12;
			item.mana = 5;
			item.channel = true;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GoblinRockBar", 12);
			recipe.AddIngredient(null, "SteelBar", 12);
			recipe.AddIngredient(null, "BrassBar", 12);
			recipe.AddIngredient(null, "WoodenStaff", 1);
			recipe.AddIngredient(ItemID.AmethystStaff, 1);
			recipe.AddIngredient(ItemID.TopazStaff, 1);
			recipe.AddIngredient(ItemID.SapphireStaff, 1);
			recipe.AddIngredient(ItemID.EmeraldStaff, 1);
			recipe.AddIngredient(ItemID.RubyStaff, 1);
			recipe.AddIngredient(ItemID.DiamondStaff, 1);
			recipe.AddIngredient(ItemID.AmberStaff, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
