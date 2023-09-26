using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Permafrost
{    
    public class PolarBullet : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 2;
		}
        public override void SetDefaults()
        {
			Projectile.penetrate = -1;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.timeLeft = 1120;
			Projectile.friendly = false;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.hostile = true;
		}
		Vector2[] trailPos = new Vector2[8];
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
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
            Texture2D textureReal = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Texture2D textureGlow = Mod.Assets.Request<Texture2D>("Projectiles/Permafrost/PolarBulletGlow").Value;
            Texture2D textureTrail = Mod.Assets.Request<Texture2D>("Projectiles/Permafrost/PolarisTrail").Value;
            Vector2 drawOrigin = new Vector2(textureReal.Width * 0.5f, 10);
            Vector2 drawOriginTrail = new Vector2(0, textureTrail.Height / 2f);
            Vector2 previousPosition = Projectile.Center;
			Rectangle frame = new Rectangle(0, 22 * (int)Projectile.ai[1], textureReal.Width, 20);
            for (int k = 0; k < trailPos.Length; k++)
            {
                float scale = Projectile.scale * 0.9f * (trailPos.Length - k) / (float)trailPos.Length;
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
					Main.spriteBatch.Draw(textureTrail, drawPos, null, color, betweenPositions.ToRotation(), drawOriginTrail, new Vector2(betweenPositions.Length() / textureTrail.Width * 2f, scale), SpriteEffects.None, 0f);
                previousPosition = currentPos;
            }
            if (!AllowTrailToEnd)
            {
                Main.spriteBatch.Draw(textureReal, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(textureGlow, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
			return false;
		}
		bool runOnce = true;
		float acceleration = 0.3f;
		public override bool PreAI()
		{
			int dustAmtMult = 3;
			if (SOTS.Config.lowFidelityMode)
				dustAmtMult = 1;
            Color Color  = new Color(187, 11, 76, 0);
            if (Projectile.ai[1] == 0)
            {
                Color = new Color(64, 74, 204, 0);
            }
            if (Projectile.ai[0] == -1)
			{
				Projectile.ai[0]--;
				for (int i = 0; i < 3 * dustAmtMult; i++)
				{
					int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, ModContent.DustType<Dusts.CopyDust4>());
					Dust dust = Main.dust[num1];
					dust.velocity *= 0.2f;
					dust.velocity += Projectile.velocity * 0.225f;
					dust.noGravity = true;
					dust.scale += 0.1f;
					dust.color = Color;
					dust.fadeIn = 0.1f;
					dust.scale *= 1.6f;
					dust.alpha = Projectile.alpha;
				}
			}
			Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.ToRadians(90);
			if (runOnce)
			{
				acceleration = 0.4f;
				Projectile.position += Projectile.velocity * 2;
				for (int i = 0; i < 1.5 * dustAmtMult; i++)
				{
					int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, ModContent.DustType<Dusts.CopyDust4>());
					Dust dust = Main.dust[num1];
					dust.velocity *= 0.1f;
					dust.velocity += Projectile.velocity * 0.5f;
					dust.noGravity = true;
					dust.scale += 0.1f;
					dust.color = Color;
					dust.fadeIn = 0.1f;
					dust.scale *= 1.4f;
					dust.alpha = Projectile.alpha;
				}
				SOTSUtils.PlaySound(SoundID.Item11, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.25f);
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
			}
			if (!runOnce)
			{
				cataloguePos();
			}
			checkPos();
			if (Projectile.timeLeft < 600 && !AllowTrailToEnd)
			{
				triggerStop();
			}
			else if(!AllowTrailToEnd && Main.rand.NextBool(20))
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, ModContent.DustType<Dusts.CopyDust4>());
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity * 0.1f;
                dust.noGravity = true;
                dust.scale += 0.1f;
                dust.color = Color;
                dust.fadeIn = 0.1f;
                dust.scale *= 1.2f;
                dust.alpha = Projectile.alpha;
            }
			Projectile.position += Projectile.velocity * acceleration;
			acceleration += 0.04f;
			if (acceleration > 10)
				acceleration = 10;
			return Projectile.friendly;
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
			width = 8;
			height = 8;
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
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.tileCollide);
			writer.Write(Projectile.friendly);
			writer.Write(AllowTrailToEnd);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.tileCollide = reader.ReadBoolean();
			Projectile.friendly = reader.ReadBoolean();
			AllowTrailToEnd = reader.ReadBoolean();
		}
	}
}
		