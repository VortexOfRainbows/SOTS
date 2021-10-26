using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Evil
{    
    public class EvilBolt : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Umbra Ball");	
		}
        public override void SetDefaults()
        {
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.alpha = 0;
			projectile.width = 32;
			projectile.height = 32;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 180;
			projectile.alpha = 255;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 24;
			hitbox = new Rectangle((int)projectile.Center.X - width/2, (int)projectile.Center.Y - width/2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = VoidPlayer.EvilColor;
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(1, 1), null, projectile.GetAlpha(color), projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		bool runOnce = true;
		public override void AI()
		{
			if (runOnce)
			{
				DustOut();
				projectile.scale = 0.1f;
				projectile.alpha = 0;
				runOnce = false;
			}
			else if (projectile.scale < 1f)
				projectile.scale += 0.05f;
			if(projectile.timeLeft < 42)
            {
				projectile.alpha += 6;
			}
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			projectile.velocity += projectile.velocity.SafeNormalize(Vector2.Zero) * 0.125f;
			if(Main.rand.NextBool(8))
			{
				Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.RainbowMk2, Main.rand.NextVector2Circular(1, 1));
				dust.velocity *= 0.5f;
				dust.velocity -= projectile.velocity * 0.05f;
				dust.color = VoidPlayer.EvilColor;
				dust.color.A = 100;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.25f;
			}
		}
		public override void Kill(int timeLeft)
		{
			DustOut();
		}
		public void DustOut()
        {
			for (int i = 0; i < 360; i += 60)
			{
				Vector2 circularLocation = new Vector2(Main.rand.NextFloat(4), 0).RotatedBy(MathHelper.ToRadians(i) + projectile.rotation);
				int dust2 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, DustID.RainbowMk2);
				Dust dust = Main.dust[dust2];
				dust.velocity = circularLocation * 0.4f;
				dust.velocity += projectile.velocity * 0.2f;
				dust.color = VoidPlayer.EvilColor;
				dust.color.A = 100;
				dust.noGravity = true;
				dust.alpha = 100;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
			}
		}
	}
}