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
    public class VulcanSplit2 : ModProjectile 
    {	int bounce = 24;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vulcan Split");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(275);
            aiType = 275; 
			projectile.height = 14;
			projectile.width = 14;
			projectile.friendly = false;
			projectile.timeLeft = 30;
			projectile.hostile = false;
		}
		public override void AI()
		{
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 14, 14, 160);
		}
		public override void Kill(int timeLeft)
		{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("VulcanDetonate"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
		
		}
	}
}
		