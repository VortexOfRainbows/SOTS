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
			item.damage = 40;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 26;
			item.useAnimation = 26;
			item.knockBack = 1.4f;
			item.ranged = true;
			item.value = Item.sellPrice(0, 15, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.width = 26;
			item.height = 30;
			item.maxStack = 1;
			item.autoReuse = true;            
			item.shoot = ModContent.ProjectileType<CataclysmOrb>(); 
            item.shootSpeed = 20f;
			item.consumable = false;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
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