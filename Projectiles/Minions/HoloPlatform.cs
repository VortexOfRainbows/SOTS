using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using SOTS.Items.Otherworld.FromChests;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SOTS.Projectiles.Minions
{
	public class HoloPlatform : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holo Platform");
		}
		public sealed override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 12;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 7200;
			projectile.sentry = false;
			projectile.netImportant = true;
		}
		private int shader = 0;
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Player player = Main.player[projectile.owner];
			if (shader != 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

				GameShaders.Armor.GetSecondaryShader(shader, player).Apply(null);
			}
			PlatformPlayer modPlayer = player.GetModPlayer<PlatformPlayer>();
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Minions/HoloPlatformChainOutline");
			Texture2D texture1 = ModContent.GetTexture("SOTS/Projectiles/Minions/HoloPlatformChainFill");
			Texture2D texture2 = ModContent.GetTexture("SOTS/Projectiles/Minions/HoloPlatformOutline");
			Texture2D texture3 = ModContent.GetTexture("SOTS/Projectiles/Minions/HoloPlatformFill");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Player owner = Main.player[projectile.owner];
			Color color = new Color(110, 110, 110, 0);
			if (owner.active && !owner.dead && !modPlayer.hideChains)
			{
				Vector2 distanceToOwner = new Vector2(projectile.Center.X, projectile.position.Y + projectile.height + 6) - owner.Center;
				float radius = distanceToOwner.Length() / 2;
				if (distanceToOwner.X < 0)
				{
					radius = -radius;
				}
				Vector2 centerOfCircle = owner.Center + distanceToOwner/2;
				float startingRadians = distanceToOwner.ToRotation();
				for(int i = 19; i > 0; i--)
				{
					Vector2 rotationPos = new Vector2(radius, 0).RotatedBy(MathHelper.ToRadians(9 * i));
					rotationPos.Y /= 3.5f;
					rotationPos = rotationPos.RotatedBy(startingRadians);
					Vector2 pos = rotationPos += centerOfCircle;
					Vector2 dynamicAddition = new Vector2(2.5f, 0).RotatedBy(MathHelper.ToRadians(i * 36 + counter * 2));
					Vector2 drawPos = pos - Main.screenPosition;
					for (int k = 0; k < 5; k++)
					{
						if (k == 0)
							spriteBatch.Draw(texture1, drawPos + dynamicAddition, null, color * 0.5f, MathHelper.ToRadians(18 * i - 45) + startingRadians, drawOrigin, projectile.scale * 0.75f, SpriteEffects.None, 0f);

						spriteBatch.Draw(texture, drawPos + dynamicAddition, null, color * (1f - (projectile.alpha / 255f)), MathHelper.ToRadians(18 * i - 45) + startingRadians, drawOrigin, projectile.scale * 0.75f, SpriteEffects.None, 0f);
					}
				}
			}

			Vector2 drawPos2 = projectile.position - Main.screenPosition;
			for (int k = 0; k < 5; k++)
			{
				if (k == 0)
					spriteBatch.Draw(texture3, drawPos2 + new Vector2(0, 2), new Rectangle(0, 0, 8, 12), color * 0.5f, 0, Vector2.Zero, projectile.scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture2, drawPos2 + new Vector2(0, 2), new Rectangle(0, 0, 8, 12), color * (1f - (projectile.alpha / 255f)), 0f, Vector2.Zero, projectile.scale, SpriteEffects.None, 0f);
			}
			for(int i = 1; i < projectile.width/8 - 1; i++)
			{
				for (int k = 0; k < 5; k++)
				{
					if (k == 0)
						spriteBatch.Draw(texture3, drawPos2 + new Vector2(8 * i, 2), new Rectangle(10, 0, 8, 12), color * 0.5f, 0, Vector2.Zero, projectile.scale, SpriteEffects.None, 0f);

					Main.spriteBatch.Draw(texture2, drawPos2 + new Vector2(8 * i, 2), new Rectangle(10, 0, 8, 12), color * (1f - (projectile.alpha / 255f)), 0f, Vector2.Zero, projectile.scale, SpriteEffects.None, 0f);
				}
			}
			for (int k = 0; k < 5; k++)
			{
				if (k == 0)
					spriteBatch.Draw(texture3, drawPos2 + new Vector2(projectile.width - 8, 2), new Rectangle(20, 0, 8, 12), color * 0.5f, 0, Vector2.Zero, projectile.scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture2, drawPos2 + new Vector2(projectile.width - 8, 2), new Rectangle(20, 0, 8, 12), color * (1f - (projectile.alpha / 255f)), 0f, Vector2.Zero, projectile.scale, SpriteEffects.None, 0f);
			}

			return false;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			if (shader != 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
			}
			base.PostDraw(spriteBatch, lightColor);
        }
        Vector2 rotateVector = new Vector2(4, 0);
		Vector2 positionToPlayer;
		Vector2? relativeToMouse;
		bool runOnce = true;
		bool clickedAlready = false;
		int counter = 0;
		int SentryID = -1;
		int SentryType = -1;
		bool runEffectOnce = false; 
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(SentryID);
			writer.Write(SentryType);
			writer.Write(positionToPlayer.X);
			writer.Write(positionToPlayer.Y);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			SentryID = reader.ReadInt32();
			SentryType = reader.ReadInt32();
			positionToPlayer.X = reader.ReadSingle();
			positionToPlayer.Y = reader.ReadSingle();
			base.ReceiveExtraAI(reader);
		}
		public override bool PreAI()
		{
			if (projectile.owner == Main.myPlayer)
				projectile.netUpdate = true;
			counter++;
			Player player = Main.player[projectile.owner];
			if (runOnce)
            {
				positionToPlayer = projectile.Center - player.Center;
				runOnce = false;
			}
			shader = player.cBack;
			return true;
        }
		public void UpdatePartnerPos()
		{
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile partner = Main.projectile[i];
				if (partner.type == projectile.type && partner.ai[0] == projectile.ai[0] && partner.ai[1] == -projectile.ai[1] && partner.owner == projectile.owner)
				{
					HoloPlatform proj = (HoloPlatform)partner.modProjectile;
					proj.positionToPlayer = new Vector2(-positionToPlayer.X, positionToPlayer.Y);
					partner.netUpdate = true;
				}
			}
		}
		public bool UpdateWithMouse()
        {
			Player player = Main.player[projectile.owner];
			if(player.whoAmI == Main.myPlayer)
            {
				Vector2 mousePos = Main.MouseWorld;
				Rectangle pos = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
				if (Main.mouseRight)
				{
					if (pos.Contains(new Point((int)mousePos.X, (int)mousePos.Y)) || relativeToMouse != null)
					{
						if(!clickedAlready)
						{
							if (relativeToMouse == null)
								relativeToMouse = projectile.position - Main.MouseWorld;
							else
								projectile.position = Main.MouseWorld + (Vector2)relativeToMouse;
							positionToPlayer = projectile.Center - player.Center;
							UpdatePartnerPos();
						}
						return false;
					}
					else
                    {
						clickedAlready = true;
						relativeToMouse = null;
					}
				}
				else
				{
					clickedAlready = false;
					relativeToMouse = null;
				}
			}
			return true;
        }
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override void AI()
		{
			Player owner = Main.player[projectile.owner];
			if (!owner.active || owner.dead)
			{
				projectile.Kill();
			}
			if(UpdateWithMouse())
			{
				projectile.Center = positionToPlayer + owner.Center;
				float overlapVelocity = 1f;
				if(!Main.mouseRight && owner.whoAmI == Main.myPlayer)
					for (int i = 0; i < Main.maxNPCs; i++)
					{
						// Fix overlap with other minions
						Projectile other = Main.projectile[i];
						if (i != projectile.whoAmI && other.active && other.owner == projectile.owner && Math.Abs(projectile.position.X - other.position.X) + Math.Abs(projectile.position.Y - other.position.Y) < projectile.width && other.type == projectile.type)
						{
							if (projectile.position.X < other.position.X) projectile.position.X -= overlapVelocity;
							else projectile.position.X += overlapVelocity;

							if (projectile.position.Y < other.position.Y) projectile.position.Y -= overlapVelocity;
							else projectile.position.Y += overlapVelocity;
							positionToPlayer = projectile.Center - owner.Center;
							UpdatePartnerPos();
						}
					}
			}
			if(SentryID == -1)
            {
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile sentry = Main.projectile[i];
					if (sentry.active && sentry.owner == projectile.owner && sentry.sentry == true && sentry.timeLeft >= Projectile.SentryLifeTime - 6 && sentry.Center.X < projectile.position.X + projectile.width && sentry.Center.X > projectile.position.X && sentry.whoAmI != projectile.whoAmI && sentry.type != projectile.type)
					{
						SentryID = sentry.whoAmI;
						SentryType = sentry.type;
						runEffectOnce = true;
						break;
					}
				}
			}
			else
			{
				bool isPartners = false;
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile partner = Main.projectile[i];
					if (partner.type == projectile.type && partner.ai[0] == projectile.ai[0] && partner.ai[1] == -projectile.ai[1] && partner.owner == projectile.owner)
					{
						HoloPlatform proj = (HoloPlatform)partner.modProjectile;
						if (proj.SentryID == SentryID)
							isPartners = true;
					}
				}

				Projectile sentry = Main.projectile[SentryID];
				if (sentry.active && sentry.owner == projectile.owner && sentry.sentry == true && sentry.whoAmI != projectile.whoAmI && sentry.type != projectile.type && SentryType == sentry.type && !isPartners)
				{
					projectile.width = (1 + sentry.width / 8) * 8;
					if (sentry.timeLeft < 6 || projectile.timeLeft <= 4)
						sentry.timeLeft = 6;
					sentry.position = new Vector2(projectile.Center.X - sentry.width/2, projectile.position.Y - sentry.height);
					sentry.velocity *= 0f;
					sentry.tileCollide = false;
					if(counter % 6 == 0)
						sentry.netUpdate = true;
					if (runEffectOnce)
					{
						runEffectOnce = false;
						DustSpawn(sentry);
					}
				}
				else
                {
					SentryType = -1;
					projectile.width = 40;
					SentryID = -1;
				}
			}
			projectile.rotation = rotateVector.ToRotation();
		}
        public void DustSpawn(Projectile proj)
        {
			for (int i = 0; i < 50; i++)
			{
				int dust2 = Dust.NewDust(proj.position - new Vector2(4, 4), proj.width, proj.height, DustID.Electric, 0, 0, projectile.alpha, default, 1.25f);
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity *= 2f;
			}
		}
	}
}