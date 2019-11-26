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
    public class NukeTurtle : ModProjectile 
    {	int count = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Normal Turtle Storm");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;    
			
		}
		
        public override void SetDefaults()
        {
			projectile.height = 24;
			projectile.width = 32;
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.timeLeft = 480;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.alpha = 0;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override void Kill(int timeLeft)
        {
			Player player = Main.player[projectile.owner];
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");			
			if(projectile.ai[0] != 1)
			{
				Vector2 position = projectile.Center;
				Main.PlaySound(SoundID.Item14, (int)position.X, (int)position.Y);  
				
				int gore = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), new Vector2(Main.rand.Next(-25,21), Main.rand.Next(-20,21)), mod.GetGoreSlot("Gores/NukeTurtleGore1"), projectile.scale);
				Main.gore[gore].velocity *= Main.rand.Next(70, 121) * 0.01f;
				
				gore = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), new Vector2(Main.rand.Next(-20,21), Main.rand.Next(-25,21)), mod.GetGoreSlot("Gores/NukeTurtleGore2"), projectile.scale);
				Main.gore[gore].velocity *= Main.rand.Next(70, 121) * 0.01f;
				
				gore = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), new Vector2(Main.rand.Next(-20,26), Main.rand.Next(-20,21)), mod.GetGoreSlot("Gores/NukeTurtleGore3"), projectile.scale);
				Main.gore[gore].velocity *= Main.rand.Next(70, 121) * 0.01f;
				
				gore = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), new Vector2(Main.rand.Next(-20,21), Main.rand.Next(-20,26)), mod.GetGoreSlot("Gores/NukeTurtleGore4"), projectile.scale);
				Main.gore[gore].velocity *= Main.rand.Next(70, 121) * 0.01f;
				
				if(Main.myPlayer == projectile.owner)
				{
					Projectile.NewProjectile(position.X, position.Y, 0, 0, mod.ProjectileType("StellarHitbox"), projectile.damage, projectile.knockBack, player.whoAmI);
				}
				int radius = 2; 
				for (int x = -radius; x <= radius; x++)
				{
					for (int y = -radius; y <= radius; y++)
					{
						int xPosition = (int)(x + position.X);
						int yPosition = (int)(y + position.Y);
		 
						if (Math.Sqrt(x * x + y * y) <= radius + 0.5)  
						{
							Dust.NewDust(new Vector2(xPosition, yPosition), 0, 0, 235);
						}
					}
				}
			}
		}
		bool off = false;
		int breh = -1;
		public override void AI()
		{
			breh = breh == -1 ? Main.rand.Next(90) : breh;
			NPC target = Main.npc[(int)projectile.ai[1]];
			Player player = Main.player[projectile.owner];
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.5f / 255f, (255 - projectile.alpha) * 1.1f / 255f, (255 - projectile.alpha) * 0.4f / 255f);
			if(projectile.ai[0] == 1 && projectile.timeLeft > 90)
			{
				Vector2 stormPos = new Vector2(1800, 0).RotatedBy(MathHelper.ToRadians(count * 11));
				Main.PlaySound(SoundID.Item16, (int)(projectile.Center.X - stormPos.X), (int)(projectile.Center.Y - stormPos.Y)); //fart sound
				if(Main.myPlayer == projectile.owner)
				{
					int shard = Projectile.NewProjectile(projectile.Center.X - stormPos.X, projectile.Center.Y - stormPos.Y, 0, 0, projectile.type, projectile.damage, projectile.knockBack, player.whoAmI);
					Main.projectile[shard].ai[1] = projectile.ai[1];
					Main.projectile[shard].timeLeft = 450;
					Main.projectile[shard].rotation = (float)(MathHelper.ToRadians(180) + Math.Atan2(stormPos.Y, stormPos.X));
				}
				count += Main.rand.Next(2, 5);
			}
			else if (projectile.ai[0] == 1)
			{
				projectile.alpha = 255;
			}
			projectile.rotation += 1.7f;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			float speed = 19.55f;
			if(target.active == true && target.whoAmI != -1 && off)
			{		
				dX = target.Center.X - projectile.Center.X;
				dY = target.Center.Y - projectile.Center.Y;
				distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
				speed /= distance;
				projectile.velocity.X *= Main.rand.Next(14, 31) * 0.034f;
				projectile.velocity.Y *= Main.rand.Next(14, 31) * 0.034f;
				projectile.velocity += new Vector2(dX * speed, dY * speed);
			}
			else
			{
				off = true;
				projectile.velocity.X *= Main.rand.Next(14, 31) * 0.03f;
				projectile.velocity.Y *= Main.rand.Next(14, 31) * 0.03f;
				if(breh >= projectile.timeLeft - 300 && projectile.ai[0] != 1)
				{
				projectile.Kill();
				}
			}
			for(int i = 0; i < 1000; i++)
			{
				Projectile proj = Main.projectile[i];
				if(proj.owner == player.whoAmI && proj.type ==  projectile.type && proj.ai[0] == 1 && projectile.ai[0] != 1 && breh >= proj.timeLeft - 6)
				{
					projectile.Kill();
				}
			}
		}
	}
}
	