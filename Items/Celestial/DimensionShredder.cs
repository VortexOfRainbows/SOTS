using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Celestial;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Celestial
{
	public class DimensionShredder : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.ChainGun); //chaingun
			Item.damage = 37;
            Item.width = 94;   
            Item.height = 42;   
			Item.rare = ItemRarityID.Yellow;
			Item.useTime = 4;
			Item.useAnimation = 4;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.shootSpeed = 15.5f;
			Item.scale = 0.85f;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Celestial/DimensionShredderGlow").Value;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -14;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetY = 0;
			}
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
			return new Vector2(-14, 0f);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<SanguiteBar>(15).AddIngredient(ItemID.ChainGun, 1).AddTile(TileID.MythrilAnvil).Register();
		}
		int num = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			position += velocity.SafeNormalize(Vector2.Zero) * 20 + new Vector2(2.5f).RotatedBy(MathHelper.ToRadians(num * 60 % 360));
			if(player.altFunctionUse != 2)
			{
				num++;
				if (num % 30 == 0)
				{
					SOTSUtils.PlaySound(SoundID.Item78, position, 0.5f, -0.1f);
					Vector2 randomized = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-60, 60)));
					Projectile.NewProjectile(source, position, randomized * 0.5f, ModContent.ProjectileType<DimensionalFlame>(), damage, knockback, player.whoAmI, 0, (Main.rand.NextFloat(0.1f, 0.7f) * (Main.rand.Next(2) * 2 - 1)));
				}
				if (num % 2 == 0)
					for (int i = 0; i < Main.maxProjectiles; i++)
					{
						Projectile projectile = Main.projectile[i];
						if (projectile.active && projectile.owner == player.whoAmI && projectile.type == ModContent.ProjectileType<DimensionalFlame>())
						{
							Vector2 center = new Vector2(projectile.Center.X, projectile.Center.Y);
							Vector2 toCursor = Main.MouseWorld - center;
							Vector2 toVelo = new Vector2(velocity.Length(), 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-1, 1)) + toCursor.ToRotation());
							Projectile proj2 = Projectile.NewProjectileDirect(source, center, toVelo, type, damage, knockback, player.whoAmI);
							proj2.GetGlobalProjectile<SOTSProjectile>().affixID = -4; //this sould sync automatically on the SOTSProjectile end
						}
					}
				Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
				proj.GetGlobalProjectile<SOTSProjectile>().affixID = -4; //this sould sync automatically on the SOTSProjectile end
			}
			else
			{
				SOTSUtils.PlaySound(SoundID.Item78, position, 0.7f, -0.2f);
				for(int i = -1; i <= 1; i++)
				{
					Vector2 randomized = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-12, 12) + 36 * i));
					Projectile.NewProjectile(source, position, randomized * 0.5f, ModContent.ProjectileType<DimensionalFlame>(), damage, knockback, player.whoAmI, 0, (Main.rand.NextFloat(0.1f, 0.7f) * (Main.rand.Next(2) * 2 - 1)));
				}
			}
			return false; 
		}
        public override float UseSpeedMultiplier(Player player)
        {
			return player.altFunctionUse == 2 ? 0.1f : 1;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}
