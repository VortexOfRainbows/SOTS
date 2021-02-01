using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;

namespace SOTS.Items.Pyramid
{
	public class JeweledGauntlet : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jeweled Gauntlet");
			Tooltip.SetDefault("");
		}
		public override void SafeSetDefaults()
		{
			item.melee = true;
			item.width = 34;
			item.height = 32;
			item.damage = 54;
            item.useTime = 12;
            item.useAnimation = 12;
			item.useStyle = 5;
			item.knockBack = 8.5f;
            item.value = Item.sellPrice(0, 4, 50, 0);
            item.rare = 6;
            item.UseSound = SoundID.Item19;
            item.autoReuse = true;       
			item.shoot = mod.ProjectileType("PhantomFist"); 
            item.shootSpeed = 9f;
			item.consumable = false;
			item.noMelee = true;
			item.noUseGraphic = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 1;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(12)); // This defines the projectiles random spread . 30 degree spread.
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
        public override void GetVoid(Player player)
        {
			voidMana = 3;
        }
        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SandstoneWarhammer", 1);
			recipe.AddIngredient(null, "SpiritGlove", 1);
			recipe.AddIngredient(null, "CursedMatter", 4);
			recipe.AddIngredient(ItemID.SoulofNight, 15);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}