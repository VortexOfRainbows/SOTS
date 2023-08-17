using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace SOTS.Projectiles 
{    
    public class BoreBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/BoreBulletTrail");
			Vector2 drawOrigin = new Vector2(texture.Width/2, texture.Height/2);
			Vector2 lastPosition = Projectile.Center;
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				float scale = 1f - 0.5f *(k / (float)Projectile.oldPos.Length);
				Vector2 drawPos = Projectile.oldPos[k] + new Vector2(Projectile.width/2, Projectile.height/2);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				color.A = 190;
				float lengthTowards = Vector2.Distance(lastPosition, drawPos) / texture.Height / scale;
				Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, color * scale * 0.8f, Projectile.rotation, drawOrigin, new Vector2(1, lengthTowards) * scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				lastPosition = drawPos;
			}
			return true;
		}
		public override void SetDefaults()
        {
			Projectile.CloneDefaults(ProjectileID.Bullet);
			Projectile.light *= 0.5f;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 6;
			Projectile.width = 10;
			Projectile.height = 24;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 240;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 8;
			height = 8;
            return true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.ai[0] = -2;
			Projectile.netUpdate = true;
		}
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			modifiers.ScalingArmorPenetration += 0.2f;
			modifiers.ArmorPenetration += 10;
		}
        public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 20; i++)
			{
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 4, 4, DustID.Stone);
				dust.noGravity = true;
				dust.velocity *= 0.9f;
				dust.velocity += Projectile.oldVelocity.SafeNormalize(Vector2.Zero) * 0.4f;
				dust.scale *= 1.4f;
			}
			base.Kill(timeLeft);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if (Projectile.penetrate > 2)
				Projectile.penetrate--;
			else
			{
				Projectile.Kill();
				return true;
            }
			Projectile.ai[0] = -1;
			Projectile.netUpdate = true;
			Projectile.velocity = oldVelocity;
            return false;
        }
        bool runOnce = true;
        public override void AI()
		{
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			if (Projectile.alpha > 0)
				Projectile.alpha -= 20;
			else
				Projectile.alpha = 0;
			if(runOnce || Projectile.ai[0] < 0)
            {
				if(!runOnce)
                {
					if (Projectile.ai[0] == -2)
						Projectile.ai[0] = 15;
					else
						Projectile.ai[0] = 30;
					SOTSUtils.PlaySound(SoundID.Item22, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.75f, 0.1f);
				}
				Projectile.velocity *= 0.04f;
				runOnce = false;
            }
			if (Projectile.ai[0] > 0)
			{
				Projectile.tileCollide = false;
				Projectile.ai[0]--;
				Projectile.velocity *= 0.5f;
				for (int i = -1; i <= 1; i += 2)
				{
					Vector2 circular = new Vector2(2 * i, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[0] * 12));
					circular.Y *= 0.5f;
					circular = circular.RotatedBy(Projectile.rotation);
					Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 4, 4, DustID.Stone);
					dust.noGravity = true;
					dust.velocity *= 0.1f;
					dust.velocity += circular;
					dust.scale *= 1.2f;
				}
			}
			else
				Projectile.tileCollide = true;
			Vector2 normalizedVelocity = Projectile.velocity.SafeNormalize(Vector2.Zero);
			Projectile.velocity += normalizedVelocity * 0.4f;
			//amogus :)
		}
	}
}
		
			