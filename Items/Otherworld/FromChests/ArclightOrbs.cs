using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Items.Otherworld.Furniture;
using SOTS.Items.Nature;
using SOTS.Projectiles.Otherworld;

namespace SOTS.Items.Otherworld.FromChests
{
	public class ArclightOrbs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arclight Orbs");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.knockBack = 1.4f;
			Item.DamageType = DamageClass.Ranged;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.width = 26;
			Item.height = 30;
			Item.maxStack = 1;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<ArclightBomb>(); 
            Item.shootSpeed = 17.75f;
			Item.consumable = false;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(6)); // This defines the projectiles random spread . 30 degree spread.
				perturbedSpeed *= 1f - (0.05f * i);
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
				if(Main.rand.Next(15) < 5) //33%
					Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X * 0.75f, perturbedSpeed.Y * 0.75f, type, damage, knockback, player.whoAmI);
			}
			return false; 
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<SporeBombs>(1).AddIngredient<HardlightAlloy>(12).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}
}