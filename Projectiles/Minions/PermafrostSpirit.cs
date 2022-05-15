using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Buffs.MinionBuffs;
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
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;   
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
		}
		public override void PostDraw(Color lightColor)
		{
			base.PostDraw(spriteBatch, lightColor);
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Minions/PermafrostSpiritBand").Value;
			float alpha = (48 - Projectile.ai[0]) / 48f;
			Color color = new Color(90, 90, 90, 0) * alpha;
		
			/*for (int k = 0; k < 9; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.25f;
				float y = Main.rand.Next(-10, 11) * 0.25f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color, 0f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}*/
			
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, 26);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.NextFloat(-1, 1);
				float y = Main.rand.NextFloat(-1, 1);
				Main.spriteBatch.Draw(texture, Projectile.Center + new Vector2(0, -1) - Main.screenPosition + new Vector2(x, y), null, color, Projectile.velocity.X * 0.04f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
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
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			#region Active check
			if (player.dead || !player.active) 
			{
				player.ClearBuff(ModContent.BuffType<PermafrostSpiritAid>());
			}
			if (player.HasBuff(ModContent.BuffType<PermafrostSpiritAid>()))
			{
				Projectile.timeLeft = 2;
			}
			#endregion

			#region General behavior
			bool found = false;
			int ofTotal = 0;
			int total = 0;
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (Projectile.type == proj.type && proj.active && Projectile.active && proj.owner == Projectile.owner)
				{
					if (proj == Projectile)
					{
						found = true;
					}
					if (!found)
						ofTotal++;
					total++;
				}
			}
			if (Main.myPlayer == player.whoAmI)
				Projectile.ai[1] = ofTotal;
			#endregion

			#region Find target
			float distanceFromTarget = 1200f;
			Vector2 targetCenter = Projectile.Center;
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
			if(Projectile.ai[0] > 0)
            {
				Projectile.ai[0] -= 0.5f;
            }
			else
            {
				Projectile.ai[0] = 0;
            }
			if (foundTarget)
			{
				Vector2 circular = new Vector2(0, 12 + total * 4 + npcWidthHeight / 2 - Projectile.ai[0] * 0.4f).RotatedBy(MathHelper.ToRadians(-2 * modPlayer.orbitalCounter + (360f / total * Projectile.ai[1])));
				circular.Y *= 0.8f;
				Vector2 toPos = targetCenter - new Vector2(0, npcWidthHeight * 1.1f + 64 + total * 4) + circular;
				Vector2 direction = toPos - Projectile.Center;
				float distance = direction.Length();
				bool inRange = distance < 96 + npcWidthHeight;
				direction = direction.SafeNormalize(Vector2.Zero);
				if (distance > speed)
				{
					distance = speed;
				}
				direction *= distance;
				Projectile.velocity = direction;
				int fireRate = 180;
				if((int)(-modPlayer.orbitalCounter + (float)fireRate / total * Projectile.ai[1]) % fireRate == 0 && inRange)
				{
					for (int i = 0; i < 360; i += 10)
					{
						Vector2 circularLocation = new Vector2(-Main.rand.NextFloat(9, 10), 0).RotatedBy(MathHelper.ToRadians(i) + Projectile.rotation);
						int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, DustID.RainbowMk2);
						Dust dust = Main.dust[dust2];
						dust.velocity = circularLocation * 0.35f;
						dust.color = new Color(190 - Main.rand.Next(50), 220 - Main.rand.Next(50), 250, 150);
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 1.8f;
					}
					SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 71, 0.55f, -0.3f);
					if (Main.myPlayer == Projectile.owner)
					{
						for(int i = -1; i <= 1; i++)
                        {
							Vector2 shotSpread = new Vector2(0, 6.5f).RotatedBy(MathHelper.ToRadians(22.5f * i));
							Projectile.NewProjectile(Projectile.Center, shotSpread, ModContent.ProjectileType<FrostSpear>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
						}
					}
					Projectile.ai[0] = 80;
				}
			}
			else
			{
				GoIdle();
				Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
				float distanceToIdlePosition = vectorToIdlePosition.Length();
				if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1400f)
				{
					Projectile.position = idlePosition;
					Projectile.velocity *= 0.1f;
					Projectile.netUpdate = true;
				}
			}
			#endregion

			Lighting.AddLight(Projectile.Center, 1.0f * 0.4f * ((255 - Projectile.alpha) / 255f), 2.4f * 0.4f * ((255 - Projectile.alpha) / 255f), 2.5f * 0.4f * ((255 - Projectile.alpha) / 255f));
			MoveAwayFromOthers();

			if (Main.myPlayer == player.whoAmI)
			{
				Projectile.netUpdate = true;
			}
		}
	}
}