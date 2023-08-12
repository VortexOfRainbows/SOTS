using Microsoft.Xna.Framework;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Utilities;

namespace SOTS.Projectiles.Pyramid.Aten
{    
    public class AtenStarExplosion : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Aten Starsplosion");
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
			Projectile.DamageType = DamageClass.Melee;
            Projectile.localNPCHitCooldown = 10;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 0;
            if (Main.rand.NextBool(5))
                target.AddBuff(BuffID.OnFire, 300);
        }
        bool runOnce = true;
        public override bool PreAI()
        {
            return runOnce;
        }
        public override void AI()
        {
            Vector2 atLoc = Projectile.Center;
            SOTSUtils.PlaySound(SoundID.Item105, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.8f, -0.15f);
            DustHelper.DrawStar(atLoc, DustID.Torch, 4, 4.5f, 1.5f, 1.85f, 0.75f, 0.75f, true, 10, 0);
            for (int i = 0; i < 360; i += 10)
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
                if (Main.rand.NextBool(3))
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(atLoc.X + circularLocation.X - 4, atLoc.Y + circularLocation.Y - 4), 4, 4, DustID.Torch);
                    dust.noGravity = true;
                    dust.velocity *= 0.5f;
                    dust.velocity += -ogCL * (4f + 2f * Projectile.ai[0]) * mult;
                    dust.scale = 1.55f;
                }
                if(Main.rand.NextBool(3))
                {
                    Color colorMan = Color.Lerp(new Color(255, 230, 140), new Color(180, 90, 20), Main.rand.NextFloat(1));
                    Dust dust = Dust.NewDustDirect(new Vector2(atLoc.X + circularLocation.X - 4, atLoc.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
                    dust.color = colorMan;
                    dust.noGravity = true;
                    dust.velocity *= 0.5f;
                    dust.velocity += -ogCL * (3f + 1.5f * Projectile.ai[0]) * mult;
                    dust.fadeIn = 0.1f;
                    dust.scale *= 2.45f;
                    dust.alpha = 100;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    Vector2 circularLocation = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(k * 90));
                    if (Main.rand.NextBool(4))
                    {
                        Dust dust = Dust.NewDustDirect(new Vector2(atLoc.X + circularLocation.X - 4, atLoc.Y + circularLocation.Y - 4), 4, 4, DustID.Torch);
                        dust.noGravity = true;
                        dust.velocity *= 0.5f + i * 0.1f;
                        dust.velocity += circularLocation * (0.5f + i * 0.5f);
                        dust.scale *= 1.95f;
                    }
                    if (Main.rand.NextBool(4))
                    {
                        Color colorMan = Color.Lerp(new Color(255, 230, 140), new Color(180, 90, 20), Main.rand.NextFloat(1));
                        Dust dust = Dust.NewDustDirect(new Vector2(atLoc.X + circularLocation.X - 4, atLoc.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
                        dust.color = colorMan;
                        dust.noGravity = true;
                        dust.velocity *= 0.3f + i * 0.09f;
                        dust.velocity += circularLocation * (0.5f + i * 0.5f);
                        dust.scale *= 1.6f;
                        dust.fadeIn = 0.1f;
                        dust.alpha = 100;
                    }
                }
            }
            runOnce = false;
        }
	}
}
		