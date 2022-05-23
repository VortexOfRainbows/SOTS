using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Celestial;
using Terraria.DataStructures;

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
			Item.CloneDefaults(ItemID.ChainGun); //chaingun
			Item.damage = 42;
            Item.width = 48;   
            Item.height = 32;   
			Item.rare = ItemRarityID.Yellow;
			Item.useTime = 4;
			Item.useAnimation = 4;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.shootSpeed = 15.5f;
		}
        public override bool CanConsumeAmmo(Item ammo, Player player)
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
			CreateRecipe(1).AddIngredient<SanguiteBar>(15).AddIngredient(ItemID.ChainGun, 1).AddTile(TileID.MythrilAnvil).Register();
		}
		int num = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			num++;
			if (num % 30 == 0)
			{
				Vector2 randomized = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-60, 60)));
				Projectile.NewProjectile(source, position, randomized * 0.25f, ModContent.ProjectileType<DimensionalFlame>(), damage, knockback, player.whoAmI);
			}
			if(num % 2 == 0)
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile projectile = Main.projectile[i];
					if (projectile.active && projectile.owner == player.whoAmI && projectile.type == ModContent.ProjectileType<DimensionalFlame>())
					{
						Vector2 center = new Vector2(projectile.Center.X, projectile.Center.Y);
						Vector2 toCursor = Main.MouseWorld - center;
						Vector2 toVelo = new Vector2(velocity.Length(), 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-1, 1)) + toCursor.ToRotation());
						Projectile.NewProjectile(source, center, toVelo, type, damage, knockback, player.whoAmI);
					}
				}
			if (num % 3 == 0)
			{
				Projectile.NewProjectile(source, position.X + (velocity.Y * 0.2f), position.Y - (velocity.X * 0.2f), velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
				return false;
			}
			if(num % 3 == 1)
			{
				Projectile.NewProjectile(source, position.X - (velocity.Y * 0.2f), position.Y + (velocity.X * 0.2f), velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
				return false;
			}
			return true; 
		}
	}
}
