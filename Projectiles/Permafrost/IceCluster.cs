using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;

namespace SOTS.Projectiles.Permafrost
{    
    public class IceCluster : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Cluster");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(48);
            aiType = 48;
			projectile.ranged = true;
			projectile.thrown = false;
			projectile.penetrate = 1;
			projectile.width = 18;
			projectile.height = 18;
		}
		bool runOnce = true;
		bool alt = false;
		int counter = 0;
        public override bool PreAI()
        {
			if(runOnce)
            {
				if (projectile.ai[0] != -1)
					alt = true;
				runOnce = false;
            }
            return base.PreAI();
        }
        public override void AI()
		{
			counter++;
			if(counter == 4 && !alt)
            {
				for(float j = 0; j <= 0.1f; j += 0.1f)
					for(int i = 0; i < 360; i += 10)
					{
						Vector2 circular = new Vector2(2 + 4 * j, 0).RotatedBy(MathHelper.ToRadians(i));
						circular.X *= 0.5f;
						circular = circular.RotatedBy(projectile.velocity.ToRotation());
						int snow = Dust.NewDust(projectile.Center + projectile.velocity * j + circular - new Vector2(5), 0, 0, alt ? ModContent.DustType<ModIceDust>() : ModContent.DustType<ModSnowDust>());
						Main.dust[snow].noGravity = true;
						Main.dust[snow].velocity *= 0f;
						Main.dust[snow].velocity += 0.6f * circular;
					}
            }
			if(Main.rand.NextBool(2) && (counter >= 5 || alt))
			{
				int snow = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(5), projectile.width, projectile.height, alt ? ModContent.DustType<ModIceDust>() : ModContent.DustType<ModSnowDust>());
				Main.dust[snow].noGravity = true;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = alt ? Mod.Assets.Request<Texture2D>("Projectiles/Permafrost/IceClusterAlt").Value : Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, lightColor, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void Kill(int timeLeft)
        {
			if(projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("IcePulse"), projectile.damage, 0, projectile.owner, alt ? -1 : 0);
				for (int i = 0; i < 15; i++)
				{
					int snow = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, alt ? ModContent.DustType<ModIceDust>() : ModContent.DustType<ModSnowDust>());
					Main.dust[snow].noGravity = true;
					Main.dust[snow].velocity *= 2;
					Main.dust[snow].scale *= 1.2f;
				}
			}
		}
	}
}
		
			