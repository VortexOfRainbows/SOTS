using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Projectiles.Permafrost
{    
    public class PolarMortar : ModProjectile 
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(randIncrement);
			writer.Write(Projectile.tileCollide);
			writer.Write(Projectile.friendly);
			writer.Write(endHow);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			randIncrement = reader.ReadSingle();
			Projectile.tileCollide = reader.ReadBoolean();
			Projectile.friendly = reader.ReadBoolean();
			endHow = reader.ReadInt32();
		}
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Polar Mortar");
		}
        public override void SetDefaults()
        {
			Projectile.aiStyle = 0;
			Projectile.height = 28;
			Projectile.width = 44;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.timeLeft = 7200;
			Projectile.tileCollide = true;
			Projectile.alpha = 255;
			Projectile.netImportant = true;
			Projectile.extraUpdates = 1;
		}
		float randIncrement = 3;
		float counter = 0;
		bool runOnce = true;
		bool runOnce2 = true;
		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * ((255f - Projectile.alpha) / 255f);
		}
		public override bool ShouldUpdatePosition()
		{
			return endHow == 0;
		}
		Vector2[] trailPos = new Vector2[12];
		public void TrailPreDraw(ref Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Permafrost/PolarMortarTrail").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			float drawAmt = 1f;
			if (SOTS.Config.lowFidelityMode)
				drawAmt = 0.5f;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = Projectile.scale * 1.8f * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					break;
				}
				Color color = new Color(100, 100, 100, 0);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = color * ((trailPos.Length - k) / (float)trailPos.Length) * 0.33f;
				float max = betweenPositions.Length() / (5f * scale) * drawAmt;
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 3; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.1f * scale;
						float y = Main.rand.Next(-10, 11) * 0.1f * scale;
						if (j == 0)
						{
							x = 0;
							y = 0;
						}
						if (trailPos[k] != Projectile.Center)
							spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation(), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
		}
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
			TrailPreDraw(spriteBatch, lightColor);
			return endHow == 0;
		}
		public override bool PreAI()
		{
			int dustAmtMult = 3;
			if (SOTS.Config.lowFidelityMode)
				dustAmtMult = 1;
			if (Projectile.ai[0] == -1)
			{
				Vector2 position = Projectile.Center;
				SoundEngine.PlaySound(SoundID.Item14, (int)position.X, (int)position.Y);
				Projectile.ai[0]--;
				for (int i = 0; i < 15 * dustAmtMult; i++)
				{
					int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, Mod.Find<ModDust>("CopyDust4").Type);
					Dust dust = Main.dust[num1];
					dust.velocity *= 2.75f;
					dust.velocity += Projectile.velocity * 0.2f;
					dust.noGravity = true;
					dust.scale += 1.0f;
					dust.color = new Color(250, 100, 100, 100);
					dust.fadeIn = 0.1f;
					dust.scale *= 1.5f;
					dust.alpha = Projectile.alpha;
				}
			}
			if (runOnce)
			{
				for (int i = 0; i < 8.5 * dustAmtMult; i++)
				{
					int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, Mod.Find<ModDust>("CopyDust4").Type);
					Dust dust = Main.dust[num1];
					dust.velocity *= 1.5f;
					dust.noGravity = true;
					dust.scale += 0.1f;
					dust.color = new Color(250, 100, 100, 100);
					dust.fadeIn = 0.1f;
					dust.scale *= 1.4f;
					dust.alpha = Projectile.alpha;
				}
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				if(Main.netMode != 1)
					randIncrement = Main.rand.NextFloat(1.5f, 4.5f);
				Projectile.netUpdate = true;
				runOnce = false;
			}
			if (!runOnce)
			{
				if(Projectile.timeLeft % 3 == 0)
					cataloguePos();
			}
			checkPos();
			if (Projectile.timeLeft < 1000 && endHow == 0)
			{
				triggerStop();
			}
			return endHow != 1;
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
		int endHow = 0;
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			triggerStop();
			return false;
		}
		public void triggerStop()
		{
			endHow = 1;
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
				Projectile.alpha -= 4;
            }
			else
            {
				Projectile.alpha = 0;
				Projectile.hostile = true;
            }
			counter += randIncrement;
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.9f / 255f, (255 - Projectile.alpha) * 0.1f / 255f, (255 - Projectile.alpha) * 0.3f / 255f);
			Vector2 toLocation = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			Vector2 toTheLocation = toLocation - Projectile.Center;
			Vector2 SaveToLocatio = toTheLocation;
			Vector2 circular = new Vector2(3f + counter * 0.025f, 0).RotatedBy(MathHelper.ToRadians(counter));
			toTheLocation = toTheLocation.SafeNormalize(Vector2.Zero) * circular.Y;
			if(counter < 270)
			{
				Projectile.Center -= toTheLocation;
				Projectile.rotation = SaveToLocatio.ToRotation();
				if (toTheLocation.X < 0)
				{
					Projectile.rotation -= MathHelper.Pi;
					Projectile.spriteDirection = -1;
				}
				else
				{
					Projectile.spriteDirection = 1;
				}
			}
			if (counter >= 270)
			{
				if(runOnce2)
					Projectile.velocity = toTheLocation.SafeNormalize(Vector2.Zero) * -12;
				runOnce2 = false;
				Projectile.velocity.Y += 0.09f;
				Projectile.rotation = Projectile.velocity.ToRotation();
				if (Projectile.velocity.X < 0)
				{
					Projectile.rotation -= MathHelper.Pi;
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
	