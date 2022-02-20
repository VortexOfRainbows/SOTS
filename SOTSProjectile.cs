using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Projectiles.Nature;
using SOTS.Projectiles.Otherworld;
using SOTS.Projectiles.Permafrost;
using SOTS.Void;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static SOTS.SOTS;

namespace SOTS
{
	public class SOTSProjectile : GlobalProjectile
	{
		public override bool InstancePerEntity => true;
		public override void PostAI(Projectile projectile)
		{
			NatureSlimeUnit(projectile);
			HomingUnit(projectile);
			counter++;
			if (projectile.arrow || projectile.type == ModContent.ProjectileType<ChargedHardlightArrow>())
				FrostFlakeUnit(projectile, frostFlake - 2);
		}
		public int frostFlake = 0;
		public int affixID = 0;
		public bool hasHitYet = false;
		public bool effect = true;
		public int counter = 0;
		public int petAdvisorID = -1;
		private float spinCounter = 0;
		public void SendClientChanges(Player player, Projectile projectile)
		{
			// Send a Mod Packet with the changes.
			var packet = mod.GetPacket();
			packet.Write((byte)SOTSMessageType.SyncGlobalProj);
			packet.Write((byte)player.whoAmI);
			packet.Write(projectile.identity);
			packet.Write(frostFlake);
			packet.Write(affixID);
			packet.Send();
		}
		private Vector2 initialVelo = Vector2.Zero;
		public void FrostFlakeUnit(Projectile projectile, int level)
        {
			if (level < -1)
				return;
			else if(level <= 0)
            {
				frostFlake += 2;
				if (Main.myPlayer == projectile.owner && Main.netMode == NetmodeID.MultiplayerClient)
					SendClientChanges(Main.player[projectile.owner], projectile);
            }
			if (projectile.type == ModContent.ProjectileType<ChargedHardlightArrow>())
				return;
			spinCounter += projectile.velocity.Length() * MathHelper.ToRadians(0.75f) * projectile.direction;
			if (level == 1)
			{
				if (!Main.rand.NextBool(3))
				{
					Dust dust = Dust.NewDustDirect(projectile.Center + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(116, 125, 238));
					dust.noGravity = true;
					dust.scale = 1;
					dust.velocity = Vector2.Zero;
					dust.fadeIn = 0.1f;
				}
			}
			else if(level == 2)
			{
				for(int i = 0; i < 5; i++)
				{
					Vector2 spawnPos = Vector2.Lerp(projectile.Center, projectile.oldPosition + projectile.Size / 2, i * 0.2f);
					Dust dust = Dust.NewDustDirect(spawnPos + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(116, 125, 238));
					dust.noGravity = true;
					dust.scale = 1.2f;
					dust.velocity = Vector2.Zero;
					dust.fadeIn = 0.1f;
				}
			}
			for(int i = 0; i < level; i++)
            {
				if(Main.rand.NextBool(2))
				{
					Dust dust = Dust.NewDustDirect(projectile.Center + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(116, 125, 238));
					dust.noGravity = true;
					dust.scale = dust.scale * 0.5f + (1 + level) * 0.5f;
					dust.velocity = (dust.velocity * 0.3f + Main.rand.NextVector2Circular(0.6f, 0.6f) + projectile.velocity * 0.5f);
					dust.fadeIn = 0.1f;
				}
			}
			if(initialVelo == Vector2.Zero)
				initialVelo = projectile.velocity;
			if(projectile.velocity.X == initialVelo.X && projectile.velocity.Y != initialVelo.Y)
				projectile.velocity = initialVelo;
        }
        public void HomingUnit(Projectile projectile)
		{
			if (hasHitYet || !projectile.active || projectile.damage <= 0 || counter > 900f || SOTSPlayer.typhonBlacklist.Contains(projectile.type))
				return;
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			float distP = Vector2.Distance(player.Center, projectile.Center);
			if (!player.active || distP > 2000f)
				return;
			if (modPlayer.petAdvisor && counter >= 5 && modPlayer.typhonRange > 0)
			{
				if (petAdvisorID == -1)
				{
					//Main.NewText("Advisor Check " + projectile.whoAmI);
					for (int i = 0; i < Main.projectile.Length; i++)
					{
						Projectile proj = Main.projectile[i];
						if (proj.active && proj.owner == projectile.owner && proj.type == ModContent.ProjectileType<AdvisorPet>())
						{
							petAdvisorID = i;
							break;
						}
					}
				}
				else
				{
					Projectile proj = Main.projectile[petAdvisorID];
					if (!(proj.active && proj.owner == projectile.owner && proj.type == ModContent.ProjectileType<AdvisorPet>()))
					{
						petAdvisorID = -1;
					}
				}
			}
			if (counter >= 5)
			{
				if (modPlayer.typhonRange > 0)
				{
					float minDist = modPlayer.typhonRange * 2;
					int target2 = -1;
					float speed = projectile.velocity.Length();
					bool capable = speed > 1f && (projectile.ranged || projectile.melee || projectile.magic || projectile.thrown || (!projectile.sentry && !projectile.minion)) && (projectile.modProjectile == null || projectile.modProjectile.ShouldUpdatePosition()) && (projectile.modProjectile == null || projectile.modProjectile.CanDamage());
					if (projectile.friendly == true && projectile.hostile == false && player.heldProj != projectile.whoAmI && (capable || SOTSPlayer.typhonWhitelist.Contains(projectile.type)))
					{
						//Main.NewText("past Check " + projectile.whoAmI);
						for (int i = 0; i < Main.npc.Length; i++)
						{
							NPC target = Main.npc[i];
							if (target.CanBeChasedBy())
							{
								float distance = Vector2.Distance(projectile.Center, target.Center);
								if (distance < minDist)
								{
									Rectangle increasedHitbox = new Rectangle(projectile.Hitbox.X - modPlayer.typhonRange, projectile.Hitbox.Y - modPlayer.typhonRange, projectile.width + 2 * modPlayer.typhonRange, projectile.height + 2 * modPlayer.typhonRange);
									if (target.Hitbox.Intersects(increasedHitbox))
									{
										if (Collision.CanHitLine(projectile.position, projectile.width, projectile.height, target.position, target.width, target.height))
										{
											minDist = distance;
											target2 = i;
										}
									}
								}
							}
						}
						if (target2 != -1)
						{
							NPC toHit = Main.npc[target2];
							if (toHit.active)
							{
								Vector2 goTo = (toHit.Center - projectile.Center).SafeNormalize(Vector2.Zero) * speed;
								Vector2 velocity1 = projectile.velocity.SafeNormalize(Vector2.Zero);
								Vector2 velocity2 = goTo.SafeNormalize(Vector2.Zero);
								float close = (velocity1 - velocity2).Length() * 40f;
								projectile.velocity = goTo;
								if (petAdvisorID != -1 && effect)
								{
									Projectile proj = Main.projectile[petAdvisorID];
									if ((int)((90 - (int)close) * 2.5 + 45) < 255)
									{
										LaserTo(petAdvisorID, projectile, 90 - (int)close);
										float recalc = (close - 40) / 40f; //1 max, -1 min
										AdvisorPet pet = (AdvisorPet)proj.modProjectile;
										float num = -1f * recalc;
										pet.eyeReset = num - 1.5f;
										pet.fireToX = projectile.Center.X;
										pet.fireToY = projectile.Center.Y;
										pet.glow = 11.5f + 3.5f * recalc;
										Main.PlaySound(SoundID.Item, (int)proj.Center.X, (int)proj.Center.Y, 8, 1.35f * (0.75f + 0.5f * recalc));
									}
									effect = false;
								}
								hasHitYet = true;
							}
						}
					}
				}
			}
		}
		public bool hasFrostBloomed = false;
		public int bloomingHookAssignment = -1;
		public void NatureSlimeUnit(Projectile projectile)
		{
			Player player = Main.player[projectile.owner];
			if (player.active && projectile.minion && projectile.active && !SOTSPlayer.symbioteBlacklist.Contains(projectile.type) && (Main.projPet[projectile.type] || VoidPlayer.isVoidMinion(projectile.type)) && projectile.damage > 0)
			{
				SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
				if(modPlayer.symbioteDamage > 0 && projectile.owner == Main.myPlayer)
				{
					if (bloomingHookAssignment == -1)
					{
						bloomingHookAssignment = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, ModContent.ProjectileType<BloomingHookMinion>(), modPlayer.symbioteDamage, 0.4f, Main.myPlayer, projectile.identity);
					}
					Projectile hook = Main.projectile[bloomingHookAssignment];
					if (!hook.active || hook.type != ModContent.ProjectileType<BloomingHookMinion>())
					{
						bloomingHookAssignment = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, ModContent.ProjectileType<BloomingHookMinion>(), modPlayer.symbioteDamage, 0.4f, Main.myPlayer, projectile.identity);
					}
					hook.timeLeft = 6;
				}
			}
			else
				return;
        }
        public override void Kill(Projectile projectile, int timeLeft)
        {
			if(!hasFrostBloomed && frostFlake >= 3)
			{
				FrostBloom(projectile);
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
		{
			if (!hasFrostBloomed && frostFlake >= 3)
				FrostBloom(projectile);
		}
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
		{
			if (!hasFrostBloomed && frostFlake >= 3)
				FrostBloom(projectile);
			return base.OnTileCollide(projectile, oldVelocity);
        }
        public void FrostBloom(Projectile projectile)
		{
			hasFrostBloomed = true;
			int ffValue = frostFlake - 2;
			frostFlake = 0;
			Vector2 manipulateVelo = projectile.oldVelocity;
			//TODO: add these visual effect onto the residual projectile to increase multiplayer compatability
			if (Main.myPlayer == projectile.owner)
			{
				if (ffValue > 0 && projectile.type != ModContent.ProjectileType<ChargedHardlightArrow>())
				{
					float damageMult = 2;
					if (ffValue == 2)
						damageMult = 6;
					Projectile.NewProjectile(projectile.Center, manipulateVelo, ModContent.ProjectileType<FrostflakePulse>(), (int)(projectile.damage * damageMult), projectile.knockBack, Main.myPlayer, ffValue, spinCounter);
				}
				if (Main.netMode == NetmodeID.MultiplayerClient)
					SendClientChanges(Main.player[projectile.owner], projectile);
			}
		}
        public override bool PreDraw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
		{
			if (frostFlake == 4 && projectile.type != ModContent.ProjectileType<ChargedHardlightArrow>() && !hasFrostBloomed)
			{
				for (int i = 0; i < 3; i++)
				{
					Vector2 spawnPos = Vector2.Lerp(projectile.Center + projectile.velocity.SafeNormalize(Vector2.Zero) * 8, projectile.Center + projectile.velocity.SafeNormalize(Vector2.Zero) * -120f, i * 0.3f);
					float percent = (1 - 0.3f * i);
					float alphaMult = (counter - i * 7) / 14f;
					if (alphaMult > 0.7f)
						alphaMult = 0.7f;
					else if (alphaMult < 0)
						alphaMult = 0;
					float dist1 = 8 * percent;
					float dist2 = 6 * percent;
					DrawStar(spawnPos, alphaMult, projectile.velocity.ToRotation(), spinCounter + MathHelper.ToRadians(30 * i), 6, dist1, dist2, 0.6f);
				}
			}
			/*(if (bloomingHookAssignment != -1)
			{
				Projectile hook = Main.projectile[bloomingHookAssignment];
				if (hook.active && hook.type == ModContent.ProjectileType<BloomingHookMinion>())
                {
					BloomingHookMinion minion = hook.modProjectile as BloomingHookMinion;
					minion.Draw(spriteBatch, lightColor);
                }
			}*/
            return base.PreDraw(projectile, spriteBatch, lightColor);
        }
        public static void LaserTo(int advisorId, Projectile projectile, int extraAlpha)
		{
			Player player = Main.player[projectile.owner];
			int alpha = (int)(extraAlpha * 2.5 + 40);
			Projectile proj = Main.projectile[advisorId];
			Vector2 projPos = proj.Center + new Vector2(0, 8);
			Vector2 toProjectile = projectile.Center - projPos;
			Vector2 newtoProjectile = toProjectile.SafeNormalize(Vector2.Zero) * 4;
			Vector2 currentPos = projPos;
			Vector2 savePos = projPos;
			Color color = new Color(255, 182, 242, 0);
			int remaining = 3000;
			int interator = 0;
			currentPos += newtoProjectile * 2;
			while (remaining > 0)
			{
				remaining--;
				interator++;
				currentPos += newtoProjectile;
				int num1 = Dust.NewDust(new Vector2(currentPos.X - 4, currentPos.Y - 4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, alpha);
				Dust dust = Main.dust[num1];
				dust.velocity *= 0.2f;
				dust.noGravity = true;
				dust.color = color;
				dust.fadeIn = 0.2f;
				dust.scale *= 1.25f;
				dust.shader = GameShaders.Armor.GetShaderFromItemId(player.miscDyes[1].type);
				toProjectile = projectile.Center - currentPos;
				if (toProjectile.Length() < Math.Sqrt(proj.width * projectile.height) + 8 && remaining > 60)
				{
					for (int i = 0; i < 360; i += 10)
					{
						Vector2 currentFromProjectile = currentPos - projectile.Center;
						currentFromProjectile = currentFromProjectile.RotatedBy(MathHelper.ToRadians(i));
						currentFromProjectile += projectile.Center;
						interator++;
						num1 = Dust.NewDust(new Vector2(currentFromProjectile.X - 4, currentFromProjectile.Y - 4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, alpha);
						dust = Main.dust[num1];
						dust.velocity *= 0.2f;
						dust.noGravity = true;
						dust.color = color;
						dust.fadeIn = 0.2f;
						dust.scale *= 1.25f;
						dust.shader = GameShaders.Armor.GetShaderFromItemId(player.miscDyes[1].type);
					}
					remaining = 20;
					currentPos = new Vector2((float)Math.Sqrt(proj.width * projectile.height) + 8, 0).RotatedBy(projectile.velocity.ToRotation()) + projectile.Center;
				}
				if (remaining < 20)
				{
					newtoProjectile = projectile.velocity.SafeNormalize(Vector2.Zero) * 3;
				}
				savePos = currentPos;
			}
			currentPos = savePos;
			for (int i = 0; i < 8; i++)
			{
				newtoProjectile = projectile.velocity.RotatedBy(MathHelper.ToRadians(-160)).SafeNormalize(Vector2.Zero) * 3;
				currentPos += newtoProjectile;
				interator++;
				int num1 = Dust.NewDust(new Vector2(currentPos.X - 4, currentPos.Y - 4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, alpha);
				Dust dust = Main.dust[num1];
				dust.velocity *= 0.2f;
				dust.noGravity = true;
				dust.color = color;
				dust.fadeIn = 0.2f;
				dust.scale *= 1.25f;
				dust.shader = GameShaders.Armor.GetShaderFromItemId(player.miscDyes[1].type);
			}
			currentPos = savePos;
			for (int i = 0; i < 8; i++)
			{
				newtoProjectile = projectile.velocity.RotatedBy(MathHelper.ToRadians(160)).SafeNormalize(Vector2.Zero) * 3;
				currentPos += newtoProjectile;
				interator++;
				int num1 = Dust.NewDust(new Vector2(currentPos.X - 4, currentPos.Y - 4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, alpha);
				Dust dust = Main.dust[num1];
				dust.velocity *= 0.2f;
				dust.noGravity = true;
				dust.color = color;
				dust.fadeIn = 0.2f;
				dust.scale *= 1.25f;
				dust.shader = GameShaders.Armor.GetShaderFromItemId(player.miscDyes[1].type);
			}
		}
		public static void DrawStar(Vector2 location, float alphaMult, float rotation, float spin = 0, int pointAmount = 6, float innerDistAdd = 10, float innerDistMin = 8, float xCompress = 0.6f, int density = 180)
		{
			Vector2 fireFrom = location; 
			Texture2D texture = ModContent.GetTexture("SOTS/Assets/StrangeGradient");
			for (float k = 0; k < 360; k += 360 / (float)density)
			{
				float length = innerDistAdd + innerDistMin;
				float rad = MathHelper.ToRadians(k);
				float rand = 0;
				float x = (float)Math.Cos(rad + rand);
				float y = (float)Math.Sin(rad + rand);
				float mult = (Math.Abs((rad * (pointAmount / 2) % (float)Math.PI) - (float)Math.PI / 2) * innerDistAdd) + innerDistMin;//triangle wave function
				Vector2 circular = new Vector2(x, y).RotatedBy(spin) * mult;
				circular.X *= xCompress;
				Vector2 scale = new Vector2(circular.Length() / length, 0.5f);
				circular = circular.RotatedBy(rotation);
				Main.spriteBatch.Draw(texture, fireFrom + circular - Main.screenPosition, null, new Color(116, 125, 238, 0) * alphaMult * (1 / (circular.Length() / length)), circular.ToRotation(), new Vector2(texture.Width / 2, texture.Height / 2), scale * 1.25f, SpriteEffects.None, 0f);
			}
		}
		public static void DustStar(Vector2 location, Vector2 velocity, float rotation, int total = 30, float spin = 0, int pointAmount = 6, float innerDistAdd = 10, float innerDistMin = 8, float xCompress = 0.6f, float scaleMult = 1f)
		{
			for (float k = 0; k < total; k++)
			{
				float rad = MathHelper.ToRadians(k * 360f / total);
				float rand = 0;
				float x = (float)Math.Cos(rad + rand);
				float y = (float)Math.Sin(rad + rand);
				float mult = (Math.Abs((rad * (pointAmount / 2) % (float)Math.PI) - (float)Math.PI / 2) * innerDistAdd) + innerDistMin;//triangle wave function
				Vector2 circular = new Vector2(x, y).RotatedBy(spin) * mult;
				circular.X *= xCompress;
				circular = circular.RotatedBy(rotation);
				Dust dust = Dust.NewDustDirect(circular + location - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(116, 125, 238));
				dust.noGravity = true;
				dust.scale = (dust.scale * 0.5f + 1) * scaleMult;
				dust.velocity = dust.velocity * 0.1f + velocity;
				dust.fadeIn = 0.1f;
			}
		}
	}
}