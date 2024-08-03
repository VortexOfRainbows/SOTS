using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;

namespace SOTS.Projectiles.Blades
{    
    public class VertebraekerSlash : SOTSBlade
    {
        public static Color vertebraekerRed = new Color(255, 185, 81);
        public static Color vertebraekerOrange = new Color(209, 117, 61);
        public override Color color1 => vertebraekerRed;
		public override Color color2 => vertebraekerOrange;
		public override void SafeSetDefaults()
        {
            Projectile.timeLeft = 7200;
            Projectile.localNPCHitCooldown = 4;
            Projectile.DamageType = ModContent.GetInstance<VoidMelee>();
            Projectile.friendly = true;
        }
        public override void SwingSound(Player player)
        {
            SOTSUtils.PlaySound(SoundID.Item71, (int)player.Center.X, (int)player.Center.Y, 0.75f, 0.6f * Projectile.ai[1]);
        }
        public override float HitboxWidth => 30;
        public override float AdditionalTipLength => base.AdditionalTipLength;
        public override Vector2 drawOrigin => new Vector2(12, 52);
        public override float ArmAngleOffset => 15;
        public override float MaxSwipeDistance => 112;
        public override float MinSwipeDistance => 112;
        public override float GetBaseSpeed(float swordLength)
        {
            return (2.5f + (1.0f / (float)Math.Pow(swordLength / MaxSwipeDistance, 2f)));
        }
		public override float MeleeSpeedMultiplier => 0.1f;
        public override float ArcStartDegrees => 270 - 60f / Projectile.ai[1];
		public override void SlashPattern(Player player, int slashNumber)
        {
            float speedBonus = 0.14f;
            if (slashNumber < 12)
                speedBonus = 0;
            if (slashNumber == 2)
                speedBonus = -1.0f;
            int damage = Projectile.damage;
            if (slashNumber == 1)
            {
                damage = (int)(damage * 1.0f);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 9f, ModContent.ProjectileType<VertebraekerThrow>(), (int)(damage * 1.2f), Projectile.knockBack, player.whoAmI, 0, 0);
            }
            else
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity, Type, damage, Projectile.knockBack, player.whoAmI, -FetchDirection * slashNumber, Projectile.ai[1] + speedBonus);
                if (proj.ModProjectile is VertebraekerSlash a)
                {
                    a.distance = distance * 0.91f + 10;
                }
            }
        }
        public override float swingSizeMult => 0.7f + 0.3f * Projectile.ai[1];
        public override float swipeDegreesTotal => 262.5f + (1800f / distance / Projectile.ai[1]);
        public override void SpawnDustDuringSwing(Player player, float bladeLength, Vector2 bladeDirection)
        {
            if (dustAway != Vector2.Zero)
            {
                float amt = Main.rand.NextFloat(1.0f, 1.4f) * distance / 180f;
                for (int i = 0; i < amt * 0.6f; i++) //generates dust at the end of the blade
                {
                    float dustScale = 1f;
                    float rand = Main.rand.NextFloat(0.9f, 1.1f);
                    int type = ModContent.DustType<Dusts.CopyDust4>();
                    if (Main.rand.NextBool(5))
                        type = DustID.RedTorch;
                    Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) + dustAway.SafeNormalize(Vector2.Zero) * 24, 16, 16, type);
                    dust.velocity *= 0.8f / rand;
                    dust.velocity += dustAway.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1.2f, 2.0f) * rand;
                    dust.noGravity = true;
                    dust.scale *= 0.2f / rand;
                    dust.scale += 1.3f / rand * dustScale;
                    dust.fadeIn = 0.1f;
                    if (type == ModContent.DustType<Dusts.CopyDust4>())
                        dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
                }
                Vector2 toProjectile = Projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
                for (int i = 0; i < amt; i++) //generates dust throughout the length of the blade
                {
                    float rand = Main.rand.NextFloat(0.9f, 1.1f);
                    int type = ModContent.DustType<Dusts.CopyDust4>();
                    if (Main.rand.NextBool(3))
                        type = DustID.RedTorch;
                    Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) + (toProjectile.SafeNormalize(Vector2.Zero)) * 24 - toProjectile * Main.rand.NextFloat(0.95f), 16, 16, type);
                    dust.velocity *= 0.1f / rand;
                    dust.velocity += dustAway.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(90 * FetchDirection)) * Main.rand.NextFloat(0.4f, 0.9f) * rand;
                    dust.noGravity = true;
                    dust.scale *= 0.2f / rand;
                    dust.scale += 1.1f * rand;
                    dust.fadeIn = 0.1f;
                    if (type == ModContent.DustType<Dusts.CopyDust4>())
                        dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f));
                }
            }
        }
        //public override float TrailOffsetFromTip => 0.9f;
    }
}
		
			