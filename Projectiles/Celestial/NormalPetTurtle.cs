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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Main.projFrames[Projectile.type] = 1;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.LightPet[Projectile.type] = true;
		}
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(197);
            aiType = 197; //bone key
			Projectile.netImportant = true;
            Projectile.width = 32;
            Projectile.height = 24; 
            Projectile.timeLeft = 30;
            Projectile.penetrate = -1; 
            Projectile.friendly = false; 
            Projectile.hostile = false; 
			Projectile.alpha = 0;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
			player.skeletron = false; 
            return true;
        }
		int counter = 0;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 1.1f / 255f, (255 - Projectile.alpha) * 0.4f / 255f);
			Projectile.rotation += 0.0034f;
			counter++;
			Projectile.alpha -= counter % 25 == 0 ? 1 : 0;
			for(int i = 0; i < 200; i++)
			{
				NPC target = Main.npc[i];
	
				float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
				float shootToY = target.position.Y - Projectile.Center.Y;
				float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
	
				if(distance < 360f && !target.friendly && target.active && target.lifeMax > Main.rand.Next(3000000) && Projectile.alpha < 30 && target.CanBeChasedBy())
				{
					distance = 3f / distance;
				
					shootToX *= distance * 5;
					shootToY *= distance * 5;
					Projectile.alpha = 255;
					Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("NukeTurtle"), 0, 1f, player.whoAmI, 1, target.whoAmI);
				}
			}
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
            if (player.dead || (player.ownedProjectileCounts[mod.ProjectileType("NormalPetTurtle")] > 1 && Projectile.alpha < 50))
            {
                modPlayer.TurtleTem = false;
            }
            if (modPlayer.TurtleTem || Projectile.alpha >= 50)
            {
                Projectile.timeLeft = 6;
            }
		}
	}
}