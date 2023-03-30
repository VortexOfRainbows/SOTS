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
using SOTS.Items.Otherworld.EpicWings;
using SOTS.Common.GlobalNPCs;
using SOTS.Items.Otherworld.FromChests;
using SOTS.NPCs.Boss;
using SOTS.Items.Slime;
using SOTS.Items.Otherworld;
using SOTS.Items.Permafrost;
using SOTS.Items.Celestial;
using SOTS.Items.MusicBoxes;
using SOTS.NPCs.Boss.Polaris;
using SOTS.NPCs.Boss.Curse;
using SOTS.NPCs.Boss.Advisor;
using SOTS.Items.Banners;
using Terraria.Graphics.Shaders;
using SOTS.Items.Dyes;
using SOTS.Items.Fragments;
using SOTS.Items.Otherworld.Furniture;
using Terraria.Graphics.Effects;
using Catalyst.Backgrounds;
using SOTS.NPCs.Boss.Lux;
using SOTS.Items;
using SOTS.Items.Chaos;
using SOTS.Items.Otherworld.Blocks;
using ReLogic.Content;
using Terraria.GameContent;
using System.Linq;

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
		public static bool SOTSTexturePackEnabled = false;
		public static bool SOTSTexturePackEnabledWithTiles => SOTSTexturePackEnabled && Config.additionalTexturePackVisuals;
		public static bool SOTSTexturePackEnabledAudio => false; //SOTSTexturePackEnabled && Config.playAmbientAudio;
		public static PrimTrailManager primitives;

		public static ModKeybind BlinkHotKey;
		public static ModKeybind ArmorSetHotKey;
		public static ModKeybind MachinaBoosterHotKey;
		internal static SOTS Instance;

		public static Effect AtenTrail;
		public static Effect WaterTrail;
		public static Effect FireTrail;
		public static Effect FireballShader;
		public static Effect GodrayShader;
		public static Effect VisionShader;
		public static SOTSConfig Config
        {
			get => ModContent.GetInstance<SOTSConfig>();
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
			string blinkKeyBind = LocalizationLoader.GetOrCreateTranslation("Mods.SOTS.KeyBindName.Blink").GetTranslation(GameCulture.FromName(Language.ActiveCulture.Name));
            string armorSetKeyBind = LocalizationLoader.GetOrCreateTranslation("Mods.SOTS.KeyBindName.ArmorSet").GetTranslation(GameCulture.FromName(Language.ActiveCulture.Name));
            string mfmKeyBind = LocalizationLoader.GetOrCreateTranslation("Mods.SOTS.KeyBindName.MFM").GetTranslation(GameCulture.FromName(Language.ActiveCulture.Name));
            BlinkHotKey = KeybindLoader.RegisterKeybind(this, Language.GetTextValue(blinkKeyBind), "V");//TODO: Localize it when 1.4.4 comes
			ArmorSetHotKey = KeybindLoader.RegisterKeybind(this, armorSetKeyBind, "F");
			MachinaBoosterHotKey = KeybindLoader.RegisterKeybind(this, mfmKeyBind, "C");
			SOTSWorld.LoadUI();
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
			SOTSItem.LoadArrays();
			SOTSTile.LoadArrays();
			SOTSWall.LoadArrays();
			SOTSPlayer.LoadArrays();
			SOTSProjectile.LoadArrays();
			DebuffNPC.LoadArrays();
			if(Main.netMode != NetmodeID.Server)
			{
				Ref<Effect> TPrismdyeRef = new Ref<Effect>((Effect)Assets.Request<Effect>("Effects/TPrismEffect", AssetRequestMode.ImmediateLoad));
				Ref<Effect> voidMageShader = new Ref<Effect>((Effect)Assets.Request<Effect>("Effects/VMShader", AssetRequestMode.ImmediateLoad));
				GameShaders.Armor.BindShader(ModContent.ItemType<TaintedPrismDye>(), new ArmorShaderData(TPrismdyeRef, "TPrismDyePass")).UseColor(0.3f, 0.4f, 0.4f);
				Filters.Scene["VMFilter"] = new Filter(new ScreenShaderData(voidMageShader, "VMShaderPass"), EffectPriority.VeryHigh);
				Filters.Scene["VMFilter"].Load();
				AtenTrail = Instance.Assets.Request<Effect>("Effects/AtenTrail", AssetRequestMode.ImmediateLoad).Value;
				WaterTrail = Instance.Assets.Request<Effect>("Effects/WaterTrail", AssetRequestMode.ImmediateLoad).Value;
				FireTrail = Instance.Assets.Request<Effect>("Effects/FireTrail", AssetRequestMode.ImmediateLoad).Value;
				FireballShader = Instance.Assets.Request<Effect>("Effects/FireballShader", AssetRequestMode.ImmediateLoad).Value;
				GodrayShader = Instance.Assets.Request<Effect>("Effects/GodrayShader", AssetRequestMode.ImmediateLoad).Value;
				VisionShader = Instance.Assets.Request<Effect>("Effects/VisionShader", AssetRequestMode.ImmediateLoad).Value;
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
			RequestTileLocations
		}
		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			int msgType = reader.ReadByte();
			if (msgType == (int)SOTSMessageType.SyncTileLocations || msgType == (int)SOTSMessageType.RequestTileLocations)
            {
				Common.Systems.ImportantTilesWorld.HandlePacket(reader, whoAmI, msgType);
				return;
            }
			/* if (Main.netMode == NetmodeID.Server)
				NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Handling Packet: " + msgType), Color.Gray);
			else
				Main.NewText("Handling Packet: " + msgType); */
			switch (msgType)
			{
				case (int)SOTSMessageType.SOTSSyncPlayer:
					byte playernumber = reader.ReadByte();
					SOTSPlayer modPlayer = Main.player[playernumber].GetModPlayer<SOTSPlayer>();
					TestWingsPlayer testPlayer = Main.player[playernumber].GetModPlayer<TestWingsPlayer>();
					VoidPlayer voidPlayer = Main.player[playernumber].GetModPlayer<VoidPlayer>();
					bool creativeFlight = reader.ReadBoolean();
					testPlayer.creativeFlight = creativeFlight;
					int lootingSouls = reader.ReadInt32();
					voidPlayer.lootingSouls = lootingSouls;
					break;
                case (int)SOTSMessageType.SyncCreativeFlight:
					playernumber = reader.ReadByte(); 
					testPlayer = Main.player[playernumber].GetModPlayer<TestWingsPlayer>();
					testPlayer.creativeFlight = reader.ReadBoolean();
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)SOTSMessageType.SyncCreativeFlight);
						packet.Write(playernumber);
						packet.Write(testPlayer.creativeFlight);
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
						packet.Send(-1, playernumber2);
					}
					break;
			}
		}
		public override void PostSetupContent()
        {
            Mod bossChecklist;
			bool available = ModLoader.TryGetMod("BossChecklist", out bossChecklist);
			if (available)
            {
				// AddBoss, bossname, order or value in terms of vanilla bosses, inline method for retrieving downed value.
				//bossChecklist.Call(....
				// To include a description:
				//bossChecklist.Call("AddBoss", "Putrid Pinky", 4.2f, (Func<bool>)(() => SOTSWorld.downedPinky));
				//bossChecklist.Call("AddBossWithInfo", "Putrid Pinky", 4.2f, (Func<bool>)(() => SOTSWorld.downedPinky), "Use [i:" + ItemType("JarOfPeanuts") + "]");	
				//bossChecklist.Call("AddBossWithInfo", "Pharaoh's Curse", 4.3f, (Func<bool>)(() => SOTSWorld.downedCurse), "Find the [i:" + ItemType("Sarcophagus") + "] in the pyramid");

				///The following is a PUTRID PINKY template that should work for boss checklist!
				bossChecklist.Call( 
					"AddBoss",
					this,
					$"$Mods.SOTS.BossChecklist.BossName.PutridPinky",
					new List<int>() { ModContent.NPCType<PutridPinkyPhase2>() },
					4.25f,
					(Func<bool>)(() => SOTSWorld.downedPinky),
					() => true,
					new List<int>() { ModContent.ItemType<PutridPinkyMusicBox>(), ModContent.ItemType<PutridPinkyTrophy>() },
					ModContent.ItemType<JarOfPeanuts>(),
					Language.GetTextValue("Mods.SOTS.BossChecklist.BossDescription.PutridPinky", "[i:" + ModContent.ItemType<JarOfPeanuts> () + "]"),
					$"$Mods.SOTS.BossChecklist.BossDisappear.PutridPinky",
					(SpriteBatch sb, Rectangle rect, Color color) => {
						Texture2D texture = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/PutridPinky1_Display").Value;
						Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
						sb.Draw(texture, centered, color);
					}
					);
				bossChecklist.Call(
					"AddBoss",
					this,
					$"$Mods.SOTS.BossChecklist.BossName.PharaohsCurse",
					new List<int>() { ModContent.NPCType<PharaohsCurse>() },
					4.5f,
					(Func<bool>)(() => SOTSWorld.downedCurse),
					() => true,
					new List<int>() { },
					ModContent.ItemType<Sarcophagus>(),
					Language.GetTextValue("Mods.SOTS.BossChecklist.BossDescription.PharaohsCurse", "[i:" + ModContent.ItemType<Sarcophagus> () + "]"),
					$"$Mods.SOTS.BossChecklist.BossDisappear.PharaohsCurse",
					(SpriteBatch sb, Rectangle rect, Color color) => {
						Texture2D texture = ModContent.Request<Texture2D>("SOTS/BossCL/PharaohPortrait").Value;
						Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
						sb.Draw(texture, centered, color);
					}
					);
				bossChecklist.Call(
					"AddBoss",
					this,
					$"$Mods.SOTS.BossChecklist.BossName.TheAdvisor",
					new List<int>() { ModContent.NPCType<TheAdvisorHead>() },
					5.9f,
					(Func<bool>)(() => SOTSWorld.downedAdvisor),
					() => true,
					new List<int>() { ModContent.ItemType<AdvisorMusicBox>() },
					ModContent.ItemType<AvaritianGateway>(),
					Language.GetTextValue("Mods.SOTS.BossChecklist.BossDescription.TheAdvisor"),
					$"$Mods.SOTS.BossChecklist.BossDisappear.TheAdvisor",
					(SpriteBatch sb, Rectangle rect, Color color) => {
						Texture2D texture = ModContent.Request<Texture2D>("SOTS/BossCL/AdvisorPortrait").Value;
						Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
						sb.Draw(texture, centered, color);
					}
					);
				bossChecklist.Call(
					"AddBoss",
					this,
					$"$Mods.SOTS.BossChecklist.BossName.Polaris",
					new List<int>() { ModContent.NPCType<Polaris>() },
					9.05f,
					(Func<bool>)(() => SOTSWorld.downedAmalgamation),
					() => true,
					new List<int>() { ModContent.ItemType<PolarisMusicBox>() },
					new List<int>() { ModContent.ItemType<FrostedKey>(), ModContent.ItemType<FrostArtifact>() },
					Language.GetTextValue("Mods.SOTS.BossChecklist.BossDescription.PharaohsCurse", "[i:" + ModContent.ItemType<FrostArtifact> () + "]", "[i:" + ModContent.ItemType<FrostedKey> () + "]"),
					$"$Mods.SOTS.BossChecklist.BossDisappear.Polaris",
					(SpriteBatch sb, Rectangle rect, Color color) => {
						Texture2D texture = ModContent.Request<Texture2D>("SOTS/BossCL/PolarisPortrait").Value;
						Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
						sb.Draw(texture, centered, color);
					}
					);
				bossChecklist.Call(
					"AddBoss",
					this,
					$"$Mods.SOTS.BossChecklist.BossName.Lux",
					new List<int>() { ModContent.NPCType<Lux>() },
					12.5f,
					(Func<bool>)(() => SOTSWorld.downedLux),
					() => true,
					new List<int>() { },
					new List<int>() { ModContent.ItemType<ElectromagneticLure>() },
					Language.GetTextValue("Mods.SOTS.BossChecklist.BossDescription.Lux"),
					$"$Mods.SOTS.BossChecklist.BossDisappear.Lux",
					(SpriteBatch sb, Rectangle rect, Color color) => {
						Texture2D texture = ModContent.Request<Texture2D>("SOTS/BossCL/LuxBossLog").Value;
						Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
						sb.Draw(texture, centered, color);
					}
					);
				bossChecklist.Call(
					"AddBoss",
					this,
					$"$Mods.SOTS.BossChecklist.BossName.SubspaceSerpent",
					new List<int>() { ModContent.NPCType<SubspaceSerpentHead>(), ModContent.NPCType<SubspaceSerpentBody>(), ModContent.NPCType<SubspaceSerpentTail>() },
					12.9f,
					(Func<bool>)(() => SOTSWorld.downedSubspace),
					() => true,
					new List<int>() { ModContent.ItemType<SubspaceSerpentMusicBox>() },
					new List<int>() { ModContent.ItemType<CatalystBomb>() },
					Language.GetTextValue("Mods.SOTS.BossChecklist.BossDescription.SubspaceSerpent", "[i:" + ModContent.ItemType<CatalystBomb>() + "]"),
					$"$Mods.SOTS.BossChecklist.BossDisappear.SubspaceSerpent",
					(SpriteBatch sb, Rectangle rect, Color color) => {
						Texture2D texture = ModContent.Request<Texture2D>("SOTS/BossCL/SubspaceSerpentPortrait").Value;
						Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
						sb.Draw(texture, centered, color);
					}
					);

				//bossChecklist.Call("AddBossWithInfo", "The Advisor", 5.9f, (Func<bool>)(() => SOTSWorld.downedAdvisor), "Destroy the 4 tethered Otherworldly Constructs on the Planetarium");
				//bossChecklist.Call("AddBossWithInfo", "Cryptic Carver", 5.2f, (Func<bool>)(() => SOTSWorld.downedCarver), "Use [i:" + ItemType("MargritArk") + "]");
				//bossChecklist.Call("AddBossWithInfo", "Ethereal Entity", 6.5f, (Func<bool>)(() => SOTSWorld.downedEntity), "Use [i:" + ItemType("PlanetariumDiamond") + "] in a planetarium biome");

				//bossChecklist.Call("AddBossWithInfo", "Antimaterial Antlion", 7.21f, (Func<bool>)(() => SOTSWorld.downedAntilion), "Use [i:" + ItemType("ForbiddenPyramid") + "] in a desert biome");
				//bossChecklist.Call("AddBossWithInfo", "Icy Amalgamation", 8.21f, (Func<bool>)(() => SOTSWorld.downedAmalgamation), "Use [i:" + ItemType("FrostedKey") + "] on a [i:" + ItemType("FrostArtifact") + "] in a snow biome");
				//bossChecklist.Call("AddBossWithInfo", "Celestial Serpent", 11.1f, (Func<bool>)(() => SOTSWorld.downedLux), "Use [i:" + ItemType("CelestialTorch") + "] during night time");
				// bossChecklist.Call("AddBossWithInfo", "Subspace Serpent", 11.2f, (Func<bool>)(() => SOTSWorld.downedSubspace), "Tear a rift in hell by detonating a [i:" + ItemType("CatalystBomb") + "]");

			}
			/*for(int i = Main.maxItems; i < ItemLoader.ItemCount; i++)
            {
				SOTSUtils.SetResearchCost(i, 1);
            }*/
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
			if (check == null || !check.HasTile)
			{
				return Similarity.None;
			}
			if (check.TileType == myType || Main.tileMerge[myType][check.TileType] || (Main.tileBrick[check.TileType] && check.TileType != mergeType))
			{
				return Similarity.Same;
			}
			if (check.TileType == mergeType)
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
				mergedUp = (mergedLeft = (mergedRight = (mergedDown = false)));
				return;
			}
			Main.tileMerge[myType][mergeType] = false;
			Tile tileLeft = Main.tile[x - 1, y];
			Tile tileRight = Main.tile[x + 1, y];
			Tile tileUp = Main.tile[x, y - 1];
			Tile tileDown = Main.tile[x, y + 1];
			Tile tileTopLeft = Main.tile[x - 1, y - 1];
			Tile tileTopRight = Main.tile[x + 1, y - 1];
			Tile tileBottomLeft = Main.tile[x - 1, y + 1];
			Tile check = Main.tile[x + 1, y + 1];
			Similarity leftSim = ((!forceSameLeft) ? GetSimilarity(tileLeft, myType, mergeType) : Similarity.Same);
			Similarity rightSim = ((!forceSameRight) ? GetSimilarity(tileRight, myType, mergeType) : Similarity.Same);
			Similarity upSim = ((!forceSameUp) ? GetSimilarity(tileUp, myType, mergeType) : Similarity.Same);
			Similarity downSim = ((!forceSameDown) ? GetSimilarity(tileDown, myType, mergeType) : Similarity.Same);
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
