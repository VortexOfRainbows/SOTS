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

namespace SOTS.Projectiles.Celestial
{    
    public class GreenShockBlast : ModProjectile 
    {
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursewave");
			
		}
		
        public override void SetDefaults()
        {
			projectile.width = 18;
			projectile.height = 38;
			projectile.friendly = false;
			projectile.timeLeft = 720;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.alpha = 35;
		}
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.1f / 255f, (255 - projectile.alpha) * 0.9f / 255f, (255 - projectile.alpha) * 0.3f / 255f);
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
		}
		public override void OnHitPlayer(Player target, int damage, bool crit) 
		{
			target.AddBuff(39, 240, false); //cursed flames
		}
	}
}
		