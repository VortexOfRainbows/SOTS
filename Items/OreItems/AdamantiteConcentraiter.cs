using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class AdamantiteConcentraiter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Adamantite Concentrator");
			Tooltip.SetDefault("Concentrate pure death onto your enemies");
		}
		public override void SetDefaults()
		{
            item.damage = 16;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 40;     //gun image width
            item.height = 40;   //gun image  height
            item.useTime = 6;  //how fast 
            item.useAnimation = 6;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 100000;
            item.rare = 6;
            item.UseSound = SoundID.Item28;
            item.autoReuse = false;
            item.shootSpeed = 12;
			item.mana = 7;
			item.channel = true;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
			item.shoot = mod.ProjectileType("RedLaser");
			item.noUseGraphic = false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.AdamantiteBar, 14);
			recipe.AddIngredient(null, "BrassBar", 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
