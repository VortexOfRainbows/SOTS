using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth
{    
    public class PerfectStarLaser2 : ModProjectile 
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(Projectile.friendly);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			Projectile.friendly = reader.ReadBoolean();
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Laser");
		}
        public override void SetDefaults()
        {
			Projectile.penetrate = -1; 
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 1500;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.extraUpdates = 4;
			Projectile.localNPCHitCooldown = 15;
			Projectile.usesLocalNPCImmunity = true;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 12;
			height = 12;
			return true;
		}
		int pierceCount = 0;
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			pierceCount++;
			int maxPierce = -10 * (int)Projectile.ai[1];
			if(pierceCount > maxPierce && Projectile.ai[1] != -3)
				TriggerStop();
		}
		bool runOnce = true;
		Vector2[] trailPos = new Vector2[10];
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (runOnce)
			{
				return false;
			}
			float scale = 0.5f;
			if (Projectile.ai[1] == -3)
				scale = 1.5f;
			if (Projectile.ai[1] == -2)
				scale = 1f;
			if (Projectile.ai[1] == -1)
				scale = 0.5f;
			float width = Projectile.width * scale;
			float height = Projectile.height * scale;
			for (int i = 0; i < trailPos.Length * 0.5f; i += 3)
			{
				Vector2 pos = trailPos[i];
				projHitbox = new Rectangle((int)pos.X - (int)width / 2, (int)pos.Y - (int)height / 2, (int)width, (int)height);
				if (projHitbox.Intersects(targetHitbox))
				{
					return true;
				}
			}
			return false;
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPoint, 8f, ref point);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			if(Projectile.ai[1] != -2)
				texture = Terraria.GameContent.TextureAssets.Projectile[ModContent.ProjectileType<PerfectStarLaser>()].Value;
			float scale2 = 0.5f;
			if(Projectile.ai[1] == -3)
				scale2 = 1.5f;
			if (Projectile.ai[1] == -2)
				scale2 = 1f;
			if (Projectile.ai[1] == -1)
				scale2 = 0.5f;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float sqrt = (float)Math.Sqrt((trailPos.Length - k) / (float)trailPos.Length);
				float scale = Projectile.scale * (0.6f * sqrt + 0.4f) * scale2;
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				if (trailPos[k] != Projectile.Center)
				{
					Vector2 drawPos = trailPos[k] - Main.screenPosition;
					Color color = new Color(100, 100, 100, 0);
					color = Projectile.GetAlpha(color);
					float max = betweenPositions.Length() / (texture.Width * 0.2f);
					for (int i = 0; i < max; i++)
					{
						drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
						Main.spriteBatch.Draw(texture, drawPos, null, color, betweenPositions.ToRotation(), drawOrigin, new Vector2(1, scale), SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
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
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			TriggerStop();
			return false;
		}
		public void TriggerStop()
		{
			Projectile.tileCollide = false;
			Projectile.velocity *= 0f;
			Projectile.friendly = false;
			Projectile.netUpdate = true;
		}
		public override bool PreAI()
		{
			int trailLength = (int)Projectile.ai[0];
			if (runOnce)
			{
				for (int i = 0; i < 10 + 10 * -Projectile.ai[1]; i++)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
					Color color2 = Color.Lerp(new Color(175, 218, 118, 0), new Color(74, 186, 54, 0), Main.rand.NextFloat(1));
					dust.color = color2;
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 1.5f;
					dust.alpha = Projectile.alpha;
					dust.velocity *= 1.5f * (-0.4f * Projectile.ai[1]);
					dust.velocity += Projectile.velocity * Main.rand.NextFloat(0, 1f) * 1.2f * (-0.4f * Projectile.ai[1]); ;
				}
				Projectile.rotation = Projectile.velocity.ToRotation();
				trailPos = new Vector2[trailLength];
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
			}
			if (Projectile.timeLeft > trailLength && !Projectile.friendly)
            {
				Projectile.timeLeft = trailLength;
            }
			if(Projectile.timeLeft < trailLength || Main.rand.NextBool((int)(8 + 2 * Projectile.ai[1])))
            {
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
				Color color2 = Color.Lerp(new Color(175, 218, 118, 0), new Color(74, 186, 54, 0), Main.rand.NextFloat(1));
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.0f - 0.2f * Projectile.ai[1];
				dust.alpha = Projectile.alpha;
				dust.velocity *= 0.8f * (-0.4f * Projectile.ai[1]);
			}
			else if (Projectile.timeLeft <= trailLength && Projectile.friendly)
            {
				TriggerStop();
            }		
			return true;
		}
		public override void AI()
		{
			cataloguePos();
		}
	}
}
		