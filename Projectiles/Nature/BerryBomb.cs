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
    public class BerryBomb : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Berry Bomb");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(48);
            aiType = 48; 
			projectile.thrown = false;
			projectile.magic = false;
			projectile.melee = false;
			projectile.ranged = true;
			projectile.width = 14;
			projectile.height = 14;
			projectile.penetrate = 1;
			projectile.timeLeft = 900;
			projectile.alpha = 0;
		}
		public override void AI()
		{
			if(projectile.timeLeft >= 800)
			{
				projectile.scale = 0.01f * Main.rand.Next(86, 121);
				projectile.timeLeft = Main.rand.Next(21, 43);
			}
		}
		public override void Kill(int timeLeft)
        {
			Vector2 position = projectile.Center;
			Main.PlaySound(SoundID.NPCHit1, (int)position.X, (int)position.Y);  
			for(int i = 0; i < 12; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 33);
				Main.dust[num1].noGravity = true;
			}
			if(projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < 3; i++)
				{ 
					int proj = Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-100, 101) * 0.03f, Main.rand.Next(-100, 101) * 0.03f, 406, (int)(projectile.damage * 0.7f) + 1, 0, projectile.owner); //slime gun
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].hostile = false;
					Main.projectile[proj].timeLeft = Main.rand.Next(12, 24);
				}
			}
		}
	}
}
		
			