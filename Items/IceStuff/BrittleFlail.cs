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
			Tooltip.SetDefault("Snaps upon colliding with an enemy\nUpon snapping, multiple projectiles are released");
		}
        public override void SetDefaults()
        {
            item.width = 46;
            item.height = 32;
            item.value = Item.sellPrice(0, 4, 0, 0);
            item.rare = 7;
            item.noMelee = true;
            item.useStyle = 5;
            item.useAnimation = 40;
            item.useTime = 40;
            item.knockBack = 7f;
            item.damage = 45;
            item.noUseGraphic = true; 
            item.shoot = mod.ProjectileType("BrittleFlailProj");
            item.shootSpeed = 18.5f;
            item.UseSound = SoundID.Item1;
            item.melee = true; 
            item.channel = true;
            item.autoReuse = true;
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AbsoluteBar", 7);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
            
        }
    }
}