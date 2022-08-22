using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Dusts;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Temple
{
	public class Helios : ModProjectile
	{
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Helios");
		}
		public override void SetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 50;
			Projectile.aiStyle = 19;
			Projectile.penetrate = -1;
			Projectile.scale = 1.5f;
			Projectile.alpha = 0;
			Projectile.hide = true;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.timeLeft = 300;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
			//target.AddBuff(ModContent.BuffType<Shattered>(), 1200);
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Player player = Main.player[Projectile.owner];
			float point = 0f;
			float rotation = Projectile.velocity.ToRotation();
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, player.Center + rotation.ToRotationVector2() * 16, 48f * Projectile.scale, ref point))
				return true;
			return base.Colliding(projHitbox, targetHitbox);
		}
		public float movementFactor = 0;
		Vector2 originalVelo = Vector2.Zero;
		bool runOnce = true;
		float counter = 0;
        public override bool? CanHitNPC(NPC target)
        {
            return counter > 0 ? base.CanHitNPC(target) : false;
        }
		int nextProj = 1;
        public override bool PreAI()
        {
			if(runOnce)
            {
				originalVelo = Projectile.velocity;
				runOnce = false;
			}
			int useTime = (int)Projectile.ai[0];
			Player player = Main.player[Projectile.owner];
			Projectile.direction = player.direction;
			float degrees = 360 * (counter / useTime);
			counter++;

			Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
			Vector2 mousePosition = new Vector2(-90, 0).RotatedBy(originalVelo.ToRotation());
			mousePosition += new Vector2(54, 0).RotatedBy(MathHelper.ToRadians(degrees * Projectile.direction) + originalVelo.ToRotation());
			Vector2 toMouse = mousePosition;
			Projectile.velocity = new Vector2(-Projectile.velocity.Length(), 0).RotatedBy(toMouse.ToRotation());

			Projectile.direction = player.direction;
			player.heldProj = Projectile.whoAmI;
			player.itemTime = 3;
			player.itemAnimation = 3;
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
			for(int i = 0; i < 3; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, ModContent.DustType<CopyDust4>(), Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f);
				dust.noGravity = true;
				dust.scale *= 1.6f - 0.1f * i;
				dust.fadeIn = 0.1f;
				dust.color = new Color(160, 80, 30, 0);
				if(i == 1)
					dust.color = new Color(240, 180, 30, 0);
				if(i == 2)
					dust.color = new Color(160, 140, 70, 0);
				dust.alpha = 30;
				dust.velocity += Projectile.velocity * (0.33f + 0.25f * i);
				dust.velocity *= 0.2f + 0.1f * i;
			}
			if(degrees > 45 * nextProj && nextProj <= 6)
            {
				nextProj++;
				if (Main.myPlayer == Projectile.owner)
				{
					Vector2 perturbedSpeed = Projectile.velocity;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<HeliosSlash>(), (int)(Projectile.damage * 0.4f), Projectile.knockBack, Projectile.owner, 0, -2);
				}
			}
			return false;
		}
	}
}