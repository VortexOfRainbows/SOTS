using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using SOTS.Items.Otherworld.FromChests;
using System;
using System.Collections.Generic;
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
			Projectile.width = 40;
			Projectile.height = 12;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 7200;
			Projectile.sentry = false;
			Projectile.netImportant = true;
			Projectile.ignoreWater = true;
			Projectile.hide = true;
		}
        private int shader = 0;
		public void Draw(SpriteBatch spriteBatch)
		{
			Player player = Main.player[Projectile.owner];
			if (shader != 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				GameShaders.Armor.GetSecondaryShader(shader, player).Apply(null);
			}
			PlatformPlayer modPlayer = player.GetModPlayer<PlatformPlayer>();
			Texture2D textureChainOutline = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/HoloPlatformChainOutline");
			Texture2D textureChainFill = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/HoloPlatformChainFill");
			Texture2D texturePlatformOutline;
			Texture2D texturePlatformFill;
			Rectangle frameLeft;
			Rectangle frameMiddle;
			Rectangle frameRight;
			float verticalOffset = 2;
			if (modPlayer.fortress)
			{
				texturePlatformOutline = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/HoloPlatformRookOutline");
				texturePlatformFill = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/HoloPlatformRookFill");
				verticalOffset = -2;
				frameLeft = new Rectangle(0, 0, 10, 16);
				frameMiddle = new Rectangle(13, 0, 1, 16);
				frameRight = new Rectangle(34, 0, 10, 16);
			}
			else
			{
				texturePlatformOutline = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/HoloPlatformOutline");
				texturePlatformFill = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/HoloPlatformFill");
				frameLeft = new Rectangle(0, 0, 8, 12);
				frameMiddle = new Rectangle(11, 0, 1, 12);
				frameRight = new Rectangle(20, 0, 8, 12);
			}
			Vector2 drawOrigin = new Vector2(textureChainOutline.Width * 0.5f, textureChainOutline.Height * 0.5f);
			Player owner = Main.player[Projectile.owner];
			Color color = new Color(100, 100, 100, 0);
			if (owner.active && !owner.dead && !modPlayer.hideChains)
			{
				Vector2 distanceToOwner = new Vector2(Projectile.Center.X, Projectile.position.Y + Projectile.height + 6) - owner.Center;
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
					for (int k = 0; k < 4; k++)
					{
						if (k == 0)
							spriteBatch.Draw(textureChainFill, drawPos + dynamicAddition, null, color * 0.5f, MathHelper.ToRadians(18 * i - 45) + startingRadians, drawOrigin, Projectile.scale * 0.75f, SpriteEffects.None, 0f);
						spriteBatch.Draw(textureChainOutline, drawPos + dynamicAddition, null, color * (1f - (Projectile.alpha / 255f)), MathHelper.ToRadians(18 * i - 45) + startingRadians, drawOrigin, Projectile.scale * 0.75f, SpriteEffects.None, 0f);
					}
				}
			}

			Vector2 drawPos2 = Projectile.position - Main.screenPosition;
			for (int k = 0; k < 4; k++)
			{
				if (k == 0)
					spriteBatch.Draw(texturePlatformFill, drawPos2 + new Vector2(0, verticalOffset), frameLeft, color * 0.5f, 0, Vector2.Zero, Projectile.scale, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(texturePlatformOutline, drawPos2 + new Vector2(0, verticalOffset), frameLeft, color * (1f - (Projectile.alpha / 255f)), 0f, Vector2.Zero, Projectile.scale, SpriteEffects.None, 0f);
			}
			float middleWidth = Projectile.width - frameLeft.Width - frameRight.Width;
			if(!modPlayer.fortress)
			{
				Vector2 horiScale = new Vector2(middleWidth, 1f);
				if (middleWidth > 0)
				{
					for (int k = 0; k < 4; k++)
					{
						if (k == 0)
							spriteBatch.Draw(texturePlatformFill, drawPos2 + new Vector2(frameLeft.Width, verticalOffset), frameMiddle, color * 0.5f, 0, Vector2.Zero, horiScale, SpriteEffects.None, 0f);
						Main.spriteBatch.Draw(texturePlatformOutline, drawPos2 + new Vector2(frameLeft.Width, verticalOffset), frameMiddle, color * (1f - (Projectile.alpha / 255f)), 0f, Vector2.Zero, horiScale, SpriteEffects.None, 0f);
					}
				}
			}
			else
            {
				middleWidth = (middleWidth - 10) / 2;
				Vector2 horiScale = new Vector2(middleWidth, 1f);
				if (middleWidth > 0)
				{
					for (int k = 0; k < 4; k++)
					{
						if (k == 0)
							spriteBatch.Draw(texturePlatformFill, drawPos2 + new Vector2(frameLeft.Width, verticalOffset), frameMiddle, color * 0.5f, 0, Vector2.Zero, horiScale, SpriteEffects.None, 0f);
						Main.spriteBatch.Draw(texturePlatformOutline, drawPos2 + new Vector2(frameLeft.Width, verticalOffset), frameMiddle, color * (1f - (Projectile.alpha / 255f)), 0f, Vector2.Zero, horiScale, SpriteEffects.None, 0f);
					}
					for (int k = 0; k < 4; k++)
					{
						if (k == 0)
							spriteBatch.Draw(texturePlatformFill, drawPos2 + new Vector2(frameLeft.Width + middleWidth, verticalOffset), new Rectangle(22, 0, 10, 16), color * 0.5f, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
						Main.spriteBatch.Draw(texturePlatformOutline, drawPos2 + new Vector2(frameLeft.Width + middleWidth, verticalOffset), new Rectangle(22, 0, 10, 16), color * (1f - (Projectile.alpha / 255f)), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					for (int k = 0; k < 4; k++)
					{
						if (k == 0)
							spriteBatch.Draw(texturePlatformFill, drawPos2 + new Vector2(frameLeft.Width + middleWidth + 10, verticalOffset), frameMiddle, color * 0.5f, 0, Vector2.Zero, horiScale, SpriteEffects.None, 0f);
						Main.spriteBatch.Draw(texturePlatformOutline, drawPos2 + new Vector2(frameLeft.Width + middleWidth + 10, verticalOffset), frameMiddle, color * (1f - (Projectile.alpha / 255f)), 0f, Vector2.Zero, horiScale, SpriteEffects.None, 0f);
					}
				}
			}
			for (int k = 0; k < 4; k++)
			{
				if (k == 0)
					spriteBatch.Draw(texturePlatformFill, drawPos2 + new Vector2(Projectile.width - frameRight.Width, verticalOffset), frameRight, color * 0.5f, 0, Vector2.Zero, Projectile.scale, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(texturePlatformOutline, drawPos2 + new Vector2(Projectile.width - frameRight.Width, verticalOffset), frameRight, color * (1f - (Projectile.alpha / 255f)), 0f, Vector2.Zero, Projectile.scale, SpriteEffects.None, 0f);
			}
			if (shader != 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
			}
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
			if (Projectile.owner == Main.myPlayer)
				Projectile.netUpdate = true;
			counter++;
			Player player = Main.player[Projectile.owner];
			if (runOnce)
            {
				positionToPlayer = Projectile.Center - player.Center;
				runOnce = false;
			}
			shader = SOTSPlayer.ModPlayer(player).platformShader;
			return true;
        }
		public void UpdatePartnerPos()
		{
			for (int i = 0; i < Main.Projectile.Length; i++)
			{
				Projectile partner = Main.projectile[i];
				if (partner.type == Projectile.type && partner.ai[0] == Projectile.ai[0] && partner.ai[1] == -Projectile.ai[1] && partner.owner == Projectile.owner)
				{
					HoloPlatform proj = (HoloPlatform)partner.modProjectile;
					proj.positionToPlayer = new Vector2(-positionToPlayer.X, positionToPlayer.Y);
					partner.netUpdate = true;
				}
			}
		}
		public bool UpdateWithMouse()
        {
			Player player = Main.player[Projectile.owner];
			if(player.whoAmI == Main.myPlayer)
            {
				Vector2 mousePos = Main.MouseWorld;
				Rectangle pos = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
				if (Main.mouseRight)
				{
					if (pos.Contains(new Point((int)mousePos.X, (int)mousePos.Y)) || relativeToMouse != null)
					{
						if(!clickedAlready)
						{
							if (relativeToMouse == null)
								relativeToMouse = Projectile.position - Main.MouseWorld;
							else
								Projectile.position = Main.MouseWorld + (Vector2)relativeToMouse;
							positionToPlayer = Projectile.Center - player.Center;
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
			Player owner = Main.player[Projectile.owner];
			if (!owner.active || owner.dead)
			{
				Projectile.Kill();
			}
			if(UpdateWithMouse())
			{
				Projectile.Center = positionToPlayer + owner.Center;
				float overlapVelocity = 1f;
				if(!Main.mouseRight && owner.whoAmI == Main.myPlayer)
					for (int i = 0; i < Main.maxNPCs; i++)
					{
						// Fix overlap with other minions
						Projectile other = Main.projectile[i];
						if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width && other.type == Projectile.type)
						{
							if (Projectile.position.X < other.position.X) Projectile.position.X -= overlapVelocity;
							else Projectile.position.X += overlapVelocity;

							if (Projectile.position.Y < other.position.Y) Projectile.position.Y -= overlapVelocity;
							else Projectile.position.Y += overlapVelocity;
							positionToPlayer = Projectile.Center - owner.Center;
							UpdatePartnerPos();
						}
					}
			}
			if(SentryID == -1)
            {
				for (int i = 0; i < Main.Projectile.Length; i++)
				{
					Projectile sentry = Main.projectile[i];
					if (sentry.active && sentry.owner == Projectile.owner && sentry.sentry == true && sentry.timeLeft >= Projectile.SentryLifeTime - 6 && sentry.Center.X < Projectile.position.X + Projectile.width && sentry.Center.X > Projectile.position.X && sentry.whoAmI != Projectile.whoAmI && sentry.type != Projectile.type)
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
				for (int i = 0; i < Main.Projectile.Length; i++)
				{
					Projectile partner = Main.projectile[i];
					if (partner.type == Projectile.type && partner.ai[0] == Projectile.ai[0] && partner.ai[1] == -Projectile.ai[1] && partner.owner == Projectile.owner)
					{
						HoloPlatform proj = (HoloPlatform)partner.modProjectile;
						if (proj.SentryID == SentryID)
							isPartners = true;
					}
				}

				Projectile sentry = Main.projectile[SentryID];
				if (sentry.active && sentry.owner == Projectile.owner && sentry.sentry == true && sentry.whoAmI != Projectile.whoAmI && sentry.type != Projectile.type && SentryType == sentry.type && !isPartners)
				{
					Projectile.width = sentry.width + 8;
					if (sentry.timeLeft < 6 || Projectile.timeLeft <= 4)
						sentry.timeLeft = 6;
					sentry.position = new Vector2(Projectile.Center.X - sentry.width/2, Projectile.position.Y - sentry.height);
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
					Projectile.width = 48;
					SentryID = -1;
				}
			}
			Projectile.rotation = rotateVector.ToRotation();
		}
        public void DustSpawn(Projectile proj)
        {
			for (int i = 0; i < 50; i++)
			{
				int dust2 = Dust.NewDust(proj.position - new Vector2(4, 4), proj.width, proj.height, DustID.Electric, 0, 0, Projectile.alpha, default, 1.25f);
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity *= 2f;
			}
		}
	}
}