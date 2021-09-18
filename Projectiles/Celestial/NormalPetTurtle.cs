using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Celestial
{    
    public class NormalPetTurtle : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Turtle Tem");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
			Main.projFrames[projectile.type] = 1;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.LightPet[projectile.type] = true;
		}
        public override void SetDefaults()
        {
            projectile.CloneDefaults(197);
            aiType = 197; //bone key
			projectile.netImportant = true;
            projectile.width = 32;
            projectile.height = 24; 
            projectile.timeLeft = 30;
            projectile.penetrate = -1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
			projectile.alpha = 0;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
			player.skeletron = false; 
            return true;
        }
		int counter = 0;
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.5f / 255f, (255 - projectile.alpha) * 1.1f / 255f, (255 - projectile.alpha) * 0.4f / 255f);
			projectile.rotation += 0.0034f;
			counter++;
			projectile.alpha -= counter % 25 == 0 ? 1 : 0;
			for(int i = 0; i < 200; i++)
			{
				NPC target = Main.npc[i];
	
				float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
				float shootToY = target.position.Y - projectile.Center.Y;
				float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
	
				if(distance < 360f && !target.friendly && target.active && target.lifeMax > Main.rand.Next(3000000) && projectile.alpha < 30 && target.CanBeChasedBy())
				{
					distance = 3f / distance;
				
					shootToX *= distance * 5;
					shootToY *= distance * 5;
					projectile.alpha = 255;
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("NukeTurtle"), 0, 1f, player.whoAmI, 1, target.whoAmI);
				}
			}
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
            if (player.dead || (player.ownedProjectileCounts[mod.ProjectileType("NormalPetTurtle")] > 1 && projectile.alpha < 50))
            {
                modPlayer.TurtleTem = false;
            }
            if (modPlayer.TurtleTem || projectile.alpha >= 50)
            {
                projectile.timeLeft = 6;
            }
		}
	}
}