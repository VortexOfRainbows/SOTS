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
    public class BlockBeam : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brass Beam");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(260);
            aiType = 260; //18 is the demon scythe style
			projectile.alpha = 255;
			projectile.timeLeft = 180;
		}
		public override void AI()
		{
			projectile.alpha = 255;
		}
		public override void Kill(int timeLeft)
        {
			int explosionRadius = 2;
			int minTileX = (int)(projectile.position.X / 16f - (float)explosionRadius);
			int maxTileX = (int)(projectile.position.X / 16f + (float)explosionRadius);
			int minTileY = (int)(projectile.position.Y / 16f - (float)explosionRadius);
			int maxTileY = (int)(projectile.position.Y / 16f + (float)explosionRadius);
			if (minTileX < 0)
			{
				minTileX = 0;
			}
			if (maxTileX > Main.maxTilesX)
			{
				maxTileX = Main.maxTilesX;
			}
			if (minTileY < 0)
			{
				minTileY = 0;
			}
			if (maxTileY > Main.maxTilesY)
			{
				maxTileY = Main.maxTilesY;
			}
			bool canKillWalls = false;
			for (int x = minTileX; x <= maxTileX; x++)
			{
				for (int y = minTileY; y <= maxTileY; y++)
				{
					float diffX = Math.Abs((float)x - projectile.position.X / 16f);
					float diffY = Math.Abs((float)y - projectile.position.Y / 16f);
					double distance = Math.Sqrt((double)(diffX * diffX + diffY * diffY));
					if (distance < (double)explosionRadius && Main.tile[x, y] != null && Main.tile[x, y].wall == 0)
					{
						canKillWalls = true;
						break;
					}
				}
			}
			for (int i = minTileX; i <= maxTileX; i++)
			{
				for (int j = minTileY; j <= maxTileY; j++)
				{
					float diffX = Math.Abs((float)i - projectile.position.X / 16f);
					float diffY = Math.Abs((float)j - projectile.position.Y / 16f);
					double distanceToTile = Math.Sqrt((double)(diffX * diffX + diffY * diffY));
					if (distanceToTile < (double)explosionRadius)
					{
						bool canKillTile = true;
						if (Main.tile[i, j] != null && Main.tile[i, j].active())
						{
							canKillTile = true;
							if (Main.tileDungeon[(int)Main.tile[i, j].type] || Main.tile[i, j].type == 88 || Main.tile[i, j].type == 21 || Main.tile[i, j].type == 26 || Main.tile[i, j].type == 107 || Main.tile[i, j].type == 108 || Main.tile[i, j].type == 111 || Main.tile[i, j].type == 226 || Main.tile[i, j].type == 237 || Main.tile[i, j].type == 221 || Main.tile[i, j].type == 222 || Main.tile[i, j].type == 223 || Main.tile[i, j].type == 211 || Main.tile[i, j].type == 404)
							{
								canKillTile = false;
							}
							if (!Main.hardMode && Main.tile[i, j].type == 58)
							{
								canKillTile = false;
							}
							if (!TileLoader.CanExplode(i, j))
							{
								canKillTile = false;
							}
							if (canKillTile)
							{
								WorldGen.KillTile(i, j, false, false, false);
								if (!Main.tile[i, j].active() && Main.netMode != 0)
								{
									NetMessage.SendData(17, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
								}
							}
						}
						if (canKillTile)
						{
							for (int x = i - 1; x <= i + 1; x++)
							{
								for (int y = j - 1; y <= j + 1; y++)
								{
									if(Main.tile[x, y] != null && Main.tile[x, y].wall > 0)
									{
										if (Main.tile[x, y].wall == 0 && Main.netMode != 0)
										{
											NetMessage.SendData(17, -1, -1, null, 2, (float)x, (float)y, 0f, 0, 0, 0);
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
		
			