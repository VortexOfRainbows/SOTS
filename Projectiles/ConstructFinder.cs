using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SOTS.Dusts;
using System.Linq;
using SOTS.NPCs.Boss.Excavator;
using Terraria.ID;

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
			for(int i = 0; i < 20; i++)
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
			Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>());
			dust.velocity *= 0.1f;
			dust.noGravity = true;
			dust.color = Color.Lerp(new Color(0, 192, 255, 100), new Color(0, 90, 136, 100), 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[0])));
			dust.fadeIn = 0.1f;
			dust.scale = 1.2f;
			dust.alpha = Projectile.alpha;
			bool lookingForExcavator = Projectile.ai[1] <= -1;
			int followThreshold = 20;
            if (lookingForExcavator)
            {
				followThreshold = 120;
				if (Projectile.ai[0] >= 100 && Projectile.ai[1] == -1)
				{
					SOTSUtils.PlaySound(SoundID.Roar, Projectile.Center, 1.0f, -0.34f);
					NPC.SpawnOnPlayer(Projectile.owner, ModContent.NPCType<Excavator>()); //should work in multiplayer
					Projectile.ai[1] = -2;
				}
            }
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
		