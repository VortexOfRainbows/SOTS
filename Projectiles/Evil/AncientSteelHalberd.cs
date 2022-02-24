using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Dusts;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Evil
{
	public class AncientSteelHalberd : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Steel Halberd");
		}

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.aiStyle = 19;
			projectile.penetrate = -1;
			projectile.scale = 1.2f;
			projectile.alpha = 0;
			projectile.hide = true;
			projectile.ownerHitCheck = true;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 60;
		}
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			crit = false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
			target.AddBuff(ModContent.BuffType<Shattered>(), 1200);
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0f;
			float rotation = projectile.velocity.ToRotation();
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center + rotation.ToRotationVector2() * 15, projectile.Center + rotation.ToRotationVector2() * -88, 24f * projectile.scale, ref point))
				return true;
			return base.Colliding(projHitbox, targetHitbox);
		}
		public float movementFactor = 0;
		Vector2 originalVelo = Vector2.Zero;
		bool runOnce = true;
		float counter = 0;
		bool runOnce2 = true;
		public override void AI()
		{
			if(runOnce)
            {
				originalVelo = projectile.velocity;
				runOnce = false;
			}
			int useTime = (int)projectile.ai[0];
			Player player = Main.player[projectile.owner];
			float degrees = 360 * (counter / useTime);
			counter++;

			Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
			Vector2 mousePosition = new Vector2(-40, 0).RotatedBy(originalVelo.ToRotation());
			mousePosition += new Vector2(32, 0).RotatedBy(MathHelper.ToRadians(degrees * projectile.direction) + originalVelo.ToRotation());
			Vector2 toMouse = mousePosition;
			projectile.velocity = new Vector2(-projectile.velocity.Length(), 0).RotatedBy(toMouse.ToRotation());

			projectile.direction = player.direction;
			player.heldProj = projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;
			projectile.position.X = ownerMountedCenter.X - (float)(projectile.width / 2);
			projectile.position.Y = ownerMountedCenter.Y - (float)(projectile.height / 2);
			if (!player.frozen)
			{
				float progress = counter / useTime;
				float sin = (float)Math.Sin(progress * MathHelper.Pi);
				movementFactor = MathHelper.Lerp(10, 24, sin);
			}
			projectile.position += projectile.velocity * movementFactor;
			if (counter >= useTime)
			{
				projectile.Kill();
			}
			projectile.rotation = (projectile.Center - player.Center).ToRotation() + MathHelper.ToRadians(135f);
			projectile.spriteDirection = -projectile.direction;
			if (projectile.spriteDirection == -1)
			{
				projectile.rotation -= MathHelper.ToRadians(90f);
			}
			Dust dust = Dust.NewDustDirect(projectile.position, projectile.height, projectile.width, DustID.Silver, projectile.velocity.X * .2f, projectile.velocity.Y * .2f, 100, Scale: 1.2f);
			dust.velocity += projectile.velocity * 0.33f;
			dust.velocity *= 0.2f;
			dust.noGravity = true;
			dust.color = Color.LightGray;
		}
	}
}