using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;
using SOTS.Void;

namespace SOTS.Projectiles.Celestial
{    
    public class VoidspaceFlameHitbox : ModProjectile 
    {
        public override string Texture => "SOTS/Projectiles/Celestial/SmallStellarHitbox";
        public override void SetDefaults()
        {
			Projectile.height = 80;
			Projectile.width = 80;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 5;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
		}
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SetCrit();
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return true;
        }
        private bool TargetableNPC(NPC target)
        {
            float dist = Projectile.Center.Distance(target.Center);
            if (dist < 2000)
            {
                if (target.active && !target.friendly && target.lifeMax > 5 && !target.dontTakeDamage)
                {
                    return true;
                }
            }
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return TargetableNPC(target);
        }
		bool runOnce = true;
        public override void AI()
		{
			if(runOnce)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if(TargetableNPC(npc))
                    {
                        for (float j = 0; j < 30; j++)
                        {
                            Vector2 pos = npc.Center - new Vector2(4, 4);
                            Dust dust = Dust.NewDustDirect(pos, 0, 0, ModContent.DustType<CopyDust4>());
                            dust.noGravity = true;
                            dust.fadeIn = 0.1f;
                            dust.scale *= 2.25f;
                            dust.color = new Color(70, 255, 60);
                            dust.velocity *= 2.5f;
                        }
                        DrawDustBetweenPoints(npc.Center, Projectile.Center);
                    }
                }
                SOTSUtils.PlaySound(SoundID.Item74, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1f, 0.2f);
                runOnce = false;
			}
		}
        public void DrawDustBetweenPoints(Vector2 point1, Vector2 point2)
        {
            float step = 10;
            float dist = point1.Distance(point2);
            for (float j = 0; j <= 1; j += step / dist)
            {
                Vector2 pos = Vector2.Lerp(point1, point2, j) - new Vector2(4, 4);
                Dust dust = Dust.NewDustDirect(pos, 0, 0, ModContent.DustType<CopyDust4>());
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.scale *= 1.75f;
                dust.color = new Color(70, 255, 60);
                dust.velocity *= 0.25f;
                dust.velocity += (point1 - point2).SafeNormalize(Vector2.Zero);
            }
        }
    }
}
		