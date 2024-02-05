using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Planetarium
{    
    public class Seeker : ModProjectile 
    {
        public override void SetDefaults()
        {
			Projectile.height = 16;
			Projectile.width = 16;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 7200;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
		}
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			modifiers.DisableCrit();
        }
        public override bool PreDraw(ref Color lightColor)
        {
			return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (target.life <= 0 && Projectile.owner == Main.myPlayer)
            {
                SupernovaHammer.SpawnSeekers(Projectile.GetSource_FromThis(), target.Center, 2, (int)(0.75f * Projectile.damage) + 1, target.whoAmI);
            }
		}
		public override bool? CanHitNPC(NPC target)
        {
			return target.whoAmI == lastID ? (bool?)null : false;
		}
        int lastID = -1;
		float lastLength = 200f;
		float counter = 1f;
        public override void OnKill(int timeLeft)
        {
			for(int i = 0; i  < 10; i++)
			{
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 5, Projectile.Center.Y - 5), 0, 0, ModContent.DustType<Dusts.CopyDust4>());
                dust.velocity *= 1.5f;
                dust.noGravity = true;
                dust.scale *= 1.2f;
                dust.scale += 0.4f;
                dust.color = Color.Lerp(new Color(0, 200, 220, 100), new Color(220, 200, 30, 100), new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[0])).X + 0.5f);
                dust.fadeIn = 0.1f;
                dust.alpha = Projectile.alpha;
            }
        }
        public override void AI()
		{
			Projectile.ai[0]++;
			Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 5, Projectile.Center.Y - 5), 0, 0, ModContent.DustType<Dusts.CopyDust4>());
            dust.velocity *= 0.1f;
			dust.noGravity = true;
            dust.scale *= 0.7f;
            dust.scale += 0.7f;
			dust.color = Color.Lerp(new Color(0, 200, 220, 100), new Color(220, 200, 30, 100), new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[0])).X + 0.5f);
			dust.fadeIn = 0.1f;
			dust.alpha = Projectile.alpha;
			int npcId = (int)Projectile.ai[1];
			if(Projectile.ai[1] >= 0 && Main.npc[npcId].active == false)
            {
				Projectile.ai[1] = -1;
				return;
			}
			Projectile.velocity *= 0.97f;
			if (lastID == -1)
			{
				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy() && npc.whoAmI != npcId)
					{
						Vector2 toNPC = npc.Center - Projectile.Center;
						if (toNPC.Length() < lastLength)
						{
							lastLength = toNPC.Length();
							lastID = npc.whoAmI;
						}
					}
				}
				lastLength += 6;
				if(lastLength > 1200)
				{
					if (Projectile.alpha < 255)
					{
						Projectile.alpha++;
						Projectile.timeLeft -= 2;
					}
                }
			}
			else 
			{
				if(Projectile.alpha > 0)
				{
					Projectile.alpha--;
					Projectile.timeLeft += 2;
				}
				NPC npc = Main.npc[lastID];
				if (npc.CanBeChasedBy() && npc.whoAmI != npcId)
				{
					Vector2 toNPC = npc.Center - Projectile.Center;
					toNPC = toNPC.SafeNormalize(Vector2.Zero);
					Projectile.velocity += 0.1f * toNPC * counter;
					if(counter < 5)
						counter += 0.01f;
				}
				else
				{
					lastID = -1;
					lastLength = 200f;
				}
			}
			if(Projectile.alpha >= 200)
            {
				Projectile.Kill();
            }
		}	
	}
}
		