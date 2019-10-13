using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
 
namespace SOTS.Items.IceStuff
{
    public class BrittleFlail : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brittle Flail");
			Tooltip.SetDefault("");
		}
        public override void SetDefaults()
        {
            item.damage = 40;
            item.width = 42;
            item.height = 32;
            item.value = Item.sellPrice(0, 7, 25, 0);
            item.rare = 7;
            item.noMelee = true;
            item.useStyle = 5;
            item.useAnimation = 30;
            item.useTime = 30;
            item.knockBack = 7f;
            item.noUseGraphic = true; 
            item.shoot = mod.ProjectileType("BrittleFlailProj");
            item.shootSpeed = 13.5f;
            item.UseSound = SoundID.Item1;
            item.melee = true; 
            item.channel = true;
            item.autoReuse = true;
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AbsoluteBar", 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
            
        }
    }
}