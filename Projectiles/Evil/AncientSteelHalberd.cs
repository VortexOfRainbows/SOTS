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
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = 19;
			Projectile.penetrate = -1;
			Projectile.scale = 1.2f;
			Projectile.alpha = 0;
			Projectile.hide = true;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			crit = false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
			target.AddBuff(ModContent.BuffType<Shattered>(), 1200);
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0f;
			float rotation = Projectile.velocity.ToRotation();
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + rotation.ToRotationVector2() * 15, Projectile.Center + rotation.ToRotationVector2() * -88, 24f * Projectile.scale, ref point))
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
				originalVelo = Projectile.velocity;
				runOnce = false;
			}
			int useTime = (int)Projectile.ai[0];
			Player player = Main.player[Projectile.owner];
			float degrees = 360 * (counter / useTime);
			counter++;

			Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
			Vector2 mousePosition = new Vector2(-40, 0).RotatedBy(originalVelo.ToRotation());
			mousePosition += new Vector2(32, 0).RotatedBy(MathHelper.ToRadians(degrees * Projectile.direction) + originalVelo.ToRotation());
			Vector2 toMouse = mousePosition;
			Projectile.velocity = new Vector2(-Projectile.velocity.Length(), 0).RotatedBy(toMouse.ToRotation());

			Projectile.direction = player.direction;
			player.heldProj = Projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;
			Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
			Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);
			if (!player.frozen)
			{
				float progress = counter / useTime;
				float sin = (float)Math.Sin(progress * MathHelper.Pi);
				movementFactor = MathHelper.Lerp(10, 24, sin);
			}
			Projectile.position += Projectile.velocity * movementFactor;
			if (counter >= useTime)
			{
				Projectile.Kill();
			}
			Projectile.rotation = (Projectile.Center - player.Center).ToRotation() + MathHelper.ToRadians(135f);
			Projectile.spriteDirection = -Projectile.direction;
			if (Projectile.spriteDirection == -1)
			{
				Projectile.rotation -= MathHelper.ToRadians(90f);
			}
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, DustID.Silver, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 100, Scale: 1.2f);
			dust.velocity += Projectile.velocity * 0.33f;
			dust.velocity *= 0.2f;
			dust.noGravity = true;
			dust.color = Color.LightGray;
		}
	}
}