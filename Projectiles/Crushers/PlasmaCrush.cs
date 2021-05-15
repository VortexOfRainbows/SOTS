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
			projectile.width = 32;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.timeLeft = 40;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.melee = true;
			projectile.alpha = 10;
			projectile.usesIDStaticNPCImmunity = true;
			projectile.idStaticNPCHitCooldown = 10;
		}
		Color color = Color.White;
		public override bool PreAI()
		{
			projectile.alpha += 7;
			if (projectile.alpha > 255)
				projectile.alpha = 255;
			if (runOnce)
			{
				trailPos[0] = projectile.Center;
				Projectile last = this.projectile;
				bool found = false;
				for (short i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.type == projectile.type && proj.active && proj.owner == projectile.owner && proj.identity == (int)(projectile.ai[0] + 0.5f))
					{
						found = true;
						last = proj;
						break;
					}
				}
				if (found)
					trailPos[1] = last.Center;
				else
					trailPos[1] = projectile.Center;
				runOnce = false;
			}
			return base.PreAI();
		}
		Vector2[] trailPos = new Vector2[2];
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = trailPos[0];
			float scale = projectile.scale * 2.5f;
			Vector2 drawPos = trailPos[1] - Main.screenPosition;
			Vector2 currentPos = trailPos[1];
			Vector2 betweenPositions = previousPosition - currentPos;
			float max = betweenPositions.Length() / (2.5f * scale);
			for (int i = 0; i < max; i++)
			{
				Color color = this.color;
				color = color.MultiplyRGBA(new Color(120, 150, 110, 0));
				drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
				if (i == 0)
					color *= 0.5f;
				for (int j = 0; j < 4; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f * scale;
					float y = Main.rand.Next(-10, 11) * 0.1f * scale;
					if (j <= 1)
					{
						x = 0;
						y = 0;
					}
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, projectile.GetAlpha(color), betweenPositions.ToRotation(), drawOrigin, scale, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
		bool runOnce = true;
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), trailPos[0], trailPos[1], 30f, ref point))
			{
				return true;
			}
			return false;
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
    }
}
		