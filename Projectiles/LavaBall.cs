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

namespace SOTS.Projectiles 
{    
    public class LavaBall : ModProjectile 
    {	int enemybore = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lava Ball");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; 
			projectile.penetrate = 1;
			projectile.width = 8;
			projectile.height = 8;


		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			damage += (int)(target.defense * 0.5f);
		}
		public override void AI()
		{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 8, 8, 6);
			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			
			
		}
	}
}
		
			