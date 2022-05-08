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
            Item.damage = 30;
            Item.width = 36;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 4, 50, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.noMelee = true;
            Item.useStyle = 5;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.knockBack = 5f;
            Item.noUseGraphic = true; 
            Item.shoot = mod.ProjectileType("PlasmaBall");
            Item.shootSpeed = 12.5f;
            Item.UseSound = SoundID.Item1;
            Item.melee = true; 
            Item.channel = true;
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