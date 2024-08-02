using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using SOTS.Utilities;
using SOTS.Void;
using SOTS.Prim.Trails;

namespace SOTS.Projectiles.Blades
{    
    public class SeleneSlash : SOTSBlade
	{
		public override Color color1 => ColorHelpers.VoidAnomaly;
		public override Color color2 => new Color(80, 60, 90);
		public override void SafeSetDefaults()
		{
			Projectile.localNPCHitCooldown = 120;
			Projectile.DamageType = DamageClass.Melee;
			delayDeathTime = 0;
			Projectile.extraUpdates++;
		}
		public override float HitboxWidth => 80;
		public override float AdditionalTipLength => -52;
		//public override float handleOffset => 20;
		public override float HeldDistFromPlayer => -34;
		public override Vector2 drawOrigin => new Vector2(48, 152);
		public override bool isDiagonalSprite => false;
		public override float OffsetAngleIfNotDiagonal => -2;
        public override void SwingSound(Player player)
		{
			SOTSUtils.PlaySound(SoundID.Item71, (int)player.Center.X, (int)player.Center.Y, 0.75f, 0.6f * speedModifier); //playsound function
		}
		public override float speedModifier => Projectile.ai[1];
		public override float GetBaseSpeed(float swordLength)
		{
			return 3f + (1.0f / (float)Math.Pow(swordLength / MaxSwipeDistance, 2f)) + (thisSlashNumber == 1 ? 2.4f : -0.6f);
		}
		public override float MeleeSpeedMultiplier => 0.75f; //melee speed only has 50% effectiveness on this weapon
		public override float OverAllSpeedMultiplier => 2f;
		public override float MinSwipeDistance => 360;
		public override float MaxSwipeDistance => 360;
		public override float ArcStartDegrees => 200;
		public override float swipeDegreesTotal => thisSlashNumber == 1 ? 230f : 330f;
		public override float swingSizeMult => 1.0f;
		public override float ArcOffsetFromPlayer => 0.25f;
		public override float delayDeathSlowdownAmount => 0.85f;
		public override Color? DrawColor => null;
        public override Vector2 ModifySwingVector2(Vector2 original, float yDistanceCompression, int swingNumber)
		{
			original.Y *= 0.8f * yDistanceCompression; //turn circle into an oval by compressing the y value
			return original;
		}
		public override void SlashPattern(Player player, int slashNumber)
		{
			int damage = Projectile.damage;
			if (slashNumber > 0)
			{
				Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity, Type, damage, Projectile.knockBack, player.whoAmI, FetchDirection * slashNumber, Projectile.ai[1] + 0.1f);
				if (proj.ModProjectile is SeleneSlash v)
				{
					if (slashNumber >= 2)
					{
						v.distance = distance * 0.99f;
					}
					else if (slashNumber == 1)
					{
						v.distance = distance * 0.96f;
						v.delayDeathTime = 20;
					}
				}
			}
		}
		int nextProj = 1;
        public override void AI()
        {
            base.AI(); //This is needed in order to do the Blade Stuff
			int slashCount = 10;
			int slashDelay = 30;
			if (timeLeftCounter > slashDelay * nextProj && nextProj <= slashCount)
			{
				nextProj++;
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - dustAway.SafeNormalize(Vector2.Zero) * 24, dustAway.SafeNormalize(Vector2.Zero) * (12 + (thisSlashNumber == 1 ? 24 : (thisSlashNumber == 2 ? 12 : 0))), ModContent.ProjectileType<SkipSlash>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner, 0, -2);
				}
			}
		}
        public override float ArmAngleOffset => 10; //hold it with a backwards grip because thats funny
        public override void SpawnDustDuringSwing(Player player, float bladeLength, Vector2 bladeDirection)
		{
			float amt = Main.rand.NextFloat(1.5f, 3.5f);
			float dustScale = 1f;
			float rand = Main.rand.NextFloat(0.6f, 0.7f);
			int type = ModContent.DustType<Dusts.CopyDust4>();
			if (Main.rand.NextBool(3))
				type = ModContent.DustType<Dusts.PixelDust>();
			for(int i = 0; i < 1 + amt / 2.0f; i++)
			{
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) - bladeDirection.SafeNormalize(Vector2.Zero) * 32, 16, 16, type);
				dust.velocity *= 0.55f;
				dust.velocity += bladeDirection.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1.2f, 2.0f) * rand;
				dust.noGravity = true;
				dust.scale *= 0.2f * rand;
				dust.scale += 1.25f * rand * dustScale;
				dust.fadeIn = 0.2f;
				dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
				dust.color.A = 0;
				if (type != ModContent.DustType<Dusts.CopyDust4>())
				{
					dust.fadeIn = 8f;
					dust.scale = 2f;
				}
			}
			Vector2 toProjectile = Projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
			for (int i = 0; i < amt; i++) //generates dust throughout the length of the blade
			{
				if(Main.rand.NextBool(5))
				{
					rand = Main.rand.NextFloat(1.0f, 2.0f);
					Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) - toProjectile.SafeNormalize(Vector2.Zero) * 24 - toProjectile * Main.rand.NextFloat(0.4f), 16, 16, type);
					dust.velocity *= 0.1f;
					dust.velocity += bladeDirection.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(90 * FetchDirection)) * Main.rand.NextFloat(0.3f, 0.4f) * rand;
					dust.noGravity = true;
					dust.scale *= 0.1f;
					dust.scale += rand;
					dust.fadeIn = 0.1f;
					dust.color = Color.Lerp(color2, color1, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
					dust.color.A = 0;
					if (type != ModContent.DustType<Dusts.CopyDust4>())
					{
						dust.fadeIn = 8f;
						dust.scale = 2f;
					}
				}
			}
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        { 
        }
        public override float TrailLengthMultiplier => GetArcLength() * 0.576f;
		public override float TrailOffsetFromTip => -GetArcLength() * 0.5f + 20;
    }
}
		
			