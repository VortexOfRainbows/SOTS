using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SOTS.Dusts;
using System.Linq;
using SOTS.NPCs.Boss.Excavator;
using Terraria.ID;
using SOTS.Projectiles.AbandonedVillage;

namespace SOTS.Projectiles
{    
    public class ConstructFinder : ModProjectile 
    {
        public override void SetDefaults()
        {
			Projectile.height = 16;
			Projectile.width = 16;
			Projectile.timeLeft = 7200;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.extraUpdates = 3;
		}
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			modifiers.DisableCrit();
        }
        public override bool PreDraw(ref Color lightColor)
        {
			return false;
		}
		public override bool? CanHitNPC(NPC target)
        {
			return false;
		}
        public override void OnKill(int timeLeft)
        {
            bool lookingForExcavator = Projectile.ai[1] <= -1;
            if (lookingForExcavator)
            {
                SOTSUtils.PlaySound(SoundID.Roar, Projectile.Center, 1.0f, -0.34f);
                NPC.SpawnOnPlayer(Projectile.owner, ModContent.NPCType<Excavator>()); //should work in multiplayer
                float dustCount = 90;
                int type = ModContent.DustType<CopyDust4>();
                for (int i = 0; i < dustCount; ++i)
                {
                    float r = MathHelper.TwoPi * i / dustCount;
                    Vector2 circular = new Vector2(Main.rand.NextFloat(11.5f, 12.5f) + MathF.Sin(r * 12), 0).RotatedBy(r);
                    Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 0, 0, type);
                    dust.velocity = dust.velocity * 0.1f + circular;
                    dust.noGravity = true;
                    dust.color = ExcavatorOrb.Color;
                    dust.fadeIn = 0.1f;
                    dust.scale *= 1.7f;
                    dust.alpha = Projectile.alpha;
                    if(i % 2 == 0)
                    {
                        dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 0, 0, type);
                        dust.velocity = dust.velocity * 0.1f + circular * (1 - Main.rand.NextFloat(1) * Main.rand.NextFloat(1));
                        dust.noGravity = true;
                        dust.color = ExcavatorOrb.Color;
                        dust.fadeIn = 0.1f;
                        dust.scale *= 1.7f;
                        dust.alpha = Projectile.alpha;
                    }
                }
                return;
            }
            for (int i = 0; i < 20; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 0.8f;
				dust.noGravity = true;
				dust.color = Color.Lerp(new Color(0, 192, 255, 100), new Color(0, 90, 136, 100), 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(Main.rand.NextFloat(360))));
				dust.fadeIn = 0.1f;
				dust.scale *= 1.2f;
				dust.alpha = Projectile.alpha;
			}
        }
        public override void AI()
		{
			Projectile.ai[0]++;
			bool lookingForExcavator = Projectile.ai[1] <= -1;
			int followThreshold = 20;
            if (lookingForExcavator)
            {
                if(Projectile.extraUpdates < 5)
                {
                    Projectile.extraUpdates = 5;
                    SOTSUtils.PlaySound(SoundID.Item121, Projectile.Center, 1.0f, -0.15f);
                }
                followThreshold = 390;
                float percent = Projectile.ai[0] / followThreshold;
                Projectile.velocity.Y = -0.1f + percent * percent * -0.95f;
				if (Projectile.ai[0] >= followThreshold && Projectile.ai[1] == -1)
				{
                    Projectile.Kill();
                    return;
				}
                else
                {
                    Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>());
                    dust.velocity *= 0.1f + 0.2f * percent;
                    dust.noGravity = true;
                    dust.color = ExcavatorOrb.Color;
                    dust.fadeIn = 0.1f;
                    dust.scale = 0.1f + 1.4f * percent;
                    dust.alpha = Projectile.alpha;
                    float r = MathHelper.ToRadians(Projectile.ai[0] * 2.5f);
                    float radiusSize = 12 + Projectile.ai[0] * 0.25f;
                    for(int i = -1; i <= 1; i += 2)
                    {
                        Vector2 offset = new Vector2(i, 0).RotatedBy(r) * radiusSize;
                        offset.Y *= 0.5f;
                        dust = PixelDust.Spawn(Projectile.Center + offset, 0, 0, Main.rand.NextVector2Circular(0.1f, 0.1f), ExcavatorOrb.Color, 8);
                        dust.scale = 1.0f + percent * 0.5f;
                        if(Main.rand.NextBool(8))
                        {
                            dust = PixelDust.Spawn(Projectile.Center, 0, 0, offset * 0.17f + Projectile.velocity * 2f, ExcavatorOrb.Color, 11);
                            dust.scale = 0.9f + percent * 0.4f;
                        }
                    }
                }
            }
			else
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>());
                dust.velocity *= 0.1f;
                dust.noGravity = true;
                dust.color = Color.Lerp(new Color(0, 192, 255, 100), new Color(0, 90, 136, 100), 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[0])));
                dust.fadeIn = 0.1f;
                dust.scale = 1.2f;
                dust.alpha = Projectile.alpha;
                if (Projectile.ai[0] > followThreshold)
                {
                    int npcId = -1;
                    for (int i = 0; i < 200; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.CanBeChasedBy() &&
                            ((Common.GlobalNPCs.DebuffNPC.Constructs.Contains(npc.type) && !lookingForExcavator) || (lookingForExcavator && npc.type == ModContent.NPCType<Excavator>()))
                            && npc.Distance(Projectile.Center) > 64)
                        {
                            npcId = i;
                            break;
                        }
                    }
                    if (npcId == -1)
                    {
                        Projectile.Kill();
                    }
                    else
                    {
                        NPC npc = Main.npc[npcId];
                        Vector2 toNPC = npc.Center - Projectile.Center;
                        float speed = 3 + Projectile.ai[2];
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, toNPC.SafeNormalize(Vector2.Zero) * speed, 0.03f);
                        if (lookingForExcavator)
                            Projectile.ai[2] += 0.02f;
                    }
                }
                else
                    Projectile.velocity *= lookingForExcavator ? 0.975f : 0.95f;
            }
		}	
	}
}
		