using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Celestial
{    
    public class PlasmaCutterTrail : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Cutter Trail");
		}
        public override void SetDefaults()
        {
			projectile.width = 32;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.timeLeft = 3600;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.alpha = 255;
			projectile.ai[1] = -1;
			projectile.extraUpdates = 1;
			projectile.usesIDStaticNPCImmunity = true;
			projectile.idStaticNPCHitCooldown = 10;
		}
		Color color = Color.White;
		public override bool PreAI()
		{
			if (runOnce)
			{
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
			}
			return base.PreAI();
		}
		public override void PostAI()
		{
			checkPos();
			cataloguePos();
		}
		Vector2[] trailPos = new Vector2[50];
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return true;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = trailPos[0];
			if (previousPosition == Vector2.Zero)
			{
				return true;
			}
			for (int k = 1; k < trailPos.Length; k++)
			{
				float scale = projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				scale *= 1.25f;
				if (trailPos[k] == Vector2.Zero)
				{
					return true;
				}
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				float max = betweenPositions.Length() / (2.5f * scale);
				for (int i = 0; i < max; i++)
				{
					Color color = this.color * ((trailPos.Length - k) / (float)trailPos.Length) * 0.25f;
					color = color.MultiplyRGBA(new Color(120, 150, 110, 0));
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					if (i == 0)
						color *= 0.5f;
					for (int j = 0; j < 5; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.1f * scale;
						float y = Main.rand.Next(-10, 11) * 0.1f * scale;
						if (j <= 1)
						{
							x = 0;
							y = 0;
						}
						if (trailPos[k] != projectile.Center)
							Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation(), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
			return true;
		}
		bool runOnce = true;
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
		}
		int endHow = 0;
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Player player = Main.player[projectile.owner];
			float point = 0f;
			Vector2 previousPosition = projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				scale *= 1.25f;
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				Vector2 currentPos = trailPos[k];
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), previousPosition, currentPos, 12f * scale, ref point))
				{
					return true;
				}
				previousPosition = currentPos;
			}
			return false;
		}
		bool end = false;
		float counter = 0;
		public override void AI()
		{
			counter++;
			if (projectile.ai[1] != -1 && end == false)
			{
				Projectile proj = Main.projectile[(int)projectile.ai[1]];
				if(proj.active && proj.type == ModContent.ProjectileType<PlasmaCutter>() && proj.owner == projectile.owner && ((int)proj.ai[1] == projectile.whoAmI || (int)proj.ai[0] == projectile.whoAmI))
				{
					Vector2 center = proj.Center;
					projectile.Center = center;
					projectile.velocity = proj.velocity;
					projectile.rotation = proj.rotation;
					float circular = new Vector2(projectile.ai[0], 0).RotatedBy(MathHelper.ToRadians(counter * 12f)).X;
					projectile.Center += new Vector2(0, circular - 24).RotatedBy(MathHelper.ToRadians(45) + projectile.rotation);
					projectile.timeLeft = 120;
				}
				else
                {
					end = true;
				}
			}
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
    }
}
		