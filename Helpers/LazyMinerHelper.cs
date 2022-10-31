using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SOTS.Projectiles.Celestial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.Player;

public static class LazyMinerHelper
{
    public static bool FakePickTile(Player self, int x, int y, int pickPower)
	{
		int num = self.hitTile.HitObject(x, y, 1);
        #region run pre-emptive tile checks
        Tile tile = Main.tile[x, y];
		if (tile.TileType == 504)
		{
			return false;
		}
		int num2 = RunGetPickaxeDamage(self, x, y, pickPower, num, tile);
		if (!WorldGen.CanKillTile(x, y))
		{
			num2 = 0;
		}
		if (Main.getGoodWorld)
		{
			num2 *= 2;
		}
		if (RunDoesPickTargetTransformOnKill(self, self.hitTile, num2, x, y, pickPower, num, tile))
		{
			num2 = 0;
		}
		if (num2 == 0)
			return false;
		if(!SOTS.WorldgenHelpers.SOTSWorldgenHelper.TrueTileSolid(x, y))
        {
			return false;
        }
        #endregion

		//Following is the actual stuff that does stuff
        int damageTileAmount = self.hitTile.AddDamage(num, num2);
		if (damageTileAmount >= 100)
		{
			damageTileAmount = self.hitTile.AddDamage(num, -damageTileAmount);
			///THIS IS WHERE I WILL PUT STUFF THAT
			///adds mined tile to cache
			///finds future tiles of same type to add to cache
			///breaks all tiles in cache once mouse is released or 100 tiles are added to the cache
			///this will be done through a projectile, to allow multiple caches of block types to be added
			if (damageTileAmount >= 100)
			{
				bool num3 = Main.tile[x, y].HasTile;
				WorldGen.KillTile(x, y);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(17, -1, -1, null, 0, x, y);
				}
				RunClearMiningCacheAt(self, x, y, 1);
			}
		}
		else
		{
			WorldGen.KillTile(x, y, fail: true);
			if (Main.netMode == 1)
			{
				NetMessage.SendData(17, -1, -1, null, 0, x, y, 1f);
				NetMessage.SendData(125, -1, -1, null, Main.myPlayer, x, y, num2);
			}
		}
		if (num2 != 0)
		{
			self.hitTile.Prune();
		}
		return true;
	}
	public static bool RunDoesPickTargetTransformOnKill(Player self, HitTile hitCounter, int damage, int x, int y, int pickPower, int bufferIndex, Tile tileTarget)
	{
		Type type = self.GetType();
		MethodInfo method = type.GetMethod("DoesPickTargetTransformOnKill", BindingFlags.NonPublic | BindingFlags.Instance);
		if (method == null)
			return false;
		return (bool)method.Invoke(self, new object[] { hitCounter, damage, x, y, pickPower, bufferIndex, tileTarget });
	}
	public static int RunGetPickaxeDamage(Player self, int x, int y, int pickPower, int hitBufferIndex, Tile tileTarget)
	{
		Type type = self.GetType();
		MethodInfo method = type.GetMethod("GetPickaxeDamage", BindingFlags.NonPublic | BindingFlags.Instance);
		if (method == null)
			return 0;
		return (int)method.Invoke(self, new object[] { x, y, pickPower, hitBufferIndex, tileTarget });
	}
	public static int RunClearMiningCacheAt(Player self, int x, int y, int hitTileCacheType)
	{
		Type type = self.GetType();
		MethodInfo method = type.GetMethod("ClearMiningCacheAt", BindingFlags.NonPublic | BindingFlags.Instance);
		if (method == null)
			return 0;
		return (int)method.Invoke(self, new object[] { x, y, hitTileCacheType });
	}
}