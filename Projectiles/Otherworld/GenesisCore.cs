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
    public class GenesisCore : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Genesis Core");
		}
        public override void SetDefaults()
        {
			projectile.height = 30;
			projectile.width = 30;
			projectile.magic = true;
			projectile.timeLeft = 7200;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.alpha = 100;
		}
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			crit = false;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = new Color(255, 70, 70, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 4 + num * 2; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.25f * (1 + projectile.ai[1] * 10);
				Vector2 direction = new Vector2(x, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(361)));
				if(k < 2)
                {
					direction *= 0;
				}
				Main.spriteBatch.Draw(texture, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X), (float)(projectile.Center.Y - (int)Main.screenPosition.Y)) + direction, null, color * ((255 - projectile.alpha) / 255f), direction.ToRotation(), drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public float DistanceMult = 1f;
		int num = 0;
		int counter = 0;
		int counter2 = 0;
		public override void AI()
		{
			counter++;
			projectile.ai[0] += 5;
			if (num >= 28)
			{
				if(DistanceMult > 0)
				{
					DistanceMult -= 0.75f;
					DistanceMult *= 0.8f;
				}
				else
                {
					DistanceMult *= 0;
                }
				if (DistanceMult <= 0)
				{
					counter2++;
					if(counter2 >= 20)
						projectile.Kill();
				}
			}
			else
				DistanceMult += 0.015f;
			if(counter % 5 == 0)
            {
				if (num < 24)
				{
					Vector2 velo = projectile.velocity.SafeNormalize(new Vector2(1, 0)) * 6;
					Projectile.NewProjectile(projectile.Center, velo, ModContent.ProjectileType<GenesisArc>(), projectile.damage, projectile.knockBack, projectile.owner, projectile.whoAmI, 0);
				}
				num++;
			}
			if (projectile.ai[1] < 1.9)
				projectile.ai[1] += 0.0075f;
			projectile.velocity *= 0.975f;
		}
		public void resetVector2(ref Vector2 loc, int i)
		{
			loc = new Vector2(14, 0).RotatedBy(MathHelper.ToRadians(i * 10));
			loc.X += Main.rand.Next(-5, 6);
			loc.Y += Main.rand.Next(-5, 6);
			loc *= 0.1f;
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 94, 1.5f);
			Vector2 circularLocation = new Vector2(10, 0);
			for(int j = 1; j < 3; j++)
			{
				for (int i = 0; i < 72; i++)
				{
					resetVector2(ref circularLocation, i);
					int dust = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 3), 0, 0, 235);
					Main.dust[dust].velocity = circularLocation;
					Main.dust[dust].velocity *= Main.rand.NextFloat(4.5f, 15.5f) * j;
					Main.dust[dust].scale *= 2f - (j - 1);
					Main.dust[dust].noGravity = true;

					if (Main.rand.NextBool(2))
					{
						resetVector2(ref circularLocation, i);
						dust = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 3), 0, 0, 235);
						Main.dust[dust].velocity = circularLocation;
						Main.dust[dust].velocity *= Main.rand.NextFloat(0.4f, 2.1f) * j;
						Main.dust[dust].scale *= 6f - (j - 1);
						Main.dust[dust].noGravity = true;
					}

					if (Main.rand.NextBool(2))
					{
						resetVector2(ref circularLocation, i);
						dust = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 3), 0, 0, 235);
						Main.dust[dust].velocity = circularLocation;
						Main.dust[dust].velocity *= Main.rand.NextFloat(1.5f, 4.5f) * j;
						Main.dust[dust].scale *= 5f - (j - 1);
						Main.dust[dust].noGravity = true;
					}

					resetVector2(ref circularLocation, i);
					dust = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 3), 0, 0, 235);
					Main.dust[dust].velocity = circularLocation;
					Main.dust[dust].velocity *= Main.rand.NextFloat(2.25f, 7.5f) * j;
					Main.dust[dust].scale *= 4f - (j - 1);
					Main.dust[dust].noGravity = true;

					resetVector2(ref circularLocation, i);
					dust = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 3), 0, 0, 235);
					Main.dust[dust].velocity = circularLocation;
					Main.dust[dust].velocity *= Main.rand.NextFloat(3.75f, 12.5f) * j;
					Main.dust[dust].scale *= 3f - (j - 1);
					Main.dust[dust].noGravity = true;
				}
			}

			Player player = Main.player[projectile.owner];
			if (projectile.owner == Main.myPlayer)
			{
				List<int> blackList = new List<int>();
				for (int j = 0; j < 36; j++)
				{
					int npcIndex = -1;
					double distanceTB = 1256;
					for (int i = 0; i < 200; i++) //find first enemy
					{
						NPC npc = Main.npc[i];
						if (!npc.friendly && npc.lifeMax > 5 && npc.active && !npc.dontTakeDamage && !blackList.Contains(npc.whoAmI))
						{
							if (npcIndex != i)
							{
								float disX = projectile.Center.X - npc.Center.X;
								float disY = projectile.Center.Y - npc.Center.Y;
								double dis = Math.Sqrt(disX * disX + disY * disY);
								if (dis < distanceTB)
								{
									distanceTB = dis;
									npcIndex = i;
								}
							}
						}
					}
					if (npcIndex != -1)
					{
						blackList.Add(npcIndex);
						NPC npc = Main.npc[npcIndex];
						if (!npc.friendly && npc.lifeMax > 5 && npc.active && !npc.dontTakeDamage)
						{
							Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("OriginLightningZap"), (int)(projectile.damage * 1f), 0, projectile.owner, npc.whoAmI);
						}
					}
				}
			}
			base.Kill(timeLeft);
        }
    }
}
		