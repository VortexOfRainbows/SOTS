using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using SOTS.Void;
using SOTS.Buffs;
using System.Linq;
using SOTS.Projectiles.Inferno;
using SOTS.Buffs.MinionBuffs;

namespace SOTS.Projectiles.Minions
{
	public class InfernoSpirit : SpiritMinion
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Inferno Spirit");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
        public sealed override void SetDefaults()
		{
			projectile.width = 34;
			projectile.height = 34;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.ignoreWater = true;
			projectile.localNPCHitCooldown = 10;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Color color2 = VoidPlayer.InfernoColorAttemptDegrees(VoidPlayer.soulColorCounter);
			color2.A = 0;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(color2) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		float[] recoilMult = new float[7];
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 9; k++)
			{
				float x = Main.rand.NextFloat(-2, 2f);
				float y = Main.rand.NextFloat(-2, 2f);
				Main.spriteBatch.Draw(texture, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color, 0f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			texture = mod.GetTexture("Projectiles/Minions/InfernoSpiritBall");
			drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			if(!runOnce)
            {
				for (int i = 0; i < 7; i++)
				{
					Vector2 normal = new Vector2(24 + 20 * recoilMult[i], 0).RotatedBy(MathHelper.ToRadians(i * 360f / 7 + modPlayer.orbitalCounter));
					for (int k = 0; k < 5; k++)
					{
						float x = Main.rand.NextFloat(-2, 2f);
						float y = Main.rand.NextFloat(-2, 2f);
						Main.spriteBatch.Draw(texture, projectile.Center + normal - Main.screenPosition + new Vector2(x, y), null, color, 0f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
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
		public int[] UpdateTargets(int initialTarget = -1)
		{
			for (int i = 0; i < 7; i++)
			{
				if (runOnce)
				{
					recoilMult[i] = 1;
				}
				else
				{
					recoilMult[i] = MathHelper.Lerp(recoilMult[i], 1f, 0.06f);
				}
			}
			runOnce = false;
			int[] foundNpcList = { -1, -1, -1, -1, -1, -1, -1 };
			if (initialTarget != -1)
			{
				foundNpcList[0] = initialTarget;
			}
			else
			{
				foundNpcList[0] = findTargets(foundNpcList);
			}
			for(int i = 1; i < 7; i++)
			{
				foundNpcList[i] = findTargets(foundNpcList);
			}
			return foundNpcList;
		}
		public int findTargets(int[] npcList)
		{
			float minDist = 800;
			int target2 = -1;
			float distance;
			for (int i = 0; i < Main.npc.Length - 1; i++)
			{
				NPC target = Main.npc[i];
				if (target.CanBeChasedBy() && !npcList.Contains(target.whoAmI))
				{
					distance = Vector2.Distance(target.Center, projectile.Center);
					if (distance < minDist)
					{
						minDist = distance;
						target2 = i;
					}
				}
			}
			return target2;
		}
		public bool fireProj(int target, int arm)
		{
			if (projectile.owner == Main.myPlayer)
			{
				Player player = Main.player[projectile.owner];
				SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
				float speed = 3f;
				if (target != -1)
				{
					NPC toHit = Main.npc[target];
					if (toHit.active)
					{
						Vector2 normal = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(arm * 360f / 7 + modPlayer.orbitalCounter));
							Projectile.NewProjectile(projectile.Center + normal * 24, normal * speed + Main.rand.NextVector2Circular(0.2f, 0.2f), ModContent.ProjectileType<InfernoLaser>(), projectile.damage, 0, Main.myPlayer, target, Main.rand.NextFloat(360));
						return true;
					}
				}
			}
			return false;
		}
		public override void AI() 
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (player.dead || !player.active) 
			{
				player.ClearBuff(ModContent.BuffType<InfernoSpiritAid>());
			}
			if (player.HasBuff(ModContent.BuffType<InfernoSpiritAid>()))
			{
				projectile.timeLeft = 6;
			}
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
			{
				if (total > 0)
					projectile.ai[1] = modPlayer.orbitalCounter * 1.25f + (ofTotal * 360f / total);
			}
			#region Find target
			float distanceFromTarget = 960f;
			int initialTarget = -1;
			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, projectile.Center);
				if (between < distanceFromTarget) 
				{
					initialTarget = npc.whoAmI;
				}
			}
			int[] foundNpcList = UpdateTargets(initialTarget);
			initialTarget = foundNpcList[0];
			#endregion
			#region Movement
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 96f;
			if (initialTarget != -1)
			{
				Vector2 goTo = idlePosition * 2f;
				Vector2 positionAverage = Vector2.Zero;
				float amountActive = 2f;
				projectile.ai[0]++;
				for (int i = 0; i < 7; i++)
				{
					int ID = foundNpcList[i];
					if (ID != -1)
					{
						Vector2 npcCenter = Main.npc[ID].Center;
						positionAverage += npcCenter;
						amountActive++;
						if (projectile.ai[0] > 50)
						{
							int arm = i * 3 % 7;
							fireProj(ID, arm);
							recoilMult[arm] = 0f;
						}
					}
				}
				goTo += positionAverage;
				goTo /= amountActive;
				goTo += new Vector2(0, -96) + new Vector2(32 + total * 16, 0).RotatedBy(MathHelper.ToRadians(-projectile.ai[1]));
				goTo -= projectile.Center;
				Vector2 newGoTo = goTo.SafeNormalize(Vector2.Zero);
				float speed = 6f + goTo.Length() * 0.005f;
				if (speed > goTo.Length())
					speed = goTo.Length();
				projectile.velocity = newGoTo * speed;
				if (projectile.ai[0] > 50)
				{
					Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 20, 1.1f, -0.2f);
					projectile.ai[0] = 0;
                }
			}
			else
			{
				projectile.ai[0] = 0;
				GoIdle();
			}
			Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
			float distanceToIdlePosition = vectorToIdlePosition.Length();
			if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1600f)
			{
				projectile.Center = idlePosition;
				projectile.velocity *= 0.1f;
				projectile.netUpdate = true;
			}
			#endregion

			Lighting.AddLight(projectile.Center, VoidPlayer.Inferno2.R / 255f, VoidPlayer.Inferno2.G / 255f * ((255 - projectile.alpha) / 255f), VoidPlayer.Inferno2.B / 255f * ((255 - projectile.alpha) / 255f));
			MoveAwayFromOthers(true, 0.11f, 2f);
			if (Main.myPlayer == player.whoAmI)
			{
				projectile.netUpdate = true;
			}
		}
	}
}