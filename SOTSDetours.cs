using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Items.Furniture;
using SOTS.Items.Furniture.Earthen;
using SOTS.Items.Furniture.Nature;
using SOTS.Projectiles.Chaos;
using SOTS.Projectiles.Inferno;
using SOTS.Projectiles.Minions;
using SOTS.Utilities;
using SOTS.WorldgenHelpers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Social;

namespace SOTS
{
	public static class SOTSDetours
	{
		public static RenderTarget2D TargetProj;
		public static void Initialize()
		{
			On.Terraria.NetMessage.SendData += NetMessage_SendData;

			On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;
			On.Terraria.Main.DrawNPCs += Main_DrawNPCs;
			On.Terraria.Main.DrawPlayers_AfterProjectiles += Main_DrawPlayers_AfterProjectiles;
			//The following is for Time Freeze
			//order of updates: player, NPC, gore, projectile, item, dust, time
			On.Terraria.Player.Update += Player_Update;
			On.Terraria.Player.PickTile += Player_PickTile;
			On.Terraria.NPC.UpdateNPC_Inner += NPC_UpdateNPC_Inner;
			On.Terraria.Gore.Update += Gore_Update;
            On.Terraria.Projectile.Update += Projectile_Update;
			On.Terraria.Item.UpdateItem += Item_UpdateItem;
			On.Terraria.Dust.UpdateDust += Dust_UpdateDust;
			On.Terraria.Main.UpdateTime += Main_UpdateTime;
			//On.Terraria.NPC.UpdateCollision += NPC_UpdateCollision;

			//The following is to allow Plating Doors to function as tiles for housing (in conjuction with Tmodloader stuff)
			On.Terraria.WorldGen.CloseDoor += Worldgen_CloseDoor;
			On.Terraria.WorldGen.OpenDoor += Worldgen_OpenDoor;

			//1.4 worldgen fix
			On.Terraria.WorldGen.FillWallHolesInSpot += Worldgen_FillWallHolesInSpot;

			//1.4 ZombieHand
			On.Terraria.Player.ItemCheck_MeleeHitNPCs += Player_ItemCheck_MeleeHitNPCs;

			Main.OnPreDraw += Main_OnPreDraw;
			if (!Main.dedServ)
				ResizeTargets();
		}
		public static void Unload()
		{
			On.Terraria.NetMessage.SendData -= NetMessage_SendData;

			On.Terraria.Main.DrawProjectiles -= Main_DrawProjectiles;
			On.Terraria.Main.DrawNPCs -= Main_DrawNPCs;
			On.Terraria.Main.DrawPlayers_AfterProjectiles -= Main_DrawPlayers_AfterProjectiles;

			//order of updates: player, NPC, gore, projectile, item, dust, time
			On.Terraria.Player.Update -= Player_Update;
			On.Terraria.Player.PickTile -= Player_PickTile;
			On.Terraria.NPC.UpdateNPC_Inner -= NPC_UpdateNPC_Inner;
			On.Terraria.Gore.Update -= Gore_Update;
			On.Terraria.Projectile.Update -= Projectile_Update;
			On.Terraria.Item.UpdateItem -= Item_UpdateItem;
			On.Terraria.Dust.UpdateDust -= Dust_UpdateDust;
			On.Terraria.Main.UpdateTime -= Main_UpdateTime;

			On.Terraria.WorldGen.CloseDoor -= Worldgen_CloseDoor;
			On.Terraria.WorldGen.OpenDoor -= Worldgen_OpenDoor;
			On.Terraria.WorldGen.FillWallHolesInSpot -= Worldgen_FillWallHolesInSpot;

			//1.4 ZombieHand
			On.Terraria.Player.ItemCheck_MeleeHitNPCs -= Player_ItemCheck_MeleeHitNPCs;

			Main.OnPreDraw -= Main_OnPreDraw;
		}
		public static void ResizeTargets()
		{
			Main.QueueMainThreadAction(() =>
			{
				TargetProj = new RenderTarget2D(Main.instance.GraphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2);
			});
		}
		private static void NetMessage_SendData(On.Terraria.NetMessage.orig_SendData orig, int msgType, int remoteClient = -1, int ignoreClient = -1, NetworkText text = null, int number = 0, float number2 = 0f, float number3 = 0f, float number4 = 0f, int number5 = 0, int number6 = 0, int number7 = 0)
        {
			if(FakePlayer.FakePlayer.SupressNetMessage13and41)
            {
				if(msgType == 13 || msgType == 41)
                {
					return;
                }
			}
			orig(msgType, remoteClient, ignoreClient, text, number, number2, number3, number4, number5, number6, number7);
		}
		private static void Player_PickTile(On.Terraria.Player.orig_PickTile orig, Player self, int x, int y, int pickPower)
		{
			if(SOTSPlayer.ModPlayer(self).LazyMinerRing)
            {
				bool DoNotMineNormally = LazyMinerHelper.FakePickTile(self, x, y, pickPower);
				if (DoNotMineNormally)
					return;
            }
			orig(self, x, y, pickPower);
		}
		private static bool Worldgen_CloseDoor(On.Terraria.WorldGen.orig_CloseDoor orig, int i, int j, bool forced)
		{
			if (Framing.GetTileSafely(i, j).HasTile)
			{
				Tile tile = Framing.GetTileSafely(i, j);
				if (TileLoader.GetTile(tile.TileType) is BlastDoorOpen BDO)
				{
					BDO.UpdateDoor(i, j);
					return true;
				}
			}
			return orig(i, j, forced);
		}
		private static bool Worldgen_OpenDoor(On.Terraria.WorldGen.orig_OpenDoor orig, int i, int j, int direction)
		{
			if (Framing.GetTileSafely(i, j).HasTile)
			{
				Tile tile = Framing.GetTileSafely(i, j);
				if (TileLoader.GetTile(tile.TileType) is BlastDoorClosed BDC)
				{
					BDC.UpdateDoor(i, j);
					return true;
				}
			}
			return orig(i, j, direction);
		}
		private static bool Worldgen_FillWallHolesInSpot(On.Terraria.WorldGen.orig_FillWallHolesInSpot orig, int originX, int originY, int maxWallsThreshold)
		{
			if(Main.wallHouse[Main.tile[originX - 1, originY].WallType] || Main.wallHouse[Main.tile[originX + 1, originY].WallType] || Main.wallHouse[Main.tile[originX, originY - 1].WallType] || Main.wallHouse[Main.tile[originX, originY + 1].WallType])
			{
				return false;
			}
			return orig(originX, originY, maxWallsThreshold);
		}
		private static void Player_Update(On.Terraria.Player.orig_Update orig, Player self, int i)
        {
			if(self != null)
			{
				if (SOTSWorld.IsFrozenThisFrame && self.active)
				{
					SOTSPlayer sPlayer = SOTSPlayer.ModPlayer(self);
					if (sPlayer != null && !sPlayer.TimeFreezeImmune)
					{
						return;
					}
				}
				if (self.active)
				{
					SOTSPlayer sPlayer = SOTSPlayer.ModPlayer(self);
					if(sPlayer != null)
						sPlayer.oldHeldProj = self.heldProj;
				}
			}
			orig(self, i);
		}
		private static void Player_ItemCheck_MeleeHitNPCs(On.Terraria.Player.orig_ItemCheck_MeleeHitNPCs orig, Player self, Item sItem, Rectangle itemRectangle, int originalDamage, float knockBack)
		{
			bool zombieHand = SOTSPlayer.ModPlayer(self).CanKillNPC;
			bool[] saveFriendly = new bool[200];
			for (int i = 0; i < 200; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.active)
                {
					saveFriendly[i] = npc.friendly;
					if(zombieHand && npc.townNPC && npc.friendly)
						npc.friendly = false;
                }
            }
			orig(self, sItem, itemRectangle, originalDamage, knockBack);
			for (int i = 0; i < 200; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.active)
				{
					npc.friendly = saveFriendly[i];
				}
			}
		}
		/*private static void NPC_UpdateCollision(On.Terraria.NPC.orig_UpdateCollision orig, NPC self)
		{
			if (self.active)
			{
				if (SOTSWorld.IsFrozenThisFrame)
				{
					return;
				}
			}
			orig(self);
		}*/
		private static void NPC_UpdateNPC_Inner(On.Terraria.NPC.orig_UpdateNPC_Inner orig, NPC self, int i)
		{
			if (self.active)
			{
				if (SOTSWorld.IsFrozenThisFrame)
				{
					/*for (int index = 0; index < 256; ++index)
					{
						if (self.immune[index] > 0)
							--self.immune[index];
					}*/
					return;
				}
				else
				{
					bool freeze = DebuffNPC.UpdateWhileFrozen(self, i);
					if (freeze)
					{
						return;
					}
				}
			}
			orig(self, i);
		}
		private static void Gore_Update(On.Terraria.Gore.orig_Update orig, Gore self)
		{
			if (SOTSWorld.IsFrozenThisFrame && self.active)
			{
				return;
			}
			orig(self);
		}
		private static void Projectile_Update(On.Terraria.Projectile.orig_Update orig, Projectile self, int i)
		{
			if(self.active)
			{
				if (SOTSWorld.IsFrozenThisFrame)
				{
					bool dragIntoFreeze = SOTSProjectile.GlobalFreezeSlowdown(self);
					if (dragIntoFreeze && SOTSProjectile.CanBeTimeFrozen(self))
					{
						self.Damage(); //turn on hitboxes
						return;
					}
				}
				else
				{
					bool freeze = SOTSProjectile.UpdateWhileFrozen(self, i);
					if (freeze)
						return;
				}
			}
			orig(self, i);
		}
		private static void Item_UpdateItem(On.Terraria.Item.orig_UpdateItem orig, Item self, int i)
		{
			if (SOTSWorld.IsFrozenThisFrame && self.active)
			{
				return;
			}
			orig(self, i);
		}
		private static void Dust_UpdateDust(On.Terraria.Dust.orig_UpdateDust orig)
		{
			if (SOTSWorld.IsFrozenThisFrame)
			{
				return;
			}
			orig();
		}
		private static void Main_UpdateTime(On.Terraria.Main.orig_UpdateTime orig)
		{
			if (SOTSWorld.IsFrozenThisFrame)
			{
				return;
			}
			orig();
		}
		private static void Main_OnPreDraw(GameTime obj)
		{
			if (Main.spriteBatch != null && !Main.dedServ)
			{
				if (SOTS.primitives != null && Main.spriteBatch != null)
				{
					SOTS.primitives.DrawTrailsProj(Main.spriteBatch, Main.graphics.GraphicsDevice);
					SOTS.primitives.DrawTrailsNPC(Main.spriteBatch, Main.graphics.GraphicsDevice);
				}
			}
		}
		private static void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
		{
			if (SOTS.primitives != null && Main.spriteBatch != null)
			{
				SOTS.primitives.DrawTargetProj(Main.spriteBatch);
			}
			if(self != null && orig != null)
            {
				orig(self); 
				PostDrawProjectiles();
			}
		}
		private static void Main_DrawNPCs(On.Terraria.Main.orig_DrawNPCs orig, Main self, bool behindTiles)
		{
			if (SOTS.primitives != null && Main.spriteBatch != null)
			{
				PreDrawNPCs();
				SOTS.primitives.DrawTargetNPC(Main.spriteBatch);
			}
			if (self != null && orig != null)
            {
				orig(self, behindTiles);
			}
		}

		private static void Main_DrawPlayers_AfterProjectiles(On.Terraria.Main.orig_DrawPlayers_AfterProjectiles orig, Main self)
		{
			PreDrawPlayers();
			orig(self);
			PostDrawPlayers();
		}
		private static void PostDrawProjectiles()
		{
			if (!Main.dedServ)
			{
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.active && proj.ModProjectile is HoloPlatform hPlatform)
					{
						hPlatform.Draw(Main.spriteBatch); //change later
					}
					if (proj.active && proj.ModProjectile is ChaosDiamondLaser dLaser)
					{
						dLaser.DrawBlack(Main.spriteBatch); //change later
					}
				}
				Main.spriteBatch.End();
			}
		}
		private static void PreDrawNPCs()
		{
			if (!Main.dedServ)
			{
				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.active)
					{
						DebuffNPC instancedNPC = npc.GetGlobalNPC<DebuffNPC>();
						if (instancedNPC.timeFrozen != 0)
							instancedNPC.DrawTimeFreeze(npc, Main.spriteBatch);
					}
				}
			}
		}
		private static void PreDrawPlayers()
		{
			if (!Main.dedServ)
			{
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				for (int i = 0; i < Main.player.Length; i++)
				{
					Player player = Main.player[i];
					if (player.active)
					{
						CurseHelper.DrawPlayerFoam(Main.spriteBatch, player);
					}
				}
				Main.spriteBatch.End();
			}
		}
		private static void PostDrawPlayers()
		{
			if (!Main.dedServ)
			{
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.active && proj.ModProjectile is IOrbitingProj modProj && modProj.inFront)
					{
						modProj.Draw(Main.spriteBatch, Color.White);
					}
					if (proj.active && proj.ModProjectile is IncineratorGloveProjectile modProj2)
					{
						modProj2.Draw(Main.spriteBatch, Color.White); 
					}
				}
				Main.spriteBatch.End();
			}
		}
	}
}
