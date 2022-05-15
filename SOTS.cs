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
using SOTS.NPCs.ArtificialDebuffs;
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

namespace SOTS
{
	public class SOTS : Mod
	{
		private Vector2 _lastScreenSize;
		private Vector2 _lastViewSize;
		public static PrimTrailManager primitives;

		public static ModHotKey BlinkHotKey;
		public static ModHotKey ArmorSetHotKey;
		public static ModHotKey MachinaBoosterHotKey;
		internal static SOTS Instance;

		public static Effect AtenTrail;
		public static Effect WaterTrail;
		public static Effect FireballShader;
		public static Effect GodrayShader;
		public static Effect VisionShader;
		public static SOTSConfig Config
        {
			get => ModContent.GetInstance<SOTSConfig>();
        }
		public SOTS()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}
		private UserInterface _VoidUserInterface;
		internal VoidUI VoidUI;
		public static int PlayerCount = 0;
		public override void Load()
		{
			//SOTSGlowmasks.LoadGlowmasks();
			Instance = ModContent.GetInstance<SOTS>();
			BlinkHotKey = RegisterHotKey("Blink", "V");
			ArmorSetHotKey = RegisterHotKey("Armor Set", "F");
			MachinaBoosterHotKey = RegisterHotKey("Modify Flight Mode", "C");
			Instance.AddEquipTexture(null, EquipType.Legs, "CursedRobe_Legs", "SOTS/Items/Pyramid/CursedRobe_Legs");
			if (!Main.dedServ)
            {
                VoidUI = new VoidUI();
                VoidUI.Activate();
                _VoidUserInterface = new UserInterface();
                _VoidUserInterface.SetState(VoidUI);

				_lastScreenSize = new Vector2(Main.screenWidth, Main.screenHeight);
				_lastViewSize = Main.ViewSize;
				//SkyManager.Instance["SOTS:LuxFight"] = new LuxSky();
			}
			Mod yabhb = ModLoader.GetMod("FKBossHealthBar");
			if (yabhb != null)
			{
				yabhb.Call("hbStart");
				yabhb.Call("hbSetTexture",
					GetTexture("UI/PinkyHealthbarLeft"),
					GetTexture("UI/PinkyHealthbarMid"),
					GetTexture("UI/PinkyHealthbarEnd"),
					GetTexture("UI/PinkyHealthbarFill"));
				yabhb.Call("hbSetMidBarOffset", -36, 12);
				yabhb.Call("hbSetBossHeadCentre", 16, 30);
				yabhb.Call("hbSetFillDecoOffset", 10);
				yabhb.Call("hbLoopMidBar", true);
				yabhb.Call("hbFinishSingle", ModContent.NPCType<PutridPinkyPhase2>());

				yabhb.Call("hbStart");
				yabhb.Call("hbSetTexture",
					GetTexture("UI/SubspaceHBLeft"),
					GetTexture("UI/SubspaceHBMid"),
					GetTexture("UI/SubspaceHBEnd"),
					GetTexture("UI/SubspaceHBFill"));
				yabhb.Call("hbSetMidBarOffset", -28, 8);
				yabhb.Call("hbSetBossHeadCentre", 32, 26);
				yabhb.Call("hbSetFillDecoOffset", 10);
				yabhb.Call("hbLoopMidBar", true);
				yabhb.Call("hbSetColours",
					new Color(155, 255, 150),
					new Color(1f, 1f, 0f), 
					new Color(1f, 0f, 0f));
				yabhb.Call("hbFinishSingle", ModContent.NPCType<SubspaceSerpentHead>());
			}
			//Music Box Stuff
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/PutridPinky"), ModContent.ItemType<PutridPinkyMusicBox>(), ModContent.TileType<PutridPinkyMusicBoxTile>());
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Advisor"), ModContent.ItemType<AdvisorMusicBox>(), ModContent.TileType<AdvisorMusicBoxTile>());
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Polaris"), ModContent.ItemType<PolarisMusicBox>(), ModContent.TileType<PolarisMusicBoxTile>());
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/SubspaceSerpent"), ModContent.ItemType<SubspaceSerpentMusicBox>(), ModContent.TileType<SubspaceSerpentMusicBoxTile>());
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/CursedPyramid"), ModContent.ItemType<AncientPyramidMusicBox>(), ModContent.TileType<AncientPyramidMusicBoxTile>());
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/PyramidBattle"), ModContent.ItemType<PyramidBattleMusicBox>(), ModContent.TileType<PyramidBattleMusicBoxTile>());
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Planetarium"), ModContent.ItemType<PlanetariumMusicBox>(), ModContent.TileType<PlanetariumMusicBoxTile>());
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/KnucklesTheme"), ModContent.ItemType<KnucklesMusicBox>(), ModContent.TileType<KnucklesMusicBoxTile>()); //WHY THE FUCK
			SOTSItem.LoadArrays();
			SOTSTile.LoadArrays();
			SOTSWall.LoadArrays();
			SOTSPlayer.LoadArrays();
			SOTSProjectile.LoadArrays();
			DebuffNPC.LoadArrays();
			if(Main.netMode != NetmodeID.Server)
			{
				Ref<Effect> TPrismdyeRef = new Ref<Effect>(GetEffect("Effects/TPrismEffect"));
				Ref<Effect> voidMageShader = new Ref<Effect>(GetEffect("Effects/VMShader"));
				GameShaders.Armor.BindShader(ModContent.ItemType<TaintedPrismDye>(), new ArmorShaderData(TPrismdyeRef, "TPrismDyePass")).UseColor(0.3f, 0.4f, 0.4f);
				Filters.Scene["VMFilter"] = new Filter(new ScreenShaderData(voidMageShader, "VMShaderPass"), EffectPriority.VeryHigh);
				Filters.Scene["VMFilter"].Load();
				AtenTrail = Instance.Assets.Request<Effect>("Effects/AtenTrail").Value;
				WaterTrail = Instance.Assets.Request<Effect>("Effects/WaterTrail").Value;
				FireballShader = Instance.Assets.Request<Effect>("Effects/FireballShader").Value;
				GodrayShader = Instance.Assets.Request<Effect>("Effects/GodrayShader").Value;
				VisionShader = Instance.Assets.Request<Effect>("Effects/VisionShader").Value;
				primitives = new PrimTrailManager();
				primitives.LoadContent(Main.graphics.GraphicsDevice);
			}
			SOTSDetours.Initialize();
		}
		public override void Unload() 
		{
			WaterTrail = null;
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
		public override void PreUpdateEntities()
		{
			if (!Main.dedServ)
			{
				if (_lastScreenSize != new Vector2(Main.screenWidth, Main.screenHeight))
				{
					if (primitives != null)
						primitives.LoadContent(Main.graphics.GraphicsDevice);
					if (SOTSDetours.TargetProj != null)
						SOTSDetours.ResizeTargets();
				}

				_lastScreenSize = new Vector2(Main.screenWidth, Main.screenHeight);
				_lastViewSize = Main.ViewSize;
			}
			bool frozen = SOTSWorld.UpdateWhileFrozen();
			if(!frozen)
            {
				SOTSWorld.Update();
				PlayerCount = 0;
				if (Main.netMode != NetmodeID.SinglePlayer) //updating this on Dedicated Server just in case I accidentally forgot to make dust not spawn in multiplayer somewhere
				{
					for (int i = 0; i < Main.player.Length; i++)
					{
						Player player = Main.player[i];
						if (player.active)
						{
							PlayerCount++;
							SOTSPlayer.ModPlayer(player).FoamStuff();
						}
					}
				}
				else
				{
					PlayerCount = 1;
					SOTSPlayer.ModPlayer(Main.LocalPlayer).FoamStuff();
				}
            }
		}
		public override void MidUpdateProjectileItem()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				primitives.UpdateTrails();
			}
		}
		public override void UpdateUI(GameTime gameTime) 
		{
			if (_VoidUserInterface != null)
			{
				_VoidUserInterface.Update(gameTime);
			}
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
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (mouseTextIndex != -1) {
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"SOTS: Void Meter",
					delegate {
						if (VoidUI.visible) {
							_VoidUserInterface.Draw(Main.spriteBatch, new GameTime());
						}
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
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
			SyncGlobalCounter
		}
		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			int msgType = reader.ReadByte();
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
							if (Projectile.active && Projectile.identity == projIdentity)
							{
								projIdentity = Projectile.whoAmI;
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
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)SOTSMessageType.SyncGlobalWorldFreeze);
						packet.Write(playernumber2);
						packet.Write(SOTSWorld.GlobalTimeFreeze);
						packet.Write(SOTSWorld.GlobalFrozen);
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
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Wormwood>(), 30).AddTile(TileID.Anvils).ReplaceResult(ItemID.SlimeStaff);
			CreateRecipe(1).AddIngredient(ModContent.ItemType<FragmentOfNature>(), 25).AddIngredient(ItemID.HermesBoots, 1).AddTile(TileID.Anvils).ReplaceResult(ItemID.FlowerBoots);
			CreateRecipe(1).AddIngredient(ModContent.ItemType<FragmentOfTide>(), 10).AddIngredient(ItemID.WaterWalkingPotion, 5).AddIngredient(ItemID.HermesBoots, 1).AddTile(TileID.Anvils).ReplaceResult(ItemID.WaterWalkingBoots);
		}
		public override void AddRecipeGroups()
		{
			RecipeGroup group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Silver Bar", new int[]
			{
				ItemID.SilverBar,
				ItemID.TungstenBar
			});
			RecipeGroup.RegisterGroup("SOTS:SilverBar", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Evil Material", new int[]
			{
				ItemID.TissueSample,
				ItemID.ShadowScale
			});
			RecipeGroup.RegisterGroup("SOTS:EvilMaterial", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Evil Bar", new int[]
			{
				ItemID.CrimtaneBar,
				ItemID.DemoniteBar
			});
			RecipeGroup.RegisterGroup("SOTS:EvilBar", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Gold Bar", new int[]
			{
				ItemID.GoldBar,
				ItemID.PlatinumBar
			});
			RecipeGroup.RegisterGroup("SOTS:GoldBar", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Gem Robe", new int[]
			{
				ItemID.RubyRobe,
				ItemID.AmethystRobe,
				ItemID.TopazRobe,
				ItemID.SapphireRobe,
				ItemID.EmeraldRobe,
				ItemID.DiamondRobe
			});
			RecipeGroup.RegisterGroup("SOTS:GemRobes", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Pre-Hardmode Ore", new int[]
			{
				ItemID.TungstenOre,
				ItemID.CopperOre,
				ItemID.TinOre,
				ItemID.IronOre,
				ItemID.LeadOre,
				ItemID.SilverOre,
				ItemID.GoldOre,
				ItemID.PlatinumOre
			});
			RecipeGroup.RegisterGroup("SOTS:PHMOre", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Tier 2 DD2 Armor", new int[]
			{
				ItemID.SquirePlating,
				ItemID.SquireGreatHelm,
				ItemID.SquireGreaves,
				ItemID.HuntressWig,
				ItemID.HuntressJerkin,
				ItemID.HuntressPants,
				ItemID.ApprenticeHat,
				ItemID.ApprenticeRobe,
				ItemID.ApprenticeTrousers,
				ItemID.MonkBrows,
				ItemID.MonkShirt,
				ItemID.MonkPants
			});
			RecipeGroup.RegisterGroup("SOTS:T2DD2Armor", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Tier 2 DD2 Accessory", new int[]
			{
				ItemID.SquireShield,
				ItemID.HuntressBuckler,
				ItemID.ApprenticeScarf,
				ItemID.MonkBelt
			});
			RecipeGroup.RegisterGroup("SOTS:T2DD2Accessory", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Dissolving Element", new int[]
			{
				ModContent.ItemType<DissolvingAether>(),
				ModContent.ItemType<DissolvingNature>(),
				ModContent.ItemType<DissolvingEarth>(),
				ModContent.ItemType<DissolvingAurora>(),
				ModContent.ItemType<DissolvingDeluge>(),
				ModContent.ItemType<DissolvingUmbra>(),
				ModContent.ItemType<DissolvingBrilliance>(),
				ModContent.ItemType<DissolvingNether>()
			});
			RecipeGroup.RegisterGroup("SOTS:DissolvingElement", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Elemental Fragment", new int[]
			{
				ModContent.ItemType<FragmentOfOtherworld>(),
				ModContent.ItemType<FragmentOfNature>(),
				ModContent.ItemType<FragmentOfEarth>(),
				ModContent.ItemType<FragmentOfPermafrost>(),
				ModContent.ItemType<FragmentOfTide>(),
				ModContent.ItemType<FragmentOfEvil>(),
				ModContent.ItemType<FragmentOfChaos>(),
				ModContent.ItemType<FragmentOfInferno>()
			});
			RecipeGroup.RegisterGroup("SOTS:ElementalFragment", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Elemental Plating", new int[]
			{
				ModContent.ItemType<UltimatePlating>(),
				ModContent.ItemType<DullPlating>(),
				ModContent.ItemType<NaturePlating>(),
				ModContent.ItemType<EarthenPlating>(),
				ModContent.ItemType<PermafrostPlating>(),
				ModContent.ItemType<TidePlating>(),
				ModContent.ItemType<EvilPlating>(),
				ModContent.ItemType<ChaosPlating>(),
				ModContent.ItemType<InfernoPlating>()
			});
			RecipeGroup.RegisterGroup("SOTS:ElementalPlating", group);

			group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Alchemical Seeds", new int[]
			{
				ItemID.DaybloomSeeds,
				ItemID.MoonglowSeeds,
				ItemID.BlinkrootSeeds,
				ItemID.ShiverthornSeeds,
				ItemID.WaterleafSeeds,
				ItemID.FireblossomSeeds,
				ItemID.DeathweedSeeds

			});
			RecipeGroup.RegisterGroup("SOTS:AlchSeeds", group);
		}
		public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
			/*
            if (Main.myPlayer != -1 && !Main.gameMenu)
            {
                if(Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].GetModPlayer<SOTSPlayer>().PlanetariumBiome) //this makes the music play only in Custom Biome
                {
                    music = this.GetSoundSlot(SoundType.Music, "Sounds/Music/JourneyFromJar");  //add where is the custom music is located
					priority = MusicPriority.BossLow;
				
                } 
            }
			
			if (Main.myPlayer != -1 && !Main.gameMenu)
            {
                if(Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].GetModPlayer<SOTSPlayer>().GeodeBiome) 
                {
                    music = this.GetSoundSlot(SoundType.Music, "Sounds/Music/GeodeMusic");  //add where is the custom music is located
					priority = MusicPriority.BossLow;
				
                } 
            }
			*/
			if (Main.myPlayer != -1 && !Main.gameMenu)
			{
				Player player = Main.player[Main.myPlayer];
				if (player.active && player.GetModPlayer<SOTSPlayer>().PyramidBiome)
				{
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/CursedPyramid");
					priority = MusicPriority.BiomeHigh;
                }
				if (player.active && player.GetModPlayer<SOTSPlayer>().PlanetariumBiome)
				{
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/Planetarium");
					priority = MusicPriority.Event;
				}
				if (SOTSWorld.SecretFoundMusicTimer > 0)
				{
					SOTSWorld.SecretFoundMusicTimer--;
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/SecretFound");
					priority = MusicPriority.BossHigh + 1;
				}
				if (NPC.AnyNPCs(ModContent.NPCType<NPCs.knuckles>()) && Main.npc[NPC.FindFirstNPC(ModContent.NPCType<NPCs.knuckles>())].Distance(player.Center) <= 7000f)
				{
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/KnucklesTheme");
					priority = MusicPriority.BossHigh;
				}
				if(SOTSPlayer.pyramidBattle) //variable only applies to local player
				{
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/PyramidBattle");
					priority = MusicPriority.Environment;
				}
			}
		}
		public override void PostSetupContent()
        {
            Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            if(bossChecklist != null)
            {
				// AddBoss, bossname, order or value in terms of vanilla bosses, inline method for retrieving downed value.
				//bossChecklist.Call(....
				// To include a description:
				//bossChecklist.Call("AddBoss", "Putrid Pinky", 4.2f, (Func<bool>)(() => SOTSWorld.downedPinky));
				//bossChecklist.Call("AddBossWithInfo", "Putrid Pinky", 4.2f, (Func<bool>)(() => SOTSWorld.downedPinky), "Use [i:" + ItemType("JarOfPeanuts") + "]");	
				//bossChecklist.Call("AddBossWithInfo", "Pharaoh's Curse", 4.3f, (Func<bool>)(() => SOTSWorld.downedCurse), "Find the [i:" + ItemType("Sarcophagus") + "] in the pyramid");

				bossChecklist.Call(
					"AddBoss",
					4.25f,
					new List<int>() { ModContent.NPCType<PutridPinkyPhase2>() },
					this,
					"Putrid Pinky",
					(Func<bool>)(() => SOTSWorld.downedPinky),
					ModContent.ItemType<JarOfPeanuts>(),
					new List<int>() { ModContent.ItemType<PutridPinkyMusicBox>(), ModContent.ItemType<PutridPinkyTrophy>() },
					new List<int>() { ModContent.ItemType<PinkyBag>(), ModContent.ItemType<VialofAcid>(), ModContent.ItemType<Wormwood>(), ItemID.PinkGel },
					"Summon in any biome at any time using a [i:" + ModContent.ItemType<JarOfPeanuts>() + "]",
					"Putrid Pinky has robbed everyone of their peanuts!",
					"SOTS/NPCs/Boss/PutridPinky1_Display",
					"SOTS/NPCs/Boss/PutridPinky1_Head_Boss",
					(Func<bool>)(() => true));
				bossChecklist.Call(
					"AddBoss",
					4.5f,
					new List<int>() { ModContent.NPCType<PharaohsCurse>() },
					this,
					"Pharaoh's Curse",
					(Func<bool>)(() => SOTSWorld.downedCurse),
					ModContent.ItemType<Sarcophagus>(),
					new List<int>() { },
					new List<int>() { ModContent.ItemType<CurseBag>(), ModContent.ItemType<CursedMatter>() },
					"Activate the [i:" + ModContent.ItemType<Sarcophagus>() + "] in the pyramid",
					"",
					"SOTS/BossCL/PharaohPortrait",
					"",
					(Func<bool>)(() => true));
				bossChecklist.Call(
					"AddBoss",
					5.9f,
					new List<int>() { ModContent.NPCType<TheAdvisorHead>() },
					this,
					"The Advisor",
					(Func<bool>)(() => SOTSWorld.downedAdvisor),
					ModContent.ItemType<AvaritianGateway>(),
					new List<int>() { ModContent.ItemType<AdvisorMusicBox>() },
					new List<int>() { ModContent.ItemType<TheAdvisorBossBag>(), ModContent.ItemType<SkywareKey>(), ModContent.ItemType<StarlightAlloy>(), ModContent.ItemType<StrangeKey>(), ModContent.ItemType<HardlightAlloy>(), ModContent.ItemType<MeteoriteKey>(), ModContent.ItemType<OtherworldlyAlloy>() },
					"Destroy the 4 tethered Otherworldly Constructs of the Planetarium",
					"",
					"SOTS/BossCL/AdvisorPortrait",
					"",
					(Func<bool>)(() => true));
				bossChecklist.Call(
					"AddBoss",
					9.05f,
					new List<int>() { ModContent.NPCType<Polaris>() },
					this,
					"Polaris",
					(Func<bool>)(() => SOTSWorld.downedAmalgamation),
					new List<int>() { ModContent.ItemType<FrostedKey>(), ModContent.ItemType<FrostArtifact>() },
					new List<int>() { ModContent.ItemType<PolarisMusicBox>() },
					new List<int>() { ModContent.ItemType<PolarisBossBag>(), ModContent.ItemType<AbsoluteBar>(), ItemID.FrostCore },
					"Activate the [i:" + ModContent.ItemType<FrostArtifact>() + "] of the snow biome using [i:" + ModContent.ItemType<FrostedKey>() + "]",
					"",
					"SOTS/BossCL/PolarisPortrait",
					"",
					(Func<bool>)(() => true));
				bossChecklist.Call(
					"AddBoss",
					12.5f,
					new List<int>() { ModContent.NPCType<Lux>() },
					this,
					"Lux",
					(Func<bool>)(() => SOTSWorld.downedLux),
					new List<int>() { ModContent.ItemType<ElectromagneticLure>() },
					new List<int>() { },
					new List<int>() { ModContent.ItemType<LuxBag>(), ModContent.ItemType<DissolvingBrilliance>(), ModContent.ItemType<PhaseOre>(), ItemID.SoulofLight },
					"Anger a Chaos Spirit",
					"",
					"SOTS/BossCL/LuxBossLog",
					"",
					(Func<bool>)(() => true));
				bossChecklist.Call(
					"AddBoss",
					12.9f,
					new List<int>() { ModContent.NPCType<SubspaceSerpentHead>(), ModContent.NPCType<SubspaceSerpentBody>(), ModContent.NPCType<SubspaceSerpentTail>() },
					this,
					"Subspace Serpent",
					(Func<bool>)(() => SOTSWorld.downedSubspace),
					new List<int>() { ModContent.ItemType<CatalystBomb>() },
					new List<int>() { ModContent.ItemType<SubspaceSerpentMusicBox>() },
					new List<int>() { ModContent.ItemType<SubspaceBag>(), ModContent.ItemType<SanguiteBar>()},
					"Tear a dimensional rift in hell by detonating a [i:" + ModContent.ItemType<CatalystBomb>() + "]",
					"",
					"SOTS/BossCL/SubspaceSerpentPortrait",
					"",
					(Func<bool>)(() => true));


				//bossChecklist.Call("AddBossWithInfo", "The Advisor", 5.9f, (Func<bool>)(() => SOTSWorld.downedAdvisor), "Destroy the 4 tethered Otherworldly Constructs on the Planetarium");
				//bossChecklist.Call("AddBossWithInfo", "Cryptic Carver", 5.2f, (Func<bool>)(() => SOTSWorld.downedCarver), "Use [i:" + ItemType("MargritArk") + "]");
				//bossChecklist.Call("AddBossWithInfo", "Ethereal Entity", 6.5f, (Func<bool>)(() => SOTSWorld.downedEntity), "Use [i:" + ItemType("PlanetariumDiamond") + "] in a planetarium biome");

				//bossChecklist.Call("AddBossWithInfo", "Antimaterial Antlion", 7.21f, (Func<bool>)(() => SOTSWorld.downedAntilion), "Use [i:" + ItemType("ForbiddenPyramid") + "] in a desert biome");
				//bossChecklist.Call("AddBossWithInfo", "Icy Amalgamation", 8.21f, (Func<bool>)(() => SOTSWorld.downedAmalgamation), "Use [i:" + ItemType("FrostedKey") + "] on a [i:" + ItemType("FrostArtifact") + "] in a snow biome");
				//bossChecklist.Call("AddBossWithInfo", "Celestial Serpent", 11.1f, (Func<bool>)(() => SOTSWorld.downedLux), "Use [i:" + ItemType("CelestialTorch") + "] during night time");
               // bossChecklist.Call("AddBossWithInfo", "Subspace Serpent", 11.2f, (Func<bool>)(() => SOTSWorld.downedSubspace), "Tear a rift in hell by detonating a [i:" + ItemType("CatalystBomb") + "]");

			 }
        }
		public static float lightingChange = 1f;
        public override void ModifyLightingBrightness(ref float scale)
        {
			scale *= lightingChange;
			lightingChange = 1;
		}
		//Custom Tile Merging
		public static bool[][] tileMergeTypes;
		public static float LuxLightingFadeIn = 0;
		public static float PlanetariumLightingFadeIn = 0;
		public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
		{
			if (Main.gameMenu)
			{
				return;
			}
			var player = Main.LocalPlayer;
			SOTSPlayer sPlayer = SOTSPlayer.ModPlayer(player);
			if (sPlayer.zoneLux)
			{

			}
			else if (LuxLightingFadeIn > 0)
			{
				LuxLightingFadeIn -= 0.01f;
			}
			backgroundColor = Color.Lerp(backgroundColor, new Color(15, 0, 30), 0.96f * LuxLightingFadeIn);
			tileColor = Color.Lerp(tileColor, new Color(15, 0, 30), 0.96f * LuxLightingFadeIn);
			if (sPlayer.PlanetariumBiome)
			{
				if (PlanetariumLightingFadeIn < 1)
					PlanetariumLightingFadeIn += 0.01f;
			}
			else if (PlanetariumLightingFadeIn > 0)
			{
				PlanetariumLightingFadeIn -= 0.01f;
			}
			backgroundColor = Color.Lerp(backgroundColor, new Color(0, 0, 10), 0.9f * PlanetariumLightingFadeIn);
			tileColor = Color.Lerp(tileColor, new Color(0, 0, 10), 0.9f * PlanetariumLightingFadeIn);
		}
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
			if (check.TileType == myType || Main.tileMerge[myType][check.TileType])
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
				Main.tile[x, y].frameNumber((byte)randomFrame);
			}
			else
			{
				randomFrame = Main.tile[x, y].frameNumber();
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
