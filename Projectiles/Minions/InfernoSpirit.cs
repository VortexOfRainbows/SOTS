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
			Color color2 = ColorHelpers.InfernoColorAttemptDegrees(ColorHelpers.soulColorCounter);
			color2.A = 0;
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
		float[] recoilMult = new float[7];
		public override void PostDraw(Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 9; k++)
			{
				float x = Main.rand.NextFloat(-2, 2f);
				float y = Main.rand.NextFloat(-2, 2f);
				Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color, 0f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			texture = Mod.Assets.Request<Texture2D>("Projectiles/Minions/InfernoSpiritBall").Value;
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
						Main.spriteBatch.Draw(texture, Projectile.Center + normal - Main.screenPosition + new Vector2(x, y), null, color, 0f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
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
					distance = Vector2.Distance(target.Center, Projectile.Center);
					bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height);
					bool closeThroughWall = distance < 480f; //30 blocks through walls
					if (distance < minDist && (lineOfSight || closeThroughWall))
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
			if (Projectile.owner == Main.myPlayer)
			{
				Player player = Main.player[Projectile.owner];
				SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
				float speed = 3f;
				if (target != -1)
				{
					NPC toHit = Main.npc[target];
					if (toHit.active)
					{
						Vector2 normal = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(arm * 360f / 7 + modPlayer.orbitalCounter));
							Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + normal * 24, normal * speed + Main.rand.NextVector2Circular(0.2f, 0.2f), ModContent.ProjectileType<InfernoLaser>(), Projectile.damage, 0, Main.myPlayer, target, Main.rand.NextFloat(360));
						return true;
					}
				}
			}
			return false;
		}
		public override void AI() 
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (player.dead || !player.active) 
			{
				player.ClearBuff(ModContent.BuffType<InfernoSpiritAid>());
			}
			if (player.HasBuff(ModContent.BuffType<InfernoSpiritAid>()))
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
				if (total > 0)
					Projectile.ai[1] = modPlayer.orbitalCounter * 1.25f + (ofTotal * 360f / total);
			}
			#region Find target
			float distanceFromTarget = 960f;
			int initialTarget = -1;
			// This code is required if your minion weapon has the targeting feature
			if (player.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Projectile.Center);
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
				Projectile.ai[0]++;
				for (int i = 0; i < 7; i++)
				{
					int ID = foundNpcList[i];
					if (ID != -1)
					{
						Vector2 npcCenter = Main.npc[ID].Center;
						positionAverage += npcCenter;
						amountActive++;
						if (Projectile.ai[0] > 50)
						{
							int arm = i * 3 % 7;
							fireProj(ID, arm);
							recoilMult[arm] = 0f;
						}
					}
				}
				goTo += positionAverage;
				goTo /= amountActive;
				goTo += new Vector2(0, -96) + new Vector2(32 + total * 16, 0).RotatedBy(MathHelper.ToRadians(-Projectile.ai[1]));
				goTo -= Projectile.Center;
				Vector2 newGoTo = goTo.SafeNormalize(Vector2.Zero);
				float speed = 6f + goTo.Length() * 0.005f;
				if (speed > goTo.Length())
					speed = goTo.Length();
				Projectile.velocity = newGoTo * speed;
				if (Projectile.ai[0] > 50)
				{
					SOTSUtils.PlaySound(SoundID.Item20, (int)Projectile.position.X, (int)Projectile.position.Y, 1.1f, -0.2f);
					Projectile.ai[0] = 0;
                }
			}
			else
			{
				Projectile.ai[0] = 0;
				GoIdle();
			}
			Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
			float distanceToIdlePosition = vectorToIdlePosition.Length();
			if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1600f)
			{
				Projectile.Center = idlePosition;
				Projectile.velocity *= 0.1f;
				Projectile.netUpdate = true;
			}
			#endregion

			Lighting.AddLight(Projectile.Center, ColorHelpers.Inferno2.R / 255f, ColorHelpers.Inferno2.G / 255f * ((255 - Projectile.alpha) / 255f), ColorHelpers.Inferno2.B / 255f * ((255 - Projectile.alpha) / 255f));
			MoveAwayFromOthers(true, 0.11f, 2f);
			if (Main.myPlayer == player.whoAmI)
			{
				Projectile.netUpdate = true;
			}
		}
	}
}