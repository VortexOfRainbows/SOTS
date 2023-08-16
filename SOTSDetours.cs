﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs.Debuffs;
using SOTS.Common;
using SOTS.Common.GlobalNPCs;
using SOTS.Items.Conduit;
using SOTS.Items.Furniture;
using SOTS.NPCs.Town;
using SOTS.Projectiles.Chaos;
using SOTS.Projectiles.Inferno;
using SOTS.Projectiles.Minions;
using SOTS.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.HitTile;

namespace SOTS
{
    public static class SOTSDetours
	{
		public static RenderTarget2D TargetProj;
		public static void Initialize()
		{
			On_NetMessage.SendData += NetMessage_SendData;

			On_Main.DrawProjectiles += Main_DrawProjectiles;
			On_Main.DrawNPCs += Main_DrawNPCs;
			On_Main.DrawPlayers_AfterProjectiles += Main_DrawPlayers_AfterProjectiles;
			//The following is for Time Freeze
			//order of updates: player, NPC, gore, projectile, item, dust, time
			On_Player.Update += Player_Update;
			On_NPC.UpdateNPC_Inner += NPC_UpdateNPC_Inner;
			On_Gore.Update += Gore_Update;
            On_Projectile.Update += Projectile_Update;
			On_Item.UpdateItem += Item_UpdateItem;
			On_Dust.UpdateDust += Dust_UpdateDust;
			On_Main.UpdateTime += Main_UpdateTime;
			//On_NPC.UpdateCollision += NPC_UpdateCollision;

			//The following is to allow Plating Doors to function as tiles for housing (in conjuction with Tmodloader stuff)
			On_WorldGen.CloseDoor += Worldgen_CloseDoor;
			On_WorldGen.OpenDoor += Worldgen_OpenDoor;

			//1.4 worldgen fix
			On_WorldGen.FillWallHolesInSpot += Worldgen_FillWallHolesInSpot;

			//1.4 ZombieHand
			On_Player.ItemCheck_MeleeHitNPCs += Player_ItemCheck_MeleeHitNPCs;
			Main.OnPreDraw += Main_OnPreDraw;

			//Belpohegor Ring
			On_Player.PickTile += Player_PickTile;

			//Crafting Stations carry with you (elemental amulet)
			On_Recipe.FindRecipes += Recipe_FindRecipes;

			//Synthetic Liver
			On_Player.AddBuff += Player_AddBuff;

			//Used for Turning Off hit sounds for certain weapons
			//On_NPC.StrikeNPC += NPC_StrikeNPC;

			//Used for Nerfing Soaring Insignia
			On_Player.WingMovement += Player_WingMovement;

			//Used in Archaeologist + Void Anomaly
			On_Main.DrawMiscMapIcons += Main_DrawMiscMapIcons;
			On_Lighting.GetColor9Slice_int_int_refVector3Array += Lighting_GetColor9Slice_int_int_refVector3Array;
			On_Main.DrawItem += Main_DrawItem;

			if (!Main.dedServ)
				ResizeTargets();
		}
		public static void Unload() //Apparently unloading Detours is handled automatically now..?
		{
			/*On_NetMessage.SendData -= NetMessage_SendData;

			On_Main.DrawProjectiles -= Main_DrawProjectiles;
			On_Main.DrawNPCs -= Main_DrawNPCs;
			On_Main.DrawPlayers_AfterProjectiles -= Main_DrawPlayers_AfterProjectiles;

			//order of updates: player, NPC, gore, projectile, item, dust, time
			On_Player.Update -= Player_Update;
			On_NPC.UpdateNPC_Inner -= NPC_UpdateNPC_Inner;
			On_Gore.Update -= Gore_Update;
			On_Projectile.Update -= Projectile_Update;
			On_Item.UpdateItem -= Item_UpdateItem;
			On_Dust.UpdateDust -= Dust_UpdateDust;
			On_Main.UpdateTime -= Main_UpdateTime;

			On_WorldGen.CloseDoor -= Worldgen_CloseDoor;
			On_WorldGen.OpenDoor -= Worldgen_OpenDoor;
			On_WorldGen.FillWallHolesInSpot -= Worldgen_FillWallHolesInSpot;

			//1.4 ZombieHand
			On_Player.ItemCheck_MeleeHitNPCs -= Player_ItemCheck_MeleeHitNPCs;

			Main.OnPreDraw -= Main_OnPreDraw;
			On_Player.PickTile -= Player_PickTile;*/
		}
		public static void ResizeTargets()
		{
			Main.QueueMainThreadAction(() =>
			{
				TargetProj = new RenderTarget2D(Main.instance.GraphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2);
			});
		}
		public static void Recipe_FindRecipes(On_Recipe.orig_FindRecipes orig, bool canDelayCheck = false)
        {
			if(SOTSPlayer.ModPlayer(Main.LocalPlayer).LazyCrafterAmulet)
            {
				//Main.NewText("I am being updated");
				Player player = Main.LocalPlayer;
				player.adjTile[TileID.WorkBenches] = true;
				player.adjTile[TileID.Furnaces] = true;
				player.adjTile[TileID.Anvils] = true;
				player.adjTile[TileID.AlchemyTable] = true;
				player.adjTile[TileID.Bottles] = true;
				player.adjTile[TileID.Tables] = true;
				player.alchemyTable = true;
			}
			orig(canDelayCheck);
        }
		private static void NetMessage_SendData(On_NetMessage.orig_SendData orig, int msgType, int remoteClient = -1, int ignoreClient = -1, NetworkText text = null, int number = 0, float number2 = 0f, float number3 = 0f, float number4 = 0f, int number5 = 0, int number6 = 0, int number7 = 0)
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
		private static bool Worldgen_CloseDoor(On_WorldGen.orig_CloseDoor orig, int i, int j, bool forced)
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
		private static bool Worldgen_OpenDoor(On_WorldGen.orig_OpenDoor orig, int i, int j, int direction)
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
		private static bool Worldgen_FillWallHolesInSpot(On_WorldGen.orig_FillWallHolesInSpot orig, int originX, int originY, int maxWallsThreshold)
		{
			if(Main.wallHouse[Main.tile[originX - 1, originY].WallType] || Main.wallHouse[Main.tile[originX + 1, originY].WallType] || Main.wallHouse[Main.tile[originX, originY - 1].WallType] || Main.wallHouse[Main.tile[originX, originY + 1].WallType])
			{
				return false;
			}
			return orig(originX, originY, maxWallsThreshold);
		}
		private static void Player_Update(On_Player.orig_Update orig, Player self, int i)
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
		private static void Player_PickTile(On_Player.orig_PickTile orig, Player self, int x, int y, int pickPower)
		{
			if (self != null)
			{
				if (SOTSPlayer.ModPlayer(self).bonusPickaxePower > 0)
					pickPower += SOTSPlayer.ModPlayer(self).bonusPickaxePower;
				if (SOTSPlayer.ModPlayer(self).ConduitBelt)
				{
					Tile tile = Framing.GetTileSafely(x, y);
					if (tile.TileType == ModContent.TileType<ConduitChassisTile>() || tile.TileType == ModContent.TileType<NatureConduitTile>() || tile.TileType == ModContent.TileType<EarthenConduitTile>())
                    {
						pickPower += 300;
                    }
                }
				if (SOTSPlayer.ModPlayer(self).LazyMinerRing)
				{
					bool DoNotMineNormally = Helpers.LazyMinerHelper.FakePickTile(self, x, y, pickPower);
					if (DoNotMineNormally)
						return;
				}
			}
			if (self != null) //This code only runs on client
			{
				if (SOTSPlayer.ModPlayer(self).GoldenTrowel)
				{
					Tile tile = Framing.GetTileSafely(x, y);
					int num = self.hitTile.HitObject(x, y, 1);
					HitTileObject hitTileObject = self.hitTile.data[num];
					int tileDamage = hitTileObject.damage;
					int futureDamage = Helpers.LazyMinerHelper.RunGetPickaxeDamage(self, x, y, pickPower, num, tile);
					if (!WorldGen.CanKillTile(x, y) || Helpers.LazyMinerHelper.RunDoesPickTargetTransformOnKill(self, self.hitTile, tileDamage, x, y, pickPower, num, tile))
					{
						tileDamage = 0;
					}
					if (Main.getGoodWorld)
					{
						tileDamage *= 2;
					}
					if (futureDamage + tileDamage >= 100) //Basically if the tile is broken
					{
						Helpers.LazyMinerHelper.DropBonusItems(self, x, y);
					}
				}
			}
			orig(self, x, y, pickPower);
		}
		private static void Player_ItemCheck_MeleeHitNPCs(On_Player.orig_ItemCheck_MeleeHitNPCs orig, Player self, Item sItem, Rectangle itemRectangle, int originalDamage, float knockBack)
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
		/*private static void NPC_UpdateCollision(On_NPC.orig_UpdateCollision orig, NPC self)
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
		private static void NPC_UpdateNPC_Inner(On_NPC.orig_UpdateNPC_Inner orig, NPC self, int i)
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
			if (self.ModNPC is Archaeologist arch)
			{
                NPCs.Town.VoidAnomaly.APortalIsAccepting--;
				if (NPCs.Town.VoidAnomaly.APortalIsAccepting <= 0)
					NPCs.Town.VoidAnomaly.APortalIsAccepting = 0;
				arch.ArchAI();
            }
			orig(self, i);
		}
		private static void Gore_Update(On_Gore.orig_Update orig, Gore self)
		{
			if (SOTSWorld.IsFrozenThisFrame && self.active)
			{
				return;
			}
			orig(self);
		}
		private static void Projectile_Update(On_Projectile.orig_Update orig, Projectile self, int i)
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
		private static void Item_UpdateItem(On_Item.orig_UpdateItem orig, Item self, int i)
		{
			if (SOTSWorld.IsFrozenThisFrame && self.active)
			{
				return;
			}
			orig(self, i);
		}
		private static void Dust_UpdateDust(On_Dust.orig_UpdateDust orig)
		{
			if (SOTSWorld.IsFrozenThisFrame)
			{
				return;
			}
			orig();
		}
		private static void Main_UpdateTime(On_Main.orig_UpdateTime orig)
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
		private static void Main_DrawProjectiles(On_Main.orig_DrawProjectiles orig, Main self)
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
		private static void Main_DrawNPCs(On_Main.orig_DrawNPCs orig, Main self, bool behindTiles)
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
		private static void Main_DrawPlayers_AfterProjectiles(On_Main.orig_DrawPlayers_AfterProjectiles orig, Main self)
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
					if(i < 200)
                    {
						NPC npc = Main.npc[i];
						DendroChainNPCOperators.DrawFloralBloomImage(npc);
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
						if (!npc.GetGlobalNPC<GlobalEntityNPC>().RecentlyTeleported)
						{
							DebuffNPC instancedNPC = npc.GetGlobalNPC<DebuffNPC>();
							if (instancedNPC.timeFrozen != 0)
								instancedNPC.DrawTimeFreeze(npc, Main.spriteBatch);
						}
						else
                        {
							npc.GetGlobalNPC<GlobalEntityNPC>().Draw(npc, Main.spriteBatch);
						}
						if(npc.ModNPC is Archaeologist arch)
                        {
							arch.Draw(Main.spriteBatch, Main.screenPosition, Lighting.GetColor((int)npc.Center.X / 16, (int)npc.Center.Y / 16));
                        }
						DendroChainNPCOperators.DrawChainsBetweenNPC(npc, Main.spriteBatch);
					}
				}
			}
		}
		private static void PreDrawPlayers()
		{
			if (!Main.dedServ)
			{
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				/*for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.active && proj.ModProjectile is DreamingFrame modProj)
					{
						Color color = Color.White;
						modProj.PreDraw(ref color);
					}
				}*/
				ConduitHelper.preDrawBeforePlayers();
				for (int i = 0; i < Main.player.Length; i++)
				{
					Player player = Main.player[i];
					if (player.active)
					{
						CurseHelper.DrawPlayerFoam(Main.spriteBatch, player);
						if(i == Main.myPlayer)
							ConduitHelper.DrawPlayerEffectOutline(Main.spriteBatch, player);
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
		private static void Player_AddBuff(On_Player.orig_AddBuff orig, Player self, int type, int timeToAdd, bool quiet = true, bool foodHack = false)
		{
			if (SOTSPlayer.ModPlayer(self).PotionStacking && self.whoAmI == Main.myPlayer && (!quiet || Main.netMode == NetmodeID.SinglePlayer))
			{
				if (type == BuffID.WellFed || type == BuffID.WellFed2 || type == BuffID.WellFed3)
				{
					int currentTime = 0;
					if (self.HasBuff(BuffID.WellFed))
						currentTime = self.buffTime[self.FindBuffIndex(BuffID.WellFed)];
					if (self.HasBuff(BuffID.WellFed2))
						currentTime = self.buffTime[self.FindBuffIndex(BuffID.WellFed2)];
					if (self.HasBuff(BuffID.WellFed3))
						currentTime = self.buffTime[self.FindBuffIndex(BuffID.WellFed3)];
					if(currentTime > 120)
						timeToAdd += currentTime;
				}
				else if (self.HasBuff(type) && !Main.debuff[type])
				{
					int currentTime = self.buffTime[self.FindBuffIndex(type)];
					if (currentTime > 120)
						timeToAdd += currentTime;
				}
			}
			orig(self, type, timeToAdd, quiet, foodHack);
        }
		private static void Player_WingMovement(On_Player.orig_WingMovement orig, Player self)
        {
			float startingWingTime = self.wingTime;
			orig(self);
			float WingTimeDiff = startingWingTime - self.wingTime;
			if (SOTSPlayer.ModPlayer(self).hasSoaringInsigniaFake)
			{
				if(WingTimeDiff > 0)
					self.wingTime += WingTimeDiff / 3f;
			}
		}
		private static void Main_DrawMiscMapIcons(On_Main.orig_DrawMiscMapIcons orig, Main self, SpriteBatch spriteBatch, Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale, float drawScale, ref string mouseTextString)
        {
			orig(self, spriteBatch, mapTopLeft, mapX2Y2AndOff, mapRect, mapScale, drawScale, ref mouseTextString);
			DrawSOTSMapIcons(self, spriteBatch, mapTopLeft, mapX2Y2AndOff, mapRect, mapScale, drawScale, ref mouseTextString);
		}
		private static void DrawSOTSMapIcons(Main self, SpriteBatch spriteBatch, Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale, float drawScale, ref string mouseTextString)
		{
			if(!SOTSPlayer.ModPlayer(Main.LocalPlayer).AnomalyLocator || Main.gameMenu)
            {
				return;
            }
			for(int k = 0; k < 3; k++)
			{
				Vector2 archPos = k == 0 ? Archaeologist.AnomalyPosition1 : (k == 1 ? Archaeologist.AnomalyPosition2 : Archaeologist.AnomalyPosition3);
				if (archPos != Vector2.Zero && archPos != NPCs.Town.VoidAnomaly.finalPositionAfterShatter)
				{
					float alphaMult = Archaeologist.FinalAnomalyAlphaMult;
					Vector2 vec = archPos / 16f - mapTopLeft;
					vec *= mapScale;
					vec += mapX2Y2AndOff;
					vec = vec.Floor();
					bool draw = true;
					if (mapRect.HasValue)
					{
						Rectangle value2 = mapRect.Value;
						if (!value2.Contains(vec.ToPoint()))
						{
							draw = false;
						}
					}
					if(draw)
					{
						Texture2D value = ModContent.Request<Texture2D>("SOTS/Items/Conduit/VoidAnomaly").Value;
						Rectangle rectangle = value.Frame();
						Color circleColor = new Color(130, 100, 110, 0);
						for (int j = 2; j >= -1; j--)
						{
							if (j == 0)
								circleColor = Color.Black * 0.75f;
							if (j == -1)
								circleColor = Color.Black * 0.35f;
							int radius = 4;
							if (j == -1)
								radius = 2;
							if (j == 2)
								radius = 8;
							for (int i = 0; i < 12; i++)
							{
								Vector2 circular = new Vector2(radius, 0).RotatedBy((SOTSWorld.GlobalCounter * (j % 2 * 2 - 1) + i * 30) * MathHelper.Pi / 180f);
								spriteBatch.Draw(value, vec + circular, rectangle, circleColor * 0.825f * alphaMult, 0f, rectangle.Size() / 2f, drawScale, 0, 0f);
							}
						}
						spriteBatch.Draw(value, vec, rectangle, Color.White * alphaMult, 0f, rectangle.Size() / 2f, drawScale, 0, 0f);
						Rectangle rectangle2 = Utils.CenteredRectangle(vec, rectangle.Size() * drawScale);
						if (rectangle2.Contains(Main.MouseScreen.ToPoint()))
						{
							mouseTextString = Language.GetTextValue("Mods.SOTS.Common.ArchaeologistMap");
							//_ = Main.MouseScreen + new Vector2(-28f) + new Vector2(4f, 0f);
						}
					}
				}
			}
			for(int i = 0; i < Main.item.Length; i++)
            {
				Item item = Main.item[i];
				if(item.active)
                {
					Common.GlobalEntityItem gInstance;
					if (item.TryGetGlobalItem<Common.GlobalEntityItem>(out gInstance))
					{
						if(gInstance.TeleportCounter > 0)
                        {
							Vector2 vec = item.Center / 16f - mapTopLeft;
							vec *= mapScale;
							vec += mapX2Y2AndOff;
							vec = vec.Floor();
							bool draw = true;
							if (mapRect.HasValue)
							{
								Rectangle value2 = mapRect.Value;
								if (!value2.Contains(vec.ToPoint()))
								{
									draw = false;
								}
							}
							if(draw)
							{
								Texture2D texture = TextureAssets.Item[item.type].Value;
								int frameCount = 1;
								int frame = 0;
								DrawAnimation anim = Main.itemAnimations[item.type];
								if (anim != null)
								{
									frameCount = anim.FrameCount;
									frame = anim.Frame;
								}
								Rectangle frameRect = new Rectangle(0, texture.Height / frameCount * frame, texture.Width, texture.Height / frameCount);
								spriteBatch.Draw(texture, vec, frameRect, Color.White, 0f, frameRect.Size() / 2f, drawScale, 0, 0f);
								Rectangle rectangle2 = Utils.CenteredRectangle(vec, frameRect.Size() * drawScale);
								if (rectangle2.Contains(Main.MouseScreen.ToPoint()))
								{
									mouseTextString = Language.GetTextValue("Mods.SOTS.Common.ArchaeologistItemMap") + item.HoverName;
									//_ = Main.MouseScreen + new Vector2(-28f) + new Vector2(4f, 0f);
								}
							}
						}
					}
				}
            }
		}
		private static void Lighting_GetColor9Slice_int_int_refVector3Array(On_Lighting.orig_GetColor9Slice_int_int_refVector3Array orig, int x, int y, ref Vector3[] slices)
        {
			if(Main.gameMenu) //Used in the Void Anomaly
			{
				for (int i = 0; i < slices.Length; i++)
				{
					slices[i] = new Vector3(1, 1, 1);
				}
			}
			else
				orig(x, y, ref slices);
        }
		private static void Main_DrawItem(On_Main.orig_DrawItem orig, Main self, Item item, int whoAmI)
        {
			GlobalEntityItem gen;
			if (item.TryGetGlobalItem<GlobalEntityItem>(out gen))
            {
				gen.DrawInWorld(item, Main.spriteBatch);
            }
			orig(self, item, whoAmI);
        }
		/*Code I wrote for HeartPlusUp! Calamity Mod!
		private static void GlassTileWallFraming(On_Framing.orig_WallFrame orig, int i, int j, bool resetFrame = false)
        {
			Tile tileU = Framing.GetTileSafely(i, j - 1);
			Tile tileR = Framing.GetTileSafely(i + 1, j);
			Tile tileD = Framing.GetTileSafely(i, j + 1);
			Tile tileL = Framing.GetTileSafely(i - 1, j);
			int saveTileTypeU = -1;
			int saveTileTypeR = -1;
			int saveTileTypeD = -1;
			int saveTileTypeL = -1;
			int TileType = ModContent.TileType<EutrophicGlass>();
			if (tileU != null && tileU.HasTile && tileU.TileType == TileType)
            {
				saveTileTypeU = tileU.TileType;
				tileU.TileType = 54;
			}
			if (tileR != null && tileR.HasTile && tileR.TileType == TileType)
			{
				saveTileTypeR = tileR.TileType;
				tileR.TileType = 54;
			}
			if (tileD != null && tileD.HasTile && tileD.TileType == TileType)
			{
				saveTileTypeD = tileD.TileType;
				tileD.TileType = 54;
			}
			if (tileL != null && tileL.HasTile && tileL.TileType == TileType)
			{
				saveTileTypeL = tileL.TileType;
				tileL.TileType = 54;
			}
			orig(i, j, resetFrame);
			if(saveTileTypeU != -1)
				tileU.TileType = (ushort)saveTileTypeU;
			if (saveTileTypeR!= -1)
				tileR.TileType = (ushort)saveTileTypeU;
			if (saveTileTypeD != -1)
				tileD.TileType = (ushort)saveTileTypeU;
			if (saveTileTypeL != -1)
				tileL.TileType = (ushort)saveTileTypeU;
		}*/
	}
}
