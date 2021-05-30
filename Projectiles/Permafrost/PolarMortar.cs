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
			writer.Write(projectile.tileCollide);
			writer.Write(projectile.friendly);
			writer.Write(endHow);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			randIncrement = reader.ReadSingle();
			projectile.tileCollide = reader.ReadBoolean();
			projectile.friendly = reader.ReadBoolean();
			endHow = reader.ReadInt32();
		}
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Polar Mortar");
		}
        public override void SetDefaults()
        {
			projectile.aiStyle = 0;
			projectile.height = 28;
			projectile.width = 44;
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.timeLeft = 7200;
			projectile.tileCollide = true;
			projectile.alpha = 255;
			projectile.netImportant = true;
			projectile.extraUpdates = 1;
		}
		float randIncrement = 3;
		float counter = 0;
		bool runOnce = true;
		bool runOnce2 = true;
		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * ((255f - projectile.alpha) / 255f);
		}
		public override bool ShouldUpdatePosition()
		{
			return endHow == 0;
		}
		Vector2[] trailPos = new Vector2[12];
		public void TrailPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Projectiles/Permafrost/PolarMortarTrail");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = projectile.scale * 1.8f * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					break;
				}
				Color color = new Color(100, 100, 100, 0);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = color * ((trailPos.Length - k) / (float)trailPos.Length) * 0.33f;
				float max = betweenPositions.Length() / (5f * scale);
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
						if (trailPos[k] != projectile.Center)
							spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation(), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
		}
		public void cataloguePos()
		{
			Vector2 current = projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			TrailPreDraw(spriteBatch, lightColor);
			return endHow == 0;
		}
		public override bool PreAI()
		{
			if (projectile.ai[0] == -1)
			{
				Vector2 position = projectile.Center;
				Main.PlaySound(SoundID.Item14, (int)position.X, (int)position.Y);
				projectile.ai[0]--;
				for (int i = 0; i < 45; i++)
				{
					int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(5), projectile.width, projectile.height, mod.DustType("CopyDust4"));
					Dust dust = Main.dust[num1];
					dust.velocity *= 2.75f;
					dust.velocity += projectile.velocity * 0.2f;
					dust.noGravity = true;
					dust.scale += 1.0f;
					dust.color = new Color(250, 100, 100, 100);
					dust.fadeIn = 0.1f;
					dust.scale *= 1.5f;
					dust.alpha = projectile.alpha;
				}
			}
			if (runOnce)
			{
				for (int i = 0; i < 25; i++)
				{
					int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(5), projectile.width, projectile.height, mod.DustType("CopyDust4"));
					Dust dust = Main.dust[num1];
					dust.velocity *= 1.5f;
					dust.noGravity = true;
					dust.scale += 0.1f;
					dust.color = new Color(250, 100, 100, 100);
					dust.fadeIn = 0.1f;
					dust.scale *= 1.4f;
					dust.alpha = projectile.alpha;
				}
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				if(Main.netMode != 1)
					randIncrement = Main.rand.NextFloat(1.5f, 4.5f);
				projectile.netUpdate = true;
				runOnce = false;
			}
			if (!runOnce)
			{
				if(projectile.timeLeft % 3 == 0)
					cataloguePos();
			}
			checkPos();
			if (projectile.timeLeft < 1000 && endHow == 0)
			{
				triggerStop();
			}
			return endHow != 1;
		}
		public void checkPos()
		{
			float iterator = 0f;
			Vector2 current = projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				if (current == previousPosition)
				{
					iterator++;
				}
			}
			if (iterator >= trailPos.Length)
				projectile.Kill();
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 24;
			height = 24;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough);
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
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.velocity *= 0f;
			projectile.netUpdate = true;
			projectile.ai[0] = -1;
		}
		public override void AI()
		{
			if(projectile.alpha > 0)
            {
				projectile.alpha -= 4;
            }
			else
            {
				projectile.alpha = 0;
				projectile.hostile = true;
            }
			counter += randIncrement;
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.9f / 255f, (255 - projectile.alpha) * 0.1f / 255f, (255 - projectile.alpha) * 0.3f / 255f);
			Vector2 toLocation = new Vector2(projectile.ai[0], projectile.ai[1]);
			Vector2 toTheLocation = toLocation - projectile.Center;
			Vector2 SaveToLocatio = toTheLocation;
			Vector2 circular = new Vector2(3f + counter * 0.025f, 0).RotatedBy(MathHelper.ToRadians(counter));
			toTheLocation = toTheLocation.SafeNormalize(Vector2.Zero) * circular.Y;
			if(counter < 270)
			{
				projectile.Center -= toTheLocation;
				projectile.rotation = SaveToLocatio.ToRotation();
				if (toTheLocation.X < 0)
				{
					projectile.rotation -= MathHelper.Pi;
					projectile.spriteDirection = -1;
				}
				else
				{
					projectile.spriteDirection = 1;
				}
			}
			if (counter >= 270)
			{
				if(runOnce2)
					projectile.velocity = toTheLocation.SafeNormalize(Vector2.Zero) * -12;
				runOnce2 = false;
				projectile.velocity.Y += 0.09f;
				projectile.rotation = projectile.velocity.ToRotation();
				if (projectile.velocity.X < 0)
				{
					projectile.rotation -= MathHelper.Pi;
					projectile.spriteDirection = -1;
				}
				else
				{
					projectile.spriteDirection = 1;
				}
			}
		}	
	}
}
	