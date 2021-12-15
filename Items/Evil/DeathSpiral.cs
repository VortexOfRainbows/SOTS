using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Evil;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Evil
{
	public class DeathSpiral : ModItem //I must credit pyroknight for creating examplesolareruption. 
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Death Spiral");
		}
		public override void SetDefaults()
		{
			item.damage = 40;
			item.width = 52;
			item.height = 54;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 30;
			item.useTime = 30;
			item.shootSpeed = 14f;
			item.knockBack = 4f;
			item.UseSound = SoundID.Item116;
			item.shoot = ModContent.ProjectileType<DeathSpiralProj>();
			item.value = Item.sellPrice(gold: 6);
			item.rare = ItemRarityID.LightPurple;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.channel = true;
			item.autoReuse = true;
			item.melee = true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float radius = 45f;
			float direction = Main.rand.NextFloat(0.25f, 1f) * Main.rand.NextBool().ToDirectionInt() * MathHelper.ToRadians(radius);
			for (int i = 0; i < 1; i++)
			{
				Projectile projectile = Projectile.NewProjectileDirect(player.RotatedRelativePoint(player.MountedCenter), new Vector2(speedX, speedY).RotatedBy(i * 0.785f), type, damage, knockBack, player.whoAmI, 0f, direction);
				if (projectile.modProjectile is DeathSpiralProj modItem)
				{
					modItem.firingSpeed = item.shootSpeed * 2f;
					modItem.firingAnimation = item.useAnimation;
					modItem.firingTime = item.useTime;
				}
			}
			return false;
		}
	}
}
		