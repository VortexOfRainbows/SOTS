using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Buffs.MinionBuffs;
using SOTS.Void;
using SOTS.Dusts;
using System.IO;

namespace SOTS.Projectiles.Earth.Glowmoth
{
	public class MothMinion : ModProjectile
	{
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(mousePosition.X);
			writer.Write(mousePosition.Y);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			mousePosition.X = reader.ReadSingle();
			mousePosition.Y = reader.ReadSingle();
		}
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lumina Moth");
			Main.projFrames[Projectile.type] = 1;
			//ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true; //We don't want the special right click since the minions already special target with right click

			Main.projPet[Projectile.type] = true; 
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0	; 
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; 
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; 
		}

		public sealed override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.minionSlots = 0.333f;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.netImportant = true;
			Projectile.localNPCHitCooldown = 40;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			hitbox.Width = 40;
			hitbox.Height = 40;
			hitbox.X -= 10;
			hitbox.Y -= 10;
        }
        public override bool? CanCutTiles()
		{
			return false;
		}
		public override bool MinionContactDamage()
		{
			return true;
		}
		public Vector2 mousePosition = Vector2.Zero;
		public bool runOnce = true;
		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];

			if(runOnce)
            {
				Projectile.ai[0] = Main.rand.Next(36) * 10;
				Projectile.ai[1] = Main.rand.NextFloat(0.2f, 1.2f);
				runOnce = false;
            }
			if (!CheckActive(owner))
			{
				return;
			}
			Projectile.ai[0] += 0.5f;
			Vector2 circularRandomizer = new Vector2(36 * Projectile.ai[1] * (float)Math.Sin(MathHelper.ToRadians(Projectile.identity * 20 + Projectile.ai[0] * 4f)), 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[0]));
			GeneralBehavior(owner, circularRandomizer, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition);
			int targetID = Common.GlobalNPCs.SOTSNPCs.FindTarget_Basic(Projectile.Center, out float distanceFromTarget, 600, null, true);
			bool foundTarget = false;
			Vector2 targetCenter = Projectile.Center;
			if(targetID != -1)
            {
				NPC target = Main.npc[targetID];
				targetCenter = target.Center;
				foundTarget = true;
			}
			if(Projectile.owner == Main.myPlayer) //This makes sure it only responds to if the projectile owner right clicks
			{
				if (Main.mouseRight)
				{
					mousePosition.X = Main.MouseWorld.X; 
					mousePosition.Y = Main.MouseWorld.Y;
					if(SOTSWorld.GlobalCounter % 6 == 0)
						Projectile.netUpdate = true;
				}
				else
				{
					if(mousePosition != Vector2.Zero)
						Projectile.netUpdate = true;
					mousePosition = Vector2.Zero;
                }
			}
			if(mousePosition != Vector2.Zero)
            {
				targetCenter = mousePosition;
				distanceFromTarget = Vector2.Distance(targetCenter, Projectile.Center);
				foundTarget = true;
            }
			Movement(foundTarget, distanceFromTarget, targetCenter + circularRandomizer, distanceToIdlePosition, vectorToIdlePosition);
			Visuals();
		}
        private bool CheckActive(Player owner)
		{
			if (owner.dead || !owner.active)
			{
				owner.ClearBuff(ModContent.BuffType<NightIlluminatorBuff>());
				return false;
			}
			if (owner.HasBuff(ModContent.BuffType<NightIlluminatorBuff>()))
			{
				Projectile.timeLeft = 2;
			}
			return true;
		}
		private void GeneralBehavior(Player owner, Vector2 offset, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition)
		{
			Vector2 idlePosition = owner.Center + offset;
			idlePosition.Y -= 56f;
			vectorToIdlePosition = idlePosition - Projectile.Center;
			distanceToIdlePosition = vectorToIdlePosition.Length();
			if (Main.myPlayer == owner.whoAmI && distanceToIdlePosition > 2000f)
			{
				Projectile.position = idlePosition;
				Projectile.velocity *= 0.1f;
				Projectile.netUpdate = true;
			}
			float overlapVelocity = 0.1f;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile other = Main.projectile[i];
				if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Type == other.type && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width)
				{
					if (Projectile.position.X < other.position.X)
					{
						Projectile.velocity.X -= overlapVelocity;
					}
					else
					{
						Projectile.velocity.X += overlapVelocity;
					}
					if (Projectile.position.Y < other.position.Y)
					{
						Projectile.velocity.Y -= overlapVelocity;
					}
					else
					{
						Projectile.velocity.Y += overlapVelocity;
					}
				}
			}
		}
		private void Movement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition)
		{
			// Default movement parameters (here for attacking)
			float speed = 16f;
			float inertia = 14f;

			if (foundTarget)
			{
				// Minion has a target: attack (here, fly towards the enemy)
				if (distanceFromTarget > 46f)
				{
					// The immediate range around the target (so it doesn't latch onto it when close)
					Vector2 direction = targetCenter - Projectile.Center;
					direction.Normalize();
					direction *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
					if (WorldgenHelpers.SOTSWorldgenHelper.TrueTileSolid((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16))
					{
						if (Projectile.velocity.Length() > 8f)
							Projectile.velocity *= 0.95f;
                    }
				}
				else
                {
					if (!WorldgenHelpers.SOTSWorldgenHelper.TrueTileSolid((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16))
                    {
						Projectile.velocity *= 1.14f;
					}
					else
					{
						if(Projectile.velocity.Length() > 6f)
							Projectile.velocity *= 0.97f;
					}
					Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(7) * Math.Sin(MathHelper.ToRadians(Projectile.ai[0] * 4f)));
					if (!Main.rand.NextBool(3))
					{
						Color color2 = VoidPlayer.VibrantColorAttempt(Projectile.ai[0] % 180, true);
						Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
						dust.color = color2;
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale = 0.6f * dust.scale + 0.9f;
						dust.alpha = Projectile.alpha;
						dust.velocity *= 0.5f;
						dust.velocity += new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[0])) + Projectile.velocity * 0.6f;
					}
				}
			}
			else
			{
				if (Projectile.position.X > vectorToIdlePosition.X)
                {
					Projectile.spriteDirection = 1;
                }
				else
                {
					Projectile.spriteDirection = -1;
				}

				float clamp = MathHelper.Clamp(distanceToIdlePosition / 800f, 0f, 1f);
				speed = MathHelper.Lerp(6, 16, clamp);
				inertia = MathHelper.Lerp(40, 60, clamp);

				if (distanceToIdlePosition > 30f)
				{
					vectorToIdlePosition.Normalize();
					vectorToIdlePosition *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
				}
				else if (Projectile.velocity == Vector2.Zero)
				{
					// If there is a case where it's not moving at all, give it a little "poke"
					Projectile.velocity.X = -0.15f;
					Projectile.velocity.Y = -0.05f;
				}
			}
			SpinCounter += Projectile.velocity.Length() * Math.Sign(Projectile.velocity.X);
		}
		int graduallyBringInTrail = 0;
		private void Visuals()
		{
			Projectile.rotation = Projectile.velocity.X * 0.05f;

			// This is a simple "loop through all frames from top to bottom" animation
			int frameSpeed = 5;
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= frameSpeed)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}
			if(Main.rand.NextBool(10))
			{
				Color color2 = VoidPlayer.VibrantColorAttempt(Projectile.ai[0] % 180, true);
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = 0.35f * dust.scale + 0.5f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 0.4f;
				dust.velocity -= new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[0]));
			}
			if(graduallyBringInTrail < Projectile.oldPos.Length)
				graduallyBringInTrail++;
			Lighting.AddLight(Projectile.Center, new Color(172, 252, 173).ToVector3() * 0.25f);
		}
		public float SpinCounter = 0; //since this variable is only used for visuals, it doesn't need to be synced in multiplayer
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Texture2D texture2 = ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/Glowmoth/MothMinionTrail").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			Vector2 trailOrigin = new Vector2(2, texture2.Height * 0.5f);
			Rectangle yFrame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
			float scaleOffSpeed = MathHelper.Clamp(Projectile.velocity.Length() / 15f, 0, 1);
			for (int i = 0; i < 6; i++)
			{
				Vector2 circular = new Vector2(2 + 18 * scaleOffSpeed * scaleOffSpeed, 0).RotatedBy(MathHelper.ToRadians(i * 60 + SpinCounter * 0.3f));
				Color color = new Color(80, 100, 85, 0) * (1.0f - scaleOffSpeed * 0.6f);
				Main.EntitySpriteDraw(texture, Projectile.Center + circular - Main.screenPosition, yFrame, color, Projectile.rotation, drawOrigin, Projectile.scale * (0.7f + 0.3f * scaleOffSpeed), Projectile.velocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			}
			for (int k = 1; k < graduallyBringInTrail - 1; k++)
			{
				for (int j = 0; j < 3; j++)
				{
					float scaleMult = ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * (1.0f - j * 0.2f);
					Vector2 toNextPosition = Projectile.oldPos[k] - Projectile.oldPos[k + 1];
					Vector2 drawPos = Projectile.oldPos[k] + drawOrigin - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
					Color color = new Color(60, 70, 60, 0) * scaleMult * (0.5f + scaleOffSpeed * 0.5f);
					Main.EntitySpriteDraw(texture2, drawPos + toNextPosition.SafeNormalize(Vector2.Zero) * 3 * j, null, color, toNextPosition.ToRotation(), trailOrigin, new Vector2(toNextPosition.Length() / texture2.Width * 2, Projectile.scale * (1.5f - 0.5f * scaleOffSpeed) * scaleMult), SpriteEffects.None, 0);
				}
			}
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, yFrame, Color.White, Projectile.rotation, drawOrigin, Projectile.scale * 0.95f, Projectile.velocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			return false;
		}
	}
}