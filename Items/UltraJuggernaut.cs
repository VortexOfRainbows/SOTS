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
    public class UltraJuggernaut : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ultra Juggernaut");
			Tooltip.SetDefault("Launches 6 bouncing mini spikeballs after travelling a certain distance \nYou need any help holding this?");
		}
        public override void SetDefaults()
        {
            item.width = 66;
            item.height = 66;
            item.value = Item.sellPrice(0, 15, 50, 0);
            item.rare = 7;
            item.noMelee = true;
            item.useStyle = 5;
            item.useAnimation = 40;
            item.useTime = 40;
            item.knockBack = 8f;
            item.damage = 110;
            item.noUseGraphic = true; 
            item.shoot = mod.ProjectileType("UltraJuggernautProj");
            item.shootSpeed = 14.5f;
            item.UseSound = SoundID.Item1;
            item.melee = true; 
            item.channel = true;
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "TheJuggernaut", 1);
			recipe.AddIngredient(ItemID.SoulofFright, 8);
			recipe.AddIngredient(ItemID.SoulofMight, 8);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
            
        }
    }
}