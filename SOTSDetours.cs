using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs.Debuffs;
using SOTS.Common;
using SOTS.Common.GlobalNPCs;
using SOTS.FakePlayer;
using SOTS.Items.Celestial;
using SOTS.Items.Conduit;
using SOTS.Items.Furniture;
using SOTS.Items.Pyramid.PyramidWalls;
using SOTS.NPCs.Town;
using SOTS.Projectiles.Chaos;
using SOTS.Projectiles.Inferno;
using SOTS.Projectiles.Minions;
using SOTS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Light;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.HitTile;

namespace SOTS
{
    public static class SOTSDetours
	{
		public static bool DrawingProjectileFromCache = false;
		public static RenderTarget2D TargetProj;
		public static void Initialize()
		{
			On_NetMessage.SendData += NetMessage_SendData;

			On_Main.DrawProjectiles += Main_DrawProjectiles;
			On_Main.DrawProj += Main_DrawProj;
			On_Main.DrawCachedProjs += Main_DrawCachedProjs;
			On_Main.DrawNPCs += Main_DrawNPCs;
			On_Main.DrawPlayers_AfterProjectiles += Main_DrawPlayers_AfterProjectiles;
			On_LightingEngine.GetColor += LightingEngine_GetColor;
			On_Player.ItemCheck_ManageRightClickFeatures_ShieldRaise += Player_ItemCheck_ManageRightClickFeatures_ShieldRaise;
			On_ItemSlot.Draw_SpriteBatch_ItemArray_int_int_Vector2_Color += ItemSlot_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color;
			On_ItemSlot.DrawItemIcon += ItemSlot_DrawItemIcon;
			On_Player.TryUpdateChannel += Player_TryUpdateChannel;
			On_Player.TryCancelChannel += Player_TryCancelChannel;
			On_Player.CanVisuallyHoldItem += Player_CanVisuallyHoldItem;
            //The following is for Time Freeze
            //order of updates: player, NPC, gore, projectile, item, dust, time
            On_Player.Update += Player_Update;
			On_NPC.UpdateNPC_Inner += NPC_UpdateNPC_Inner;
			On_Gore.Update += Gore_Update;
            On_Projectile.Update += Projectile_Update;
			On_Item.UpdateItem += Item_UpdateItem;
			On_Dust.UpdateDust += Dust_UpdateDust;
			On_Main.UpdateTime += Main_UpdateTime;
			On_Item.GetPrefixCategory += Item_GetPrefixCategory;
			On_PlayerDrawLayers.DrawPlayer_27_HeldItem += On_PlayerDrawLayers_DrawPlayer_27_HeldItem;
            On_PlayerDrawLayers.DrawPlayer_30_BladedGlove += On_PlayerDrawLayers_DrawPlayer_30_BladedGlove;
			On_Player.ItemCheck_EmitHeldItemLight += On_Player_ItemCheck_EmitHeldItemLight;
			On_PlayerDrawSet.BoringSetup_2 += On_PlayerDrawSet_BoringSetup_2;
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
			On_Main.CheckMonoliths += Main_CheckMonoliths;

			//Used to make sure you cannot break pyramid walls before Pharaoh's curse
			On_WorldGen.KillWall_CheckFailure += On_WorldGen_KillWall_CheckFailure;

			//Make sure fake player minions can hit npcs through walls
			On_Player.CanHit += On_Player_CanHit;

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
			//Main.NewText("resized");
			Main.QueueMainThreadAction(() =>
			{
				TargetProj = new RenderTarget2D(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight);
			});
			GreenScreenManager.UpdateWindowSize(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight);
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
				if (SOTSPlayer.ModPlayer(self).AmethystRing)
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
				FakePlayerProjectile fPPInstance;
                bool canGetGlobal = self.TryGetGlobalProjectile(out fPPInstance);
				if(canGetGlobal)
                {
					//if(self.owner >= 0)
					//	Main.NewText(i + " : " + fPPInstance.FakeOwnerIdentity + " : " + Main.player[self.owner].channel);
                    fPPInstance.UpdateFakeOwner(self);
                    if (fPPInstance.FakeOwnerIdentity != -1 && fPPInstance.FakeOwnerIdentity != FakePlayerProjectile.OwnerOfThisUpdateCycle) //Don't update this projectile here if it is owned by a fake player. instead, update it when fakeplayer updates
					{
						return;
					}
				}
			}
			orig(self, i);
		}
		private static void Main_DrawProj(On_Main.orig_DrawProj orig, Main self, int i)
        {
            if (i >= 0)
            {
				Projectile proj = Main.projectile[i];
                if (proj.active)
                {
                    FakePlayerProjectile fPPInstance;
                    bool canGetGlobal = proj.TryGetGlobalProjectile(out fPPInstance);
                    if (canGetGlobal)
                    {
                        fPPInstance.UpdateFakeOwner(proj);
                        if (fPPInstance.FakeOwnerIdentity != -1 && fPPInstance.FakeOwnerIdentity != FakePlayerProjectile.OwnerOfThisDrawCycle)
                        {
                            //Main.NewText(proj.ToString());
							if(DrawingProjectileFromCache)
							{
								FakePlayerPossessingProjectile fPPP = fPPInstance.WhoOwnsMe(proj);
								if(fPPP != null && fPPP.FakePlayer != null)
                                {
                                    fPPP.FakePlayer.LoadValuesForCachedProjectileDraw(Main.player[proj.owner], true);
                                    orig(self, i);
                                    fPPP.FakePlayer.LoadValuesForCachedProjectileDraw(Main.player[proj.owner], false);
                                }
                            }
                            return;
                        }
                    }
                }
            }
            orig(self, i);
		}
		private static void Main_DrawCachedProjs(On_Main.orig_DrawCachedProjs orig, Main self, List<int> projCache, bool startSpriteBatch)
		{
			DrawingProjectileFromCache = true;
            orig(self, projCache, startSpriteBatch);
			DrawingProjectileFromCache = false;
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
		private static void Main_CheckMonoliths(On_Main.orig_CheckMonoliths orig)
        {
			if (Main.spriteBatch != null && !Main.dedServ)
			{
				if (!Main.gameMenu && Main.graphics.GraphicsDevice != null)
                {
					bool minZoom = Get_Terraria_ModLoader_ModLoader_removeForcedMinimumZoom();
                    float val = Main.screenWidth / (minZoom ? 8192f : Main.MinimumZoomComparerX);
                    float val2 = Main.screenHeight / (minZoom ? 8192f : Main.MinimumZoomComparerY);
                    Main.ForcedMinimumZoom = Math.Max(Math.Max(1f, val), val2);
                    Main.GameViewMatrix.Effects = (SpriteEffects)((!Main.gameMenu && Main.player[Main.myPlayer].gravDir != 1f) ? 2 : 0);
                    Main.BackgroundViewMatrix.Effects = Main.GameViewMatrix.Effects;
                    Main.BackgroundViewMatrix.Zoom = new Vector2(Main.ForcedMinimumZoom);
                    Main.GameViewMatrix.Zoom = new Vector2(Main.ForcedMinimumZoom * MathHelper.Clamp(Main.GameZoomTarget, 1f, 2f));
                    SystemLoader.ModifyTransformMatrix(ref Main.GameViewMatrix);
                    GreenScreenManager.SetupGreenscreens(Main.spriteBatch, Main.graphics.GraphicsDevice);
				}
			}
            orig();
		}
		public static bool Get_Terraria_ModLoader_ModLoader_removeForcedMinimumZoom()
        {
            Type type = typeof(ModLoader);
            FieldInfo field = type.GetField("removeForcedMinimumZoom", BindingFlags.NonPublic | BindingFlags.Static);
            return (bool)field.GetValue(null);
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
						GlobalEntityNPC gen;
						bool found = npc.TryGetGlobalNPC<GlobalEntityNPC>(out gen);
						if(found)
                        {
                            if (!gen.RecentlyTeleported)
                            {
                                DebuffNPC instancedNPC = npc.GetGlobalNPC<DebuffNPC>();
                                if (instancedNPC.timeFrozen != 0)
                                    instancedNPC.DrawTimeFreeze(npc, Main.spriteBatch);
                            }
                            else
                            {
                                gen.Draw(npc, Main.spriteBatch);
                            }
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

                ParticleHelper.DrawWaterParticles(true);
                FakePlayerDrawing.DrawHydroFakePlayersFull(); //Hydro servant has a totally unique shader, so it must be drawn in different steps... The particle layer will be drawn before this 
                FakePlayerDrawing.DrawFakePlayers(0, DrawStateID.All); //Subspace servant has no shader, and thus can be drawn in its entirety right away
                FakePlayerDrawing.DrawFakePlayers(2, DrawStateID.All); //Subspace servant has no shader, and thus can be drawn in its entirety right away

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
					if(proj.active)
                    {
                        if (proj.ModProjectile is IOrbitingProj modProj && modProj.inFront)
                        {
                            modProj.Draw(Main.spriteBatch, Color.White);
                        }
                        if (proj.ModProjectile is IncineratorGloveProjectile modProj2)
                        {
                            modProj2.Draw(Main.spriteBatch, Color.White);
                        }
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
		private static Vector3 LightingEngine_GetColor(On_LightingEngine.orig_GetColor orig, LightingEngine self, int x, int y)
		{
			if (FakePlayerProjectile.FullBrightThisDrawCycle)
			{
                return new Vector3(1, 1, 1);
            }
			return orig(self, x, y);
		}
		private static void Player_ItemCheck_ManageRightClickFeatures_ShieldRaise(On_Player.orig_ItemCheck_ManageRightClickFeatures_ShieldRaise orig, Player self, bool generalCheck)
		{
			if(FakePlayerProjectile.OwnerOfThisUpdateCycle == -1)
				orig(self, generalCheck);
		}
		private static void ItemSlot_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color(On_ItemSlot.orig_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color orig, SpriteBatch spriteBatch, Item[] inv, int context, int slot, Vector2 position, Color lightColor = default(Color))
        {
            PlayerInventorySlotsManager.SavedInventoryColor = Main.inventoryBack;
            Player player = Main.player[Main.myPlayer];
            Item item = inv[slot];
            if (PlayerInventorySlotsManager.DrawSubspaceSlot(item))
                Main.inventoryBack = Color.Transparent;
            if (slot == 49)
            {
				FakeModPlayer subPlayer = FakeModPlayer.ModPlayer(player);
				if(subPlayer.servantActive && !subPlayer.servantIsVanity)
				{
					if(item.type <= ItemID.None || item.stack <= 0)
					{
						int saveItemType = item.type;
						int saveItemStack = item.stack;
						item.type = ModContent.ItemType<SubspaceLocket>();
						item.stack = 1;
						PlayerInventorySlotsManager.FakeBorderDrawCycle = true;
                        orig(spriteBatch, inv, context, slot, position, lightColor);
                        PlayerInventorySlotsManager.FakeBorderDrawCycle = false;
                        item.type = saveItemType;
						item.stack = saveItemStack;
                        return;
					}
				}
            }
            orig(spriteBatch, inv, context, slot, position, lightColor);
		}
		private static float ItemSlot_DrawItemIcon(On_ItemSlot.orig_DrawItemIcon orig, Item item, int context, SpriteBatch spriteBatch, Vector2 screenPositionForItemCenter, float scale, float sizeLimit, Color environmentColor)
        {
            PlayerInventorySlotsManager.PreDrawSlots(item, spriteBatch, screenPositionForItemCenter, Color.White);
            Main.inventoryBack = PlayerInventorySlotsManager.SavedInventoryColor;
            if (!PlayerInventorySlotsManager.FakeBorderDrawCycle)
            {
                return orig(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);
            }
			return 0f;
        }
        private static void Player_TryUpdateChannel(On_Player.orig_TryUpdateChannel orig, Player self, Projectile projectile)
        {
            orig(self, projectile);
        }
        private static void Player_TryCancelChannel(On_Player.orig_TryCancelChannel orig, Player self, Projectile projectile)
        {
			orig(self, projectile);
        }
		private static PrefixCategory? Item_GetPrefixCategory(On_Item.orig_GetPrefixCategory orig, Item self)
		{
			if(self.IsAPrefixableAccessory())
			{
				if(self.ModItem != null && self.ModItem.Mod != null)
                {
                    if (self.ModItem.Mod is SOTS)
                    {
                        return PrefixCategory.Accessory;
                    }
                }
			}
			return orig(self);
		}
		private static bool Player_CanVisuallyHoldItem(On_Player.orig_CanVisuallyHoldItem orig, Player self, Item item)
        {
            if (FakeModPlayer.ModPlayer(self).hasHydroFakePlayer)
            {
                bool isHydroPlayerUsingAnItem = FakePlayer.FakePlayer.CheckItemValidityFull(self, item, item, 1);
                if (isHydroPlayerUsingAnItem)
                {
					if(FakePlayerProjectile.OwnerOfThisDrawCycle != -1 || FakePlayerProjectile.OwnerOfThisUpdateCycle != -1)
						return orig(self, item);
					else
						return false;
                }
            }
            return orig(self, item);
        }
		private static void On_PlayerDrawLayers_DrawPlayer_27_HeldItem(On_PlayerDrawLayers.orig_DrawPlayer_27_HeldItem orig, ref PlayerDrawSet drawInfo)
        {
			Player self = drawInfo.drawPlayer;
			if (FakeModPlayer.ModPlayer(self).hasHydroFakePlayer)
            {
                Item item = drawInfo.drawPlayer.HeldItem;
                if (FakePlayerProjectile.OwnerOfThisDrawCycle == -1)
                {
                    bool isHydroPlayerUsingAnItem = FakePlayer.FakePlayer.CheckItemValidityFull(self, item, item, 1);
					if (isHydroPlayerUsingAnItem)
					{
                        if (drawInfo.drawPlayer.heldProj >= 0 && drawInfo.shadow == 0f)
                        {
                            drawInfo.projectileDrawPosition = drawInfo.DrawDataCache.Count;
                        }
                        return;
                    }
				}
			}
			orig(ref drawInfo);
        }
        private static void On_PlayerDrawLayers_DrawPlayer_30_BladedGlove(On_PlayerDrawLayers.orig_DrawPlayer_30_BladedGlove orig, ref PlayerDrawSet drawInfo)
        {
            Player self = drawInfo.drawPlayer;
            if (FakeModPlayer.ModPlayer(self).hasHydroFakePlayer)
            {
                Item item = drawInfo.drawPlayer.HeldItem;
                if (FakePlayerProjectile.OwnerOfThisDrawCycle == -1)
                {
                    bool isHydroPlayerUsingAnItem = FakePlayer.FakePlayer.CheckItemValidityFull(self, item, item, 1);
                    if (isHydroPlayerUsingAnItem)
                    {
                        return;
                    }
                }
            }
            orig(ref drawInfo);
        }
		private static void On_Player_ItemCheck_EmitHeldItemLight(On_Player.orig_ItemCheck_EmitHeldItemLight orig, Player self, Item item)
        {
            if (FakeModPlayer.ModPlayer(self).hasHydroFakePlayer)
            {
                if (FakePlayerProjectile.OwnerOfThisUpdateCycle == -1)
                {
                    bool isHydroPlayerUsingAnItem = FakePlayer.FakePlayer.CheckItemValidityFull(self, item, item, 1);
                    if (isHydroPlayerUsingAnItem)
                    {
                        return;
                    }
                }
            }
            orig(self, item);
		}
		private static void On_PlayerDrawSet_BoringSetup_2(On_PlayerDrawSet.orig_BoringSetup_2 orig, ref PlayerDrawSet self, Player player, List<DrawData> drawData, List<int> dust, List<int> gore, Vector2 drawPosition, float shadowOpacity, float rotation, Vector2 rotationOrigin)
		{
			orig(ref self, player, drawData, dust, gore, drawPosition, shadowOpacity, rotation, rotationOrigin);
            if (FakeModPlayer.ModPlayer(player).hasHydroFakePlayer)
            {
				if(FakePlayerProjectile.OwnerOfThisDrawCycle == -1)
                {
                    bool isHydroPlayerUsingAnItem = FakePlayer.FakePlayer.CheckItemValidityFull(player, player.HeldItem, player.HeldItem, 1);
                    if (isHydroPlayerUsingAnItem)
                    {
                        self.weaponDrawOrder = WeaponDrawOrder.BehindFrontArm;
                    }
                }
            }
        }
		private static bool On_WorldGen_KillWall_CheckFailure(On_WorldGen.orig_KillWall_CheckFailure orig, bool fail, Tile tileCache)
        {
            if (SOTSWall.unsafePyramidWall.Contains(tileCache.WallType))
            {
                return !SOTSWorld.downedCurse;
			}
            return orig(fail, tileCache);
		}
		private static bool On_Player_CanHit(On_Player.orig_CanHit orig, Player self, Entity ent)
        {
            if (FakePlayerProjectile.OwnerOfThisUpdateCycle != -1) //Allow hitting through walls with fake players
            {
				return true;
			}
			return orig(self, ent);
		}
    }
}
