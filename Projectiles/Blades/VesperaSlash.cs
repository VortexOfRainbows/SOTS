using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;
using SOTS.Projectiles.Earth;

namespace SOTS.Projectiles.Blades
{    
    public class VesperaSlash : SOTSBlade
	{
		public override Color color1 => new Color(166, 221, 145);
		public override Color color2 => new Color(46, 63, 77);
		public override void SafeSetDefaults()
		{
			Projectile.localNPCHitCooldown = 40;
			Projectile.DamageType = ModContent.GetInstance<VoidMelee>();
			delayDeathTime = 12;
			Projectile.extraUpdates = 1;
		}
		public override float HitboxWidth => 18;
		public override float AdditionalTipLength => 0;
		public override float HeldDistFromPlayer => 10;
		public override Vector2 drawOrigin => new Vector2(6, 41);
        public override void SwingSound(Player player)
		{
			SOTSUtils.PlaySound(SoundID.Item71, (int)player.Center.X, (int)player.Center.Y, 0.75f, thisSlashNumber == 1 ? -0.5f : 0.7f); //playsound function
		}
		public override float speedModifier => Projectile.ai[1];
		public override float GetBaseSpeed(float swordLength)
		{
			return 3f + (1.0f / (float)Math.Pow(swordLength / MaxSwipeDistance, 2f)) + (thisSlashNumber == 1 ? 2.0f : -1f);
		}
		public override float MeleeSpeedMultiplier => 0.5f; //melee speed only has 50% effectiveness on this weapon
		public override float OverAllSpeedMultiplier => 5f;
		public override float MinSwipeDistance => 120;
		public override float MaxSwipeDistance => 120;
		public override float ArcStartDegrees => thisSlashNumber == 1 ? 215 : 200;
		public override float swipeDegreesTotal => (thisSlashNumber == 1 ? 215f : 242.5f) + (1800f / distance / speedModifier);
		public override float swingSizeMult => 1.0f;
		public override float ArcOffsetFromPlayer => 0.3f;
		public override float delayDeathSlowdownAmount => 0.5f;
		public override Color? DrawColor => null;
		private float nextIntervalForRocks = 80;
        public override void PostAI()
        {
            base.PostAI();
			if(timeLeftCounter > nextIntervalForRocks && thisSlashNumber == 1)
			{
				if (nextIntervalForRocks >= 130)
					nextIntervalForRocks += 100000;
				if(Main.myPlayer == Projectile.owner)
                {
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(0, -12), new Vector2(Main.player[Projectile.owner].direction * 1.35f, -Main.rand.NextFloat(4, 6)), ModContent.ProjectileType<EvostonePebble>(), (int)(Projectile.damage * 0.7f), Projectile.knockBack, Main.myPlayer);
                }
				nextIntervalForRocks += 25f;
			}
        }
        public override Vector2 ModifySwingVector2(Vector2 original, float yDistanceCompression, int swingNumber)
		{
			if (original.Y * Projectile.ai[0] > 0)
				yDistanceCompression += 0.1f;
			original.Y *= (swingNumber == 1 ? 0.5f : 0.775f) / speedModifier * yDistanceCompression; //turn circle into an oval by compressing the y value
			return original;
		}
		public override void SlashPattern(Player player, int slashNumber)
		{
			int damage = Projectile.damage;
			if (slashNumber > 0)
			{
				Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity, Type, damage, Projectile.knockBack * 2f, player.whoAmI, -FetchDirection * slashNumber, Projectile.ai[1]);
				if (proj.ModProjectile is VesperaSlash v)
				{
					if (slashNumber == 1)
					{
						v.distance = 148;
						v.delayDeathTime = 8;
					}
				}
			}
		}
		public override float ArmAngleOffset => -5;
        public override void SpawnDustDuringSwing(Player player, float bladeLength, Vector2 bladeDirection)
        {
			if (Main.rand.NextBool(1 + Projectile.extraUpdates))
			{
				float amt = Main.rand.NextFloat(0.5f, 1.2f);
				float dustScale = 1f;
				float rand = Main.rand.NextFloat(0.6f, 0.7f);
				int type = ModContent.DustType<Dusts.CopyDust4>();
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) - bladeDirection.SafeNormalize(Vector2.Zero) * 8, 16, 16, type);
				dust.velocity *= 0.45f;
				dust.velocity += bladeDirection.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1.2f, 2.0f) * rand;
				dust.noGravity = true;
				dust.scale *= 0.2f * rand;
				dust.scale += 1.1f * rand * dustScale;
				dust.fadeIn = 0.1f;
				dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));

				Vector2 toProjectile = Projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
				for (int i = 0; i < amt; i++) //generates dust throughout the length of the blade
				{
					rand = Main.rand.NextFloat(0.9f, 1.1f);
					type = ModContent.DustType<Dusts.CopyDust4>();
					if (Main.rand.NextBool(3))
						type = DustID.Obsidian;
					dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) - (toProjectile.SafeNormalize(Vector2.Zero)) * 8 - toProjectile * Main.rand.NextFloat(0.95f), 16, 16, type);
					dust.velocity *= 0.1f;
					dust.velocity += bladeDirection.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(90 * FetchDirection)) * Main.rand.NextFloat(0.3f, 0.4f) * rand;
					dust.noGravity = true;
					dust.scale *= 0.1f;
					dust.scale += rand;
					dust.fadeIn = 0.1f;
					dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
				}
			}
        }
        public override float TrailLengthMultiplier => base.TrailLengthMultiplier;
        public override float TrailOffsetFromTip => 0.88f;
    }
}
		
			