using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
 
namespace SOTS.Items
{
    public class TheJuggernaut : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Juggernaut");
			Tooltip.SetDefault("This is a BIG flail");
		}
        public override void SetDefaults()
        {
            item.width = 54;
            item.height = 42;
            item.value = Item.sellPrice(0, 3, 50, 0);
            item.rare = 6;
            item.noMelee = true;
            item.useStyle = 5;
            item.useAnimation = 40;
            item.useTime = 40;
            item.knockBack = 8f;
            item.damage = 80;
            item.noUseGraphic = true; 
            item.shoot = mod.ProjectileType("JuggernautProj");
            item.shootSpeed = 14.5f;
            item.UseSound = SoundID.Item1;
            item.melee = true; 
            item.channel = true;
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.AdamantiteBar, 11);
			recipe.AddIngredient(ItemID.SoulofNight, 13);
			recipe.AddIngredient(ItemID.Chain, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
            
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TitaniumBar, 11);
            recipe.AddIngredient(ItemID.SoulofNight, 13);
            recipe.AddIngredient(ItemID.Chain, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
		}
    }
}