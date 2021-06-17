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
using SOTS.Void;
using SOTS.Items.Pyramid;
using SOTS.Items.Otherworld.EpicWings;
using SOTS.NPCs.ArtificialDebuffs;
using SOTS.Items.Otherworld.FromChests;
using SOTS.NPCs.Boss;
using SOTS.Items.GelGear;
using SOTS.Items.Otherworld;
using SOTS.Items.IceStuff;
using SOTS.Items.Celestial;
using SOTS.Items.MusicBoxes;
using SOTS.NPCs.Boss.Polaris;
using SOTS.NPCs.Boss.Curse;

namespace SOTS
{
	public class SOTS : Mod
	{
		public static ModHotKey BlinkHotKey;
		public static ModHotKey ArmorSetHotKey;
		internal static SOTS Instance;
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
		public override void Load()
		{
			//SOTSGlowmasks.LoadGlowmasks();
			Instance = ModContent.GetInstance<SOTS>();
			BlinkHotKey = RegisterHotKey("Blink", "V");
			ArmorSetHotKey = RegisterHotKey("Armor Set", "F");

			if (!Main.dedServ)
            {
                VoidUI = new VoidUI();
                VoidUI.Activate();
                _VoidUserInterface = new UserInterface();
                _VoidUserInterface.SetState(VoidUI);
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
			}
			//Music Box Stuff
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/PutridPinky"), ItemType("PutridPinkyMusicBox"), TileType("PutridPinkyMusicBoxTile"));
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Advisor"), ItemType("AdvisorMusicBox"), TileType("AdvisorMusicBoxTile"));
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Polaris"), ItemType("PolarisMusicBox"), TileType("PolarisMusicBoxTile"));
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/SubspaceSerpent"), ItemType("SubspaceSerpentMusicBox"), TileType("SubspaceSerpentMusicBoxTile"));
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/AncientPyramid"), ItemType("AncientPyramidMusicBox"), TileType("AncientPyramidMusicBoxTile"));
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Planetarium"), ItemType("PlanetariumMusicBox"), TileType("PlanetariumMusicBoxTile"));
		}
		public override void Unload() 
		{
			//SOTSGlowmasks.UnloadGlowmasks();
			Instance = null;
			VoidBarSprite._backgroundTexture = null;
			VoidBarBorder._backgroundTexture = null;
			VoidBarBorder2._backgroundTexture = null;
			BarDivider._backgroundTexture = null;
			SoulBar._backgroundTexture = null;
			VoidUI.voidUI = null;
			BlinkHotKey = null;
			ArmorSetHotKey = null;
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
			OrbitalCounterChanged,
			SyncCreativeFlight,
			SyncLootingSoulsAndVoidMax,
			SyncGlobalNPC,
			SyncPlayerKnives,
			SyncRexFlower
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
					int orbitalCounter = reader.ReadInt32();
					modPlayer.orbitalCounter = orbitalCounter;
					bool creativeFlight = reader.ReadBoolean();
					testPlayer.creativeFlight = creativeFlight;
					int lootingSouls = reader.ReadInt32();
					voidPlayer.lootingSouls = lootingSouls;
					// SyncPlayer will be called automatically, so there is no need to forward this data to other clients.
					break;
				case (int)SOTSMessageType.OrbitalCounterChanged:
					playernumber = reader.ReadByte();
					modPlayer = Main.player[playernumber].GetModPlayer<SOTSPlayer>();
					modPlayer.orbitalCounter = reader.ReadInt32();
					// Unlike SyncPlayer, here we have to relay/forward these changes to all other connected clients
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)SOTSMessageType.OrbitalCounterChanged);
						packet.Write(playernumber);
						packet.Write(modPlayer.orbitalCounter);
						packet.Send(-1, playernumber);
					}
					break;
                case (int)SOTSMessageType.SyncCreativeFlight:
					playernumber = reader.ReadByte(); 
					testPlayer = Main.player[playernumber].GetModPlayer<TestWingsPlayer>();
					testPlayer.creativeFlight = reader.ReadBoolean();
					// Unlike SyncPlayer, here we have to relay/forward these changes to all other connected clients
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
					// Unlike SyncPlayer, here we have to relay/forward these changes to all other connected clients
					if (Main.netMode == NetmodeID.Server)
					{
						var packet = GetPacket();
						packet.Write((byte)SOTSMessageType.SyncLootingSoulsAndVoidMax);
						packet.Write(playernumber);
						packet.Write(voidPlayer.lootingSouls);
						packet.Write(voidPlayer.voidMeterMax);
						packet.Write(voidPlayer.voidMeterMax2);
						packet.Write(voidPlayer.voidMeter);
						packet.Send(-1, playernumber);
					}
					break;
				case (int)SOTSMessageType.SyncGlobalNPC:
					playernumber = reader.ReadByte();
					int npcNumber = reader.ReadInt32();
					DebuffNPC debuffNPC = (DebuffNPC)GetGlobalNPC("DebuffNPC");
					debuffNPC = (DebuffNPC)debuffNPC.Instance(Main.npc[npcNumber]);
					debuffNPC.HarvestCurse = reader.ReadInt32();
					debuffNPC.PlatinumCurse = reader.ReadInt32();
					debuffNPC.DestableCurse = reader.ReadInt32();
					debuffNPC.BleedingCurse = reader.ReadInt32();
					// Unlike SyncPlayer, here we have to relay/forward these changes to all other connected clients
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
					// Unlike SyncPlayer, here we have to relay/forward these changes to all other connected clients
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
			}
		}
		public override void AddRecipes()
		{
			TransmutationAltar.AddTransmutationRecipes(this);

			ModRecipe recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "Wormwood", 30);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(ItemID.SlimeStaff, 1);
			recipe.AddRecipe();
			
			/*just in case temple gets cucked
			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.TempleKey, 2);
			recipe.AddIngredient(ItemID.LihzahrdPowerCell, 1); //power cell
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(ItemID.LihzahrdAltar, 1); //altar
			recipe.AddRecipe();
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.TempleKey, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(ItemID.LihzahrdPowerCell, 2); //power cell
			recipe.AddRecipe();
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.LihzahrdBrick, 30); //lizahrd brick
			recipe.AddIngredient(ItemID.FallenStar, 5); //lizahrd brick
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(ItemID.LihzahrdPowerCell, 1);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.TempleKey, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(ItemID.LihzahrdBrick, 75);
			recipe.AddRecipe(); */
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "FragmentOfChaos", 125);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(ItemID.RodofDiscord, 1); //rod of discord
			recipe.AddRecipe();
			
			recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "FragmentOfNature", 25);
			recipe.AddIngredient(ItemID.HermesBoots, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(ItemID.FlowerBoots, 1);
			recipe.AddRecipe();
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
                    music = (SOTSPlayer.ModPlayer(player).weakerCurse || SOTSWorld.downedBoss2) ? GetSoundSlot(SoundType.Music, "Sounds/Music/AncientPyramid") : MusicID.Desert;
					priority = MusicPriority.BiomeHigh;
                }
			}
			if (Main.myPlayer != -1 && !Main.gameMenu)
			{
				Player player = Main.player[Main.myPlayer];
				if (player.active && player.GetModPlayer<SOTSPlayer>().PlanetariumBiome)
				{
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/Planetarium");
					priority = MusicPriority.Event;
				}
			}
			if (Main.myPlayer != -1 && !Main.gameMenu)
			{
				//Player player = Main.player[Main.myPlayer];
				if (SOTSWorld.SecretFoundMusicTimer > 0)
				{
					SOTSWorld.SecretFoundMusicTimer--;
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/SecretFound");
					priority = MusicPriority.BossHigh + 1;
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
					new List<int>() { ModContent.ItemType<PutridPinkyMusicBox>() },
					new List<int>() { ModContent.ItemType<PinkyBag>(), ModContent.ItemType<VialofAcid>(), ModContent.ItemType<Wormwood>(), ItemID.PinkGel },
					"Summon in any biome at any time using a [i:" + ModContent.ItemType<JarOfPeanuts>() + "]",
					"{0} has robbed everyone of their peanuts!",
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
					"SOTS/BossCL/Depression",
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
					11.4f,
					new List<int>() { ModContent.NPCType<CelestialSerpentHead>(), ModContent.NPCType<CelestialSerpentBody>(), ModContent.NPCType<CelestialSerpentTail>() },
					this,
					"Celestial Serpent",
					(Func<bool>)(() => SOTSWorld.downedCelestial),
					new List<int>() { ModContent.ItemType<CelestialTorch>() },
					new List<int>() { },
					new List<int>() { ModContent.ItemType<CelestialBag>(), ModContent.ItemType<StarShard>(), ModContent.ItemType<StrangeFruit>() },
					"Use a [i:" + ModContent.ItemType<CelestialTorch>() + "] during the night",
					"",
					"SOTS/NPCs/Boss/CelestialSerpent_Display",
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
				//bossChecklist.Call("AddBossWithInfo", "Celestial Serpent", 11.1f, (Func<bool>)(() => SOTSWorld.downedCelestial), "Use [i:" + ItemType("CelestialTorch") + "] during night time");
               // bossChecklist.Call("AddBossWithInfo", "Subspace Serpent", 11.2f, (Func<bool>)(() => SOTSWorld.downedSubspace), "Tear a rift in hell by detonating a [i:" + ItemType("CatalystBomb") + "]");

			 }
        }
		public override void AddRecipeGroups()
		{
			RecipeGroup group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Silver Bar", new int[]
			{
				ItemID.SilverBar,
				ItemID.TungstenBar
			});
			RecipeGroup.RegisterGroup("SOTS:SilverBar", group);
		}
	}
}
