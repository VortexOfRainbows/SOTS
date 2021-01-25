using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.Projectiles.Minions
{
	public class HoloEye : ModProjectile
	{
		bool hasTarget = false;
		private float aiCounter
		{
			get => projectile.ai[0];
			set => projectile.ai[0] = value;
		}
		private float aiCounter2
		{
			get => projectile.ai[1];
			set => projectile.ai[1] = value;
		}
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(target.X);
			writer.Write(target.Y);
			writer.Write(cursor.X);
			writer.Write(cursor.Y);
			writer.Write(hasTarget);
			writer.Write(eyeReset);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			target.X = reader.ReadSingle();
			target.Y = reader.ReadSingle();
			cursor.X = reader.ReadSingle();
			cursor.Y = reader.ReadSingle();
			hasTarget = reader.ReadBoolean();
			eyeReset = reader.ReadSingle();
		}
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holo Eye");
			Main.projFrames[projectile.type] = 1;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
		}
		public sealed override void SetDefaults()
		{
			projectile.width = 36;
			projectile.height = 36;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.minion = true;
			projectile.minionSlots = 0f;
			projectile.penetrate = -1;
			projectile.timeLeft = 300;
			projectile.netImportant = true;
		}
		private int shader = 0;
		float eyeReset = 1f;
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = mod.GetTexture("Projectiles/Minions/HoloEye");
			Texture2D texture3 = mod.GetTexture("Projectiles/Minions/HoloEyePupil");
			Texture2D texture4 = mod.GetTexture("Projectiles/Minions/HoloEyeFill");
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawOrigin3 = new Vector2(texture3.Width * 0.5f, texture3.Height * 0.5f);
			for (int k = 0; k < 4; k++)
			{
				Vector2 between = new Vector2(10, 0).RotatedBy(projectile.rotation);
				if (between.Length() > 1.1f)
				{
					between.Normalize();
				}
				else
				{
					between = Vector2.Zero;
				}

				if (k == 0)
					Main.spriteBatch.Draw(texture4, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X), (float)(projectile.Center.Y - (int)Main.screenPosition.Y)), null, color * 0.5f * ((255 - projectile.alpha) / 255f), 0f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X), (float)(projectile.Center.Y - (int)Main.screenPosition.Y)), null, color * ((255 - projectile.alpha) / 255f), 0f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture3, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X), (float)(projectile.Center.Y - (int)Main.screenPosition.Y)) + between * (6 * eyeReset), null, color * ((255 - projectile.alpha) / 255f), 0f, drawOrigin3, 0.5f + projectile.scale - (eyeReset * 0.3f), SpriteEffects.None, 0f);
			}
			Player player = Main.player[projectile.owner];
			if (shader != 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Player owner = Main.player[projectile.owner];
			if (shader != 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

				GameShaders.Armor.GetSecondaryShader(shader, owner).Apply(null);
			}
			Texture2D texture1 = ModContent.GetTexture("SOTS/Projectiles/Minions/HoloPlatformChainOutline");
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Minions/HoloPlatformChainFill");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 center = projectile.Center;
			Vector2 toPlayer = projectile.Center - owner.Center;
			toPlayer = toPlayer.SafeNormalize(new Vector2(1, 0));
			center -= toPlayer * projectile.width / 1.75f;
			if (owner.active && !owner.dead)
			{
				Vector2 distanceToOwner = center - owner.Center;
				float radius = distanceToOwner.Length() / 2;
				if (distanceToOwner.X < 0)
				{
					radius = -radius;
				}
				Vector2 centerOfCircle = owner.Center + distanceToOwner/2;
				float startingRadians = distanceToOwner.ToRotation();
				Color color = new Color(100, 100, 100, 0);
				for(int i = 9; i > 0; i--)
				{
					Vector2 rotationPos = new Vector2(radius, 0).RotatedBy(MathHelper.ToRadians(18 * i));
					rotationPos.Y /= 5f;
					rotationPos = rotationPos.RotatedBy(startingRadians);
					Vector2 pos = rotationPos += centerOfCircle;
					Vector2 dynamicAddition = new Vector2(0.75f + 2f * rotationPos.SafeNormalize(Vector2.Zero).Y, 0).RotatedBy(MathHelper.ToRadians(i * 36 + aiCounter * 2));
					Vector2 drawPos = pos - Main.screenPosition;
					for (int k = 0; k < 5; k++)
					{
						if (k == 0)
							spriteBatch.Draw(texture, drawPos + dynamicAddition, null, color * 0.5f * (1f - (projectile.alpha / 255f)), MathHelper.ToRadians(18 * i - 45) + startingRadians, drawOrigin, projectile.scale * 0.75f, SpriteEffects.None, 0f);

						spriteBatch.Draw(texture1, drawPos + dynamicAddition, null, color * (1f - (projectile.alpha / 255f)), MathHelper.ToRadians(18 * i - 45) + startingRadians, drawOrigin, projectile.scale * 0.75f, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		int frame = 0;
		Vector2 rotateVector = new Vector2(4, 0);
		Vector2 cursor = Vector2.Zero;
		public Vector2 FindTarget()
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			float distanceFromTarget = 1080f;
			Vector2 targetCenter = projectile.Center;
			bool foundTarget = false;
			hasTarget = true;
			cursor = Main.MouseWorld;
			if (projectile.timeLeft > 100)
			{
				projectile.timeLeft = 300;
			}
			if (modPlayer.HoloEyeAutoAttack)
			{
				if (player.HasMinionAttackTargetNPC)
				{
					NPC npc = Main.npc[player.MinionAttackTargetNPC];
					float between = Vector2.Distance(npc.Center, projectile.Center);
					bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
					if (between < distanceFromTarget && lineOfSight)
					{
						distanceFromTarget = between;
						targetCenter = npc.Center + npc.velocity * 2;
						foundTarget = true;
					}
				}
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
							}
						}
					}
				}
			}
			if(targetCenter != projectile.Center)
				return targetCenter;

			hasTarget = false;
			return cursor;	
		}
		public Vector2 target = new Vector2(-1, -1);
		public override void AI()
		{
			Player owner = Main.player[projectile.owner];
			SOTSPlayer modPlayer = owner.GetModPlayer<SOTSPlayer>();
			Vector2 playerCenter = owner.Center - new Vector2(0, 56);
			shader = owner.cHead;
			if (projectile.owner == Main.myPlayer)
			{
				target = FindTarget();
				projectile.netUpdate = true;
			}
			aiCounter++;
			if (!owner.active || owner.dead)
			{
				projectile.Kill();
			}
			Vector2 distanceToOwner = playerCenter - projectile.Center;
			Vector2 distanceToOwner2 = playerCenter - projectile.Center;
			Vector2 distanceToTarget = target - projectile.Center;
			float distanceToTarget2 = distanceToTarget.Length();

			distanceToTarget = distanceToTarget.SafeNormalize(Vector2.Zero);
			if(aiCounter2 >= 0)
			{
				rotateVector += distanceToTarget * 1;
			}
			else
            {
				aiCounter2++;
            }
			rotateVector = new Vector2(4, 0).RotatedBy(rotateVector.ToRotation());
			projectile.velocity *= 0;
			if (distanceToOwner2.Length() > 18)
			{
				distanceToOwner.Normalize();
				projectile.velocity += distanceToOwner * (distanceToOwner2.Length() - 18);
			}
			
			if (aiCounter2 >= 0)
			{
				if (hasTarget)
				{
					Vector2 dynamicAddition = new Vector2(0.05f, 0).RotatedBy(MathHelper.ToRadians(aiCounter * 2));
					Vector2 added = new Vector2(0.5f, 0).RotatedBy(projectile.rotation);
					projectile.velocity += added + dynamicAddition;
				}
				else
				{
					distanceToOwner.Normalize();
					projectile.velocity += distanceToOwner * distanceToOwner2.Length() * 0.1f;
				}
			}

			float overlapVelocity = 0.4f;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				// Fix overlap with other minions
				Projectile other = Main.projectile[i];
				if (i != projectile.whoAmI && other.active && other.owner == projectile.owner && Math.Abs(projectile.position.X - other.position.X) + Math.Abs(projectile.position.Y - other.position.Y) < projectile.width && other.type == projectile.type)
				{
					if (projectile.position.X < other.position.X) projectile.velocity.X -= overlapVelocity;
					else projectile.velocity.X += overlapVelocity;

					if (projectile.position.Y < other.position.Y) projectile.velocity.Y -= overlapVelocity;
					else projectile.velocity.Y += overlapVelocity;
				}
			}
			if(eyeReset < 1)
            {
				eyeReset += 0.05f;
            }
			else
            {
				eyeReset = 1f;
			}
			if (modPlayer.HoloEyeAttack && aiCounter2 >= 0)
			{
				VoidPlayer voidPlayer = VoidPlayer.ModPlayer(owner);
				voidPlayer.voidMeter -= 6 * voidPlayer.voidCost;
				modPlayer.HoloEyeAttack = false;
				Vector2 distanceToMouse = cursor - projectile.Center;
				rotateVector = distanceToMouse;
				rotateVector = new Vector2(4, 0).RotatedBy(rotateVector.ToRotation());
				Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 94, 0.75f);
				if (Main.myPlayer == projectile.owner)
				{
					Projectile.NewProjectile(projectile.Center, rotateVector * 5.75f, mod.ProjectileType("DestabilizingBeam"), projectile.damage, 1f, owner.whoAmI, 0, -1);
				}
				eyeReset = -0.9f;
				aiCounter2 = -60;
			}
			projectile.rotation = rotateVector.ToRotation();
			if(hasTarget && modPlayer.HoloEyeAutoAttack)
			{
				aiCounter2++;
				if (aiCounter2 >= 54 && distanceToTarget2 < 1080)
				{
					Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 96, 0.5f);
					if (Main.myPlayer == projectile.owner)
					{
						Projectile.NewProjectile(projectile.Center, rotateVector * 5.75f, mod.ProjectileType("CodeBurst"), projectile.damage, 1f, owner.whoAmI, 0, -1);
					}
					eyeReset = -0.8f;
					aiCounter2 = 0;
				}
			}
			else if(aiCounter2 > 0)
            {
				aiCounter2 = 0;
            }
			projectile.frame = frame;
		}
	}
}