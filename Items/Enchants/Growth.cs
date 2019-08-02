using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class Growth : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Relic IV : Growth");
			Tooltip.SetDefault("X");
		}
		public override void SetDefaults()
		{
            item.damage = 4;  //gun damage
			item.magic = true;
            item.width = 66;     //gun image width
            item.height = 66;   //gun image  height
            item.useTime = 3;  //how fast 
            item.useAnimation = 15;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 1000000000;
            item.rare = 5;
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("XBolt"); 
            item.shootSpeed = 9;
			item.expert = true;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"voXels", 1);
			recipe.AddIngredient(null,"PrimordialMismatch", 1);
			recipe.AddIngredient(null,"ChlorophyteCrossFive", 1);
			recipe.AddIngredient(null,"Healherb", 1);
			recipe.AddIngredient(null,"AntimaterialMandible", 15);
			recipe.AddIngredient(null,"CoreOfStatus", 5);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
