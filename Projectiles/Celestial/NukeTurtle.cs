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
using Terraria.Graphics.Shaders;

namespace SOTS.Projectiles.Celestial
{    
    public class NukeTurtle : ModProjectile 
    {	int count = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Normal Turtle Storm");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.LightPet[Projectile.type] = true;
		}
		
        public override void SetDefaults()
        {
			Projectile.height = 24;
			Projectile.width = 32;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.timeLeft = 480;
			Projectile.tileCollide = false;
			Projectile.minion = false;
			Projectile.alpha = 0;
		}
		public override bool PreAI()
		{
			return true;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override void Kill(int timeLeft)
        {
			Player player = Main.player[Projectile.owner];
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");			
			if(Projectile.ai[0] != 1)
			{
				Vector2 position = Projectile.Center;
				SoundEngine.PlaySound(SoundID.Item14, (int)position.X, (int)position.Y);  
				
				int gore = Gore.NewGore(new Vector2(Projectile.position.X, Projectile.position.Y), new Vector2(Main.rand.Next(-25,21), Main.rand.Next(-20,21)), mod.GetGoreSlot("Gores/NukeTurtleGore1"), Projectile.scale);
				Main.gore[gore].velocity *= Main.rand.Next(70, 121) * 0.01f;
				
				gore = Gore.NewGore(new Vector2(Projectile.position.X, Projectile.position.Y), new Vector2(Main.rand.Next(-20,21), Main.rand.Next(-25,21)), mod.GetGoreSlot("Gores/NukeTurtleGore2"), Projectile.scale);
				Main.gore[gore].velocity *= Main.rand.Next(70, 121) * 0.01f;
				
				gore = Gore.NewGore(new Vector2(Projectile.position.X, Projectile.position.Y), new Vector2(Main.rand.Next(-20,26), Main.rand.Next(-20,21)), mod.GetGoreSlot("Gores/NukeTurtleGore3"), Projectile.scale);
				Main.gore[gore].velocity *= Main.rand.Next(70, 121) * 0.01f;
				
				gore = Gore.NewGore(new Vector2(Projectile.position.X, Projectile.position.Y), new Vector2(Main.rand.Next(-20,21), Main.rand.Next(-20,26)), mod.GetGoreSlot("Gores/NukeTurtleGore4"), Projectile.scale);
				Main.gore[gore].velocity *= Main.rand.Next(70, 121) * 0.01f;
				
				if(Main.myPlayer == Projectile.owner)
				{
					Projectile.NewProjectile(position.X, position.Y, 0, 0, mod.ProjectileType("StellarHitbox"), 110, Projectile.knockBack, player.whoAmI);
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
			Player player = Main.player[Projectile.owner];
			breh = breh == -1 ? Main.rand.Next(90) : breh;
			NPC target = Main.npc[(int)Projectile.ai[1]];
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 1.1f / 255f, (255 - Projectile.alpha) * 0.4f / 255f);
			if(Projectile.ai[0] == 1 && Projectile.timeLeft > 90)
			{
				Vector2 stormPos = new Vector2(1800, 0).RotatedBy(MathHelper.ToRadians(count * 11));
				SoundEngine.PlaySound(SoundID.Item16, (int)(Projectile.Center.X - stormPos.X), (int)(Projectile.Center.Y - stormPos.Y)); //fart sound
				if(Main.myPlayer == Projectile.owner)
				{
					int shard = Projectile.NewProjectile(Projectile.Center.X - stormPos.X, Projectile.Center.Y - stormPos.Y, 0, 0, Projectile.type, 0, Projectile.knockBack, player.whoAmI);
					Main.projectile[shard].ai[1] = Projectile.ai[1];
					Main.projectile[shard].timeLeft = 450;
					Main.projectile[shard].rotation = (float)(MathHelper.ToRadians(180) + Math.Atan2(stormPos.Y, stormPos.X));
				}
				count += Main.rand.Next(2, 5);
			}
			else if (Projectile.ai[0] == 1)
			{
				Projectile.alpha = 255;
			}
			Projectile.rotation += 1.7f;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			float speed = 19.55f;
			if(target.active == true && target.whoAmI != -1 && off)
			{		
				dX = target.Center.X - Projectile.Center.X;
				dY = target.Center.Y - Projectile.Center.Y;
				distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
				speed /= distance;
				Projectile.velocity.X *= Main.rand.Next(14, 31) * 0.034f;
				Projectile.velocity.Y *= Main.rand.Next(14, 31) * 0.034f;
				Projectile.velocity += new Vector2(dX * speed, dY * speed);
			}
			else
			{
				off = true;
				Projectile.velocity.X *= Main.rand.Next(14, 31) * 0.03f;
				Projectile.velocity.Y *= Main.rand.Next(14, 31) * 0.03f;
				if(breh >= Projectile.timeLeft - 300 && Projectile.ai[0] != 1)
				{
				Projectile.Kill();
				}
			}
			for(int i = 0; i < 1000; i++)
			{
				Projectile proj = Main.projectile[i];
				if(proj.owner == player.whoAmI && proj.type ==  Projectile.type && proj.ai[0] == 1 && Projectile.ai[0] != 1 && breh >= proj.timeLeft - 6)
				{
					Projectile.Kill();
				}
			}
		}
	}
}
	