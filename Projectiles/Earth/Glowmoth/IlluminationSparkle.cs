using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using SOTS.Dusts;
using Terraria.ID;
using SOTS.Utilities;
using System;
using SOTS.Buffs;

namespace SOTS.Projectiles.Earth.Glowmoth 
{    
    public class IlluminationSparkle : ModProjectile
    {
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<WebbedNPC>(), 20);
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int width = 70;
            hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
        }
        public override void SetDefaults()
        {
			Projectile.friendly = false;
			Projectile.DamageType = ModContent.GetInstance<Void.VoidGeneric>();
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.alpha = 0;
			Projectile.timeLeft = 30;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
            Projectile.hide = true;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            crit = false;
            damage = 1;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if(target.whoAmI == (int)Projectile.ai[0])
                return null;
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(100, 100, 100, 0) * (1 - Projectile.alpha / 255f) * 2.5f;
        }
        bool runOnce = true;
        public override bool PreAI()
        {
            if(runOnce)
            {
                Projectile.timeLeft = (int)Projectile.ai[1];
                runOnce = false;
            }
            int target = (int)Projectile.ai[0];
            if(target >= 0)
            {
                NPC npc = Main.npc[target];
                if(npc.active && !npc.friendly)
                {
                    Projectile.Center = npc.Center;
                }
                else
                {
                    Projectile.Kill();
                }
            }
            Projectile.alpha = (int)(150 - 150 * Projectile.timeLeft / Projectile.ai[1]);
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity *= 0.925f;
            if(Projectile.timeLeft < 3)
            {
                Projectile.friendly = true;
            }
            return true;
        }
        public void StarDust(Vector2 center)
        {
            Vector2 startingLocation;
            float degrees = Main.rand.NextFloat(360);
            for (int j = 0; j < 4; j++)
            {
                Vector2 offset = new Vector2(0, 4).RotatedBy(MathHelper.ToRadians(j * 90) + Projectile.rotation);
                for (int i = -3; i < 3; i++)
                {
                    degrees += 360f / 28f;
                    startingLocation = new Vector2(i, 8 - Math.Abs(i) * 2).RotatedBy(MathHelper.ToRadians(j * 90) + Projectile.rotation);
                    Vector2 velo = offset + startingLocation;
                    Dust dust = Dust.NewDustPerfect(center + velo * 0.4f, ModContent.DustType<CopyDust4>());
                    dust.noGravity = true;
                    dust.velocity *= 0.04f;
                    dust.scale = 1.4f;
                    dust.fadeIn = 0.1f;
                    dust.alpha = 100;
                    dust.color = ColorHelpers.VibrantColorAttempt(degrees);
                    dust.velocity += velo * 0.11f;
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            if(timeLeft <= 1)
            {
                SOTSUtils.PlaySound(SoundID.Item30, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.4f, -0.25f);
                StarDust(Projectile.Center + Main.rand.NextVector2Circular(48, 48));
            }
        }
    }
}
		
			