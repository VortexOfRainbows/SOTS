using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Otherworld;
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
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		private float aiCounter2
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
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
			Main.projFrames[Projectile.type] = 1;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 36;
			Projectile.height = 36;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.minionSlots = 0f;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 300;
			Projectile.netImportant = true;
		}
		private int shader = 0;
		float eyeReset = 1f;
		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Minions/HoloEye").Value;
			Texture2D texture3 = Mod.Assets.Request<Texture2D>("NPCs/HoloEyePupil").Value;
			Texture2D texture4 = Mod.Assets.Request<Texture2D>("NPCs/HoloEyeFill").Value;
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawOrigin3 = new Vector2(texture3.Width * 0.5f, texture3.Height * 0.5f);
			for (int k = 0; k < 4; k++)
			{
				Vector2 between = new Vector2(10, 0).RotatedBy(Projectile.rotation);
				if (between.Length() > 1.1f)
				{
					between.Normalize();
				}
				else
				{
					between = Vector2.Zero;
				}

				if (k == 0)
					Main.spriteBatch.Draw(texture4, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X), (float)(Projectile.Center.Y - (int)Main.screenPosition.Y)), null, color * 0.5f * ((255 - Projectile.alpha) / 255f), 0f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X), (float)(Projectile.Center.Y - (int)Main.screenPosition.Y)), null, color * ((255 - Projectile.alpha) / 255f), 0f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(texture3, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X), (float)(Projectile.Center.Y - (int)Main.screenPosition.Y)) + between * (6 * eyeReset), null, color * ((255 - Projectile.alpha) / 255f), 0f, drawOrigin3, 0.5f + Projectile.scale - (eyeReset * 0.3f), SpriteEffects.None, 0f);
			}
			Player player = Main.player[Projectile.owner];
			if (shader != 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Player owner = Main.player[Projectile.owner];
			if (shader != 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

				GameShaders.Armor.GetSecondaryShader(shader, owner).Apply(null);
			}
			Texture2D texture1 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/HoloPlatformChainOutline");
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/HoloPlatformChainFill");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 center = Projectile.Center;
			Vector2 toPlayer = Projectile.Center - owner.Center;
			toPlayer = toPlayer.SafeNormalize(new Vector2(1, 0));
			center -= toPlayer * Projectile.width / 1.75f;
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
							Main.spriteBatch.Draw(texture, drawPos + dynamicAddition, null, color * 0.5f * (1f - (Projectile.alpha / 255f)), MathHelper.ToRadians(18 * i - 45) + startingRadians, drawOrigin, Projectile.scale * 0.75f, SpriteEffects.None, 0f);
						Main.spriteBatch.Draw(texture1, drawPos + dynamicAddition, null, color * (1f - (Projectile.alpha / 255f)), MathHelper.ToRadians(18 * i - 45) + startingRadians, drawOrigin, Projectile.scale * 0.75f, SpriteEffects.None, 0f);
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
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			float distanceFromTarget = 1080f;
			Vector2 targetCenter = Projectile.Center;
			bool foundTarget = false;
			hasTarget = true;
			cursor = Main.MouseWorld;
			if (Projectile.timeLeft > 100)
			{
				Projectile.timeLeft = 300;
			}
			if (modPlayer.HoloEyeAutoAttack)
			{
				if (player.HasMinionAttackTargetNPC)
				{
					NPC npc = Main.npc[player.MinionAttackTargetNPC];
					float between = Vector2.Distance(npc.Center, Projectile.Center);
					bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
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
							float between = Vector2.Distance(npc.Center, Projectile.Center);
							bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
							bool inRange = between < distanceFromTarget;
							bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);

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
			if(targetCenter != Projectile.Center)
				return targetCenter;

			hasTarget = false;
			return cursor;	
		}
		public Vector2 target = new Vector2(-1, -1);
		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = owner.GetModPlayer<SOTSPlayer>();
			Vector2 playerCenter = owner.Center - new Vector2(0, 56);
			shader = owner.cHead;
			if (Projectile.owner == Main.myPlayer)
			{
				target = FindTarget();
				Projectile.netUpdate = true;
			}
			aiCounter++;
			if (!owner.active || owner.dead)
			{
				Projectile.Kill();
			}
			Vector2 distanceToOwner = playerCenter - Projectile.Center;
			Vector2 distanceToOwner2 = playerCenter - Projectile.Center;
			Vector2 distanceToTarget = target - Projectile.Center;
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
			Projectile.velocity *= 0;
			if (distanceToOwner2.Length() > 18)
			{
				distanceToOwner.Normalize();
				Projectile.velocity += distanceToOwner * (distanceToOwner2.Length() - 18);
			}
			
			if (aiCounter2 >= 0)
			{
				if (hasTarget)
				{
					Vector2 dynamicAddition = new Vector2(0.05f, 0).RotatedBy(MathHelper.ToRadians(aiCounter * 2));
					Vector2 added = new Vector2(0.5f, 0).RotatedBy(Projectile.rotation);
					Projectile.velocity += added + dynamicAddition;
				}
				else
				{
					distanceToOwner.Normalize();
					Projectile.velocity += distanceToOwner * distanceToOwner2.Length() * 0.1f;
				}
			}

			float overlapVelocity = 0.4f;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				// Fix overlap with other minions
				Projectile other = Main.projectile[i];
				if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width && other.type == Projectile.type)
				{
					if (Projectile.position.X < other.position.X) Projectile.velocity.X -= overlapVelocity;
					else Projectile.velocity.X += overlapVelocity;

					if (Projectile.position.Y < other.position.Y) Projectile.velocity.Y -= overlapVelocity;
					else Projectile.velocity.Y += overlapVelocity;
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
				Vector2 distanceToMouse = cursor - Projectile.Center;
				rotateVector = distanceToMouse;
				rotateVector = new Vector2(4, 0).RotatedBy(rotateVector.ToRotation());
				SOTSUtils.PlaySound(SoundID.Item94, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.75f);
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, rotateVector * 5.75f, ModContent.ProjectileType<DestabilizingBeam>(), Projectile.damage, 1f, owner.whoAmI, 0, -1);
				}
				eyeReset = -0.9f;
				aiCounter2 = -60;
			}
			Projectile.rotation = rotateVector.ToRotation();
			if(hasTarget && modPlayer.HoloEyeAutoAttack)
			{
				aiCounter2++;
				if (aiCounter2 >= 54 && distanceToTarget2 < 1080)
				{
					SOTSUtils.PlaySound(SoundID.Item96, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.5f);
					if (Main.myPlayer == Projectile.owner)
					{
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, rotateVector * 5.75f, ModContent.ProjectileType<CodeBurst>(), Projectile.damage, 1f, owner.whoAmI, 0, -1);
					}
					eyeReset = -0.8f;
					aiCounter2 = 0;
				}
			}
			else if(aiCounter2 > 0)
            {
				aiCounter2 = 0;
            }
			Projectile.frame = frame;
		}
	}
}