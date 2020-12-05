using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Celestial
{    
    public class CeremonialSlash : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ceremonial Slash");
		}
		
        public override void SetDefaults()
        {
			Main.projFrames[projectile.type] = Main.projFrames[595];  //28 proj frames
			projectile.width = 96;
			projectile.height = 60;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 64;
			projectile.alpha = 0;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < 8; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.55f;
				float y = Main.rand.Next(-10, 11) * 0.55f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y),
				new Rectangle(0, 60 * (((int)projectile.ai[0] + projectile.frame) % 28), 96, 60), color * (1f - (projectile.alpha / 255f)), projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation();
			projectile.frame++;
			projectile.alpha += 6;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 4;
			if(crit && damage > 40 && projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("HealProj"), 0, 0, projectile.owner, damage / 40, 7);
			}
        }
		public void LaunchLaser(Vector2 area)
		{
			Player player  = Main.player[projectile.owner];
			//Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("RedLaser"), projectile.damage, 0, projectile.owner, area.X, area.Y);
		}
	}
}
		
			