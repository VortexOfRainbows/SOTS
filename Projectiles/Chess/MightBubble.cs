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

namespace SOTS.Projectiles.Chess
{    
    public class MightBubble : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Might Bubble");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; //18 is the demon scythe style
			projectile.width = 96;
			projectile.height = 96;
			projectile.penetrate = 1;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.timeLeft = 600;
		}
		
		public override void AI()
		{
			
			projectile.alpha = 120;
			
}
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.statLife += 5;
			player.HealEffect(5);
			player.velocity.Y -= 10;
			projectile.timeLeft = 2;
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 409, 45, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 409, 45, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 409, 45, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 409, 45, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 409, 45, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 409, 45, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 409, 45, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 409, 45, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 409, 45, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 409, 45, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 409, 45, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 409, 45, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 409, 45, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 409, 45, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 409, 45, 1, 0);
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 409, 45, 1, 0);
}
	}
}
		
			