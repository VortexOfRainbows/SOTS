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
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Berry Bomb");
			Main.projFrames[projectile.type] = 3;
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(48);
            aiType = 48; 
			projectile.thrown = false;
			projectile.magic = false;
			projectile.melee = false;
			projectile.ranged = true;
			projectile.width = 22;
			projectile.height = 26;
			projectile.penetrate = 1;
			projectile.timeLeft = 900;
			projectile.alpha = 0;
			projectile.hide = true;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 14;
			height = 14;
			return true;
        }
        public override void AI()
		{
			if(projectile.timeLeft >= 800)
			{
				projectile.spriteDirection = projectile.direction;
				projectile.hide = false;
				projectile.frame = Main.rand.Next(3);
				projectile.scale = 0.01f * Main.rand.Next(86, 121);
				projectile.timeLeft = Main.rand.Next(21, 43);
			}
		}
		public override void Kill(int timeLeft)
        {
			Vector2 position = projectile.Center;
			SoundEngine.PlaySound(SoundID.NPCHit1, (int)position.X, (int)position.Y);  
			for(int i = 0; i < 12; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 33);
				Main.dust[num1].noGravity = true;
			}
			if(projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < 3; i++)
				{ 
					Projectile proj = Projectile.NewProjectileDirect(projectile.Center, Main.rand.NextVector2Circular(3, 3), ProjectileID.SlimeGun, 0, projectile.owner);
					proj.timeLeft = Main.rand.Next(12, 24);
				}
			}
		}
	}
}
		
			