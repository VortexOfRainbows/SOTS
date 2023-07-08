using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SOTS.Items.Invidia;
using SOTS.Items.Slime;
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
using static SOTS.Items.Furniture.Functional.MineralariumTE;
using static Terraria.HitTile;
using static Terraria.Player;

namespace SOTS.Helpers
{
	public static class LazyMinerHelper
	{
		public static LazyMinerProjectile savedProj;
		public static void DropBonusItems(Player self, int x, int y)
		{
			Tile tile = Main.tile[x, y];
			Vector2 position = new Vector2(x * 16 + 8, y * 16 + 8);
			int itemQuant;
			int itemType = GoldenTrowelItem(tile, out itemQuant);
			if (itemType != -1 && itemQuant > 0)
			{
				Vector2 oldPos = self.Center;
				self.Center = position;
				self.QuickSpawnItem(new EntitySource_TileBreak(x, y), itemType, itemQuant);
				self.Center = oldPos;
			}
		}
		public static int GoldenTrowelItem(Tile tile, out int itemQuant)
		{
			int type = -1;
			itemQuant = 1;
			if (OreType.CountsAsOre(tile.TileType) || Main.rand.NextBool(5)) //All this math below works out to be roughly 10.76 copper coins per block. Which is a lot, but the platinum and gold are realistically quite rare. Additionally, ores drop about 53.83 per block
			{
				type = ItemID.CopperCoin;
				itemQuant = Main.rand.Next(6, 11);
				if (Main.rand.NextBool(11))
				{
					type = ItemID.SilverCoin;
					itemQuant = Main.rand.Next(2, 5);
					if (Main.rand.NextBool(80))
					{
						type = ItemID.GoldCoin;
						itemQuant = 1;
						if (Main.rand.NextBool(100))
						{
							type = ItemID.PlatinumCoin;
							itemQuant = 1;
						}
					}
				}
			}
			else if (tile.TileType == TileID.Mud || tile.TileType == TileID.Dirt)
			{
				if (Main.rand.NextBool(200))
					type = ModContent.ItemType<Peanut>();
				else if (Main.rand.NextBool(500))
					type = ModContent.ItemType<Evostone>();
			}
			else if(tile.TileType == TileID.Stone)
            {
				if (Main.rand.NextBool(30))
				{
					type = ItemID.SiltBlock;
					itemQuant = Main.rand.Next(1, 4);
                }
			}
			else if(tile.TileType == TileID.Ash)
            {
				if (Main.rand.NextBool(50))
					type = ModContent.ItemType<Evostone>();
			}
			else if (tile.TileType == TileID.SnowBlock ||
				tile.TileType == TileID.IceBlock ||
				tile.TileType == TileID.HallowedIce ||
				tile.TileType == TileID.CorruptIce ||
				tile.TileType == TileID.FleshIce)
			{
				if (Main.rand.NextBool(30))
				{
					type = ItemID.SlushBlock;
					itemQuant = Main.rand.Next(1, 4);
				}
			}
			else if (tile.TileType == TileID.Sand ||
				tile.TileType == TileID.HardenedSand ||
				tile.TileType == TileID.Sandstone ||
				tile.TileType == TileID.Pearlsand ||
				tile.TileType == TileID.Crimsand ||
				tile.TileType == TileID.Ebonsand ||
				tile.TileType == TileID.CrimsonSandstone ||
				tile.TileType == TileID.HallowSandstone ||
				tile.TileType == TileID.CorruptSandstone ||
				tile.TileType == TileID.CorruptHardenedSand ||
				tile.TileType == TileID.CrimsonHardenedSand ||
				tile.TileType == TileID.HallowHardenedSand)
			{
				if (Main.rand.NextBool(30))
				{
					itemQuant = Main.rand.Next(1, 4);
					type = ItemID.DesertFossil;
				}
			}
			return type;
        }
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
			if (!WorldGen.CanKillTile(x, y) || RunDoesPickTargetTransformOnKill(self, self.hitTile, num2, x, y, pickPower, num, tile))
			{
				num2 = 0;
			}
			if (Main.getGoodWorld)
			{
				num2 *= 2;
			}
			if (num2 == 0)
				return false;
			if (!WorldgenHelpers.SOTSWorldgenHelper.TrueTileSolid(x, y))
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
}