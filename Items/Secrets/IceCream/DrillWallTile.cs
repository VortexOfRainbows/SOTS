using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Secrets.IceCream
{
	public class DrillWallTile : ModWall
	{
		
		public override void SetDefaults()
		{
			Main.wallLargeFrames[Type] = (byte) 1;
			Main.wallHouse[Type] = true;
			dustType = 72;
			drop = mod.ItemType("DrillWall");
			AddMapEntry(new Color(100, 67, 188));
		}
		public override	bool PreDraw(int i, int j, SpriteBatch spriteBatch) {
			bool canKillTile = true;
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
			if(canKillTile && Main.tile[i, j].type != mod.TileType("IceCreamBrickTile") && Main.tile[i, j].type != mod.TileType("PlanetariumBlock") && Main.tile[i, j].type != 226)
			{
           	Projectile.NewProjectile(i * 16 + 8, j * 16 + 8, 0, 0, mod.ProjectileType("AlphaBlockBreak"), 0, 0, 1);
			}
			return true;
		}
	}
}