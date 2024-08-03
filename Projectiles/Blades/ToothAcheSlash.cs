using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;

namespace SOTS.Projectiles.Blades
{    
    public class ToothAcheSlash : SOTSBlade
    {

        public static Color toothAcheLime = new Color(174, 213, 56);
        public static Color toothAcheGreen = new Color(110, 132, 22);
        public override Color color1 => toothAcheLime;
        public override Color color2 => toothAcheGreen;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.AddBuff(BuffID.Poisoned, 180);
        }
        public override void SafeSetDefaults()
        {
            Projectile.timeLeft = 7200;
            Projectile.localNPCHitCooldown = 4;
            Projectile.DamageType = ModContent.GetInstance<VoidMelee>();
            Projectile.friendly = true;
        }
        public override void SwingSound(Player player)
        {
            SOTSUtils.PlaySound(SoundID.Item71, (int)player.Center.X, (int)player.Center.Y, 0.75f, 0.75f * Projectile.ai[1]);
        }
        public override float AdditionalTipLength => base.AdditionalTipLength;
        public override float HitboxWidth => 44f;
        public override Vector2 drawOrigin => new Vector2(12, 60);
        public override float ArmAngleOffset => 12;
        public override float MaxSwipeDistance => 160;
        public override float MinSwipeDistance => 160;
        public override float MeleeSpeedMultiplier => 0.3f;
        public override float GetBaseSpeed(float swordLength)
        {
			if((int)Math.Abs(Projectile.ai[0]) == 2)
                spinSpeed *= 0.6f;
            return 2.2f + (1.2f / (float)Math.Pow(swordLength / MaxSwipeDistance, 2f));
        }
        public override float ArcStartDegrees => 200 + 15f / speedModifier;
        public override void SlashPattern(Player player, int slashNumber)
        {
            float speedBonus = 0f;
            if (slashNumber == 3)
            {
                speedBonus = 0.3f;
            }
            if (slashNumber == 2)
            {
                speedBonus = -0.3f;
            }
            if (slashNumber == 1)
            {
                speedBonus = 0.1f;
            }
            int damage = Projectile.damage;
            if (slashNumber == 1)
            {
                damage = (int)(damage * 1.0f);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 9f, ModContent.ProjectileType<ToothAcheThrow>(), (int)(damage * 0.8f), Projectile.knockBack, player.whoAmI, 0, FetchDirection);
            }
            else
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity, Type, damage, Projectile.knockBack, player.whoAmI, -FetchDirection * slashNumber, Projectile.ai[1] + speedBonus);
                if (proj.ModProjectile is ToothAcheSlash a)
                {
                    a.distance = distance;
                    if (slashNumber == 3)
                        a.distance = distance * 1.08f + 24;
                    else if (slashNumber == 2)
                        a.distance = distance * 0.9f;
                    else
                        a.distance = distance * 0.95f + 16;
                }
            }
        }
        public override Vector2 ModifySwingVector2(Vector2 original, float yDistanceCompression, int swingNumber)
        {
            original.Y *= 0.85f / speedModifier * yDistanceCompression; //turn circle into an oval by compressing the y value
            if (swingNumber == 3)
                original.Y *= 0.45f;
            if (swingNumber == 2)
                original.Y *= 0.75f;
            return original;
        }
        public override float swingSizeMult => 0.7f + 0.3f * Projectile.ai[1];
        public override float swipeDegreesTotal => 270.0f + (1800f / distance) + ((int)Math.Abs(Projectile.ai[0]) == 3 ? 10 : 0);
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
                        type = DustID.JungleSpore;
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
                        type = DustID.JungleSpore;
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
		
			