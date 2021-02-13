using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles
{
    public class RecollectHook : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Recollect Hook");
		}
        public override void SetDefaults()
        {
			projectile.width = 20;
			projectile.height = 20;
			projectile.penetrate = -1;
			projectile.timeLeft = 7200;
			projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.alpha = 255;
        } 
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }
        int hookId = -1;
        public override bool PreAI()
        {
            if (projectile.ai[0] == -1)
                projectile.Kill();
            NPC owner = Main.npc[(int)projectile.ai[0]];
            if (!owner.active || owner.type != ModContent.NPCType<PutridPinkyPhase2>())
            {
                projectile.ai[0] = -1;
                projectile.Kill();
            }
            if (hookId == -1)
            {
                NPC hook = Main.npc[(int)projectile.ai[1]];
                if (hook.type == mod.NPCType("HookTurret") && hook.active && (int)hook.localAI[0] == owner.whoAmI)
                {
                    Vector2 toHook = hook.Center - projectile.Center;
                    toHook = toHook.SafeNormalize(Vector2.Zero);
                    projectile.velocity = projectile.velocity.Length() * toHook;
                    if (projectile.Hitbox.Intersects(hook.Hitbox))
                    {
                        Main.PlaySound(SoundID.NPCHit1, hook.Center);
                        projectile.velocity *= 0.05f;
                        projectile.Center = hook.Center;
                        hookId = hook.whoAmI;
                    }
                }
                else
                {
                    projectile.Kill();
                }
            }
            else
            {
                NPC hook = Main.npc[hookId];
                if (hook.type == mod.NPCType("HookTurret") && hook.active && (int)hook.localAI[0] == owner.whoAmI)
                {
                    hook.Center = projectile.Center;
                }
                else
                {
                    projectile.Kill();
                }
                float rotationDist = owner.ai[3];
                Vector2 toOwner = owner.Center - projectile.Center;
                if (toOwner.Length() < rotationDist)
                {
                    if (hook.type == mod.NPCType("HookTurret") && hook.active && (int)hook.localAI[0] == owner.whoAmI)
                    {
                        float healthMult = (float)owner.life / (float)owner.lifeMax;
                        if (healthMult > 1) 
                            healthMult = 1;
                        float[] temp = hook.ai;
                        float temp2 = hook.localAI[1];
                        hook.type = ModContent.NPCType<NPCs.Boss.PutridHook>();
                        hook.SetDefaults(ModContent.NPCType<NPCs.Boss.PutridHook>());
                        hook.life = (int)(hook.life * healthMult);
                        if (hook.life < hook.lifeMax)
                            hook.life += 50;
                        if (hook.life > hook.lifeMax)
                            hook.life = hook.lifeMax;
                        hook.ai = temp;
                        hook.ai[2] = (int)MathHelper.ToDegrees((hook.Center - owner.Center).ToRotation());
                        hook.ai[3] = 0f;
                        hook.localAI[1] = 0; // temp2;
                        hook.localAI[0] = owner.whoAmI;
                        hook.netUpdate = true;
                        Main.PlaySound(SoundID.Splash, (int)hook.Center.X, (int)hook.Center.Y, 0, 1.2f);
                    }
                    projectile.Kill();
                }
                else
                {
                    toOwner = toOwner.SafeNormalize(Vector2.Zero);
                    projectile.velocity = projectile.velocity.Length() * toOwner;
                }
            }
            if (projectile.velocity.Length() < 17)
                projectile.velocity *= 1.1f;
            return true;
		}
    }
}