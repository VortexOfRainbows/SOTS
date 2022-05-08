using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Projectiles.Celestial;

namespace SOTS.Items.Celestial
{
	public class CataclysmSpheres : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cataclysm Spheres");
			Tooltip.SetDefault("Throw a cluster of charged bombs that explodes into homing cataclysm lightning for 90% damage");
		}
		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.useStyle = ItemUseStyleID.SwingThrow;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.knockBack = 1.4f;
			Item.ranged = true;
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
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			int numberProjectiles = 5;
			for (int i = -2; i < numberProjectiles - 2; i++)
			{
				float mult = 1.0f - Math.Abs(i * 0.05f);
				Vector2 perturbedSpeed = new Vector2(speedX * mult, speedY * mult).RotatedBy(MathHelper.ToRadians(2.5f * i));
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<SanguiteBar>(), 10);
			recipe.AddIngredient(ModContent.ItemType<ArclightOrbs>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}