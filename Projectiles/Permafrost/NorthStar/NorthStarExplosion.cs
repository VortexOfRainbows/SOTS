using Microsoft.Xna.Framework;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Utilities;
using SOTS.Void;
using System;

namespace SOTS.Projectiles.Permafrost.NorthStar
{    
    public class NorthStarExplosion : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("North Starsplosion");
		}
        public override void SetDefaults()
        {
			Projectile.height = 180;
			Projectile.width = 180;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 3;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
			Projectile.melee = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 0;
            target.AddBuff(BuffID.Frostburn, 300);
        }
        bool runOnce = true;
        public override bool PreAI()
        {
            return runOnce;
        }
        public override void AI()
        {
            Vector2 atLoc = Projectile.Center;
            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 105, 0.8f, -0.15f);
            DrawStar(atLoc, ModContent.DustType<CopyDust4>(), 4, 6f, 1f, 1.0f, 0.5f, 0.5f, true, 15, 0);
            for (int i = 0; i < 360; i += 6)
            {
                Vector2 circularLocation = new Vector2(24, 0).RotatedBy(MathHelper.ToRadians(-i));
                Vector2 ogCL = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(-i));
                if (i < 90)
                {
                    circularLocation += new Vector2(-24, 24);
                }
                else if (i < 180)
                {
                    circularLocation += new Vector2(24, 24);
                }
                else if (i < 270)
                {
                    circularLocation += new Vector2(24, -24);
                }
                else
                {
                    circularLocation += new Vector2(-24, -24);
                }
                float mult = circularLocation.Length() / 16f;
                if(!Main.rand.NextBool(3))
                {
                    Color colorMan = Color.Lerp(new Color(240, 250, 255, 100), new Color(200, 250, 255, 100), Main.rand.NextFloat(1));
                    Dust dust = Dust.NewDustDirect(new Vector2(atLoc.X + circularLocation.X - 4, atLoc.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
                    dust.color = colorMan;
                    dust.noGravity = true;
                    dust.velocity *= 0.5f;
                    dust.velocity += -ogCL * (4f + 2f * Projectile.ai[0]) * mult;
                    dust.fadeIn = 0.1f;
                    dust.scale *= 1.5f;
                    dust.alpha = 180;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    Vector2 circularLocation = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(k * 90));
                    if (Main.rand.NextBool(4))
                    {
                        Color colorMan = Color.Lerp(new Color(240, 250, 255, 100), new Color(200, 250, 255, 100), Main.rand.NextFloat(1));
                        Dust dust = Dust.NewDustDirect(new Vector2(atLoc.X + circularLocation.X - 4, atLoc.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
                        dust.color = colorMan;
                        dust.noGravity = true;
                        dust.velocity *= 0.3f + i * 0.09f;
                        dust.velocity += circularLocation * (0.5f + i * 0.5f);
                        dust.scale *= 1.5f;
                        dust.fadeIn = 0.1f;
                        dust.alpha = 180;
                    }
                }
            }
            runOnce = false;
        }
        public void DrawStar(Vector2 position, int dustType, float pointAmount = 5, float mainSize = 1, float dustDensity = 1, float dustSize = 1f, float pointDepthMult = 1f, float pointDepthMultOffset = 0.5f, bool noGravity = false, float randomAmount = 0, float rotationAmount = -1)
        {
            float rot;
            if (rotationAmount < 0) { rot = Main.rand.NextFloat(0, (float)Math.PI * 2); } else { rot = rotationAmount; }

            float density = 1 / dustDensity * 0.1f;

            for (float k = 0; k < 6.28f; k += density)
            {
                float rand = 0;
                if (randomAmount > 0) { rand = Main.rand.NextFloat(-0.01f, 0.01f) * randomAmount; }

                float x = (float)Math.Cos(k + rand);
                float y = (float)Math.Sin(k + rand);
                float mult = ((Math.Abs(((k * (pointAmount / 2)) % (float)Math.PI) - (float)Math.PI / 2)) * pointDepthMult) + pointDepthMultOffset;//triangle wave function
                Dust dust = Dust.NewDustPerfect(position, dustType, new Vector2(x, y).RotatedBy(rot) * mult * mainSize, 0, default, dustSize);
                dust.color = new Color(240, 250, 255, 100);
                dust.noGravity = noGravity;
                dust.fadeIn = 0.1f;
                dust.scale *= 1.6f;
                dust.alpha = 180;
            }
        }
    }
}
		