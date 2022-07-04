using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using SOTS.Void;
using SOTS.Buffs;
using SOTS.Buffs.MinionBuffs;

namespace SOTS.Projectiles.Minions
{
	public class OtherworldlySpirit : SpiritMinion
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Otherworldly Spirit");
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
        public sealed override void SetDefaults()
		{
			SetSpiritMinionDefaults();
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.ignoreWater = true;
			Projectile.localNPCHitCooldown = 10;
		}
        public override bool PreDraw(ref Color lightColor)
		{
			Color color2 = VoidPlayer.OtherworldColor;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(color2) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(texture, drawPos, null, color * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
        public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = VoidPlayer.OtherworldColor * 0.75f;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 9; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.25f;
				float y = Main.rand.Next(-10, 11) * 0.25f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color, 0f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			texture = Mod.Assets.Request<Texture2D>("Projectiles/Minions/OtherworldlySpiritBall").Value;
			drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int i = 0; i < orbLocations.Length; i++)
            {
				if(!orbLocations[i].Equals(Projectile.Center))
				{
					for (int k = 0; k < 5; k++)
					{
						float x = Main.rand.Next(-10, 11) * 0.25f;
						float y = Main.rand.Next(-10, 11) * 0.25f;
						Main.spriteBatch.Draw(texture, orbLocations[i] - Main.screenPosition + new Vector2(x, y), null, color, 0f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
					}
				}
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
		bool runOnce = true;
		Vector2[] orbLocations = new Vector2[4];
		float[] orbCounter = new float[4];
		public void rotateOrbs(Vector2 npcCenter)
		{
			if (runOnce)
			{
				runOnce = false;
				for (int i = 0; i < orbLocations.Length; i++)
				{
					orbLocations[i] = Projectile.Center;
				}
			}
			for (int i = 0; i < orbLocations.Length; i++)
			{
				if(orbCounter[i] >= -30)
				{
					float distance = 42f;
					if (orbCounter[i] < 0)
					{
						orbCounter[i]++;
						Vector2 toLocation = new Vector2(distance + (orbCounter[i] * distance / 30f), 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[0] * 1.67f + 90 * i)) + Projectile.Center;
						orbLocations[i] = toLocation;
					}
					else if (orbCounter[i] >= 0)
					{
						Vector2 toLocation;
						if (!npcCenter.Equals(Vector2.Zero) && (i == 0 || orbCounter[i - 1] > 8 || orbCounter[i - 1] < 0 || orbCounter[i] > 0))
						{
							float currentAI = orbCounter[i]++;
							Vector2 rotationalVelo = new Vector2(0, 48 * 0.33f).RotatedBy(MathHelper.ToRadians(55f + currentAI * 8.5f));
							toLocation = npcCenter - orbLocations[i];
							toLocation = new Vector2(rotationalVelo.X, 0).RotatedBy(toLocation.ToRotation());
							if (currentAI >= 30)
							{
								//Terraria.Audio.SoundEngine.PlaySound(2, (int)Projectile.Center.X, (int)Projectile.Center.Y, 43, 0.4f);
								if (Main.myPlayer == Projectile.owner)
								{
									Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), orbLocations[i], new Vector2(1, 0).RotatedBy(toLocation.ToRotation()) * 12, ModContent.ProjectileType<OtherworldLightning>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 0);
								}
								orbCounter[i] = -60;
								orbLocations[i] = Projectile.Center;
								return;
							}
							else
							{
								orbLocations[i] += toLocation;
							}
						}
						else
						{
							toLocation = new Vector2(distance, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[0] * 1.67f + 90 * i)) + Projectile.Center;
							orbLocations[i] = toLocation;
							orbCounter[i] = 0;
						}
					}
				}
				else
                {
					if ((i == 0 || orbCounter[i - 1] >= 0) && (npcCenter.Equals(Vector2.Zero) || (orbCounter[0] < 0 && orbCounter[1] < 0 && orbCounter[2] < 0 && orbCounter[3] < 0)))
						orbCounter[i]++;
					orbLocations[i] = Projectile.Center;
                }
			}
		}
		public override void AI() 
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			#region Active check
			if (player.dead || !player.active) 
			{
				player.ClearBuff(ModContent.BuffType<OtherworldlySpiritAid>());
			}
			if (player.HasBuff(ModContent.BuffType<OtherworldlySpiritAid>()))
			{
				Projectile.timeLeft = 6;
			}
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
			{
				if (Projectile.ai[1] != ofTotal)
					Projectile.netUpdate = true;
				Projectile.ai[1] = ofTotal;
			}
			if (total > 0)
				Projectile.ai[0] = modPlayer.orbitalCounter + (Projectile.ai[1] * 360f / total);
			#endregion
			#region Find target
			float distanceFromTarget = 1200f;
			Vector2 targetCenter = Projectile.Center;
			bool foundTarget = false;
			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Projectile.Center);
				if (between < distanceFromTarget) 
				{
					distanceFromTarget = between;
					targetCenter = npc.Center;
					foundTarget = true;
				}
			}
			if (!foundTarget) 
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy()) 
					{
						float between = Vector2.Distance(npc.Center, Projectile.Center);
						float between2 = Vector2.Distance(npc.Center, player.Center);
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height);
						
						bool closeThroughWall = between2 < 160f; //10 blocks through walls //should attack semi-reliably through walls
						if (inRange && (lineOfSight || closeThroughWall) && between < distanceFromTarget)
						{
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
						}
					}
				}
			}
			#endregion

			#region Movement
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 96f;
			if (foundTarget)
			{
				float speed = -14f;
				Vector2 npcCenter = targetCenter;
				Vector2 direction = npcCenter - Projectile.Center;
				float distance = direction.Length();
				direction = direction.SafeNormalize(Vector2.Zero);
				int intDirection = direction.X > 0 ? 1 : -1;
				Vector2 rotateBy = new Vector2(0, 18 * intDirection).RotatedBy(Projectile.rotation);
				rotateBy += direction.SafeNormalize(Vector2.Zero) * intDirection;
				Projectile.rotation = rotateBy.ToRotation() - MathHelper.ToRadians(90) * intDirection;
				direction *= (float)Math.Pow(distance, 1.25) * 0.006f + speed;
				Projectile.velocity += direction;
				Projectile.velocity *= 0.5f;
				rotateOrbs(npcCenter);
			}
			else
			{
				rotateOrbs(Vector2.Zero);
				GoIdle();
			}
			Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
			float distanceToIdlePosition = vectorToIdlePosition.Length();
			if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1400f)
			{
				Projectile.Center = idlePosition;
				Projectile.velocity *= 0.1f;
				Projectile.netUpdate = true;
			}
			#endregion

			Lighting.AddLight(Projectile.Center, VoidPlayer.OtherworldColor.R / 255f, VoidPlayer.OtherworldColor.G / 255f * ((255 - Projectile.alpha) / 255f), VoidPlayer.OtherworldColor.B / 255f * ((255 - Projectile.alpha) / 255f));
			MoveAwayFromOthers(true, 0.11f, 2f);
		}
	}
}