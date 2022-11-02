using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SOTS.WorldgenHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Base
{    
    public class LazyMinerProjectile : ModProjectile 
    {	
        public static bool PlayerOwnsLazyMiner(Player owner)
        {
            for(int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if(proj.active && proj.owner == owner.whoAmI && proj.type == ModContent.ProjectileType<LazyMinerProjectile>() && proj.ai[0] != -1)
                {
                    return true;
                }
            }
            return false;
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("belphagor");
		}
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = -1; 
            Projectile.friendly = false; 
            Projectile.tileCollide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            float alphaMult = Math.Clamp(initializeCounter / 6f, 0, 1f);
            for (int k = 0; k < tileLocations.Count; k++)
            {
                Main.spriteBatch.Draw(texture, tileLocations[k].ToVector2() * 16 - Main.screenPosition, null, new Color(100, 100, 100, 0) * alphaMult, Projectile.rotation, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            return false;
        }
        public List<Point> tileLocations = new List<Point>();
        bool playerHasReleasedChannel => Projectile.ai[0] == -1;
        float initializeCounter = -2;
		public override void AI() //The projectile's AI/ what the projectile does
        {
            Player player = Main.player[Projectile.owner];
            Projectile.Center = player.Center;
            initializeCounter++;
            if (player.whoAmI == Main.myPlayer)
            {
                Projectile.timeLeft = 1800;
                if (Main.mouseLeftRelease && !playerHasReleasedChannel && initializeCounter > 3)
                {
                    Projectile.ai[0] = -1;
                    Projectile.netUpdate = true;
                    return;
                }
                else if (playerHasReleasedChannel)
                {
                    Projectile.timeLeft = 1800;
                    if (tileLocations.Count > 0) //this way it does it every frame
                    {
                        Point pt = tileLocations[0];
                        int x = pt.X;
                        int y = pt.Y;
                        WorldGen.KillTile(x, y);
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, x, y);
                        }
                        tileLocations.RemoveAt(0);
                    }
                    if(tileLocations.Count <= 0)
                    {
                        Projectile.Kill();
                    }
                }
            }
		}
        public void FindNextTile(int x, int y, int type, int playerDir = 1)
        {
            if (!doesContainPointAt(x, y))
            {
                tileLocations.Add(new Point(x, y));
                return;
            }
            tileLocations.Sort(new SortByDistanceToTile(x, y));
            bool foundATile = false;
            for (int k = 0; k < tileLocations.Count; k++)
            {
                Point t = tileLocations[k];
                if(!SOTSWorldgenHelper.TrueTileSolid(t.X, t.Y, true))
                {
                    tileLocations.RemoveAt(k);
                    k--;
                }
                else if (!foundATile)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        int tX = t.X;
                        int tY = t.Y;
                        if (i == 1 || i == 2)
                            tX += playerDir * (i == 1 ? 1 : -1);
                        else if (i == 0)
                            tY -= 1;
                        else
                            tY += 1;
                        Tile tile = Main.tile[tX, tY];
                        if (!doesContainPointAt(tX, tY) && tile.TileType == type && SOTSWorldgenHelper.TrueTileSolid(tX, tY, true))
                        {
                            tileLocations.Add(new Point(tX, tY));
                            foundATile = true;
                            break;
                        }
                    }
                }
            }
            if(!foundATile || tileLocations.Count >= 50)
            {
                Projectile.ai[0] = -1;
                Projectile.netUpdate = true;
            }
            tileLocations.Sort(new SortByDistanceToTile(x, y));
        }
        public bool doesContainPointAt(int x, int y)
        {
            for (int k = 0; k < tileLocations.Count; k++)
            {
                Point t = tileLocations[k];
                if(t.X == x && t.Y == y)
                {
                    return true;
                }
            }
            return false;
        }
        public class SortByDistanceToTile : IComparer<Point>
        {
            // Call CaseInsensitiveComparer.Compare with the parameters reversed.
            public int X;
            public int Y;
            public SortByDistanceToTile(int x, int y)
            {
                X = x;
                Y = y;
            }
            public double lengthFromCoordinates(Point p, int x, int y)
            {
                y = p.Y - y;
                x = p.X - x;
                return Math.Sqrt(x * x + y * y);
            }
            public int Compare(Point point1, Point point2)
            {
                double length1 = lengthFromCoordinates(point1, X, Y);
                double length2 = lengthFromCoordinates(point2, X, Y);
                return length1 > length2 ? 1 : length1 == length2 ? 0 : -1;
            }
        }
    }
}