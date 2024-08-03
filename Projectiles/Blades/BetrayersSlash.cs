using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Blades
{    
    public class BetrayersSlash : SOTSBlade
	{
		public override Color color1 => new Color(189, 6, 26);
		public override Color color2 => new Color(151, 176, 218);
		public override void SafeSetDefaults()
		{
			Projectile.localNPCHitCooldown = 20;
			Projectile.DamageType = DamageClass.Melee;
			delayDeathTime = 8;
		}
		public override float HitboxWidth => 24;
		public override float AdditionalTipLength => 4;
		public override float HeldDistFromPlayer => 16;
		public override Vector2 drawOrigin => new Vector2(7, 55);
		public override bool isDiagonalSprite => false;
        public override void SwingSound(Player player)
		{
			SOTSUtils.PlaySound(SoundID.Item71, (int)player.Center.X, (int)player.Center.Y, 0.75f, 0.6f * speedModifier); //playsound function
		}
		public override float speedModifier => Projectile.ai[1];
		public override float GetBaseSpeed(float swordLength)
		{
			return 3f + (1.0f / (float)Math.Pow(swordLength / MaxSwipeDistance, 2f)) + (thisSlashNumber == 1 ? 2.4f : -0.6f);
		}
		public override float MeleeSpeedMultiplier => 0.5f; //melee speed only has 50% effectiveness on this weapon
		public override float OverAllSpeedMultiplier => 4f;
		public override float MinSwipeDistance => 130;
		public override float MaxSwipeDistance => 130;
		public override float ArcStartDegrees => thisSlashNumber == 1 ? 215 : 240;
		public override float swipeDegreesTotal => (thisSlashNumber == 1 ? 205f : 202.5f) + (1800f / distance / speedModifier);
		public override float swingSizeMult => 1.0f;
		public override float ArcOffsetFromPlayer => thisSlashNumber == 1 ? 0.25f : 0.3f;
		public override float delayDeathSlowdownAmount => 0.5f;
		public override Color? DrawColor => null;
        public override Vector2 ModifySwingVector2(Vector2 original, float yDistanceCompression, int swingNumber)
		{
			if (original.Y * Projectile.ai[0] > 0)
				yDistanceCompression += 0.6f;
			original.Y *= (swingNumber == 1 ? 1.15f : 0.55f) / speedModifier * yDistanceCompression; //turn circle into an oval by compressing the y value
			return original;
		}
		public override void SlashPattern(Player player, int slashNumber)
		{
			int damage = Projectile.damage;
			if (slashNumber > 0)
			{
				Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity, Type, (int)(damage * 1.5f), Projectile.knockBack * 4.5f, player.whoAmI, -FetchDirection * slashNumber, Projectile.ai[1]);
				if (proj.ModProjectile is BetrayersSlash v)
				{
					if (slashNumber == 1)
					{
						v.distance = distance * 0.72f;
						v.delayDeathTime = 16;
					}
				}
			}
		}
		public override float ArmAngleOffset => -15; //hold it with a backwards grip because thats funny
        public override void SpawnDustDuringSwing(Player player, float bladeLength, Vector2 bladeDirection)
		{
			float amt = Main.rand.NextFloat(0.5f, 1.2f);
			float dustScale = 1f;
			float rand = Main.rand.NextFloat(0.6f, 0.7f);
			int type = ModContent.DustType<Dusts.CopyDust4>();
			if (Main.rand.NextBool(2))
				type = DustID.Blood;
			Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) + bladeDirection.SafeNormalize(Vector2.Zero) * 24, 16, 16, type);
			dust.velocity *= 0.45f;
			dust.velocity += bladeDirection.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1.2f, 2.0f) * rand;
			dust.noGravity = true;
			dust.scale *= 0.2f * rand;
			dust.scale += 1.1f * rand * dustScale;
			dust.fadeIn = 0.1f;
			if (type == ModContent.DustType<Dusts.CopyDust4>())
				dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
			else
			{
				dust.scale += 0.3f;
				dust.noGravity = false;
			}

			Vector2 toProjectile = Projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
			for (int i = 0; i < amt; i++) //generates dust throughout the length of the blade
			{
				rand = Main.rand.NextFloat(0.9f, 1.1f);
				type = ModContent.DustType<Dusts.CopyDust4>();
				if (Main.rand.NextBool(3))
					type = DustID.Blood;
				dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) + (toProjectile.SafeNormalize(Vector2.Zero)) * 24 - toProjectile * Main.rand.NextFloat(0.95f), 16, 16, type);
				dust.velocity *= 0.1f;
				dust.velocity += bladeDirection.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(90 * FetchDirection)) * Main.rand.NextFloat(0.3f, 0.4f) * rand;
				dust.noGravity = true;
				dust.scale *= 0.1f;
				dust.scale += rand;
				dust.fadeIn = 0.1f;
				if (type == ModContent.DustType<Dusts.CopyDust4>())
					dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
				else
				{
					dust.scale += 0.3f;
					dust.noGravity = false;
				}
			}
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			if(Main.myPlayer == Projectile.owner)
            {
				Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(90 * FetchDirection));
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, direction, ModContent.ProjectileType<BloodSplatter>(), 0, 0, Main.myPlayer);
            }
        }
        public override float TrailLengthMultiplier => base.TrailLengthMultiplier;
        public override float TrailOffsetFromTip => 0.875f;
    }
}
		
			