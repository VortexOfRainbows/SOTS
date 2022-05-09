using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{    
    public class PlasmaCrush : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Crush");
		}
        public override void SetDefaults()
        {
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.timeLeft = 40;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.melee = true;
			Projectile.alpha = 10;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}
		Color color = Color.White;
		public override bool PreAI()
		{
			Projectile.alpha += 7;
			if (Projectile.alpha > 255)
				Projectile.alpha = 255;
			if (runOnce)
			{
				trailPos[0] = Projectile.Center;
				Projectile last = this.projectile;
				bool found = false;
				for (short i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.type == Projectile.type && proj.active && proj.owner == Projectile.owner && proj.identity == (int)(Projectile.ai[0] + 0.5f))
					{
						found = true;
						last = proj;
						break;
					}
				}
				if (found)
					trailPos[1] = last.Center;
				else
					trailPos[1] = Projectile.Center;
				runOnce = false;
			}
			return base.PreAI();
		}
		Vector2[] trailPos = new Vector2[2];
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = trailPos[0];
			float scale = Projectile.scale * 2.5f;
			Vector2 drawPos = trailPos[1] - Main.screenPosition;
			Vector2 currentPos = trailPos[1];
			Vector2 betweenPositions = previousPosition - currentPos;
			float max = betweenPositions.Length() / (texture.Width * 0.5f * scale);
			for (int i = 0; i < max; i++)
			{
				Color color = this.color;
				color = color.MultiplyRGBA(new Color(170, 255, 160, 0));
				drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
				if (i == 0)
					color *= 0.5f;
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f * scale;
					float y = Main.rand.Next(-10, 11) * 0.1f * scale;
					if (j == 0)
					{
						x = 0;
						y = 0;
					}
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, Projectile.GetAlpha(color), betweenPositions.ToRotation(), drawOrigin, scale, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
		bool runOnce = true;
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), trailPos[0], trailPos[1], 30f, ref point);
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
    }
}
		