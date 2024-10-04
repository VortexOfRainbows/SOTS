using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Projectiles.Celestial;
using Terraria.DataStructures;

namespace SOTS.Items.Celestial
{
	public class CataclysmSpheres : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.knockBack = 1.4f;
			Item.DamageType = DamageClass.Ranged;
			Item.value = Item.sellPrice(0, 15, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.width = 26;
			Item.height = 30;
			Item.maxStack = 1;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<CataclysmOrb>(); 
            Item.shootSpeed = 20f;
			Item.consumable = false;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int numberProjectiles = 5;
			for (int i = -2; i < numberProjectiles - 2; i++)
			{
				float mult = 1.0f - Math.Abs(i * 0.05f);
				Vector2 perturbedSpeed = new Vector2(velocity.X * mult, velocity.Y * mult).RotatedBy(MathHelper.ToRadians(2.5f * i));
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			}
			return false; 
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<SanguiteBar>(10).AddIngredient<ArclightOrbs>(1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}