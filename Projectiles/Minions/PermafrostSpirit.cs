using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Projectiles.Permafrost;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Minions
{
	public class PermafrostSpirit : SpiritMinion
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Permafrost Spirit");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 7;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;   
		}
		public sealed override void SetDefaults()
		{
			projectile.width = 34;
			projectile.height = 34;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			base.PostDraw(spriteBatch, lightColor);
			Texture2D texture = mod.GetTexture("Projectiles/Minions/PermafrostSpiritBand");
			float alpha = (48 - projectile.ai[0]) / 48f;
			Color color = new Color(90, 90, 90, 0) * alpha;
		
			/*for (int k = 0; k < 9; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.25f;
				float y = Main.rand.Next(-10, 11) * 0.25f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color, 0f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}*/
			
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, 26);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.NextFloat(-1, 1);
				float y = Main.rand.NextFloat(-1, 1);
				Main.spriteBatch.Draw(texture, projectile.Center + new Vector2(0, -1) - Main.screenPosition + new Vector2(x, y), null, color, projectile.velocity.X * 0.04f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
		public override bool MinionContactDamage()
		{
			return true;
		}
		public override void AI() 
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			#region Active check
			if (player.dead || !player.active) 
			{
				player.ClearBuff(ModContent.BuffType<PermafrostSpiritAid>());
			}
			if (player.HasBuff(ModContent.BuffType<PermafrostSpiritAid>()))
			{
				projectile.timeLeft = 2;
			}
			#endregion

			#region General behavior
			bool found = false;
			int ofTotal = 0;
			int total = 0;
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (projectile.type == proj.type && proj.active && projectile.active && proj.owner == projectile.owner)
				{
					if (proj == projectile)
					{
						found = true;
					}
					if (!found)
						ofTotal++;
					total++;
				}
			}
			if (Main.myPlayer == player.whoAmI)
				projectile.ai[1] = ofTotal;
			#endregion

			#region Find target
			float distanceFromTarget = 1200f;
			Vector2 targetCenter = projectile.Center;
			bool foundTarget = false;
			float npcWidthHeight = 0;

			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, player.Center);
				if (between < distanceFromTarget) 
				{
					distanceFromTarget = between;
					targetCenter = npc.Center;
					foundTarget = true;
					npcWidthHeight = (float)Math.Sqrt(npc.width * npc.height);
				}
			}
			if (!foundTarget) 
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy()) 
					{
						float between = Vector2.Distance(npc.Center, player.Center);
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height);
						
						bool closeThroughWall = between < 160f; //should attack semi-reliably through walls
						if (inRange && (lineOfSight || closeThroughWall) && between < distanceFromTarget)
						{
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
							npcWidthHeight = (float)Math.Sqrt(npc.width * npc.height);
						}
					}
				}
			}
			#endregion

			#region Movement
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 96f;
			float speed = 12.0f;
			if(projectile.ai[0] > 0)
            {
				projectile.ai[0] -= 0.5f;
            }
			else
            {
				projectile.ai[0] = 0;
            }
			if (foundTarget)
			{
				Vector2 circular = new Vector2(0, 12 + total * 4 + npcWidthHeight / 2 - projectile.ai[0] * 0.4f).RotatedBy(MathHelper.ToRadians(-2 * modPlayer.orbitalCounter + (360f / total * projectile.ai[1])));
				circular.Y *= 0.8f;
				Vector2 toPos = targetCenter - new Vector2(0, npcWidthHeight * 1.1f + 64 + total * 4) + circular;
				Vector2 direction = toPos - projectile.Center;
				float distance = direction.Length();
				bool inRange = distance < 96 + npcWidthHeight;
				direction = direction.SafeNormalize(Vector2.Zero);
				if (distance > speed)
				{
					distance = speed;
				}
				direction *= distance;
				projectile.velocity = direction;
				int fireRate = 180;
				if((int)(-modPlayer.orbitalCounter + (float)fireRate / total * projectile.ai[1]) % fireRate == 0 && inRange)
				{
					for (int i = 0; i < 360; i += 10)
					{
						Vector2 circularLocation = new Vector2(-Main.rand.NextFloat(9, 10), 0).RotatedBy(MathHelper.ToRadians(i) + projectile.rotation);
						int dust2 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, DustID.RainbowMk2);
						Dust dust = Main.dust[dust2];
						dust.velocity = circularLocation * 0.35f;
						dust.color = new Color(190 - Main.rand.Next(50), 220 - Main.rand.Next(50), 250, 150);
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 1.8f;
					}
					Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 71, 0.55f, -0.3f);
					if (Main.myPlayer == projectile.owner)
					{
						for(int i = -1; i < 2; i++)
                        {
							Vector2 shotSpread = new Vector2(0, 6.5f).RotatedBy(MathHelper.ToRadians(22.5f * i));
							Projectile.NewProjectile(projectile.Center, shotSpread, ModContent.ProjectileType<FrostSpear>(), projectile.damage, projectile.knockBack, projectile.owner);
						}
					}
					projectile.ai[0] = 80;
				}
			}
			else
			{
				GoIdle();
				Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
				float distanceToIdlePosition = vectorToIdlePosition.Length();
				if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1400f)
				{
					projectile.position = idlePosition;
					projectile.velocity *= 0.1f;
					projectile.netUpdate = true;
				}
			}
			#endregion

			Lighting.AddLight(projectile.Center, 1.0f * 0.4f * ((255 - projectile.alpha) / 255f), 2.4f * 0.4f * ((255 - projectile.alpha) / 255f), 2.5f * 0.4f * ((255 - projectile.alpha) / 255f));
			MoveAwayFromOthers();

			if (Main.myPlayer == player.whoAmI)
			{
				projectile.netUpdate = true;
			}
		}
	}
}