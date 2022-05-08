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
    public class MacaroniMoon : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Macaroni Moon");
		}
        public override void SetDefaults()
        {
			projectile.height = 16;
			projectile.width = 20;
			projectile.magic = true;
			projectile.timeLeft = 7200;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.tileCollide = false;
		}
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			crit = false;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 6; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.25f;
				float y = Main.rand.Next(-10, 11) * 0.25f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * ((255 - projectile.alpha) / 255f), projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			return false;
		}
		bool foundTarget = false;
		int lastNpc = -1;
		public Vector2 FindTarget()
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			float distanceFromTarget = 1080f;
			Vector2 targetCenter = projectile.Center;
			Vector2 cursor = Main.MouseWorld;
			if (!foundTarget)
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy() && npc.active)
					{
						float between = Vector2.Distance(npc.Center, projectile.Center);
						bool closest = Vector2.Distance(projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);

						if (((closest || !foundTarget) && inRange) && lineOfSight)
						{
							distanceFromTarget = between;
							targetCenter = npc.Center + npc.velocity * 2;
							foundTarget = true;
							lastNpc = i;
						}
					}
				}
			}
			else
			{
				NPC npc = Main.npc[lastNpc];
				if (npc.CanBeChasedBy() && npc.active)
				{
					bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
					targetCenter = npc.Center + npc.velocity * 2;
				}
				else
                {
					foundTarget = false;
                }
			}
			if(targetCenter != projectile.Center)
				return targetCenter;
			return cursor;	
		}
		float spin = 1800;
		public override void AI()
		{
			projectile.velocity *= 0.96f;
			if(spin > 0)
            {
				spin -= 3;
				spin *= 0.97f;
            }
			if(spin < 0)
            {
				spin = 0;
            }
			if(Main.myPlayer == projectile.owner)
			{
				projectile.ai[0] = (FindTarget() - projectile.Center).ToRotation();
				projectile.netUpdate = true;
			}
			projectile.rotation = projectile.ai[0] + MathHelper.ToRadians(spin);
			if(spin <= 0)
			{
				projectile.ai[1]++;
				if (projectile.ai[1] >= 10)
				{
					SoundEngine.PlaySound(2, (int)(projectile.Center.X), (int)(projectile.Center.Y), 9, 0.75f);
					if (Main.myPlayer == projectile.owner)
					{
						Projectile.NewProjectile(projectile.Center, new Vector2(4, 0).RotatedBy(projectile.rotation), mod.ProjectileType("MacaroniBeam"), projectile.damage, projectile.knockBack, projectile.owner);
					}
					projectile.Kill();
				}
			}
		}	
	}
}
		