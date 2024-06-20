using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using static Humanizer.In;

namespace SOTS.Projectiles.Permafrost
{    
    public class PolarMortar : ModProjectile 
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(randIncrement);
			writer.Write(Projectile.tileCollide);
			writer.Write(Projectile.friendly);
			writer.Write(AllowTrailToEnd);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			randIncrement = reader.ReadSingle();
			Projectile.tileCollide = reader.ReadBoolean();
			Projectile.friendly = reader.ReadBoolean();
            AllowTrailToEnd = reader.ReadBoolean();
		}
        public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 2;
		}
        public override void SetDefaults()
        {
			Projectile.aiStyle = 0;
			Projectile.height = 32;
			Projectile.width = 32;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.timeLeft = 7200;
			Projectile.tileCollide = true;
			Projectile.alpha = 255;
			Projectile.netImportant = true;
			Projectile.extraUpdates = 1;
		}
		float randIncrement = 2.5f;
		float counter = 0;
		bool runOnce = true;
		bool runOnce2 = true;
		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * ((255f - Projectile.alpha) / 255f);
		}
		public override bool ShouldUpdatePosition()
		{
			return !AllowTrailToEnd;
		}
		Vector2[] trailPos = new Vector2[12];
		public void cataloguePos()
		{
			Vector2 current = Projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
		}
		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D textureReal = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Permafrost/PolarMortarTrail").Value;
            Texture2D textureGlow = Mod.Assets.Request<Texture2D>("Projectiles/Permafrost/PolarMortarGlow").Value;
            Vector2 drawOrigin = new Vector2(textureReal.Width * 0.5f, textureReal.Height / 4f);
            Vector2 drawOriginTrail = new Vector2(0, texture.Height / 2f);
            Vector2 previousPosition = Projectile.Center;
            Rectangle frame = new Rectangle(0, textureReal.Height / 2 * (int)(Projectile.ai[1] % 2), textureReal.Width, textureReal.Height / 2);
            for (int k = 0; k < trailPos.Length; k++)
            {
                if (trailPos[k] == Vector2.Zero)
                {
                    break;
                }
                Color color = new Color(100, 100, 100, 0);
                Vector2 drawPos;
                Vector2 currentPos = trailPos[k];
                Vector2 betweenPositions = currentPos - previousPosition;
                color = color * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
                drawPos = previousPosition - Main.screenPosition;
                if (trailPos[k] != Projectile.Center)
                    Main.spriteBatch.Draw(texture, drawPos, null, color, betweenPositions.ToRotation(), drawOriginTrail, new Vector2(betweenPositions.Length() / texture.Width, 1f), SpriteEffects.None, 0f);
                previousPosition = currentPos;
            }
            lightColor = Color.Lerp(lightColor, Color.White, 0.25f);
			if (!AllowTrailToEnd)
            {
				Main.spriteBatch.Draw(textureReal, Projectile.Center - Main.screenPosition, frame, Projectile.GetAlpha(lightColor), Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(textureGlow, Projectile.Center - Main.screenPosition, frame, Projectile.GetAlpha(Color.White), Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
			}
            return false;
		}
		public override bool PreAI()
		{
            Color Color = new Color(187, 11, 76, 0);
            if ((int)(Projectile.ai[1] % 2) != 0)
            {
                Color = new Color(64, 74, 204, 0);
            }
            if (Projectile.ai[0] == -1)
			{
				Vector2 position = Projectile.Center;
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, position);
				Projectile.ai[0]--;
                int dustAmtMult = 2;
                if (SOTS.Config.lowFidelityMode)
                    dustAmtMult = 1;
                for (int i = 0; i < 20 * dustAmtMult; i++)
				{
                    Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, ModContent.DustType<Dusts.CopyDust4>());
					dust.velocity *= 2.0f;
					dust.velocity += Projectile.velocity * 0.25f;
					dust.noGravity = true;
                    dust.scale *= 0.5f;
                    dust.scale += 1.5f;
					dust.color = Color;
					dust.fadeIn = 0.1f;
					dust.alpha = Projectile.alpha;
					if(Main.rand.NextBool(2))
					{
						dust.type = ModContent.DustType<PixelDust>();
						dust.fadeIn = 7;
						dust.scale = 2;
					}
				}
				if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, -Projectile.velocity * 0.5f + new Vector2(0, -2) + Main.rand.NextVector2Circular(4, 4), ModContent.ProjectileType<PolarisMines>(), (int)(Projectile.damage * 0.8f), 0, Main.myPlayer, (int)(Projectile.ai[1] % 2), Main.rand.NextFloat(120));
                }
            }
            if (runOnce)
            {
                for (int i = 0; i < trailPos.Length; i++)
                {
                    trailPos[i] = Vector2.Zero;
                }
				if(Projectile.velocity == Vector2.Zero)
                {
                    randIncrement = 2.5f;
                }
				else
				{
					counter = 200;
					runOnce2 = false;
				}
				Projectile.netUpdate = true;
				runOnce = false;
			}
			if (!runOnce)
			{
				if(Projectile.timeLeft % 3 == 0)
					cataloguePos();
			}
			checkPos();
			if (Projectile.timeLeft < 1000 && !AllowTrailToEnd)
			{
				triggerStop();
            }
            else if (!AllowTrailToEnd && Main.rand.NextBool(15))
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, ModContent.DustType<Dusts.CopyDust4>());
                dust.velocity *= 0.2f;
                dust.velocity += Projectile.velocity * 0.2f;
                dust.noGravity = true;
                dust.scale += 0.5f;
                dust.color = Color;
                dust.fadeIn = 0.1f;
                dust.alpha = Projectile.alpha;
            }
            return !AllowTrailToEnd;
		}
		public void checkPos()
		{
			float iterator = 0f;
			Vector2 current = Projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				if (current == previousPosition)
				{
					iterator++;
				}
			}
			if (iterator >= trailPos.Length)
				Projectile.Kill();
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 24;
			height = 24;
			return true;
        }
        bool AllowTrailToEnd = false;
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			triggerStop();
			return false;
		}
		public void triggerStop()
		{
            AllowTrailToEnd = true;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.velocity *= 0f;
			Projectile.netUpdate = true;
			Projectile.ai[0] = -1;
		}
		public override void AI()
		{
			if(Projectile.alpha > 0)
            {
				Projectile.alpha -= 12;
            }
			if(Projectile.alpha <= 0)
            {
				Projectile.alpha = 0;
				Projectile.hostile = true;
            }
			counter += randIncrement;
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.9f / 255f, (255 - Projectile.alpha) * 0.1f / 255f, (255 - Projectile.alpha) * 0.3f / 255f);
			Vector2 toLocation = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			Vector2 toTheLocation = toLocation - Projectile.Center;
			Vector2 SaveToLocatio = toTheLocation;
			Vector2 circular = new Vector2(2.25f + counter * 0.1f, 0).RotatedBy(MathHelper.ToRadians(counter * 270f / 190f));
			toTheLocation = toTheLocation.SafeNormalize(Vector2.Zero) * circular.Y;
			if(counter < 190)
			{
				Projectile.Center -= toTheLocation;
				Projectile.rotation = SaveToLocatio.ToRotation();
				if(counter < 30)
					if (toTheLocation.X < 0)
					{
						Projectile.spriteDirection = -1;
					}
					else
					{
						Projectile.spriteDirection = 1;
					}
			}
			if (counter >= 190)
			{
				if(runOnce2)
					Projectile.velocity = toTheLocation.SafeNormalize(Vector2.Zero) * -11.0f;
				runOnce2 = false;
				Projectile.velocity.Y += 0.104f;
				Projectile.rotation = Projectile.velocity.ToRotation();
				if (Projectile.velocity.X < 0)
				{
					Projectile.spriteDirection = -1;
				}
				else
				{
					Projectile.spriteDirection = 1;
				}
			}
		}	
	}
}
	