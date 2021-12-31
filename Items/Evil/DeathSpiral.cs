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
			Tooltip.SetDefault("Surrounds you with lightning tentacles\nHas a chance to apply a stacking, permanent bleed to hit enemies for 5 damage per second");
		}
		public override void SetDefaults()
		{
			item.damage = 50;
			item.width = 52;
			item.height = 54;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 36;
			item.useTime = 36;
			item.shootSpeed = 27f;
			item.knockBack = 3.5f;
			item.UseSound = SoundID.Item116;
			item.shoot = ModContent.ProjectileType<DeathSpiralProj>();
			item.value = Item.sellPrice(gold: 10);
			item.rare = ItemRarityID.LightPurple;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.channel = true;
			item.autoReuse = true;
			item.melee = true;
		}	
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for(int direction = -1; direction <= 1; direction += 2)
				for (int i = 0; i < 4; i++)
				{
					Vector2 velocity = new Vector2(speedX, speedY) * (0.9f - 0.125f * i);
					Vector2 velocity2 = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(40 * i + 35) * direction) * 1.25f;
					velocity = velocity + velocity2;
					velocity *= 0.5f;
					Projectile.NewProjectileDirect(player.RotatedRelativePoint(player.MountedCenter), velocity, type, damage, knockBack, player.whoAmI, -1f, MathHelper.ToRadians(25 + 15 * i) * -direction);
				}
			speedX *= 1.0f;
			speedY *= 1.0f;
			Projectile.NewProjectileDirect(player.RotatedRelativePoint(player.MountedCenter), new Vector2(speedX, speedY) * 1.1f, type, damage, knockBack, player.whoAmI, -0, 0);
			return false;
		}
	}
}
		