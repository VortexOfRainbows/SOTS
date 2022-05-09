using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System;

namespace SOTS.Projectiles.Celestial
{    
    public class FluidFollower : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("my curse behind me");
		}
        public override void SetDefaults()
        {
			Projectile.penetrate = -1;
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.alpha = 0;
			Projectile.timeLeft = 20;
			Projectile.friendly = false;
		}
        public override bool PreDraw(ref Color lightColor)
		{
			/*
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			Rectangle tempBodyFrame = player.bodyFrame;
			player.bodyFrame = modPlayer.storedFramesBody[0];
			int tempWingFrame = player.wingFrame;
			player.wingFrame = modPlayer.storedFramesWings[0];
			Rectangle tempLegsFrame = player.legFrame;
			player.legFrame = modPlayer.storedFramesLegs[0];
			int tempDirection = player.direction;
			player.direction = modPlayer.storedDirection[0];
			int tempUseAnimation = player.itemAnimation;
			player.itemAnimation = 0;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			int shader = GameShaders.Armor.GetShaderIdFromItemId(ItemID.BlackDye);
			GameShaders.Armor.GetSecondaryShader(shader, player).Apply(null);
			Filters.Scene.Activate("BloodMoon", Projectile.Center).GetShader().UseColor(0, 0, 0);
			Main.instance.DrawPlayer(player, Projectile.Center - new Vector2(player.width / 2, player.height / 2), player.fullRotation, new Vector2(player.width/2, player.height/2), -1f);
			Filters.Scene.Deactivate("BloodMoon");
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
			player.bodyFrame = tempBodyFrame;
			player.wingFrame = tempWingFrame;
			player.legFrame = tempLegsFrame;
			player.direction = tempDirection;
			player.itemAnimation = tempUseAnimation;
			return false;
			*/

			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Player player = Main.player[Projectile.owner];
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Celestial/FluidFollowerOutline").Value;
			Texture2D texture3 = Mod.Assets.Request<Texture2D>("Projectiles/Celestial/FluidFollowerEye").Value;
			Texture2D texture4 = Mod.Assets.Request<Texture2D>("Projectiles/Celestial/FluidFollowerFill").Value;
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawOrigin3 = new Vector2(texture3.Width * 0.5f, texture3.Height * 0.5f);
			Vector2 between = player.Center - Projectile.Center;
			if (between.Length() > 1.1f)
			{
				between.Normalize();
			}
			else
			{
				between = Vector2.Zero;
			}
			DrawSpikes(spriteBatch);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.White), 0f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(texture3, Projectile.Center - Main.screenPosition + between * 5, null, Projectile.GetAlpha(Color.White), 0f, drawOrigin3, Projectile.scale, SpriteEffects.None, 0f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				if(k <= 1)
                {
					x = 0;
					y = 0;
					Main.spriteBatch.Draw(texture4, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X), (float)(Projectile.Center.Y - (int)Main.screenPosition.Y)), null, Projectile.GetAlpha(Color.White.MultiplyRGBA(color)) * 0.5f, 0f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
				}
				Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, Projectile.GetAlpha(Color.White.MultiplyRGBA(color)) * 0.7f, 0f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(texture3, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y) + between * 5, null, Projectile.GetAlpha(Color.White.MultiplyRGBA(color)) * 0.7f, 0f, drawOrigin3, Projectile.scale, SpriteEffects.None, 0f);
			}
		}
		public void DrawSpikes(SpriteBatch spriteBatch)
		{
			if (!runOnce)
			{
				Color color = new Color(70, 255, 70, 0);
				Vector2 drawPos = Projectile.Center - Main.screenPosition;
				for (int i = 0; i < 12; i++)
				{
					counterArr[i] += randSeed1[i];
					Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/FluidFollowerSpike");
					Vector2 circular = new Vector2(16 * Projectile.scale, 0).RotatedBy(MathHelper.ToRadians(i * 30));
					int frame = 0;
					float scaleMult = 0.5f;
					if (counterArr[i] >= 10)
					{
						frame = 1;
						scaleMult = 0.575f;
					}
					if (counterArr[i] >= 20)
					{
						frame = 2;
						scaleMult = 0.65f;
					}
					if (counterArr[i] >= 30)
					{
						frame = 3;
						scaleMult = 0.725f;
					}
					if (counterArr[i] >= 40)
					{
						frame = 0; 
						counterArr[i] = 0;
						randSeed1[i] = Main.rand.NextFloat(0.8f, 1.2f);
					}
					Rectangle FrameSize = new Rectangle(0, texture.Height / 4 * frame, texture.Width, texture.Height / 4);
					for (int k = 0; k < 2; k++)
					{
						float x = Main.rand.Next(-10, 11) * 0.1f;
						float y = Main.rand.Next(-10, 11) * 0.1f;
						if (k <= 1)
						{
							x = 0;
							y = 0;
						}
						spriteBatch.Draw(texture, drawPos + circular + new Vector2(x + 0.25f, y + 0.25f), FrameSize, Projectile.GetAlpha(color) * 1f, MathHelper.ToRadians(i * 30), new Vector2(0, 10.0f), Projectile.scale * scaleMult, SpriteEffects.None, 0f);
					}
				}
			}
		}
		bool runOnce = true;
		float[] counterArr = new float[12];
		float[] randSeed1 = new float[12];
		public override void AI()
		{
			if (runOnce)
			{
				if (Main.myPlayer == Projectile.owner)
					Projectile.netUpdate = true;
				for (int i = 0; i < counterArr.Length; i++)
				{
					counterArr[i] = 0;
					randSeed1[i] = Main.rand.NextFloat(0.8f, 1.2f);
				}
				runOnce = false;
			}
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (modPlayer.FluidCurse && player.active)
			{
				Projectile.timeLeft = (int)modPlayer.FluidCurseMult;
				Vector2 toPlayer = player.Center - Projectile.Center;
				float dist = toPlayer.Length();
				float speed = (0.4f + dist * 0.1f / (float)Math.Pow(modPlayer.FluidCurseMult, 0.5f));
				if (speed > dist)
					speed = dist;
				Projectile.Center += toPlayer.SafeNormalize(Vector2.Zero) * speed;
			}
			else
			{
				Projectile.Kill();
			}
			Projectile.alpha = 255 - (int)(Projectile.timeLeft * 255f / 60f);
        }
    }
}
		
			