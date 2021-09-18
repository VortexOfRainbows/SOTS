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
			DisplayName.SetDefault("Aten Starsplosion");
		}
        public override void SetDefaults()
        {
			projectile.height = 180;
			projectile.width = 180;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 3;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
			projectile.melee = true;
            projectile.localNPCHitCooldown = 10;
            projectile.usesLocalNPCImmunity = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 0;
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
            Vector2 atLoc = projectile.Center;
            Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 105, 0.8f, -0.15f);
            DustHelper.DrawStar(atLoc, 244, 4, 1, 2, 1);
            /*for (int i = 0; i < 360; i += 10)
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
                Dust dust = Dust.NewDustDirect(new Vector2(atLoc.X + circularLocation.X - 4, atLoc.Y + circularLocation.Y - 4), 4, 4, DustID.Fire);
                dust.noGravity = true;
                dust.velocity *= 0.5f;
                dust.velocity += -ogCL * (4f + 2f * projectile.ai[0]) * mult;
                dust.scale = 1.45f;
                if(Main.rand.NextBool(3))
                {
                    Color colorMan = Color.Lerp(new Color(255, 230, 140), new Color(180, 90, 20), Main.rand.NextFloat(1));
                    dust = Dust.NewDustDirect(new Vector2(atLoc.X + circularLocation.X - 4, atLoc.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
                    dust.color = colorMan;
                    dust.noGravity = true;
                    dust.velocity *= 0.5f;
                    dust.velocity += -ogCL * (3f + 1.5f * projectile.ai[0]) * mult;
                    dust.fadeIn = 0.1f;
                    dust.scale *= 2.45f;
                    dust.alpha = 100;
                }
            }
            for(int i = 0; i < 10; i++)
            {
                for(int k = 0; k < 4; k++)
                {
                    Vector2 circularLocation = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(k * 90));
                    Dust dust = Dust.NewDustDirect(new Vector2(atLoc.X + circularLocation.X - 4, atLoc.Y + circularLocation.Y - 4), 4, 4, DustID.Fire);
                    dust.noGravity = true;
                    dust.velocity *= 0.5f + i * 0.1f;
                    dust.velocity += circularLocation * (0.5f + i * 0.5f);
                    dust.scale *= 1.95f;
                    if (Main.rand.NextBool(3))
                    {
                        Color colorMan = Color.Lerp(new Color(255, 230, 140), new Color(180, 90, 20), Main.rand.NextFloat(1));
                        dust = Dust.NewDustDirect(new Vector2(atLoc.X + circularLocation.X - 4, atLoc.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
                        dust.color = colorMan;
                        dust.noGravity = true;
                        dust.velocity *= 0.3f + i * 0.09f;
                        dust.velocity += circularLocation * (0.5f + i * 0.5f);
                        dust.scale *= 1.6f;
                        dust.fadeIn = 0.1f;
                        dust.alpha = 100;
                    }
                }
            }*/
            runOnce = false;
        }
	}
}
		