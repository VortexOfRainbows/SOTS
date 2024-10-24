using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using SOTS.Prim;
using SOTS.Void;
using SOTS.Items.Pyramid;
using SOTS.Items.Wings;
using SOTS.Common.GlobalNPCs;
using SOTS.NPCs.Boss;
using SOTS.Items.Slime;
using SOTS.Items.Permafrost;
using SOTS.Items.Celestial;
using SOTS.Items.MusicBoxes;
using SOTS.NPCs.Boss.Curse;
using SOTS.NPCs.Boss.Advisor;
using SOTS.Items.Banners;
using Terraria.Graphics.Shaders;
using SOTS.Items.Dyes;
using Terraria.Graphics.Effects;
using SOTS.NPCs.Boss.Lux;
using SOTS.Items;
using ReLogic.Content;
using Terraria.GameContent;
using SOTS.NPCs.Boss.Glowmoth;
using SOTS.Items.Earth.Glowmoth;
using SOTS.Items.Tools;
using SOTS.NPCs.Boss.Polaris.NewPolaris;
using SOTS.FakePlayer;
using SOTS.Common;
using SOTS.Common.ModPlayers;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Earth;

namespace SOTS
{
	public class SOTS : Mod
	{
		private const string SOTSTexturePackName = "Secrets of the Shadows Texture Pack";
		public static bool IsSOTSTexturePackEnabled()
		{
			if (Main.netMode == NetmodeID.Server)
				return false;
			AssetSourceController aSC = Main.AssetSourceController;
            IEnumerable<Terraria.IO.ResourcePack> list = aSC.ActiveResourcePackList.EnabledPacks;
			foreach(Terraria.IO.ResourcePack item in list)
            {
				//Main.NewText(item.Name);
				if(item.Name.Equals(SOTSTexturePackName))
                {
					return true;
                }
            }				
			return false;
		}
		private static Mod SubworldLibrary;
		public static void SetSubworld()
		{
            ModLoader.TryGetMod("SubworldLibrary", out SubworldLibrary);
        }
        public static bool InSubworld()
        {
            if (SubworldLibrary == null)
            {
                return false;
            }
            foreach (Mod mod in ModLoader.Mods)
            {
                if (mod.Name.Equals(SubworldLibrary.Name))
                {
                    continue;
                }
                bool anySubworldForMod = (SubworldLibrary.Call("AnyActive", mod) as bool?) ?? false;
                if (anySubworldForMod)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool SOTSTexturePackEnabled = false;
		public static bool SOTSTexturePackEnabledWithTiles => SOTSTexturePackEnabled && Config.additionalTexturePackVisuals;
		public static bool SOTSTexturePackEnabledAudio => false; //SOTSTexturePackEnabled && Config.playAmbientAudio;
		public static PrimTrailManager primitives;

		public static ModKeybind BlinkHotKey;
		public static ModKeybind ArmorSetHotKey;
		public static ModKeybind MachinaBoosterHotKey;
        public static ModKeybind SlowFlightHotKey;
        internal static SOTS Instance;

		public static Effect AtenTrail;
		public static Effect WaterTrail;
		public static Effect FireTrail;
		public static Effect GridTrail;
		public static Effect FireballShader;
		public static Effect GodrayShader;
		public static Effect VisionShader;
		public static Effect BarrierShader;
		public static SOTSConfig Config
        {
			get => ModContent.GetInstance<SOTSConfig>();
		}
		public static SOTSServerConfig ServerConfig
		{
			get => ModContent.GetInstance<SOTSServerConfig>();
		}
		/*public SOTS()
		{
			Properties = new ModProperties() //This seems largely unused now
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}*/
		public static int PlayerCount = 0;
		public override void Load()
		{
			//SOTSGlowmasks.LoadGlowmasks();
			Instance = ModContent.GetInstance<SOTS>();
            BlinkHotKey = KeybindLoader.RegisterKeybind(this, Language.GetOrRegister("Mods.SOTS.KeyBindName.Blink").ToString(), "V");//TODO: Localize it when 1.4.4 comes
			ArmorSetHotKey = KeybindLoader.RegisterKeybind(this, Language.GetOrRegister("Mods.SOTS.KeyBindName.ArmorSet").ToString(), "F");
			MachinaBoosterHotKey = KeybindLoader.RegisterKeybind(this, Language.GetOrRegister("Mods.SOTS.KeyBindName.MFM").ToString(), "C");
            SlowFlightHotKey = KeybindLoader.RegisterKeybind(this, Language.GetOrRegister("Mods.SOTS.KeyBindName.SlowFlight").ToString(), "LeftShift");
            SOTSWorld.LoadUI();
			SetSubworld();
			/*Mod yabhb = ModLoader.GetMod("FKBossHealthBar");
			if (yabhb != null)
			{
				yabhb.Call("hbStart");
				yabhb.Call("hbSetTexture",
					(Texture2D)ModContent.Request<Texture2D>("SOTS/UI/PinkyHealthbarLeft"),
					(Texture2D)ModContent.Request<Texture2D>("SOTS/UI/PinkyHealthbarMid"),
					(Texture2D)ModContent.Request<Texture2D>("SOTS/UI/PinkyHealthbarEnd"),
					(Texture2D)ModContent.Request<Texture2D>("SOTS/UI/PinkyHealthbarFill"));
				yabhb.Call("hbSetMidBarOffset", -36, 12);
				yabhb.Call("hbSetBossHeadCentre", 16, 30);
				yabhb.Call("hbSetFillDecoOffset", 10);
				yabhb.Call("hbLoopMidBar", true);
				yabhb.Call("hbFinishSingle", ModContent.NPCType<PutridPinkyPhase2>());

				yabhb.Call("hbStart");
				yabhb.Call("hbSetTexture",
					(Texture2D)ModContent.Request<Texture2D>("SOTS/UI/SubspaceHBLeft"),
					(Texture2D)ModContent.Request<Texture2D>("SOTS/UI/SubspaceHBMid"),
					(Texture2D)ModContent.Request<Texture2D>("SOTS/UI/SubspaceHBEnd"),
					(Texture2D)ModContent.Request<Texture2D>("SOTS/UI/SubspaceHBFill"));
				yabhb.Call("hbSetMidBarOffset", -28, 8);
				yabhb.Call("hbSetBossHeadCentre", 32, 26);
				yabhb.Call("hbSetFillDecoOffset", 10);
				yabhb.Call("hbLoopMidBar", true);
				yabhb.Call("hbSetColours",
					new Color(155, 255, 150),
					new Color(1f, 1f, 0f), 
					new Color(1f, 0f, 0f));
				yabhb.Call("hbFinishSingle", ModContent.NPCType<SubspaceSerpentHead>());
			}*/
			//Music Box Stuff
			MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Sounds/Music/PutridPinky"), ModContent.ItemType<PutridPinkyMusicBox>(), ModContent.TileType<PutridPinkyMusicBoxTile>());
			MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Sounds/Music/Advisor"), ModContent.ItemType<AdvisorMusicBox>(), ModContent.TileType<AdvisorMusicBoxTile>());
			MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Sounds/Music/Polaris"), ModContent.ItemType<PolarisMusicBox>(), ModContent.TileType<PolarisMusicBoxTile>());
			MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Sounds/Music/SubspaceSerpent"), ModContent.ItemType<SubspaceSerpentMusicBox>(), ModContent.TileType<SubspaceSerpentMusicBoxTile>());
			MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Sounds/Music/CursedPyramid"), ModContent.ItemType<AncientPyramidMusicBox>(), ModContent.TileType<AncientPyramidMusicBoxTile>());
			MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Sounds/Music/PyramidBattle"), ModContent.ItemType<PyramidBattleMusicBox>(), ModContent.TileType<PyramidBattleMusicBoxTile>());
			MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Sounds/Music/Planetarium"), ModContent.ItemType<PlanetariumMusicBox>(), ModContent.TileType<PlanetariumMusicBoxTile>());
			MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Sounds/Music/KnucklesTheme"), ModContent.ItemType<KnucklesMusicBox>(), ModContent.TileType<KnucklesMusicBoxTile>()); //WHY THE FUCK
			MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Sounds/Music/Lux"), ModContent.ItemType<LuxMusicBox>(), ModContent.TileType<LuxMusicBoxTile>());
			MusicLoader.AddMusicBox(this, MusicLoader.GetMusicSlot(this, "Sounds/Music/PharaohsCurse"), ModContent.ItemType<CurseMusicBox>(), ModContent.TileType<CurseMusicBoxTile>());
			SOTSItem.LoadArrays();
			SOTSTile.LoadArrays();
			SOTSWall.LoadArrays();
			SOTSPlayer.LoadArrays();
			SOTSProjectile.LoadArrays();
			DebuffNPC.LoadArrays();
			if(Main.netMode != NetmodeID.Server)
			{
                Asset<Effect> TPrismdyeRef = Assets.Request<Effect>("Effects/TPrismEffect", AssetRequestMode.ImmediateLoad);
                Asset<Effect> voidMageShader = Assets.Request<Effect>("Effects/VMShader", AssetRequestMode.ImmediateLoad);
				Asset<Effect> anomalyShader = Assets.Request<Effect>("Effects/AnomalyShader", AssetRequestMode.ImmediateLoad);
				GameShaders.Armor.BindShader(ModContent.ItemType<TaintedPrismDye>(), new ArmorShaderData(TPrismdyeRef, "TPrismDyePass")).UseColor(0.3f, 0.4f, 0.4f);
				Filters.Scene["VMFilter"] = new Filter(new ScreenShaderData(voidMageShader, "VMShaderPass"), EffectPriority.VeryHigh);
				Filters.Scene["VMFilter"].Load();
				Filters.Scene["AnomalyFilter"] = new Filter(new ScreenShaderData(anomalyShader, "AnomalyShaderPass"), EffectPriority.VeryHigh);
				Filters.Scene["AnomalyFilter"].Load();
				AtenTrail = Instance.Assets.Request<Effect>("Effects/AtenTrail", AssetRequestMode.ImmediateLoad).Value;
				WaterTrail = Instance.Assets.Request<Effect>("Effects/WaterTrail", AssetRequestMode.ImmediateLoad).Value;
				FireTrail = Instance.Assets.Request<Effect>("Effects/FireTrail", AssetRequestMode.ImmediateLoad).Value;
				FireballShader = Instance.Assets.Request<Effect>("Effects/FireballShader", AssetRequestMode.ImmediateLoad).Value;
				GodrayShader = Instance.Assets.Request<Effect>("Effects/GodrayShader", AssetRequestMode.ImmediateLoad).Value;
				VisionShader = Instance.Assets.Request<Effect>("Effects/VisionShader", AssetRequestMode.ImmediateLoad).Value;
				BarrierShader = Instance.Assets.Request<Effect>("Effects/BarrierShader", AssetRequestMode.ImmediateLoad).Value;
				GridTrail = Instance.Assets.Request<Effect>("Effects/GridTrail", AssetRequestMode.ImmediateLoad).Value;
				Main.QueueMainThreadAction(() => {
					primitives = new PrimTrailManager();
					primitives.LoadContent(Main.graphics.GraphicsDevice);
				});
			}
			SOTSDetours.Initialize();
			SOTSTexturePackEnabled = IsSOTSTexturePackEnabled();
		}
		public override void Unload() 
		{
			WaterTrail = null;
			FireTrail = null;
			AtenTrail = null;
			FireballShader = null;
			GodrayShader = null;
			VisionShader = null;
			Instance = null;
			VoidBarSprite._backgroundTexture = null;
			VoidBarBorder._backgroundTexture = null;
			VoidBarBorder2._backgroundTexture = null;
			BarDivider._backgroundTexture = null;
			SoulBar._backgroundTexture = null;
			VoidUI.voidUI = null;
			BlinkHotKey = null;
			ArmorSetHotKey = null;
			MachinaBoosterHotKey = null;
			SubworldLibrary = null;
			SOTSDetours.Unload();
		}
		public static Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
		{
			float u = 1 - t;
			float tt = t * t;
			float uu = u * u;
			float uuu = uu * u;
			float ttt = tt * t;
			Vector2 p = uuu * p0; //first term
			p += 3 * uu * t * p1; //second term
			p += 3 * u * tt * p2; //third term
			p += ttt * p3; //fourth term
			return p;
		}
		internal enum SOTSMessageType : int
		{
			SOTSSyncPlayer,
			SyncCreativeFlight,
			SyncLootingSoulsAndVoidMax,
			SyncGlobalNPC,
			SyncPlayerKnives,
			SyncRexFlower,
			SyncGlobalProj,
			SyncVisionNumber,
			SyncGlobalNPCTime,
			SyncGlobalProjTime,
			SyncGlobalWorldFreeze,
			SyncGlobalCounter,
			SyncGlobalGemLocks,
			SyncTileLocations,
			RequestTileLocations,
			SyncHasTeleported,
			SyncTesseractData,
			SyncConduitPlayer,
			SyncConduitPlayerAll,
            SyncGlobalNPC2,
			SyncFamineBlock
        }
		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			int msgType = reader.ReadByte();
            //Main.NewText(msgType);
            if (msgType == (int)SOTSMessageType.SyncTileLocations || msgType == (int)SOTSMessageType.RequestTileLocations)
            {
				Common.Systems.ImportantTilesWorld.HandlePacket(reader, whoAmI, msgType);
				return;
            }
			switch (msgType)
			{
				case (int)SOTSMessageType.SOTSSyncPlayer:
					byte playernumber = reader.ReadByte();
					SOTSPlayer modPlayer = Main.player[playernumber].GetModPlayer<SOTSPlayer>();
					MachinaBoosterPlayer mbPlayer = Main.player[playernumber].GetModPlayer<MachinaBoosterPlayer>();
					VoidPlayer voidPlayer = Main.player[playernumber].GetModPlayer<VoidPlayer>();
					bool creativeFlight = reader.ReadBoolean();
					mbPlayer.creativeFlight = creativeFlight;
					int lootingSouls = reader.ReadInt32();
					voidPlayer.lootingSouls = lootingSouls;
					break;
                case (int)SOTSMessageType.SyncCreativeFlight:
					playernumber = reader.ReadByte(); 
					mbPlayer = Main.player[playernumber].GetModPlayer<MachinaBoosterPlayer>();
					mbPlayer.creativeFlight = reader.ReadBoolean();
					mbPlayer.SlowFlight = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)SOTSMessageType.SyncCreativeFlight);
						packet.Write(playernumber);
						packet.Write(mbPlayer.creativeFlight);
                        packet.Write(mbPlayer.SlowFlight);
                        packet.Send(-1, playernumber);
					}
					break;
				case (int)SOTSMessageType.SyncLootingSoulsAndVoidMax:
					playernumber = reader.ReadByte();
					voidPlayer = Main.player[playernumber].GetModPlayer<VoidPlayer>();
					voidPlayer.lootingSouls = reader.ReadInt32();
					voidPlayer.voidMeterMax = reader.ReadInt32();
					voidPlayer.voidMeterMax2 = reader.ReadInt32();
					voidPlayer.voidMeter = reader.ReadSingle();
					voidPlayer.VoidMinionConsumption = reader.ReadInt32();
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)SOTSMessageType.SyncLootingSoulsAndVoidMax);
						packet.Write(playernumber);
						packet.Write(voidPlayer.lootingSouls);
						packet.Write(voidPlayer.voidMeterMax);
						packet.Write(voidPlayer.voidMeterMax2);
						packet.Write(voidPlayer.voidMeter);
						packet.Write(voidPlayer.VoidMinionConsumption);
						packet.Send(-1, playernumber);
					}
					break;
				case (int)SOTSMessageType.SyncGlobalNPC:
					playernumber = reader.ReadByte();
					int npcNumber = reader.ReadInt32();
					DebuffNPC debuffNPC = Main.npc[npcNumber].GetGlobalNPC<DebuffNPC>();
					debuffNPC.HarvestCurse = reader.ReadInt32();
					debuffNPC.PlatinumCurse = reader.ReadInt32();
					debuffNPC.DestableCurse = reader.ReadInt32();
					debuffNPC.BleedingCurse = reader.ReadInt32();
					debuffNPC.BlazingCurse = reader.ReadInt32();
					debuffNPC.AnomalyCurse = reader.ReadInt32();
                    debuffNPC.BlightCurse = reader.ReadInt32();
                    debuffNPC.OwnerOfVoidspaceCurseDamage = reader.ReadInt32();
                    if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)SOTSMessageType.SyncGlobalNPC);
						packet.Write(playernumber);
						packet.Write(npcNumber);
						packet.Write(debuffNPC.HarvestCurse);
						packet.Write(debuffNPC.PlatinumCurse);
						packet.Write(debuffNPC.DestableCurse);
						packet.Write(debuffNPC.BleedingCurse);
						packet.Write(debuffNPC.BlazingCurse);
						packet.Write(debuffNPC.AnomalyCurse);
                        packet.Write(debuffNPC.BlightCurse);
                        packet.Write(debuffNPC.OwnerOfVoidspaceCurseDamage);
                        packet.Send(-1, playernumber);
					}
					break;
				case (int)SOTSMessageType.SyncPlayerKnives:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<SOTSPlayer>();
					modPlayer.skywardBlades = reader.ReadInt32();
					modPlayer.cursorRadians = reader.ReadSingle();
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)SOTSMessageType.SyncPlayerKnives);
						packet.Write(playernumber);
						packet.Write(modPlayer.skywardBlades);
						packet.Write(modPlayer.cursorRadians);
						packet.Send(-1, playernumber);
					}
					break;
				case (int)SOTSMessageType.SyncGlobalProj:
					playernumber = reader.ReadByte();
					int projIdentity = reader.ReadInt32();
					int frostFlake = reader.ReadInt32();
					int affixID = reader.ReadInt32();
					if (Main.netMode != NetmodeID.Server)
                    {
						for(int i = 0; i < Main.maxProjectiles; i++)
                        {
							Projectile projectile = Main.projectile[i];
							if (projectile.active && projectile.identity == projIdentity)
							{
								projIdentity = projectile.whoAmI;
								break;
                            }
                        }
                    }
					SOTSProjectile sProj = Main.projectile[projIdentity].GetGlobalProjectile<SOTSProjectile>();
					sProj.frostFlake = frostFlake;
					sProj.affixID = affixID;
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)SOTSMessageType.SyncGlobalProj);
						packet.Write(playernumber);
						packet.Write(projIdentity);
						packet.Write(frostFlake);
						packet.Write(affixID);
						packet.Send(-1, playernumber);
					}
					break;
				case (int)SOTSMessageType.SyncVisionNumber:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<SOTSPlayer>();
					modPlayer.UniqueVisionNumber = reader.ReadInt32();
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)SOTSMessageType.SyncVisionNumber);
						packet.Write(playernumber);
						packet.Write(modPlayer.UniqueVisionNumber);
						packet.Send(-1, playernumber);
					}
					break;
				case (int)SOTSMessageType.SyncGlobalNPCTime:
					int playernumber2 = reader.ReadInt32();
					npcNumber = reader.ReadInt32();
					debuffNPC = Main.npc[npcNumber].GetGlobalNPC<DebuffNPC>();
					debuffNPC.timeFrozen = reader.ReadInt32();
					debuffNPC.frozen = reader.ReadBoolean();
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)SOTSMessageType.SyncGlobalNPCTime);
						packet.Write(playernumber2);
						packet.Write(npcNumber);
						packet.Write(debuffNPC.timeFrozen);
						packet.Write(debuffNPC.frozen);
						packet.Send(-1, playernumber2);
					}
					break;
				case (int)SOTSMessageType.SyncGlobalProjTime:
					playernumber2 = reader.ReadInt32();
					int projNumber = reader.ReadInt32();
					sProj = Main.projectile[projNumber].GetGlobalProjectile<SOTSProjectile>();
					sProj.timeFrozen = reader.ReadInt32();
					sProj.frozen = reader.ReadBoolean();
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)SOTSMessageType.SyncGlobalProjTime);
						packet.Write(playernumber2);
						packet.Write(projNumber);
						packet.Write(sProj.timeFrozen);
						packet.Write(sProj.frozen);
						packet.Send(-1, playernumber2);
					}
					break;
				case (int)SOTSMessageType.SyncGlobalWorldFreeze:
					playernumber2 = reader.ReadInt32();
					SOTSWorld.GlobalTimeFreeze = reader.ReadInt32();
					SOTSWorld.GlobalFrozen = reader.ReadBoolean();
					SOTSWorld.GlobalFreezeCounter = reader.ReadSingle();
					SOTSWorld.GlobalSpeedMultiplier = reader.ReadSingle();
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)SOTSMessageType.SyncGlobalWorldFreeze);
						packet.Write(playernumber2);
						packet.Write(SOTSWorld.GlobalTimeFreeze);
						packet.Write(SOTSWorld.GlobalFrozen);
						packet.Write(SOTSWorld.GlobalFreezeCounter);
						packet.Write(SOTSWorld.GlobalSpeedMultiplier);
						packet.Send(-1, playernumber2);
					}
					break;
				case (int)SOTSMessageType.SyncGlobalCounter:
					SOTSWorld.GlobalCounter = reader.ReadInt32();
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)SOTSMessageType.SyncGlobalCounter);
						packet.Write(SOTSWorld.GlobalCounter);
						packet.Send(-1, -1);
					}
					break;
				case (int)SOTSMessageType.SyncGlobalGemLocks:
					playernumber2 = reader.ReadInt32();
					SOTSWorld.RubyKeySlotted = reader.ReadBoolean();
					SOTSWorld.SapphireKeySlotted = reader.ReadBoolean();
					SOTSWorld.EmeraldKeySlotted = reader.ReadBoolean();
					SOTSWorld.TopazKeySlotted = reader.ReadBoolean();
					SOTSWorld.AmethystKeySlotted  = reader.ReadBoolean();
					SOTSWorld.DiamondKeySlotted = reader.ReadBoolean();
					SOTSWorld.AmberKeySlotted = reader.ReadBoolean();
					SOTSWorld.DreamLampSolved = reader.ReadBoolean();
                    SOTSWorld.GoldenAppleSolved = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)SOTSMessageType.SyncGlobalGemLocks);
						packet.Write(playernumber2);
						packet.Write(SOTSWorld.RubyKeySlotted);
						packet.Write(SOTSWorld.SapphireKeySlotted);
						packet.Write(SOTSWorld.EmeraldKeySlotted);
						packet.Write(SOTSWorld.TopazKeySlotted);
						packet.Write(SOTSWorld.AmethystKeySlotted);
						packet.Write(SOTSWorld.DiamondKeySlotted);
						packet.Write(SOTSWorld.AmberKeySlotted);
						packet.Write(SOTSWorld.DreamLampSolved);
                        packet.Write(SOTSWorld.GoldenAppleSolved);
                        packet.Send(-1, playernumber2);
					}
					break;
                case (int)SOTSMessageType.SyncGlobalNPC2:
                    playernumber = reader.ReadByte();
					npcNumber = reader.ReadInt32();
					debuffNPC = Main.npc[npcNumber].GetGlobalNPC<DebuffNPC>();
                    debuffNPC.CrystalCurse = reader.ReadInt32();
                    debuffNPC.TriggeredCrystalCurse = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var packet = GetPacket();
                        packet.Write((byte)SOTSMessageType.SyncGlobalNPC2);
                        packet.Write(playernumber);
                        packet.Write(npcNumber);
                        packet.Write(debuffNPC.CrystalCurse);
                        packet.Write(debuffNPC.TriggeredCrystalCurse);
                        packet.Send(-1, playernumber);
                    }
                    break;
            }
			if(msgType == (int)SOTSMessageType.SyncHasTeleported)
            {
				int whoAmItemOrNpc = reader.ReadInt32();
				Vector2 cen = reader.ReadVector2();
				Vector2 vel = reader.ReadVector2();
				bool recentlyTeleported = reader.ReadBoolean();
				NPC npc = Main.npc[whoAmItemOrNpc];
				if (npc.TryGetGlobalNPC<Common.GlobalEntityNPC>(out GlobalEntityNPC gInstance))
				{
					gInstance.RecentlyTeleported = recentlyTeleported;
					//gInstance.PreviousTeleported = recentlyTeleported;
				}
				npc.Center = cen;
				npc.velocity = vel;
            }
            if (msgType == (int)SOTSMessageType.SyncTesseractData)
            {
                byte playernumber = reader.ReadByte();
                int ID = reader.ReadInt32();
                FakeModPlayer fPlayer = FakeModPlayer.ModPlayer(Main.player[playernumber]);
                fPlayer.tesseractData[ID].AltFunctionUse = reader.ReadBoolean();
                fPlayer.tesseractData[ID].ChargeFrames = reader.ReadInt32();
                if (Main.netMode == NetmodeID.Server)
                {
					//if (Main.netMode == NetmodeID.Server)
					//	ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Handling Packet: " + msgType + ", " + playernumber + ", " + ID), Color.Gray);
                    var packet = GetPacket();
                    packet.Write((byte)msgType);
                    packet.Write((byte)playernumber);
                    packet.Write(ID);
                    packet.Write(fPlayer.tesseractData[ID].AltFunctionUse);
                    packet.Write(fPlayer.tesseractData[ID].ChargeFrames);
                    packet.Send(-1, playernumber);
                }
            }
			if(msgType == (int)SOTSMessageType.SyncConduitPlayer)
            {
                byte playernumber = reader.ReadByte();
				ConduitPlayer CP = Main.player[playernumber].ConduitPlayer();
                int valueType = reader.ReadInt32();
                int newValue = reader.ReadInt32();
				if (valueType == 0)
					CP.NaturePower = newValue;
                if (valueType == 1)
                    CP.EarthPower = newValue;
                if (valueType == 2)
                    CP.PermafrostPower = newValue;
                if (valueType == 3)
                    CP.OtherworldPower = newValue;
                if (valueType == 4)
                    CP.TidePower = newValue;
                if (valueType == 5)
                    CP.EvilPower = newValue;
                if (valueType == 6)
                    CP.InfernoPower = newValue;
                if (valueType == 7)
                    CP.ChaosPower = newValue;
                if (Main.netMode == NetmodeID.Server)
                {
                    var packet = GetPacket();
                    packet.Write((byte)SOTSMessageType.SyncConduitPlayer);
                    packet.Write((byte)playernumber);
                    packet.Write(valueType);
                    packet.Write(newValue);
                    packet.Send(-1, playernumber);
                }
            }
            if (msgType == (int)SOTSMessageType.SyncConduitPlayerAll)
            {
                byte playernumber = reader.ReadByte();
                ConduitPlayer CP = Main.player[playernumber].ConduitPlayer();
                CP.NaturePower = reader.ReadInt32();
                CP.EarthPower = reader.ReadInt32();
                CP.PermafrostPower = reader.ReadInt32();
                CP.OtherworldPower = reader.ReadInt32();
                CP.TidePower = reader.ReadInt32();
                CP.EvilPower = reader.ReadInt32();
                CP.InfernoPower = reader.ReadInt32();
                CP.ChaosPower = reader.ReadInt32();
                if (Main.netMode == NetmodeID.Server)
                {
                    var packet = GetPacket();
                    packet.Write((byte)SOTSMessageType.SyncConduitPlayerAll);
                    packet.Write((byte)playernumber);
                    packet.Write(CP.NaturePower);
                    packet.Write(CP.EarthPower);
                    packet.Write(CP.PermafrostPower);
                    packet.Write(CP.OtherworldPower);
                    packet.Write(CP.TidePower);
                    packet.Write(CP.EvilPower);
                    packet.Write(CP.InfernoPower);
                    packet.Write(CP.ChaosPower);
                    packet.Send(-1, playernumber);
                }
            }
			if(msgType == (int)SOTSMessageType.SyncFamineBlock)
			{
				int NPCID = reader.ReadInt32();
				NPC npc = Main.npc[NPCID];
				if(npc.ModNPC is NPCs.AbandonedVillage.Famished fm)
				{
					fm.ReceiveBlockData(reader);
				}
			}
        }
		public static void SendTesseractDataPacket(int playerNumber, int tesseractID)
        {
			if (Main.netMode == NetmodeID.SinglePlayer)
				return;
            FakeModPlayer fPlayer = FakeModPlayer.ModPlayer(Main.player[playerNumber]);
            var packet = Instance.GetPacket();
            packet.Write((byte)SOTSMessageType.SyncTesseractData);
            packet.Write((byte)playerNumber);
            packet.Write(tesseractID);
            packet.Write(fPlayer.tesseractData[tesseractID].AltFunctionUse);
            packet.Write(fPlayer.tesseractData[tesseractID].ChargeFrames);
            packet.Send(-1, playerNumber);
        }
		public override void PostSetupContent()
		{
			Mod bossChecklist;
			bool available = ModLoader.TryGetMod("BossChecklist", out bossChecklist);
			if (available)
			{
				/*KingSlime = 1f;
				EyeOfCthulhu = 2f;
				EaterOfWorlds = 3f; // and Brain of Cthulhu
				QueenBee = 4f;
				Skeletron = 5f;
				DeerClops = 6f;
				WallOfFlesh = 7f;
				QueenSlime = 8f;
				TheTwins = 9f;
				TheDestroyer = 10f;
				SkeletronPrime = 11f;
				Plantera = 12f;
				Golem = 13f;
				DukeFishron = 14f;
				EmpressOfLight = 15f;
				Betsy = 16f;
				LunaticCultist = 17f;
				Moonlord = 18f; */
				bossChecklist.Call(
					"LogBoss",
					this,
					nameof(PutridPinkyPhase2),
					4.25f,
					(Func<bool>)(() => SOTSWorld.downedPinky),
					new List<int>() { ModContent.NPCType<PutridPinkyPhase2>() },
					new Dictionary<string, object>()
					{
						["displayName"] = Language.GetText("Mods.SOTS.NPCs.PutridPinkyPhase2.DisplayName"),
						["spawnInfo"] = Language.GetText("Mods.SOTS.NPCs.PutridPinkyPhase2.BossChecklistIntegration.SpawnInfo"),
						["spawnItems"] = ModContent.ItemType<JarOfPeanuts>(),
						["collectibles"] = new List<int>() { ModContent.ItemType<PutridPinkyMusicBox>(), ModContent.ItemType<PutridPinkyTrophy>() },
						["availability"] = (Func<bool>)(() => true),
						//["overrideHeadTextures"] = ,
						["despawnMessage"] = Language.GetText("Mods.SOTS.NPCs.PutridPinkyPhase2.BossChecklistIntegration.DespawnMessage"),
						["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
						{
							Texture2D texture = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/PutridPinky1_Display").Value;
							Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
							sb.Draw(texture, centered, color);
						}
					}
				);
				bossChecklist.Call(
					"LogBoss",
					this,
					nameof(Glowmoth),
					2.1f,
					(Func<bool>)(() => SOTSWorld.downedGlowmoth),
					new List<int>() { ModContent.NPCType<Glowmoth>() },
					new Dictionary<string, object>()
					{
						["displayName"] = Language.GetText("Mods.SOTS.NPCs.Glowmoth.DisplayName"),
						["spawnInfo"] = Language.GetText("Mods.SOTS.NPCs.Glowmoth.BossChecklistIntegration.SpawnInfo"),
						["spawnItems"] = ModContent.ItemType<SuspiciousLookingCandle>(),
						//["collectibles"] = new List<int>() { ModContent.ItemType<PutridPinkyMusicBox>(), ModContent.ItemType<PutridPinkyTrophy>() },
						["availability"] = (Func<bool>)(() => true),
						//["overrideHeadTextures"] = ,
						["despawnMessage"] = Language.GetText("Mods.SOTS.NPCs.Glowmoth.BossChecklistIntegration.DespawnMessage"),
						["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
						{
							Texture2D texture = ModContent.Request<Texture2D>("SOTS/BossCL/GlowmothPortrait").Value;
							Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
							sb.Draw(texture, centered, color);
						}
					}
				);
				bossChecklist.Call(
					"LogBoss",
					this,
					nameof(PharaohsCurse),
					4.5f,
					(Func<bool>)(() => SOTSWorld.downedCurse),
					new List<int>() { ModContent.NPCType<PharaohsCurse>() },
					new Dictionary<string, object>()
					{
						["displayName"] = Language.GetText("Mods.SOTS.NPCs.PharaohsCurse.DisplayName"),
						["spawnInfo"] = Language.GetText("Mods.SOTS.NPCs.PharaohsCurse.BossChecklistIntegration.SpawnInfo"),
						["spawnItems"] = ModContent.ItemType<Sarcophagus>(),
						["collectibles"] = new List<int>() { ModContent.ItemType<CurseMusicBox>(), ModContent.ItemType<CurseTrophy>() },
						["availability"] = (Func<bool>)(() => true),
						//["overrideHeadTextures"] = ,
						["despawnMessage"] = Language.GetText("Mods.SOTS.NPCs.PharaohsCurse.BossChecklistIntegration.DespawnMessage"),
						["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
						{
							Texture2D texture = ModContent.Request<Texture2D>("SOTS/BossCL/PharaohPortrait").Value;
							Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
							sb.Draw(texture, centered, color);
						}
					}
				);
				bossChecklist.Call(
					"LogBoss",
					this,
					nameof(TheAdvisorHead),
					6.9f,
					(Func<bool>)(() => SOTSWorld.downedAdvisor),
					new List<int>() { ModContent.NPCType<TheAdvisorHead>() },
					new Dictionary<string, object>()
					{
						["displayName"] = Language.GetText("Mods.SOTS.NPCs.TheAdvisorHead.DisplayName"),
						["spawnInfo"] = Language.GetText("Mods.SOTS.NPCs.TheAdvisorHead.BossChecklistIntegration.SpawnInfo"),
						["spawnItems"] = ModContent.ItemType<WorldgenScanner>(),
						["collectibles"] = new List<int>() { ModContent.ItemType<AdvisorMusicBox>(), ModContent.ItemType<AdvisorTrophy>() },
						["availability"] = (Func<bool>)(() => true),
						//["overrideHeadTextures"] = ,
						["despawnMessage"] = Language.GetText("Mods.SOTS.NPCs.TheAdvisorHead.BossChecklistIntegration.DespawnMessage"),
						["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
						{
							Texture2D texture = ModContent.Request<Texture2D>("SOTS/BossCL/AdvisorPortrait").Value;
							Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
							sb.Draw(texture, centered, color);
						}
					}
				);
				bossChecklist.Call(
					"LogBoss",
					this,
					nameof(NewPolaris),
					11.01f,
					(Func<bool>)(() => SOTSWorld.downedAmalgamation),
					new List<int>() { ModContent.NPCType<NewPolaris>() },
					new Dictionary<string, object>()
					{
						["displayName"] = Language.GetText("Mods.SOTS.NPCs.Polaris.DisplayName"),
						["spawnInfo"] = Language.GetText("Mods.SOTS.NPCs.Polaris.BossChecklistIntegration.SpawnInfo"),
						["spawnItems"] = new List<int>() { ModContent.ItemType<FrostedKey>(), ModContent.ItemType<FrostArtifact>() },
						["collectibles"] = new List<int>() { ModContent.ItemType<PolarisMusicBox>() },
						["availability"] = (Func<bool>)(() => true),
						//["overrideHeadTextures"] = ,
						["despawnMessage"] = Language.GetText("Mods.SOTS.NPCs.Polaris.BossChecklistIntegration.DespawnMessage"),
						["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
						{
							Texture2D texture = ModContent.Request<Texture2D>("SOTS/BossCL/PolarisPortrait").Value;
							Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
							sb.Draw(texture, centered, color);
						}
					}
				);
				bossChecklist.Call(
					"LogBoss",
					this,
					nameof(Lux),
					16.5f,
					(Func<bool>)(() => SOTSWorld.downedLux),
					new List<int>() { ModContent.NPCType<Lux>() },
					new Dictionary<string, object>()
					{
						["displayName"] = Language.GetText("Mods.SOTS.NPCs.Lux.DisplayName"),
						["spawnInfo"] = Language.GetText("Mods.SOTS.NPCs.Lux.BossChecklistIntegration.SpawnInfo"),
						["spawnItems"] = new List<int>() { ModContent.ItemType<ElectromagneticLure>() },
						["collectibles"] = new List<int>() { ModContent.ItemType<LuxMusicBox>() },
						["availability"] = (Func<bool>)(() => true),
						//["overrideHeadTextures"] = ,
						["despawnMessage"] = Language.GetText("Mods.SOTS.NPCs.Lux.BossChecklistIntegration.DespawnMessage"),
						["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
						{
							Texture2D texture = ModContent.Request<Texture2D>("SOTS/BossCL/LuxBossLog").Value;
							Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
							sb.Draw(texture, centered, color);
						}
					}
				);
				bossChecklist.Call(
					"LogBoss",
					this,
					nameof(SubspaceSerpentHead),
					17.9f,
					(Func<bool>)(() => SOTSWorld.downedSubspace),
					new List<int>() { ModContent.NPCType<SubspaceSerpentHead>(), ModContent.NPCType<SubspaceSerpentBody>(), ModContent.NPCType<SubspaceSerpentTail>() },
					new Dictionary<string, object>()
					{
						["displayName"] = Language.GetText("Mods.SOTS.NPCs.SubspaceSerpentHead.DisplayName"),
						["spawnInfo"] = Language.GetText("Mods.SOTS.NPCs.SubspaceSerpentHead.BossChecklistIntegration.SpawnInfo"),
						["spawnItems"] = new List<int>() { ModContent.ItemType<CatalystBomb>() },
						["collectibles"] = new List<int>() { ModContent.ItemType<SubspaceSerpentMusicBox>() },
						["availability"] = (Func<bool>)(() => true),
						//["overrideHeadTextures"] = ,
						["despawnMessage"] = Language.GetText("Mods.SOTS.NPCs.SubspaceSerpentHead.BossChecklistIntegration.DespawnMessage"),
						["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
						{
							Texture2D texture = ModContent.Request<Texture2D>("SOTS/BossCL/SubspaceSerpentPortrait").Value;
							Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
							sb.Draw(texture, centered, color);
						}
					}
				);
			}
		}
		//Custom Tile Merging
		public static bool[][] tileMergeTypes;
		public enum Similarity
		{
			None,
			Same,
			Merge
		}
		public static Similarity GetSimilarity(Tile check, int myType, int mergeType)
        {
            bool bonusMerge = false;
			if (mergeType == ModContent.TileType<SootBlockTile>())
				bonusMerge = check.TileType == ModContent.TileType<SootSlabTile>();
            if (mergeType == TileID.Marble)
                bonusMerge = check.TileType == ModContent.TileType<FakeMarble>();
            if (check == null || !check.HasTile)
			{
				return Similarity.None;
			}
			if (check.TileType == myType || Main.tileMerge[myType][check.TileType] || (Main.tileBrick[check.TileType] && check.TileType != mergeType && !bonusMerge))
			{
				return Similarity.Same;
			}
			if (check.TileType == mergeType || bonusMerge)
			{
				return Similarity.Merge;
			}
			return Similarity.None;
		}
		public static void MergeWith(int type1, int type2, bool merge = true)
		{
			if (type1 != type2)
			{
				Main.tileMerge[type1][type2] = merge;
				Main.tileMerge[type2][type1] = merge;
			}
		}
		private static void SetFrame(int x, int y, int frameX, int frameY)
		{
			Tile tile = Main.tile[x, y];
			if (tile != null)
			{
				tile.TileFrameX = (short)frameX;
				tile.TileFrameY = (short)frameY;
			}
		}
		internal static void MergeWithFrameExplicit(int x, int y, int myType, int mergeType, out bool mergedUp, out bool mergedLeft, out bool mergedRight, out bool mergedDown, bool forceSameDown = false, bool forceSameUp = false, bool forceSameLeft = false, bool forceSameRight = false, bool resetFrame = true)
		{
			if (Main.tile[x, y] == null || x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
			{
				mergedUp = mergedLeft = mergedRight = mergedDown = false;
				return;
			}
            Main.tileMerge[myType][mergeType] = false;
			if(mergeType == ModContent.TileType<SootBlockTile>())
            {
                Main.tileMerge[myType][ModContent.TileType<SootSlabTile>()] = false;
            }
            if (mergeType == TileID.MarbleBlock)
            {
                Main.tileMerge[myType][ModContent.TileType<FakeMarble>()] = false;
            }
            Tile tileLeft = Main.tile[x - 1, y];
			Tile tileRight = Main.tile[x + 1, y];
			Tile tileUp = Main.tile[x, y - 1];
			Tile tileDown = Main.tile[x, y + 1];
			Tile tileTopLeft = Main.tile[x - 1, y - 1];
			Tile tileTopRight = Main.tile[x + 1, y - 1];
			Tile tileBottomLeft = Main.tile[x - 1, y + 1];
			Tile check = Main.tile[x + 1, y + 1];
            bool forceNoneLeft = false, forceNoneRight = false, forceNoneUp = false, forceNoneDown = false;
            if (Main.tile[x, y].Slope != SlopeType.Solid || Main.tile[x, y].IsHalfBlock)
            {
                forceNoneDown = Main.tile[x, y].BottomSlope;
                forceNoneLeft = Main.tile[x, y].LeftSlope;
                forceNoneUp = Main.tile[x, y].TopSlope;
                forceNoneRight = Main.tile[x, y].RightSlope;
                if (Main.tile[x, y].IsHalfBlock)
                    forceNoneUp = true;
				forceSameLeft = tileLeft.HasTile ? true : forceSameLeft;
                forceSameRight = tileRight.HasTile ? true : forceSameRight;
                forceSameUp = tileUp.HasTile ? true : forceSameUp;
                forceSameDown = tileDown.HasTile ? true : forceSameDown;
            }
            if ((tileDown.IsHalfBlock || tileDown.TopSlope) && tileDown.HasTile)
                forceNoneDown = true;
			if (tileUp.BottomSlope && tileUp.HasTile)
				forceNoneUp = true;
            if (tileLeft.RightSlope && tileLeft.HasTile)
                forceNoneLeft = true;
            if (tileRight.LeftSlope && tileRight.HasTile)
                forceNoneRight = true;
            Similarity leftSim = forceNoneLeft ? Similarity.None : (!forceSameLeft) ? GetSimilarity(tileLeft, myType, mergeType) : Similarity.Same;
			Similarity rightSim = forceNoneRight ? Similarity.None : (!forceSameRight) ? GetSimilarity(tileRight, myType, mergeType) : Similarity.Same;
			Similarity upSim = forceNoneUp ? Similarity.None : (!forceSameUp) ? GetSimilarity(tileUp, myType, mergeType) : Similarity.Same;
			Similarity downSim = forceNoneDown ? Similarity.None : (!forceSameDown) ? GetSimilarity(tileDown, myType, mergeType) : Similarity.Same;
			Similarity topLeftSim = GetSimilarity(tileTopLeft, myType, mergeType);
			Similarity topRightSim = GetSimilarity(tileTopRight, myType, mergeType);
			Similarity bottomLeftSim = GetSimilarity(tileBottomLeft, myType, mergeType);
			Similarity bottomRightSim = GetSimilarity(check, myType, mergeType);
			int randomFrame;
			if (resetFrame)
			{
				randomFrame = WorldGen.genRand.Next(3);
				Tile t = Main.tile[x, y];
				t.TileFrameNumber = (byte)randomFrame;
			}
			else
			{
				randomFrame = Main.tile[x, y].TileFrameNumber;
			}
			mergedDown = (mergedLeft = (mergedRight = (mergedUp = false)));
			switch (leftSim)
			{
				case Similarity.None:
					switch (upSim)
					{
						case Similarity.Same:
							switch (downSim)
							{
								case Similarity.Same:
									switch (rightSim)
									{
										case Similarity.Same:
											SetFrame(x, y, 0, 18 * randomFrame);
											break;
										case Similarity.Merge:
											mergedRight = true;
											SetFrame(x, y, 234 + 18 * randomFrame, 36);
											break;
										default:
											SetFrame(x, y, 90, 18 * randomFrame);
											break;
									}
									break;
								case Similarity.Merge:
									switch (rightSim)
									{
										case Similarity.Same:
											mergedDown = true;
											SetFrame(x, y, 72, 90 + 18 * randomFrame);
											break;
										case Similarity.Merge:
											SetFrame(x, y, 108 + 18 * randomFrame, 54);
											break;
										default:
											mergedDown = true;
											SetFrame(x, y, 126, 90 + 18 * randomFrame);
											break;
									}
									break;
								default:
									if (rightSim == Similarity.Same)
									{
										SetFrame(x, y, 36 * randomFrame, 72);
									}
									else
									{
										SetFrame(x, y, 108 + 18 * randomFrame, 54);
									}
									break;
							}
							break;
						case Similarity.Merge:
							switch (downSim)
							{
								case Similarity.Same:
									switch (rightSim)
									{
										case Similarity.Same:
											mergedUp = true;
											SetFrame(x, y, 72, 144 + 18 * randomFrame);
											break;
										case Similarity.Merge:
											SetFrame(x, y, 108 + 18 * randomFrame, 0);
											break;
										default:
											mergedUp = true;
											SetFrame(x, y, 126, 144 + 18 * randomFrame);
											break;
									}
									break;
								case Similarity.Merge:
									switch (rightSim)
									{
										case Similarity.Same:
											SetFrame(x, y, 162, 18 * randomFrame);
											break;
										case Similarity.Merge:
											SetFrame(x, y, 162 + 18 * randomFrame, 54);
											break;
										default:
											mergedUp = true;
											mergedDown = true;
											SetFrame(x, y, 108, 216 + 18 * randomFrame);
											break;
									}
									break;
								default:
									switch (rightSim)
									{
										case Similarity.Same:
											SetFrame(x, y, 162, 18 * randomFrame);
											break;
										case Similarity.Merge:
											SetFrame(x, y, 162 + 18 * randomFrame, 54);
											break;
										default:
											mergedUp = true;
											SetFrame(x, y, 108, 144 + 18 * randomFrame);
											break;
									}
									break;
							}
							break;
						default:
							switch (downSim)
							{
								case Similarity.Same:
									if (rightSim == Similarity.Same)
									{
										SetFrame(x, y, 36 * randomFrame, 54);
										break;
									}
									_ = 1;
									SetFrame(x, y, 108 + 18 * randomFrame, 0);
									break;
								case Similarity.Merge:
									switch (rightSim)
									{
										case Similarity.Same:
											SetFrame(x, y, 162, 18 * randomFrame);
											break;
										case Similarity.Merge:
											SetFrame(x, y, 162 + 18 * randomFrame, 54);
											break;
										default:
											mergedDown = true;
											SetFrame(x, y, 108, 90 + 18 * randomFrame);
											break;
									}
									break;
								default:
									switch (rightSim)
									{
										case Similarity.Same:
											SetFrame(x, y, 162, 18 * randomFrame);
											break;
										case Similarity.Merge:
											mergedRight = true;
											SetFrame(x, y, 54 + 18 * randomFrame, 234);
											break;
										default:
											SetFrame(x, y, 162 + 18 * randomFrame, 54);
											break;
									}
									break;
							}
							break;
					}
					return;
				case Similarity.Merge:
					switch (upSim)
					{
						case Similarity.Same:
							switch (downSim)
							{
								case Similarity.Same:
									switch (rightSim)
									{
										case Similarity.Same:
											mergedLeft = true;
											SetFrame(x, y, 162, 126 + 18 * randomFrame);
											break;
										case Similarity.Merge:
											mergedLeft = true;
											mergedRight = true;
											SetFrame(x, y, 180, 126 + 18 * randomFrame);
											break;
										default:
											mergedLeft = true;
											SetFrame(x, y, 234 + 18 * randomFrame, 54);
											break;
									}
									break;
								case Similarity.Merge:
									switch (rightSim)
									{
										case Similarity.Same:
											mergedLeft = (mergedDown = true);
											SetFrame(x, y, 36, 108 + 36 * randomFrame);
											break;
										case Similarity.Merge:
											mergedLeft = (mergedRight = (mergedDown = true));
											SetFrame(x, y, 198, 144 + 18 * randomFrame);
											break;
										default:
											SetFrame(x, y, 108 + 18 * randomFrame, 54);
											break;
									}
									break;
								default:
									if (rightSim == Similarity.Same)
									{
										mergedLeft = true;
										SetFrame(x, y, 18 * randomFrame, 216);
									}
									else
									{
										SetFrame(x, y, 108 + 18 * randomFrame, 54);
									}
									break;
							}
							break;
						case Similarity.Merge:
							switch (downSim)
							{
								case Similarity.Same:
									switch (rightSim)
									{
										case Similarity.Same:
											mergedUp = (mergedLeft = true);
											SetFrame(x, y, 36, 90 + 36 * randomFrame);
											break;
										case Similarity.Merge:
											mergedLeft = (mergedRight = (mergedUp = true));
											SetFrame(x, y, 198, 90 + 18 * randomFrame);
											break;
										default:
											SetFrame(x, y, 108 + 18 * randomFrame, 0);
											break;
									}
									break;
								case Similarity.Merge:
									switch (rightSim)
									{
										case Similarity.Same:
											mergedUp = (mergedLeft = (mergedDown = true));
											SetFrame(x, y, 216, 90 + 18 * randomFrame);
											break;
										case Similarity.Merge:
											mergedDown = (mergedLeft = (mergedRight = (mergedUp = true)));
											SetFrame(x, y, 108 + 18 * randomFrame, 198);
											break;
										default:
											SetFrame(x, y, 162 + 18 * randomFrame, 54);
											break;
									}
									break;
								default:
									if (rightSim == Similarity.Same)
									{
										SetFrame(x, y, 162, 18 * randomFrame);
									}
									else
									{
										SetFrame(x, y, 162 + 18 * randomFrame, 54);
									}
									break;
							}
							break;
						default:
							switch (downSim)
							{
								case Similarity.Same:
									if (rightSim == Similarity.Same)
									{
										mergedLeft = true;
										SetFrame(x, y, 18 * randomFrame, 198);
									}
									else
									{
										_ = 1;
										SetFrame(x, y, 108 + 18 * randomFrame, 0);
									}
									break;
								case Similarity.Merge:
									if (rightSim == Similarity.Same)
									{
										SetFrame(x, y, 162, 18 * randomFrame);
										break;
									}
									_ = 1;
									SetFrame(x, y, 162 + 18 * randomFrame, 54);
									break;
								default:
									switch (rightSim)
									{
										case Similarity.Same:
											mergedLeft = true;
											SetFrame(x, y, 18 * randomFrame, 252);
											break;
										case Similarity.Merge:
											mergedRight = (mergedLeft = true);
											SetFrame(x, y, 162 + 18 * randomFrame, 198);
											break;
										default:
											mergedLeft = true;
											SetFrame(x, y, 18 * randomFrame, 234);
											break;
									}
									break;
							}
							break;
					}
					return;
			}
			switch (upSim)
			{
				case Similarity.Same:
					switch (downSim)
					{
						case Similarity.Same:
							switch (rightSim)
							{
								case Similarity.Same:
									if (topLeftSim == Similarity.Merge || topRightSim == Similarity.Merge || bottomLeftSim == Similarity.Merge || bottomRightSim == Similarity.Merge)
									{
										if (bottomRightSim == Similarity.Merge)
										{
											SetFrame(x, y, 0, 90 + 36 * randomFrame);
										}
										else if (bottomLeftSim == Similarity.Merge)
										{
											SetFrame(x, y, 18, 90 + 36 * randomFrame);
										}
										else if (topRightSim == Similarity.Merge)
										{
											SetFrame(x, y, 0, 108 + 36 * randomFrame);
										}
										else
										{
											SetFrame(x, y, 18, 108 + 36 * randomFrame);
										}
										break;
									}
									switch (topLeftSim)
									{
										case Similarity.Same:
											if (topRightSim == Similarity.Same)
											{
												if (bottomLeftSim == Similarity.Same)
												{
													SetFrame(x, y, 18 + 18 * randomFrame, 18);
												}
												else if (bottomRightSim == Similarity.Same)
												{
													SetFrame(x, y, 18 + 18 * randomFrame, 18);
												}
												else
												{
													SetFrame(x, y, 108 + 18 * randomFrame, 36);
												}
												return;
											}
											if (bottomLeftSim != 0)
											{
												break;
											}
											if (bottomRightSim == Similarity.Same)
											{
												if (topRightSim == Similarity.Merge)
												{
													SetFrame(x, y, 0, 108 + 36 * randomFrame);
												}
												else
												{
													SetFrame(x, y, 18 + 18 * randomFrame, 18);
												}
											}
											else
											{
												SetFrame(x, y, 198, 18 * randomFrame);
											}
											return;
										case Similarity.None:
											if (topRightSim == Similarity.Same)
											{
												if (bottomRightSim == Similarity.Same)
												{
													SetFrame(x, y, 18 + 18 * randomFrame, 18);
												}
												else
												{
													SetFrame(x, y, 18 + 18 * randomFrame, 18);
												}
											}
											else
											{
												SetFrame(x, y, 18 + 18 * randomFrame, 18);
											}
											return;
									}
									SetFrame(x, y, 18 + 18 * randomFrame, 18);
									break;
								case Similarity.Merge:
									mergedRight = true;
									SetFrame(x, y, 144, 126 + 18 * randomFrame);
									break;
								default:
									SetFrame(x, y, 72, 18 * randomFrame);
									break;
							}
							break;
						case Similarity.Merge:
							switch (rightSim)
							{
								case Similarity.Same:
									mergedDown = true;
									SetFrame(x, y, 144 + 18 * randomFrame, 90);
									break;
								case Similarity.Merge:
									mergedDown = (mergedRight = true);
									SetFrame(x, y, 54, 108 + 36 * randomFrame);
									break;
								default:
									mergedDown = true;
									SetFrame(x, y, 90, 90 + 18 * randomFrame);
									break;
							}
							break;
						default:
							switch (rightSim)
							{
								case Similarity.Same:
									SetFrame(x, y, 18 + 18 * randomFrame, 36);
									break;
								case Similarity.Merge:
									mergedRight = true;
									SetFrame(x, y, 54 + 18 * randomFrame, 216);
									break;
								default:
									SetFrame(x, y, 18 + 36 * randomFrame, 72);
									break;
							}
							break;
					}
					return;
				case Similarity.Merge:
					switch (downSim)
					{
						case Similarity.Same:
							switch (rightSim)
							{
								case Similarity.Same:
									mergedUp = true;
									SetFrame(x, y, 144 + 18 * randomFrame, 108);
									break;
								case Similarity.Merge:
									mergedRight = (mergedUp = true);
									SetFrame(x, y, 54, 90 + 36 * randomFrame);
									break;
								default:
									mergedUp = true;
									SetFrame(x, y, 90, 144 + 18 * randomFrame);
									break;
							}
							break;
						case Similarity.Merge:
							switch (rightSim)
							{
								case Similarity.Same:
									mergedUp = (mergedDown = true);
									SetFrame(x, y, 144 + 18 * randomFrame, 180);
									break;
								case Similarity.Merge:
									mergedUp = (mergedRight = (mergedDown = true));
									SetFrame(x, y, 216, 144 + 18 * randomFrame);
									break;
								default:
									SetFrame(x, y, 216, 18 * randomFrame);
									break;
							}
							break;
						default:
							if (rightSim == Similarity.Same)
							{
								mergedUp = true;
								SetFrame(x, y, 234 + 18 * randomFrame, 18);
							}
							else
							{
								SetFrame(x, y, 216, 18 * randomFrame);
							}
							break;
					}
					return;
			}
			switch (downSim)
			{
				case Similarity.Same:
					switch (rightSim)
					{
						case Similarity.Same:
							SetFrame(x, y, 18 + 18 * randomFrame, 0);
							break;
						case Similarity.Merge:
							mergedRight = true;
							SetFrame(x, y, 54 + 18 * randomFrame, 198);
							break;
						default:
							SetFrame(x, y, 18 + 36 * randomFrame, 54);
							break;
					}
					break;
				case Similarity.Merge:
					if (rightSim == Similarity.Same)
					{
						mergedDown = true;
						SetFrame(x, y, 234 + 18 * randomFrame, 0);
					}
					else
					{
						SetFrame(x, y, 216, 18 * randomFrame);
					}
					break;
				default:
					switch (rightSim)
					{
						case Similarity.Same:
							SetFrame(x, y, 108 + 18 * randomFrame, 72);
							break;
						case Similarity.Merge:
							mergedRight = true;
							SetFrame(x, y, 54 + 18 * randomFrame, 252);
							break;
						default:
							SetFrame(x, y, 216, 18 * randomFrame);
							break;
					}
					break;
			}
		}
		public static void MergeWithFrame(int x, int y, int myType, int mergeType, bool forceSameDown = false, bool forceSameUp = false, bool forceSameLeft = false, bool forceSameRight = false, bool resetFrame = true)
		{
			MergeWithFrameExplicit(x, y, myType, mergeType, out var _, out var _, out var _, out var _, forceSameDown, forceSameUp, forceSameLeft, forceSameRight, resetFrame);
		}
		public static void MergeWithFrame(int x, int y, int myType, int mergeType)
		{
			if (x >= 0 && x < Main.maxTilesX && y >= 0 && y < Main.maxTilesY && Main.tile[x, y] != null)
			{
				bool forceSameUp = false;
				bool forceSameDown = false;
				bool forceSameLeft = false;
				bool forceSameRight = false;
				Tile north = Main.tile[x, y - 1];
				Tile south = Main.tile[x, y + 1];
				Tile west = Main.tile[x - 1, y];
				Tile east = Main.tile[x + 1, y];
				bool mergedUp;
				bool mergedLeft;
				bool mergedRight;
				if (north != null && north.HasTile && tileMergeTypes[myType][north.TileType])
				{
					MergeWith(myType, north.TileType, merge: false);
					TileID.Sets.ChecksForMerge[myType] = true;
					MergeWithFrameExplicit(x, y - 1, north.TileType, myType, out mergedUp, out mergedLeft, out mergedRight, out forceSameUp, forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame: false);
				}
				if (west != null && west.HasTile && tileMergeTypes[myType][west.TileType])
				{
					MergeWith(myType, west.TileType, merge: false);
					TileID.Sets.ChecksForMerge[myType] = true;
					MergeWithFrameExplicit(x - 1, y, west.TileType, myType, out mergedRight, out mergedLeft, out forceSameLeft, out mergedUp, forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame: false);
				}
				if (east != null && east.HasTile && tileMergeTypes[myType][east.TileType])
				{
					MergeWith(myType, east.TileType, merge: false);
					TileID.Sets.ChecksForMerge[myType] = true;
					MergeWithFrameExplicit(x + 1, y, east.TileType, myType, out mergedUp, out forceSameRight, out mergedLeft, out mergedRight, forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame: false);
				}
				if (south != null && south.HasTile && tileMergeTypes[myType][south.TileType])
				{
					MergeWith(myType, south.TileType, merge: false);
					TileID.Sets.ChecksForMerge[myType] = true;
					MergeWithFrameExplicit(x, y + 1, south.TileType, myType, out forceSameDown, out mergedRight, out mergedLeft, out mergedUp, forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame: false);
				}
				MergeWithFrameExplicit(x, y, myType, mergeType, out mergedUp, out mergedLeft, out mergedRight, out var _, forceSameDown, forceSameUp, forceSameLeft, forceSameRight);
			}
		}
	}
}
