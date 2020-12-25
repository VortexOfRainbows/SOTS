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
			item.damage = 20;
			item.useStyle = 1;
			item.useTime = 26;
			item.useAnimation = 26;
			item.knockBack = 1.4f;
			item.ranged = true;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.width = 26;
			item.height = 30;
			item.maxStack = 1;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("ArclightBomb"); 
            item.shootSpeed = 17.75f;
			item.consumable = false;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
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