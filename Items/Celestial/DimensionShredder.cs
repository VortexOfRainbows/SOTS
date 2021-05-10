using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Celestial;

namespace SOTS.Items.Celestial
{
	public class DimensionShredder : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dimension Shredder");
			Tooltip.SetDefault("Summons dimensional wisps around you that fire towards your cursor\n66% chance to not consume ammo\n'Tear a rift through your enemies'");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(1929); //chaingun
			item.damage = 42;
            item.width = 48;   
            item.height = 32;   
			item.rare = 8;
			item.useTime = 4;
			item.useAnimation = 4;
            item.value = Item.sellPrice(0, 15, 0, 0);
            item.shootSpeed = 15.5f;
		}
		public override bool ConsumeAmmo(Player p)
		{
			if(Main.rand.Next(3) >= 1)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 0.5f);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SanguiteBar", 15);
			recipe.AddIngredient(ItemID.ChainGun, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		int num = 0;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			num++;
			if (num % 30 == 0)
			{
				Vector2 randomized = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-60, 60)));
				Projectile.NewProjectile(position, randomized * 0.25f, ModContent.ProjectileType<DimensionalFlame>(), damage, knockBack, player.whoAmI);
			}
			if(num % 2 == 0)
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile projectile = Main.projectile[i];
					if (projectile.active && projectile.owner == player.whoAmI && projectile.type == ModContent.ProjectileType<DimensionalFlame>())
					{
						Vector2 center = new Vector2(projectile.Center.X, projectile.Center.Y);
						Vector2 toCursor = Main.MouseWorld - center;
						Vector2 toVelo = new Vector2((float)Math.Sqrt(speedX * speedX + speedY * speedY), 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-1, 1)) + toCursor.ToRotation());
						Projectile.NewProjectile(center, toVelo, type, damage, knockBack, player.whoAmI);
					}
				}
			if (num % 3 == 0)
			{
				Projectile.NewProjectile(position.X + (speedY * 0.2f), position.Y - (speedX * 0.2f), speedX, speedY, type, damage, knockBack, player.whoAmI);
				return false;
			}
			if(num % 3 == 1)
			{
				Projectile.NewProjectile(position.X - (speedY * 0.2f), position.Y + (speedX * 0.2f), speedX, speedY, type, damage, knockBack, player.whoAmI);
				return false;
			}
			return true; 
		}
	}
}
