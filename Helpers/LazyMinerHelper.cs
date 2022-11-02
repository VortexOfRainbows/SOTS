using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SOTS.Projectiles.Base;
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
using static Terraria.HitTile;
using static Terraria.Player;

public static class LazyMinerHelper
{
	public static LazyMinerProjectile savedProj;
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
			WorldGen.KillTile(x, y, fail: true);
			HitTileObject hitTileObject = self.hitTile.data[num];
			hitTileObject.damage = 0;
			if (Main.myPlayer == self.whoAmI)
            {
                if (!LazyMinerProjectile.PlayerOwnsLazyMiner(self))
				{
					Projectile proj = Projectile.NewProjectileDirect(new EntitySource_TileBreak(x, y, "SOTS:LazyMiner"), new Vector2(x * 16 + 8, y * 16 + 8), Vector2.Zero, ModContent.ProjectileType<LazyMinerProjectile>(), 0, 0, Main.myPlayer);
					savedProj = proj.ModProjectile as LazyMinerProjectile;
					savedProj.FindNextTile(x, y, tile.TileType, self.direction);
				}
				else
                {
					savedProj.FindNextTile(x, y, tile.TileType, self.direction);
				}
			}
			//RunClearMiningCacheAt(self, x, y, 1);
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