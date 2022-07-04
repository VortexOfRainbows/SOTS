using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System.IO;
using SOTS.Buffs.MinionBuffs;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Minions
{
	public class EarthenSpirit : SpiritMinion
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Earthen Spirit");
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		
		public override void SetDefaults()
		{
			SetSpiritMinionDefaults();
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.netImportant = true;
			Projectile.ignoreWater = true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.alpha);
			writer.Write(readyToFight);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.alpha = reader.ReadInt32();
			readyToFight = reader.ReadBoolean();
			base.ReceiveExtraAI(reader);
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
		public override bool MinionContactDamage()
		{
			return true;
		}
		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Projectiles/Minions/EarthenSpiritReticle").Value;
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 9; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.25f;
				float y = Main.rand.Next(-10, 11) * 0.25f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * ((255 - Projectile.alpha) / 255f), 0f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);

				x = Main.rand.Next(-10, 11) * 0.125f;
				y = Main.rand.Next(-10, 11) * 0.125f;
				float reticleAlpha = Projectile.ai[0] / 60f;
				if(reticleAlpha > 1)
				{
					reticleAlpha = 1;
				}
				if(Projectile.velocity.Length() <= 10f)
					Main.spriteBatch.Draw(texture2, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * reticleAlpha, MathHelper.ToRadians((Projectile.ai[0] + 2) * 6f), drawOrigin, Projectile.scale * reticleAlpha, SpriteEffects.None, 0f);
			}
		}
		bool readyToFight = false;
		public void dustSound()
		{
			SOTSUtils.PlaySound(SoundID.Item14, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.4f);
			for (int i = 0; i < 360; i += 20)
			{
				Vector2 circularLocation = new Vector2(5, 0).RotatedBy(MathHelper.ToRadians(i));

				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 222);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].scale = 1.2f;
				Main.dust[num1].velocity = circularLocation * 0.25f + new Vector2(Main.rand.Next(-20, 21), Main.rand.Next(-20, 21)) * 0.1f;


				num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 222);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].scale = 1.5f;
				Main.dust[num1].velocity = circularLocation * 0.45f + new Vector2(Main.rand.Next(-20, 21), Main.rand.Next(-20, 21)) * 0.2f;
			}
		}
		public override void AI() 
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);

			#region Active check
			if (player.dead || !player.active) 
			{
				player.ClearBuff(ModContent.BuffType<EarthenSpiritAid>());
			}
			if (player.HasBuff(ModContent.BuffType<EarthenSpiritAid>()))
			{
				Projectile.timeLeft = 6;
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
			float distanceFromTarget = 1000f;
			Vector2 targetCenter = Projectile.Center;
			bool foundTarget = false;

			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Projectile.Center);
				float between2 = Vector2.Distance(npc.Center, player.Center);
				if (between2 < distanceFromTarget) 
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
						bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
						
						bool closeThroughWall = between < 100f; //should attack semi-reliably through walls
						if (inRange && (lineOfSight || closeThroughWall) && between2 < distanceFromTarget)
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
			float speed = 10f;
			if (foundTarget)
			{
				if (Projectile.alpha >= 255)
					speed = 50f;

				Vector2 direction = targetCenter - Projectile.Center;
				float distance = direction.Length() + 0.1f;
				if (distance > 1.1f)
				{
					direction = direction.SafeNormalize(Vector2.Zero);
					if (distance > speed)
					{
						distance = speed;
					}
					direction *= distance;
					Projectile.velocity = direction;
				}
				else
				{
					Projectile.velocity *= 0f;
				}
				Projectile.alpha += 10;

				if (Projectile.alpha > 255)
				{
					Projectile.alpha = 255;	
					if (readyToFight)
						Projectile.ai[0]++;
					int cooldown = 80;
					if (total != 0 && (int)modPlayer.orbitalCounter % cooldown == (int)(Projectile.ai[1] * (float)cooldown / total + 0.5f) % cooldown)
					{
						if (readyToFight)
						{
							Projectile.ai[0] = 0;
							Projectile.alpha = 0;
							Projectile.friendly = true;
							dustSound();
							readyToFight = false;
						}
						else
						{
							readyToFight = true;
						}
					}
				}
				else
				{
					Projectile.friendly = false;
				}
			}
			else
			{
				GoIdle();
				readyToFight = false;
				Projectile.ai[0] = 0;
				Projectile.alpha -= 8;
				if (Projectile.alpha < 0)
				{
					Projectile.alpha = 0;
				}
				Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
				float distanceToIdlePosition = vectorToIdlePosition.Length();
				if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1400f)
				{
					Projectile.ai[0] = 0;
					Projectile.alpha = 0;
					Projectile.position = idlePosition;
					Projectile.velocity *= 0.1f;
					Projectile.netUpdate = true;
				}
			}
			#endregion

			Lighting.AddLight(Projectile.Center, 2.4f * 0.5f * ((255 - Projectile.alpha) / 255f), 2.2f * 0.5f * ((255 - Projectile.alpha) / 255f), 1.4f * 0.5f * ((255 - Projectile.alpha) / 255f));
			MoveAwayFromOthers();

			if (Main.myPlayer == player.whoAmI)
			{
				Projectile.netUpdate = true;
			}
		}
	}
}