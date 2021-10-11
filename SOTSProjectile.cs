using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Projectiles.Nature;
using SOTS.Void;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
	public class SOTSProjectile : GlobalProjectile
	{
		public override bool InstancePerEntity => true;
		public override void PostAI(Projectile projectile)
		{
			NatureSlimeUnit(projectile);
			HomingUnit(projectile);
		}
		public bool hasHitYet = false;
		public bool effect = true;
		public int counter = 0;
		public int petAdvisorID = -1;
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
			counter++;
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
		public int bloomingHookAssignment = -1;
		public void NatureSlimeUnit(Projectile projectile)
		{
			Player player = Main.player[projectile.owner];
			if (player.active && projectile.minion && projectile.active && !SOTSPlayer.symbioteBlacklist.Contains(projectile.type) && (Main.projPet[projectile.type] || VoidPlayer.isVoidMinion(projectile.type)))
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
        public override bool PreDraw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
        {
			if(bloomingHookAssignment != -1)
			{
				Projectile hook = Main.projectile[bloomingHookAssignment];
				if (hook.active && hook.type == ModContent.ProjectileType<BloomingHookMinion>())
                {
					BloomingHookMinion minion = hook.modProjectile as BloomingHookMinion;
					minion.Draw(spriteBatch, lightColor);
                }
			}
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
	}
}