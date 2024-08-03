using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;

namespace SOTS.Projectiles.Blades
{    
    public class TesseractSlash : SOTSBlade
	{
		public override Color color1 => new Color(231, 95, 203);
		public override Color color2 => Color.Black;
		public override void SafeSetDefaults()
		{
			Projectile.localNPCHitCooldown = 30;
			Projectile.DamageType = ModContent.GetInstance<VoidMelee>();
			delayDeathTime = 0;
			Projectile.extraUpdates = 4;
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {

        }
        public override float HitboxWidth => 80;
		public override float AdditionalTipLength => 0;
		//public override float handleOffset => 24;
		public override float HeldDistFromPlayer => 24;
		public override Vector2 drawOrigin => new Vector2(9, 102);
        public override void SwingSound(Player player)
		{
			SOTSUtils.PlaySound(SoundID.Item71, (int)player.Center.X, (int)player.Center.Y, 0.75f, -0.3f); //playsound function
		}
		public override float speedModifier => Projectile.ai[1];
		public override float GetBaseSpeed(float swordLength)
		{
			return 10f;
		}
		public override float MeleeSpeedMultiplier => 0.5f; //melee speed only has 80% effectiveness on this weapon
		public override float OverAllSpeedMultiplier => 4f;
		public override float MinSwipeDistance => 320;
		public override float MaxSwipeDistance => 320;
		public override float ArcStartDegrees => 150;
		public override float swipeDegreesTotal => 300f;
		public override float swingSizeMult => 1.0f;
		public override float ArcOffsetFromPlayer => 0.3f;
		public override float delayDeathSlowdownAmount => 0.7f;
		public override Color? DrawColor => null;
		//private float nextIntervalForRocks = 80;
		private bool RunOnce = true;
        public override void PostAI()
        {
			base.PostAI();
			/*if(timeLeftCounter > nextIntervalForRocks && thisSlashNumber == 1)
			{
				if (nextIntervalForRocks >= 130)
					nextIntervalForRocks += 100000;
				if(Main.myPlayer == Projectile.owner)
                {
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(0, -12), new Vector2(Main.player[Projectile.owner].direction * 1.35f, -Main.rand.NextFloat(4, 6)), ModContent.ProjectileType<EvostonePebble>(), (int)(Projectile.damage * 0.7f), Projectile.knockBack, Main.myPlayer);
                }
				nextIntervalForRocks += 25;
			}*/
        }
        public override Vector2 ModifySwingVector2(Vector2 original, float yDistanceCompression, int swingNumber)
		{
			original.Y *= 1 / speedModifier * yDistanceCompression; //turn circle into an oval by compressing the y value
			return original;
		}
		public override void SlashPattern(Player player, int slashNumber)
		{
			int damage = Projectile.damage;
			if (slashNumber > 0)
			{
				float knockBackMult = 1;
				if (slashNumber == 1)
                {
					damage = (int)(damage * 1.2f);
					knockBackMult = 2.4f;
				}
				Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity, Type, damage, Projectile.knockBack * knockBackMult, player.whoAmI, -FetchDirection * slashNumber, Projectile.ai[1]);
				if (proj.ModProjectile is ColossusSlash v)
				{
					if (slashNumber == 1)
					{
						v.distance = 180;
						v.delayDeathTime = 12;
					}
					if (slashNumber == 1)
					{
						v.distance = 230;
						v.delayDeathTime = 20;
					}
				}
			}
		}
		public override float ArmAngleOffset => 5;
        public override void SpawnDustDuringSwing(Player player, float bladeLength, Vector2 bladeDirection)
		{
			float amt = Main.rand.NextFloat(1.3f, 1.4f);
			float dustScale = 1.1f;
			float rand = Main.rand.NextFloat(1f, 1.3f);
			int type = ModContent.DustType<Dusts.AlphaDrainDust>();
			Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) - bladeDirection.SafeNormalize(Vector2.Zero) * 8, 16, 16, type);
			dust.velocity *= 0.65f;
			dust.velocity += bladeDirection.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1.5f, 2.4f) * rand;
			dust.noGravity = true;
			dust.scale *= 0.2f * rand;
			dust.scale += 1.1f * rand * dustScale;
			dust.fadeIn = 0.1f;
			dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));

			Vector2 toProjectile = Projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
			for (int i = 0; i < amt; i++) //generates dust throughout the length of the blade
			{
				rand = Main.rand.NextFloat(1.0f, 1.2f);
				type = ModContent.DustType<Dusts.AlphaDrainDust>();
				dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) - (toProjectile.SafeNormalize(Vector2.Zero)) * 8 - toProjectile * Main.rand.NextFloat(0.95f), 16, 16, type);
				dust.velocity *= 0.2f;
				dust.velocity += bladeDirection.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(90 * FetchDirection)) * Main.rand.NextFloat(0.3f, 0.5f) * rand;
				dust.noGravity = true;
				dust.scale *= 0.1f;
				dust.scale += rand;
				dust.fadeIn = 0.1f;
				dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
			}
		}
        public override float TrailLengthMultiplier => 0.75f;
		public override float TrailOffsetFromTip => base.TrailOffsetFromTip;
    }
}
		
			