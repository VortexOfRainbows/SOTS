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
    public class Shardstorm : ModProjectile 
    {	int count = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shardstorm");
			
		}
		
        public override void SetDefaults()
        {
			projectile.aiStyle = 0;
			projectile.height = 34;
			projectile.width = 32;
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.timeLeft = 49;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.alpha = 255;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 7;
        }
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.3f / 255f, (255 - projectile.alpha) * 0.1f / 255f, (255 - projectile.alpha) * 0.9f / 255f);
			if(projectile.ai[0] == 0)
			{
				if(projectile.timeLeft % 2 == 0) //will activate 24 times
				{
					Vector2 stormPos = new Vector2(348, 0).RotatedBy(MathHelper.ToRadians(count * 30));
					Main.PlaySound(SoundID.Item44, (int)(projectile.Center.X - stormPos.X), (int)(projectile.Center.Y - stormPos.Y));
					if(Main.myPlayer == projectile.owner)
					{
						int shard = Projectile.NewProjectile(projectile.Center.X - stormPos.X, projectile.Center.Y - stormPos.Y, 0, 0, projectile.type, projectile.damage, projectile.knockBack, player.whoAmI, 1, (float)(MathHelper.ToRadians(180) + Math.Atan2(stormPos.Y, stormPos.X)));
					}
					count++;
				}
			}
			else
			{
				projectile.timeLeft = projectile.timeLeft < 100 ? 720 : projectile.timeLeft;
				if(projectile.timeLeft >= 702)
				{
					projectile.alpha -= 15;
					projectile.ai[1] += MathHelper.ToRadians(10);
					projectile.velocity = new Vector2(8, 0).RotatedBy(projectile.ai[1]);
				}
				else
				{
					projectile.timeLeft--;
					projectile.friendly = true;
					projectile.penetrate = 2;
					projectile.velocity = new Vector2(24, 0).RotatedBy(projectile.ai[1]);
					projectile.alpha += 6;
					if(projectile.timeLeft <= 610)
					{
						projectile.tileCollide = true;
						projectile.Kill();
					}
				}
			}
			projectile.rotation = projectile.ai[1];
		}
	}
}
	