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
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 7200;
			Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
        } 
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }
        int hookId = -1;
        bool playOnce = true;
        public override bool PreAI()
        {
            if(playOnce)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item99, Projectile.Center);
                playOnce = false;
            }
            if (Projectile.ai[0] == -1)
                Projectile.Kill();
            NPC owner = Main.npc[(int)Projectile.ai[0]];
            if (!owner.active || owner.type != ModContent.NPCType<PutridPinkyPhase2>())
            {
                Projectile.ai[0] = -1;
                Projectile.Kill();
            }
            if (hookId == -1)
            {
                NPC hook = Main.npc[(int)Projectile.ai[1]];
                if (hook.type == ModContent.NPCType<HookTurret>() && hook.active && (int)hook.localAI[0] == owner.whoAmI)
                {
                    Vector2 toHook = hook.Center - Projectile.Center;
                    toHook = toHook.SafeNormalize(Vector2.Zero);
                    Projectile.velocity = Projectile.velocity.Length() * toHook;
                    if (Projectile.Hitbox.Intersects(hook.Hitbox))
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCHit1, hook.Center);
                        Projectile.velocity *= 0.05f;
                        Projectile.Center = hook.Center;
                        hookId = hook.whoAmI;
                    }
                }
                else
                {
                    Projectile.Kill();
                }
            }
            else
            {
                NPC hook = Main.npc[hookId];
                if (hook.type == ModContent.NPCType<HookTurret>() && hook.active && (int)hook.localAI[0] == owner.whoAmI)
                {
                    hook.Center = Projectile.Center;
                }
                else
                {
                    Projectile.Kill();
                }
                float rotationDist = owner.ai[3];
                Vector2 toOwner = owner.Center - Projectile.Center;
                if (toOwner.Length() < rotationDist)
                {
                    if (hook.type == ModContent.NPCType<HookTurret>() && hook.active && (int)hook.localAI[0] == owner.whoAmI)
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
                        SOTSUtils.PlaySound(SoundID.NPCHit1, (int)hook.Center.X, (int)hook.Center.Y, 1.2f);
                    }
                    Projectile.Kill();
                }
                else
                {
                    toOwner = toOwner.SafeNormalize(Vector2.Zero);
                    Projectile.velocity = Projectile.velocity.Length() * toOwner;
                }
            }
            if (Projectile.velocity.Length() < 17)
                Projectile.velocity *= 1.1f;
            return true;
		}
    }
}