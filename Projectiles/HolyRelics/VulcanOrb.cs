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

namespace SOTS.Projectiles.HolyRelics
{    
    public class VulcanOrb : ModProjectile 
    {	int bounce = 24;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vulcan Orb");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(263);
            aiType = 263; 
			projectile.height = 30;
			projectile.width = 30;
			projectile.friendly = false;
			projectile.timeLeft = 90;
			projectile.tileCollide = false;
			projectile.hostile = false;
		}
		public override void AI()
		{
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 30, 30, 160);
		}
		public override void Kill(int timeLeft)
		{
		
		Player player = Main.player[projectile.owner];
        SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
		
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 2, 0, mod.ProjectileType("VulcanSplit"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 2, mod.ProjectileType("VulcanSplit"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -2, 0, mod.ProjectileType("VulcanSplit"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, -2, mod.ProjectileType("VulcanSplit"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
				
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 1.6f, 1.6f, mod.ProjectileType("VulcanSplit"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 1.6f, -1.6f, mod.ProjectileType("VulcanSplit"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -1.6f, 1.6f, mod.ProjectileType("VulcanSplit"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -1.6f, -1.6f, mod.ProjectileType("VulcanSplit"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
				
		if(modPlayer.libraActive * projectile.damage == projectile.damage)
		{
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 3, 0, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 3, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -3, 0, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, -3, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 2, 2, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -2, -2, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -2, 2, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 2, -2, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 2.5f, 1.5f, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 1.5f, 2.5f, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -2.5f, 1.5f, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -1.5f, 2.5f, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 2.5f, -1.5f, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 1.5f, -2.5f, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -2.5f, -1.5f, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -1.5f, -2.5f, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 2.8f, 1f, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 1f, 2.8f, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -2.8f, 1f, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -1f, 2.8f, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 2.8f, -1f, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 1f, -2.8f, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -2.8f, -1f, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -1f, -2.8f, mod.ProjectileType("BackupArrow"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		
		
			
		}
		}
	}
}
		