using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Utilities.Terraria.Utilities;

namespace SOTS.Projectiles.Blades
{    
    public class VorpalKnifeSlash : SOTSBlade
	{
		public override Color color1 => VorpalThrow.VorpalColor1;
		public override Color color2 => VorpalThrow.VorpalColor2;
		public override void SafeSetDefaults()
		{
			Projectile.localNPCHitCooldown = 15;
			Projectile.extraUpdates = 2;
		}
		public override float HitboxWidth => 24;
		public override float AdditionalTipLength => 0;
		public override float HeldDistFromPlayer => 18;
		public override Vector2 drawOrigin => new Vector2(10, 38);
		public override void SwingSound(Player player)
		{
			SOTSUtils.PlaySound(SoundID.Item71, (int)player.Center.X, (int)player.Center.Y, 0.75f, 0.6f * speedModifier); //playsound function
			if (thisSlashNumber == 1)
				SOTSUtils.PlaySound(SoundID.DD2_MonkStaffSwing, (int)player.Center.X, (int)player.Center.Y, 1.1f, -0.1f);
		}
		public override float speedModifier => Projectile.ai[1];
		public override float GetBaseSpeed(float swordLength)
		{
			return 3f + (1.0f / (float)Math.Pow(swordLength / MaxSwipeDistance, 2f)) + (thisSlashNumber == 1 ? 2.7f : 0);
		}
		public override float MeleeSpeedMultiplier => 0.2f; //melee speed only has 20% effectiveness on this weapon
		public override float OverAllSpeedMultiplier => 5f;
		public override float MinSwipeDistance => 128;
		public override float MaxSwipeDistance => 128;
		public override float ArcStartDegrees => thisSlashNumber == 1 ? 270 : 270 - 60f / speedModifier;
		public override float swipeDegreesTotal => (thisSlashNumber == 1 ? 830f : 262.5f) + (1800f / distance / speedModifier);
		public override float swingSizeMult => 1.0f;
		public override float ArcOffsetFromPlayer => thisSlashNumber == 1 ? 0 : 0.25f;
		public override Vector2 ModifySwingVector2(Vector2 original, float yDistanceCompression, int swingNumber)
		{
			original.Y *= (0.75f + swingNumber * 0.005f) / speedModifier * yDistanceCompression; //turn circle into an oval by compressing the y value
			return original;
		}
		public override void SlashPattern(Player player, int slashNumber)
		{
			int damage = Projectile.damage;
			float speedBonus = 0;
			if (slashNumber == 2)
				speedBonus = 0.1f;
			if (slashNumber == 1)
				speedBonus = -0.3f;
			if (slashNumber > 0)
			{
				Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity, Type, damage, Projectile.knockBack, player.whoAmI, -FetchDirection * slashNumber, Projectile.ai[1] + speedBonus);
				if (proj.ModProjectile is VorpalKnifeSlash v)
				{
					if (slashNumber == 2)
						v.distance = distance * 0.9f;
					if (slashNumber == 1)
						v.distance = distance * 1.2f;
				}
			}
		}
		public override void SpawnDustDuringSwing(Player player, float bladeLength, Vector2 bladeDirection)
        {
            if (Main.rand.NextBool(1 + Projectile.extraUpdates))
            {
                float dustScale = 1f;
                float rand = Main.rand.NextFloat(0.9f, 1.1f);
                int type = ModContent.DustType<Dusts.CopyDust4>();
                if (Main.rand.NextBool(5))
                    type = DustID.GreenTorch;
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) - bladeDirection.SafeNormalize(Vector2.Zero) * 8, 16, 16, type);
                dust.velocity *= 0.45f;
                dust.velocity += bladeDirection.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1.2f, 2.0f) * rand;
                dust.noGravity = true;
                dust.scale *= 0.2f / rand;
                dust.scale += 1.1f / rand * dustScale;
                dust.fadeIn = 0.1f;
                if (type == ModContent.DustType<Dusts.CopyDust4>())
                    dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
            }

			if (Main.rand.NextBool(1 + Projectile.extraUpdates))
            {
                float amt = Main.rand.NextFloat(1.0f, 1.5f);
                Vector2 toProjectile = Projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
				for (int i = 0; i < amt; i++) //generates dust throughout the length of the blade
				{
                    float rand = Main.rand.NextFloat(0.9f, 1.1f);
					int type = ModContent.DustType<Dusts.CopyDust4>();
					if (Main.rand.NextBool(3))
						type = DustID.GreenTorch;
					Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) - (toProjectile.SafeNormalize(Vector2.Zero)) * 8 - toProjectile * Main.rand.NextFloat(0.95f), 16, 16, type);
					dust.velocity *= 0.1f;
					dust.velocity += bladeDirection.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(90 * FetchDirection)) * Main.rand.NextFloat(0.3f, 0.4f) * rand;
					dust.noGravity = true;
					dust.scale *= 0.1f;
					dust.scale += rand;
					dust.fadeIn = 0.1f;
					if (type == ModContent.DustType<Dusts.CopyDust4>())
						dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
				}
			}
        }
        public override float TrailLengthMultiplier => base.TrailLengthMultiplier;
        public override float TrailOffsetFromTip => 0.95f;
        int localCounter = 0;
        public override void PostAI()
        {
			base.PostAI();
			if (thisSlashNumber == 1)
            {
				Projectile.localNPCHitCooldown = 9;
				distance *= 0.994f;
			}
			localCounter++;
			if(localCounter % 66 == 0)
				SOTSUtils.PlaySound(SoundID.DD2_MonkStaffSwing, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.0f, -0.2f);
		}
    }
}
		
			