//I must credit pyroknight for creating examplesolareruption. 


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
//using SOTS.Items.Trophies;

namespace SOTS.Items.DeathSpiral
{
	public class DeathSpiral : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Death Spiral");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 30;
			item.useTime = 30;
			item.shootSpeed = 11f;
			item.knockBack = 4f;
			item.UseSound = SoundID.Item116;
			item.shoot = ModContent.ProjectileType<DeathSpiralProj>();
			item.value = Item.sellPrice(silver: 5);
			item.noMelee = true;
			item.noUseGraphic = true;
			item.channel = true;
			item.autoReuse = true;
			item.melee = true;
			item.damage = 40;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// How far out the inaccuracy of the shot chain can be.
			float radius = 45f;
			// Sets ai[1] to the following value to determine the firing direction.
			// The smaller the value of NextFloat(), the more accurate the shot will be. The larger, the less accurate. This changes depending on your radius.
			// NextBool().ToDirectionInt() will have a 50% chance to make it negative instead of positive.
			// The Solar Eruption uses this calculation: Main.rand.NextFloat(0f, 0.5f) * Main.rand.NextBool().ToDirectionInt() * MathHelper.ToRadians(45f);
			float direction = Main.rand.NextFloat(0.25f, 1f) * Main.rand.NextBool().ToDirectionInt() * MathHelper.ToRadians(radius);
			for (int i = 0; i < 8; i++)
			{
				Projectile projectile = Projectile.NewProjectileDirect(player.RotatedRelativePoint(player.MountedCenter), new Vector2(speedX, speedY).RotatedBy(i * 0.785f), type, damage, knockBack, player.whoAmI, 0f, direction);
				// Extra logic for the chain to adjust to item stats, unlike the Solar Eruption.
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
		