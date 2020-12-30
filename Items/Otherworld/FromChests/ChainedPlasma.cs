using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
 
namespace SOTS.Items.Otherworld.FromChests
{
    public class ChainedPlasma : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chained Plasma");
			Tooltip.SetDefault("Chains between enemies for 70% damage\nTries to move towards your cursor");
		}
        public override void SetDefaults()
        {
            item.damage = 30;
            item.width = 36;
            item.height = 34;
            item.value = Item.sellPrice(0, 4, 50, 0);
            item.rare = ItemRarityID.LightPurple;
            item.noMelee = true;
            item.useStyle = 5;
            item.useAnimation = 30;
            item.useTime = 30;
            item.knockBack = 5f;
            item.noUseGraphic = true; 
            item.shoot = mod.ProjectileType("PlasmaBall");
            item.shootSpeed = 12.5f;
            item.UseSound = SoundID.Item1;
            item.melee = true; 
            item.channel = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DissolvingAether", 1);
            recipe.AddIngredient(null, "HardlightAlloy", 12);
            recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}