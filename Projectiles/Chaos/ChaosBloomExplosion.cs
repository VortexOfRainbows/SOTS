using Microsoft.Xna.Framework;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using SOTS.Void;

namespace SOTS.Projectiles.Chaos
{    
    public class ChaosBloomExplosion : ModProjectile 
    {
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Bloom");
		}
        public override void SetDefaults()
        {
			Projectile.height = 180;
			Projectile.width = 180;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 50;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
			Projectile.DamageType = DamageClass.Ranged;
            Projectile.idStaticNPCHitCooldown = 30;
            Projectile.usesIDStaticNPCImmunity = true;
        }
        public bool isAlternate => Projectile.ai[0] < 0 && Projectile.ai[1] < 0;
        public override bool PreDraw(ref Color lightColor)
        {
            if (counter < 50 && counter > 0)
            {
                float alphaMult = counter / 5f;
                if (alphaMult > 1)
                    alphaMult = 1;
                float otherMult = (counter - 10) / 40f;
                otherMult = (float)MathHelper.Clamp(otherMult, 0, 1);
                alphaMult -= otherMult;
                float scale = 0.25f + expandAmt / 20f;
                scale *= this.scale;
                float dist1 = 6 * scale;
                float dist2 = 8 * scale;
                Color c = ColorHelpers.ChaosPink;
                c.A = 0;
                if(!isAlternate)
                    SOTSProjectile.DrawStar(Projectile.Center, c, alphaMult * 1.5f, 0, MathHelper.ToRadians(Projectile.ai[0]), 8, dist1, dist2, 0.9f, 360);
                int density = 240;
                if (isAlternate)
                    density = 180;
                SOTSProjectile.DrawStar(Projectile.Center, c, alphaMult * 1.0f, 0, MathHelper.ToRadians(Projectile.ai[1]), 6, dist1 * 0.5f, dist2 * 0.5f, 0.9f, density);
            }
            return false;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            float scale = 1.5f;
            if (isAlternate)
                scale = 1;
            int width = (int)(120 * scale);
            hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
        }
        public override bool? CanHitNPC(NPC target)
        {
            return (counter < 20 && (!isAlternate || counter > 2)) ? null : false;
        }
        float expandVelocity = 4;
        float expandAmt = 0;
        int counter = 0;
        bool runOnce = true;
        float scale = 1f;
        public override bool PreAI()
        {
            if(isAlternate)
            {
                Projectile.usesIDStaticNPCImmunity = false;
                Projectile.idStaticNPCHitCooldown = 0;
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 30;
            }
            expandAmt += expandVelocity;
            expandVelocity *= 0.94f;
            counter++;
            return runOnce;
        }
        public override void AI()
        {
            if(isAlternate)
            {
                scale = Main.rand.NextFloat(0.5f, 1.0f);
            }
            else if (Projectile.ai[0] < 0)
                scale = 1.5f;
            else
                scale = Main.rand.NextFloat(1f, 1.4f);
       
            SOTSUtils.PlaySound(SoundID.Item62, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.7f, -0.2f);
            Color colorMan = ColorHelpers.ChaosPink;
            Vector2 atLoc = Projectile.Center;
            float density = 1.5f;
            if (isAlternate)
                density = 0.5f;
            DustHelper.DrawStar(atLoc, 242, 4, 4.5f, density, 1.85f, 0.75f, 0.75f, true, 10, 0); //pink torch
            for (int i = 0; i < 360; i += 20)
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
                float mult = circularLocation.Length() / 16f * scale;
                if (Main.rand.NextBool(6) || !!isAlternate)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(atLoc.X + circularLocation.X - 4, atLoc.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
                    dust.color = colorMan;
                    dust.noGravity = true;
                    dust.velocity *= 0.5f;
                    dust.velocity += -ogCL * (4.5f) * mult;
                    dust.fadeIn = 0.1f;
                    dust.scale *= 2.45f;
                    dust.alpha = 100;
                }
            }
            if(!isAlternate)
                for (int i = 0; i < 10; i++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        Vector2 circularLocation = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(k * 90));
                        if (Main.rand.NextBool(3))
                        {
                            Dust dust = Dust.NewDustDirect(new Vector2(atLoc.X + circularLocation.X - 4, atLoc.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
                            dust.color = colorMan;
                            dust.noGravity = true;
                            dust.velocity *= 0.3f + i * 0.09f;
                            dust.velocity += circularLocation * (0.5f + i * 0.5f) * scale;
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
		