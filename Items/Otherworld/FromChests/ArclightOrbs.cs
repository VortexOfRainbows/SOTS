using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Otherworld.FromChests
{
	public class ArclightOrbs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arclight Orbs");
			Tooltip.SetDefault("Throw a cluster of thunder orbs that explode into chain lightning for 80% damage");
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
			Item.shoot = mod.ProjectileType("ArclightBomb"); 
            Item.shootSpeed = 17.75f;
			Item.consumable = false;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(6)); // This defines the projectiles random spread . 30 degree spread.
				perturbedSpeed *= 1f - (0.05f * i);
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				if(Main.rand.Next(15) < 5) //33%
					Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X * 0.75f, perturbedSpeed.Y * 0.75f, type, damage, knockBack, player.whoAmI);
			}
			return false; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SporeBombs", 1);
			recipe.AddIngredient(null, "HardlightAlloy", 12);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}