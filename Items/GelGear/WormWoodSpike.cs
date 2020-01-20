using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
 
namespace SOTS.Items.GelGear
{
    public class WormWoodSpike : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worm Wood Spike");
			Tooltip.SetDefault("Enemies get stuck on it");
		}
        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 28;
            item.value = Item.sellPrice(0, 1, 80, 0);
            item.rare = 4;
            item.noMelee = true;
            item.useStyle = 5;
            item.useAnimation = 40;
            item.useTime = 40;
            item.knockBack = 4.5f;
            item.damage = 28;
            item.noUseGraphic = true; 
            item.shoot = mod.ProjectileType("WormWoodSpike");
            item.shootSpeed = 14.5f;
            item.UseSound = SoundID.Item1;
            item.melee = true; 
            item.channel = true;
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "WormWoodCore", 1);
			recipe.AddIngredient(null, "Wormwood", 16);
			recipe.AddIngredient(ItemID.PinkGel, 32);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}