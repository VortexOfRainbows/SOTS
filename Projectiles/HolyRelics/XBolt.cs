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
    public class XBolt : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("X Bolt");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(616);
            aiType = 616; //18 is the demon scythe style
			projectile.alpha = 255;
			projectile.timeLeft = 9000;
			projectile.width = 1;
			projectile.height = 1;


		}
		
		public override void AI()
		{projectile.alpha = 255;
			wait += 1;
			if(wait >= 12)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 1, 1, 220);

			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			}
			
			projectile.velocity.Y += (float)(0.15f * (Main.rand.Next(-3, 4)));
			
			projectile.velocity.X += (float)(0.15f * (Main.rand.Next(-3, 4)));
			
			
			
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.lifeMax -= damage;
			Player player = Main.player[projectile.owner];
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
				if(player.statLife == player.statLifeMax2 && player.FindBuffIndex(mod.BuffType("HeartDelay2")) <= -1)
				{
				modPlayer.HeartSwapBonus += damage;
				}
				else
				{
				player.statLife += damage;
				}
				player.HealEffect(damage);
				
				  projectile.Kill();
			
		}
			
		
	}
}
		
			