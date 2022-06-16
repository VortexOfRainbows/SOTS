using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Otherworld;
using SOTS.Items.Otherworld.Furniture;
using SOTS.Items.Fragments;

namespace SOTS.Items.Otherworld.FromChests
{
    public class ChainedPlasma : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chained Plasma");
			Tooltip.SetDefault("Chains between enemies for 70% damage\nTries to move towards your cursor");
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.width = 36;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 4, 50, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.knockBack = 5f;
            Item.noUseGraphic = true; 
            Item.shoot = ModContent .ProjectileType<PlasmaBall>();
            Item.shootSpeed = 12.5f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee; 
            Item.channel = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<DissolvingAether>(1).AddIngredient<HardlightAlloy>(12).AddTile<HardlightFabricatorTile>().Register();
        }
    }
}