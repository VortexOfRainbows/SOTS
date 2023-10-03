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
using Terraria.Audio;

namespace SOTS.Projectiles.Nature
{    
    public class BerryBomb : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Berry Bomb");
			Main.projFrames[Projectile.type] = 3;
		}
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(48);
            AIType = 48; 
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 22;
			Projectile.height = 26;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 900;
			Projectile.alpha = 0;
			Projectile.hide = true;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 14;
			height = 14;
			return true;
        }
        public override void AI()
		{
			if(Projectile.timeLeft >= 800)
			{
				Projectile.spriteDirection = Projectile.direction;
				Projectile.hide = false;
				Projectile.frame = Main.rand.Next(3);
				Projectile.scale = 0.01f * Main.rand.Next(86, 121);
				Projectile.timeLeft = Main.rand.Next(21, 43);
			}
		}
		public override void OnKill(int timeLeft)
        {
			Vector2 position = Projectile.Center;
			SoundEngine.PlaySound(SoundID.NPCHit1, position);  
			for(int i = 0; i < 12; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Water);
				Main.dust[num1].noGravity = true;
			}
			if(Projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < 3; i++)
				{ 
					Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Circular(3, 3), ProjectileID.SlimeGun, 0, Projectile.owner);
					proj.timeLeft = Main.rand.Next(12, 24);
				}
			}
		}
	}
}
		
			