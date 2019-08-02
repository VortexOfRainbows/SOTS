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
    public class VulcanDetonate : ModProjectile 
    {	int bounce = 24;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vulcan");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(263);
            aiType = 263; 
			projectile.width = (int)(Main.screenWidth * 1.5f);
			projectile.height = (int)(Main.screenHeight * 1.5f);
			projectile.friendly = true;
			projectile.timeLeft = 5;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
		}
		public override void AI()
		{
			projectile.alpha = 255;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			damage += (int)((target.lifeMax/200));
			damage += (int)(target.life/10);
			if(target.boss == true)
			{
				damage += (int)((target.lifeMax/80));
			}
			damage += target.damage;
			damage += target.defense;
        }
		public override bool? CanCutTiles()
		{
			return false;
		}

	}
}
		