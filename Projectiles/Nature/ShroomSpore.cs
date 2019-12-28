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

namespace SOTS.Projectiles.Nature
{    
    public class ShroomSpore : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandy Water");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(512);
            aiType = 512;
			projectile.height = 16;
			projectile.width = 16;
			projectile.penetrate = 4;
			projectile.thrown = false;
			projectile.melee = true;
			projectile.magic = false;
			projectile.tileCollide = true;
			projectile.alpha = 100;
			projectile.timeLeft = 100;
		}
		public override void AI()
		{
			projectile.alpha = 260 - projectile.timeLeft;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(Main.rand.Next(7) == 0)
			target.AddBuff(BuffID.Confused, 90, false);
			
			projectile.damage = projectile.damage - (int)(projectile.damage * 0.5f);
			projectile.friendly = projectile.penetrate > 3;
		}
	}
}
		