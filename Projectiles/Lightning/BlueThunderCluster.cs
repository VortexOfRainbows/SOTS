using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Lightning
{    
    public class BlueThunderCluster : ModProjectile 
    {	int bounce = 24;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thunder Cluster");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(263);
            aiType = 263; 
			projectile.height = 58;
			projectile.width = 58;
			projectile.penetrate = 24;
			projectile.friendly = true;
			projectile.timeLeft = 150;
			projectile.tileCollide = false;
			projectile.hostile = false;
		}
		public override void AI()
		{
			projectile.alpha++;
			for(int i = 0; i < 360; i += 15)
			{
			Vector2 circularLocation = new Vector2(-28, 0).RotatedBy(MathHelper.ToRadians(i));
			
			int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 56);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 15;
        }
		public override void Kill(int timeLeft)
		{
		
		Player player = Main.player[projectile.owner];
        SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
		
		Main.PlaySound(SoundID.Item94, (int)(projectile.Center.X), (int)(projectile.Center.Y));
			if(projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 2, 0, mod.ProjectileType("BlueLightning"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 2, mod.ProjectileType("BlueLightning"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -2, 0, mod.ProjectileType("BlueLightning"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, -2, mod.ProjectileType("BlueLightning"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
						
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 1.6f, 1.6f, mod.ProjectileType("BlueLightning"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 1.6f, -1.6f, mod.ProjectileType("BlueLightning"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -1.6f, 1.6f, mod.ProjectileType("BlueLightning"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -1.6f, -1.6f, mod.ProjectileType("BlueLightning"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
			}
		}
	}
}
		