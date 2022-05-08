using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Projectiles 
{    
    public class BoreBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/BoreBulletTrail");
			Vector2 drawOrigin = new Vector2(texture.Width/2, texture.Height/2);
			Vector2 lastPosition = projectile.Center;
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				float scale = 1f - 0.5f *(k / (float)projectile.oldPos.Length);
				Vector2 drawPos = projectile.oldPos[k] + new Vector2(projectile.width/2, projectile.height/2);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				color.A = 190;
				float lengthTowards = Vector2.Distance(lastPosition, drawPos) / texture.Height / scale;
				spriteBatch.Draw(texture, drawPos - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), null, color * scale * 0.8f, projectile.rotation, drawOrigin, new Vector2(1, lengthTowards) * scale, projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				lastPosition = drawPos;
			}
			return true;
		}
		public override void SetDefaults()
        {
			projectile.CloneDefaults(ProjectileID.Bullet);
			projectile.light *= 0.5f;
			projectile.aiStyle = -1;
			projectile.penetrate = 6;
			projectile.width = 10;
			projectile.height = 24;
			projectile.alpha = 255;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 240;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 8;
			height = 8;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override void OnHitNPC(NPC n, int damage, float knockback, bool crit)
		{
			projectile.ai[0] = -2;
			projectile.netUpdate = true;
		}
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			int ignoreDefense = ((target.defense + 1) / 2);
			int flatAdded = 10;
			if (flatAdded > damage)
				flatAdded = damage;
			int baseDamage = (int)(damage * 0.8) - flatAdded - ignoreDefense;
			baseDamage = (int) MathHelper.Clamp(baseDamage, 0, damage);
			baseDamage += (int)(damage * 0.2f);
			damage = baseDamage + ignoreDefense + flatAdded;
			//Main.NewText(baseDamage);
		}
        public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 20; i++)
			{
				Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 4, 4, DustID.Stone);
				dust.noGravity = true;
				dust.velocity *= 0.9f;
				dust.velocity += projectile.oldVelocity.SafeNormalize(Vector2.Zero) * 0.4f;
				dust.scale *= 1.4f;
			}
			base.Kill(timeLeft);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if (projectile.penetrate > 2)
				projectile.penetrate--;
			else
			{
				projectile.Kill();
				return true;
            }
			projectile.ai[0] = -1;
			projectile.netUpdate = true;
			projectile.velocity = oldVelocity;
            return false;
        }
        bool runOnce = true;
        public override void AI()
		{
			projectile.spriteDirection = projectile.direction;
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			if (projectile.alpha > 0)
				projectile.alpha -= 20;
			else
				projectile.alpha = 0;
			if(runOnce || projectile.ai[0] < 0)
            {
				if(!runOnce)
                {
					if (projectile.ai[0] == -2)
						projectile.ai[0] = 15;
					else
						projectile.ai[0] = 30;
					SoundEngine.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 22, 0.75f, 0.1f);
				}
				projectile.velocity *= 0.04f;
				runOnce = false;
            }
			if (projectile.ai[0] > 0)
			{
				projectile.tileCollide = false;
				projectile.ai[0]--;
				projectile.velocity *= 0.5f;
				for (int i = -1; i <= 1; i += 2)
				{
					Vector2 circular = new Vector2(2 * i, 0).RotatedBy(MathHelper.ToRadians(projectile.ai[0] * 12));
					circular.Y *= 0.5f;
					circular = circular.RotatedBy(projectile.rotation);
					Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 4, 4, DustID.Stone);
					dust.noGravity = true;
					dust.velocity *= 0.1f;
					dust.velocity += circular;
					dust.scale *= 1.2f;
				}
			}
			else
				projectile.tileCollide = true;
			Vector2 normalizedVelocity = projectile.velocity.SafeNormalize(Vector2.Zero);
			projectile.velocity += normalizedVelocity * 0.4f;
			//amogus :)
		}
	}
}
		
			