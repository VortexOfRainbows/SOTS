using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Permafrost
{    
    public class FrostSpear : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Spear");	
		}
        public override void SetDefaults()
        {
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.alpha = 0;
			projectile.width = 14;
			projectile.height = 22;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 40;
			projectile.alpha = 0;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 60;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 40;
			hitbox = new Rectangle((int)projectile.Center.X - width/2, (int)projectile.Center.Y - width/2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.NextFloat(-1, 1);
				float y = Main.rand.NextFloat(-1, 1);
				Main.spriteBatch.Draw(texture, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, projectile.GetAlpha(color), projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void AI()
		{
			if(projectile.timeLeft < 27)
            {
				projectile.tileCollide = true;
				projectile.alpha += 8;
			}
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			projectile.velocity *= 1.04f;
			projectile.velocity.Y += 0.1f;
			if(!Main.rand.NextBool(4))
			{
				Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.RainbowMk2, Main.rand.NextVector2Circular(1, 1));
				dust.velocity *= 0.5f;
				dust.velocity -= projectile.velocity * 0.05f;
				dust.color = new Color(180 - Main.rand.Next(50), 190 - Main.rand.Next(50), 250, 150);
				dust.alpha = 100;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.25f;
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 50, 0.7f, 0.3f);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circularLocation = new Vector2(Main.rand.NextFloat(4), 0).RotatedBy(MathHelper.ToRadians(i) + projectile.rotation);
				int dust2 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, DustID.RainbowMk2);
				Dust dust = Main.dust[dust2];
				dust.velocity = circularLocation * 0.4f;
				dust.velocity += projectile.velocity * 0.2f;
				dust.color = new Color(180 - Main.rand.Next(50), 190 - Main.rand.Next(50), 250, 150);
				dust.noGravity = true;
				dust.alpha = 100;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
			}
		}
	}
}