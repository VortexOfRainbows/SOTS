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

namespace SOTS.Projectiles.Otherworld
{    
    public class Seeker : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Critical Seeker");
		}
        public override void SetDefaults()
        {
			projectile.height = 16;
			projectile.width = 16;
			projectile.melee = true;
			projectile.timeLeft = 7200;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = false;
		}
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			crit = false;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (target.life <= 0)
			{
				Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 14, 0.6f);
				if (projectile.owner == Main.myPlayer)
				{
					for (int i = 0; i < 2; i++)
					{
						Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
						Projectile.NewProjectile(target.Center.X, target.Center.Y, circular.X, circular.Y, mod.ProjectileType("Seeker"), (int)(0.75f * projectile.damage) + 1, projectile.knockBack, Main.myPlayer, Main.rand.Next(360), target.whoAmI);
					}
				}
			}
			base.OnHitNPC(target, damage, knockback, crit);
		}
		public override bool? CanHitNPC(NPC target)
        {
			return target.whoAmI == lastID ? (bool?)null : false;
		}
        int lastID = -1;
		float lastLength = 200f;
		float counter = 1f;
		public override void AI()
		{
			projectile.ai[0]++;
			int num1 = Dust.NewDust(new Vector2(projectile.position.X + 4, projectile.position.Y + 4), 8, 8, mod.DustType("CopyDust4"));
			Dust dust = Main.dust[num1];
			dust.velocity *= 0.2f;
			dust.noGravity = true;
			dust.scale += 0.1f;
			dust.color = Color.Lerp(new Color(0, 200, 220, 100), new Color(220, 200, 30, 100), new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(projectile.ai[0])).X + 0.5f);
			dust.fadeIn = 0.1f;
			dust.scale *= 1.6f;
			dust.alpha = projectile.alpha;
			int npcId = (int)projectile.ai[1];
			if(projectile.ai[1] >= 0 && Main.npc[npcId].active == false)
            {
				projectile.ai[1] = -1;
				return;
			}
			projectile.velocity *= 0.97f;
			if (lastID == -1)
			{
				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy() && npc.whoAmI != npcId)
					{
						Vector2 toNPC = npc.Center - projectile.Center;
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
					if (projectile.alpha < 255)
					{
						projectile.alpha++;
						projectile.timeLeft -= 2;
					}
                }
			}
			else 
			{
				if(projectile.alpha > 0)
				{
					projectile.alpha--;
					projectile.timeLeft += 2;
				}
				NPC npc = Main.npc[lastID];
				if (npc.CanBeChasedBy() && npc.whoAmI != npcId)
				{
					Vector2 toNPC = npc.Center - projectile.Center;
					toNPC = toNPC.SafeNormalize(Vector2.Zero);
					projectile.velocity += 0.1f * toNPC * counter;
					if(counter < 5)
						counter += 0.01f;
				}
				else
				{
					lastID = -1;
					lastLength = 200f;
				}
			}
			if(projectile.alpha >= 255)
            {
				projectile.Kill();
            }
		}	
	}
}
		