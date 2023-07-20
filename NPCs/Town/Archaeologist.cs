using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;
using SOTS.Biomes;
using System;
using SOTS.Items.Conduit;
using SOTS.Items.AbandonedVillage;
using SOTS.Common.Systems;
using SOTS.Items.Pyramid;
using SOTS.Items.Gems;
using SOTS.Items.ChestItems;
using SOTS.Items.Secrets;
using System.IO;
using SOTS.Items.Whips;
using Terraria.Map;
using Terraria.DataStructures;
using System.Reflection;
using Terraria.GameContent.Drawing;
using Terraria.Graphics;
using static SOTS.NPCs.Town.PortalDrawingHelper;
using SOTS.Dusts;
using SOTS.WorldgenHelpers;
using SOTS.Common;
using SOTS.Buffs.Debuffs;

namespace SOTS.NPCs.Town
{
	public class Archaeologist : ModNPC
	{
		public static Vector2 AnomalyPosition1 = Vector2.Zero;
		public static Vector2 AnomalyPosition2 = Vector2.Zero;
		public static Vector2 AnomalyPosition3 = Vector2.Zero;
		public static float AnomalyAlphaMult = 0f;
		public static float FinalAnomalyAlphaMult = 0f;
		public static int locationTimer = 59960;
		public const int timeToGoToSetPiece = 60000; //This is 1000 seconds
		public const int MinimumIdleCycles = 6;
		public const int MaximumIdleCycles = 22;
		public const int MinimunLookCycles = 5;
		public const int MaximumLookCycles = 14;
		public bool hasTeleportedYet = false;
        public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(hasTeleportedYet);
			writer.Write(locationTimer);
			writer.Write(InitialDirection);
			writer.Write(currentLocationType);
			writer.Write(AnomalyAlphaMult);
			writer.Write(AnomalyPosition1.X);
			writer.Write(AnomalyPosition1.Y);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
		{
			hasTeleportedYet = reader.ReadBoolean();
			locationTimer = reader.ReadInt32();
			InitialDirection = reader.ReadInt32();
			currentLocationType = reader.ReadInt32();
			AnomalyAlphaMult = reader.ReadSingle();
			AnomalyPosition1.X = reader.ReadSingle();
			AnomalyPosition1.Y = reader.ReadSingle();
		}
        //private static Profiles.StackedNPCProfile NPCProfile;
        public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 5; // The amount of frames the NPC has
			NPCID.Sets.ExtraFramesCount[Type] = 0; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs.
			NPCID.Sets.AttackFrameCount[Type] = 0;
			NPCID.Sets.DangerDetectRange[Type] = 0; // The amount of pixels away from the center of the npc that it tries to attack enemies.
			NPCID.Sets.PrettySafe[Type] = 0;
			NPCID.Sets.AttackType[Type] = 0; // Shoots a weapon.
			NPCID.Sets.AttackTime[Type] = 0; // The amount of time it takes for the NPC's attack animation to be over once it starts.
			NPCID.Sets.AttackAverageChance[Type] = 0;
			NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.
			//NPCID.Sets.ShimmerTownTransform[NPC.type] = true; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

			//This sets entry is the most important part of this NPC. Since it is true, it tells the game that we want this NPC to act like a town NPC without ACTUALLY being one.
			//What that means is: the NPC will have the AI of a town NPC, will attack like a town NPC, and have a shop (or any other additional functionality if you wish) like a town NPC.
			//However, the NPC will not have their head displayed on the map, will de-spawn when no players are nearby or the world is closed, will spawn like any other NPC, and have no happiness button when chatting.
			NPCID.Sets.ActsLikeTownNPC[Type] = true;

			//NPCID.Sets.SpawnsWithCustomName[Type] = true;
			//NPCID.Sets.AllowDoorInteraction[Type] = true;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Direction = 1
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			/*NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, -1),
				new Profiles.DefaultNPCProfile(Texture + "_Shimmer", -1)
			);*/
			NPC.behindTiles = true;
		}
		public override void SetDefaults()
		{
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 42;
			NPC.height = 60;
			NPC.aiStyle = 7;
			NPC.damage = 100;
			NPC.defense = 150;
			NPC.lifeMax = 25000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			NPC.behindTiles = true;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.netAlways = true;
		}
		public bool playerNearby = false;
		public int AnimCycles = 0;
		public int FrameY = 0;
		public int FrameSpeed = 5;
		public int TotalIdleFrameCycles = 6;
		public int TotalLookUpFrameCycles = 3;
		public int DrawTimer = 0;
		public int aiTimer = 0;
		public int InitialDirection = 0;
        public override bool CheckActive()
        {
            return false;
        }
        public override void ModifyTypeName(ref string typeName)
        {
			typeName = Language.GetTextValue("Mods.SOTS.NPCName.Archaeologist");
			if(hasPlayerChattedBefore)
			{
				typeName = Language.GetTextValue("Mods.SOTS.NPCName.ArchaeologistNearby");
			}
		}
		public void DrawHoverPlatforms(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = ModContent.Request<Texture2D>("SOTS/NPCs/Town/ArchaeologistPlatform").Value;
			int height = texture.Height;
			int width = texture.Width;
			Vector2 origin = new Vector2(width / 2, 0);
			float grainy = (float)(DrawTimer % 150);
			drawColor = NPC.GetAlpha(Color.Lerp(drawColor, Color.White, 0.5f));
			for (int i = 0; i < height - 1; i++)
			{
				float sinusoid = 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(-DrawTimer * 2.4f + i * 18));
				float bonusAlphaMult = 0;
				float xOffset = 0;
				float xScale = 1;
				float progress = (float)i / height;
				if (sinusoid > 0.9f)
				{
					sinusoid -= 0.9f;
					sinusoid *= 1 / 0.1f;
					sinusoid = sinusoid * sinusoid;
					bonusAlphaMult += sinusoid;
					xScale += sinusoid * 0.075f;
				}
				else if (grainy > 138)
				{
					int grainDirection = NPC.direction * (((i / 2) % 2) * 2 - 1);
					float grainProgress = (float)Math.Sin(MathHelper.ToRadians(360 * (grainy - 138) / 12f));
					float grainMult = 1f * (1 - 0.5f * bonusAlphaFromBeingNear);
					xOffset += grainDirection * grainProgress * grainMult * (0.6f + 0.4f * (float)Math.Sin(progress * MathHelper.Pi));
				}
				Vector2 drawFromPosition = new Vector2(NPC.Center.X, NPC.position.Y + NPC.height + height - 1) + new Vector2(xOffset, -1 * i);
				Rectangle frame = new Rectangle(0, height - (i + 1), width, 1);
				float baseAlpha = 0.10f + 0.61f * bonusAlphaFromBeingNear;
				float gradientAlpha = 0.4f * (1 - 0.9f * bonusAlphaFromBeingNear);
				spriteBatch.Draw(texture, drawFromPosition - screenPos, frame, drawColor * (bonusAlphaMult * 0.35f + (baseAlpha + gradientAlpha * (float)Math.Sqrt(progress))), NPC.rotation, origin, new Vector2(xScale, 1), SpriteEffects.None, 0f);
			}
		}
        public void Draw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = TextureAssets.Npc[NPC.type].Value;
			int height = texture.Height / 5;
			int width = texture.Width;
			Vector2 origin = new Vector2(width / 2, 0);
			int startingFrame = NPC.frame.Y;
			float grainy = (float)(DrawTimer % 150);
			drawColor = NPC.GetAlpha(Color.Lerp(drawColor, Color.White, 0.5f));
			if (screenPos != Main.screenPosition) //this should check for bestiary?
			{
				NPC.spriteDirection = -1;
				drawColor = Color.White;
				bonusAlphaFromBeingNear = 0.7f;
			}
			for (int i = 0; i < height - 1; i++)
			{
				float sinusoid = 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(-DrawTimer * 2.4f + i * 18));
				float bonusAlphaMult = 0;
				float xOffset = 0;
				float xScale = 1;
				float progress = (float)i / height;
				if (sinusoid > 0.9f)
                {
					sinusoid -= 0.9f;
					sinusoid *= 1 / 0.1f;
					sinusoid = sinusoid * sinusoid;
					bonusAlphaMult += sinusoid;
					xScale += sinusoid * 0.075f;
                }
				else if(grainy > 138)
                {
					int grainDirection = NPC.direction * (((i / 2) % 2) * 2 - 1);
					float grainProgress = (float)Math.Sin(MathHelper.ToRadians(360 * (grainy - 138) / 12f));
					float grainMult = 1f * (1 - 0.5f * bonusAlphaFromBeingNear);
					xOffset += grainDirection * grainProgress * grainMult * (0.6f + 0.4f * (float)Math.Sin(progress * MathHelper.Pi));
                }
				Vector2 drawFromPosition = new Vector2(NPC.Center.X, NPC.position.Y + NPC.height) + new Vector2(xOffset, -1 * i);
				Rectangle frame = new Rectangle(0, startingFrame + height - (i + 1), width, 1);
				float baseAlpha = 0.10f + 0.61f * bonusAlphaFromBeingNear;
				float gradientAlpha = 0.4f * (1 - 0.9f * bonusAlphaFromBeingNear);
				spriteBatch.Draw(texture, drawFromPosition - screenPos, frame, drawColor * (bonusAlphaMult * 0.35f + (baseAlpha + gradientAlpha * (float)Math.Sqrt(progress))), NPC.rotation, origin, new Vector2(xScale, 1), NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (screenPos != Main.screenPosition) //this should check for bestiary?
			{
				Draw(spriteBatch, screenPos, drawColor);
			}
			else
				DrawHoverPlatforms(spriteBatch, screenPos, drawColor);
			return false;
        }
        float bonusAlphaFromBeingNear = 0f;
        public override void FindFrame(int frameHeight)
		{
			if(playerNearby)
            {
				AnimCycles = TotalIdleFrameCycles;
            }
			NPC.frameCounter++;
			if (AnimCycles < TotalIdleFrameCycles)
			{
				if (NPC.frameCounter >= FrameSpeed)
				{
					FrameSpeed = Main.rand.Next(3, 13);
					NPC.frameCounter = 0;
					FrameY++;
				}
				if (FrameY > 1)
				{
					FrameY = 0;
					AnimCycles++;
				}
			}
			else if (AnimCycles >= TotalIdleFrameCycles)
			{
				if (NPC.frameCounter >= FrameSpeed)
				{
					FrameSpeed = 8;
					NPC.frameCounter = 0;
					if (AnimCycles >= TotalLookUpFrameCycles + TotalIdleFrameCycles)
					{
						FrameY--;
					}
					else
						FrameY++;
				}
				if (FrameY > 4)
				{
					FrameY = 4;
					AnimCycles++;
				}
				if (FrameY < 0)
				{
					FrameY = 0;
					AnimCycles = 0;
					TotalIdleFrameCycles = Main.rand.Next(MinimumIdleCycles, MaximumIdleCycles);
					TotalLookUpFrameCycles = Main.rand.Next(MinimunLookCycles, MaximumLookCycles);
				}
			}
			NPC.frame.Y = frameHeight * FrameY;
			DrawTimer++;
		}
		public void ArchAI()
        {
			if (NPC.CountNPCS(Type) > 1)
			{
				NPC.active = false;
				return;
			}
			else if (hasTeleportedYet)
			{
				AnomalyAlphaMult += 1 / 60f;
				AnomalyPosition1 = NPC.Center;
			}
			if (InitialDirection == 0)
			{
				InitialDirection = 1;
			}
			int directionToGoTo = InitialDirection;
			playerNearby = false;
			bool playerWithinSecondRange = false;
			for (int i = 0; i < Main.player.Length; i++)
			{
				Player player = Main.player[i];
				if (player.active)
				{
					if (player.Distance(NPC.Center) < 1600)
					{
						playerWithinSecondRange = true;

						if (player.Distance(NPC.Center) < 120)
						{
							playerNearby = true;
							if (NPC.Center.X > player.Center.X)
							{
								directionToGoTo = -1;
							}
							else
							{
								directionToGoTo = 1;
							}
							break;
						}
					}
				}
			}
			if (!playerWithinSecondRange)
			{
				if (locationTimer > timeToGoToSetPiece || !hasTeleportedYet)
				{
					aiTimer = 0;
					locationTimer = 0;
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						FindALocationToGoTo();
						InitialDirection = NPC.direction;
					}
					if (!hasTeleportedYet)
					{
						locationTimer = timeToGoToSetPiece - 120;
					}
				}
				else if (locationTimer > timeToGoToSetPiece - 60)
				{
					AnomalyAlphaMult = 1 - (locationTimer - timeToGoToSetPiece + 60) / 60f;
				}
				locationTimer++;
			}
			NPC.direction = NPC.spriteDirection = directionToGoTo;
			if (playerNearby)
			{
				bonusAlphaFromBeingNear += 0.02f;
			}
			else
			{
				bonusAlphaFromBeingNear -= 0.02f;
			}
			bonusAlphaFromBeingNear = MathHelper.Clamp(bonusAlphaFromBeingNear, 0, 1);
			NPC.dontTakeDamage = true;
			NPC.dontTakeDamageFromHostiles = true;
			NPC.velocity.X *= 0;
			aiTimer++;
			if (aiTimer > 120)
			{
				NPC.velocity.Y *= 0f;
				NPC.alpha = 0;
			}
			else
			{
				NPC.alpha = (int)(255 * (1f - aiTimer / 120f));
				NPC.velocity.Y += 0.1f;
			}
			AnomalyAlphaMult = MathHelper.Clamp(AnomalyAlphaMult, 0, 1);
			FinalAnomalyAlphaMult = MathHelper.Clamp(AnomalyAlphaMult * 1.1f - 0.1f, 0, 1);
		}
        public override bool PreAI()
        {
			return base.PreAI();
        }
        public override void PostAI()
        {
            base.PostAI();
			NPC.velocity.X *= 0f;
			NPC.velocity = Collision.TileCollision(new Vector2(NPC.Center.X - 8, NPC.position.Y), NPC.velocity, 16, NPC.height, false);
		}
        //Make sure to allow your NPC to chat, since being "like a town NPC" doesn't automatically allow for chatting.
        public override bool CanChat()
		{
			return true;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			ModBiomeBestiaryInfoElement Planetarium = ModContent.GetInstance<PlanetariumBiome>().ModBiomeBestiaryInfoElement;
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				Planetarium,
				new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.ArchaeologistLore")
			});
		}
		public bool hasPlayerChattedBefore = false;
		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();
			if (hasPlayerChattedBefore)
			{
				chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogue3"), 0.5);
				chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogue4"), 0.5);
				chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogue5"), 0.5);
				chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogue6"), 0.5);
				if (currentLocationType == ImportantTileID.AcediaPortal || currentLocationType == ImportantTileID.AvaritiaPortal)
                {
					if(currentLocationType != ImportantTileID.AvaritiaPortal)
						chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialoguePortalNotAvaritia"));
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialoguePortal1"));
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialoguePortal2"));
				}
				if (currentLocationType == ImportantTileID.AcediaPortal)
				{
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueAcedia1"));
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueAcedia2"));
				}
				if (currentLocationType == ImportantTileID.AvaritiaPortal)
				{
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueAvaritia1"));
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueAvaritia2"));
				}
				if (currentLocationType == ImportantTileID.bigCrystal)
				{
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueBigCrystal"));
				}
				if (currentLocationType == ImportantTileID.coconutIslandMonument)
				{
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueCoconutMonument"));
				}
				if (currentLocationType == ImportantTileID.coconutIslandMonumentBroken)
				{
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueCoconutBroken"));
				}
				if (currentLocationType == ImportantTileID.damoclesChain)
				{
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueDamocles"));
				}
				if (currentLocationType == ImportantTileID.iceMonument)
				{
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueIceMonument"));
				}
				if (currentLocationType >= ImportantTileID.gemlockAmethyst && currentLocationType <= ImportantTileID.gemlockAmber)
				{
					if(currentLocationType == ImportantTileID.gemlockDiamond)
					{
						chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueGemlockDiamond"));
					}
					else chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueGemlockNotDiamond"));
				}
			}
			else if(!hasPlayerChattedBefore)
			{
				chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogue1"));
				chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogue2"), 0.5);
				hasPlayerChattedBefore = true;
			}
			return chat; // chat is implicitly cast to a string.
		}
		public override void SetChatButtons(ref string button, ref string button2)
		{ 
			button = Language.GetTextValue("LegacyInterface.28"); //This is the key to the word "Shop"
		}
		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				shop = true;
			}
		}
		private static void AddItemToShop(Chest shop, ref int nextSlot, int itemID)
		{
			shop.item[nextSlot].SetDefaults(itemID);
			nextSlot++;
		}
		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			AddItemToShop(shop, ref nextSlot, ModContent.ItemType<AnomalyLocator>());
			AddItemToShop(shop, ref nextSlot, ModContent.ItemType<ArchaeologistToolbelt>());
			AddItemToShop(shop, ref nextSlot, ModContent.ItemType<GoldenTrowel>());
			AddItemToShop(shop, ref nextSlot, ModContent.ItemType<OldKey>());
			AddItemToShop(shop, ref nextSlot, ModContent.ItemType<ConduitChassis>());
			if (currentLocationType == ImportantTileID.AcediaPortal)
			{
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<NatureConduit>());
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<CursedApple>());
				if(SOTSWorld.DreamLampSolved)
				{
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<DreamLamp>());
				}
			}
			if (currentLocationType == ImportantTileID.AvaritiaPortal)
            {
				if(SOTSWorld.downedAdvisor)
				{
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<Items.Otherworld.MeteoriteKey>());
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<Items.Otherworld.SkywareKey>());
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<Items.Otherworld.StrangeKey>());
				}
			}
			if (currentLocationType == ImportantTileID.gemlockAmethyst)
			{
				if(SOTSWorld.AmethystKeySlotted)
				{
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<AmethystRing>());
				}
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<RockCandy>());
			}
			if (currentLocationType == ImportantTileID.gemlockTopaz)
			{
				if (SOTSWorld.TopazKeySlotted)
				{
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<TopazRing>());
				}
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<BetrayersKnife>());
			}
			if (currentLocationType == ImportantTileID.gemlockSapphire)
			{
				if (SOTSWorld.SapphireKeySlotted)
				{
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<SapphireRing>());
				}
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<BagOfAmmoGathering>());
			}
			if (currentLocationType == ImportantTileID.gemlockEmerald)
			{
				if (SOTSWorld.EmeraldKeySlotted)
				{
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<EmeraldRing>());
				}
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<Items.Invidia.VorpalKnife>());
			}
			if (currentLocationType == ImportantTileID.gemlockRuby)
			{
				if (SOTSWorld.RubyKeySlotted)
				{
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<RubyRing>());
				}
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<SyntheticLiver>());
			}
			if (currentLocationType == ImportantTileID.gemlockDiamond)
			{
				if (SOTSWorld.DiamondKeySlotted)
				{
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<DiamondRing>());
				}
			}
			if (currentLocationType == ImportantTileID.gemlockAmber)
			{
				if (SOTSWorld.AmberKeySlotted)
				{
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<AmberRing>());
				}
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<KelpWhip>());
			}
			if (currentLocationType == ImportantTileID.iceMonument)
			{
				if (NPC.downedBoss1)
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<GlazeBow>());
			}
			if (currentLocationType == ImportantTileID.coconutIslandMonument)
			{
				if (NPC.downedBoss1)
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<CoconutGun>());
			}
			if (currentLocationType == ImportantTileID.coconutIslandMonumentBroken)
			{
				if (NPC.downedBoss1)
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<CoconutGun>());
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<PhotonGeyser>());
			}
			if (currentLocationType == ImportantTileID.damoclesChain)
			{
				if (NPC.downedBoss1)
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<Items.Crushers.BoneClapper>());
				AddItemToShop(shop, ref nextSlot, ItemID.Terragrim);
			}
			if (currentLocationType == ImportantTileID.bigCrystal)
			{
				if (NPC.downedBoss1)
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<PerfectStar>());
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<VisionAmulet>());
			}
		}
		public static int currentLocationType = -1;
		public void FindALocationToGoTo()
		{
			NPC.netUpdate = true;
			int olderLocationType = currentLocationType;
			currentLocationType = 0;
			int newDirection = 0;
			Vector2? destination = ImportantTilesWorld.RandomImportantLocation(ref currentLocationType, ref newDirection);
			if(destination.HasValue)
			{
				NPC.Center = destination.Value;
				NPC.direction = newDirection;
				VoidAnomaly.KillOtherAnomalies();
				VoidAnomaly.PlaceDownAnomalies();
				AnomalyAlphaMult = 0;
				if (Main.netMode == NetmodeID.Server)
					Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("The location is at: " + currentLocationType), Color.Gray);
				else
					Main.NewText("The location is at: " + currentLocationType);
				hasTeleportedYet = true;
			}
			else
            {
				currentLocationType = olderLocationType;
            }
		}
	}
	public class VoidAnomaly : ModProjectile
	{
		public static TileDrawInfo RunGet_currentTileDrawInfo(TileDrawing tDrawer)
		{
			Type type = tDrawer.GetType();
			FieldInfo field = type.GetField("_currentTileDrawInfoNonThreaded", BindingFlags.NonPublic | BindingFlags.Instance);
			return (TileDrawInfo)field.GetValue(tDrawer);
		}
		public static void Run_DrawSingleTile(TileDrawing tDrawer, TileDrawInfo drawData, bool solidLayer, int waterStyleOverride, Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY)
		{
			Type type = tDrawer.GetType();
			MethodInfo method = type.GetMethod("DrawSingleTile", BindingFlags.NonPublic | BindingFlags.Instance);
			if (method == null)
				return;
			method.Invoke(tDrawer, new object[] { drawData, solidLayer, waterStyleOverride, screenPosition, screenOffset, tileX, tileY });
		}
		public static void DrawWall(ref SaveTileData Data, int i, int j, int h, int k, Vector2 offset, int pass)
		{
			Tile otherTile = Main.tile[i, j];
			Tile myTile = Main.tile[h, k];
			int oType = otherTile.WallType;
			if (pass == 2)
			{
				Data.OntoTileW(myTile);
			}
			else if (pass == 1)
			{
				if (otherTile.WallType != 0)
				{
					SpriteBatch spriteBatch = Main.spriteBatch;
					if (WallLoader.PreDraw(h, k, oType, spriteBatch))
					{
						DrawWallM(spriteBatch, -offset, otherTile, i, j, oType);
					}
					WallLoader.PostDraw(h, k, oType, spriteBatch);
				}
			}
			else
			{
				if (otherTile.WallType != 0)
				{
					if (!TextureAssets.Wall[oType].IsLoaded)
					{
						Main.instance.LoadWall(oType);
					}
				}
				Data.IntoSaveW(myTile);
				Data.CopyTileToTileW(otherTile, myTile);
			}
		}
		public static float WaterAlphaMult(TileDrawInfo info, int i, int j, int liquidType)
		{
			float num6 = 0.5f;
			switch (liquidType)
			{
				case 1:
					num6 = 1f;
					break;
				case 11:
					num6 = Math.Max(num6 * 1.7f, 1f);
					break;
			}
			if (j <= Main.worldSurface || num6 > 1f)
			{
				num6 = 1f;
				if (info.tileCache.WallType == 21)
				{
					num6 = 0.9f;
				}
				else if (info.tileCache.WallType > 0)
				{
					num6 = 0.6f;
				}
			}
			Tile tile = Main.tile[i + 1, j];
			Tile tile2 = Main.tile[i - 1, j];
			Tile tile3 = Main.tile[i, j - 1];
			if (info.tileCache.IsHalfBlock && tile3.LiquidAmount > 0 && info.tileCache.WallType > 0)
			{
				num6 = 0f;
			}
			if (info.tileCache.BottomSlope && ((tile2.LiquidAmount == 0 && !WorldGen.SolidTile(i - 1, j)) || (tile.LiquidAmount == 0 && !WorldGen.SolidTile(i + 1, j))))
			{
				num6 = 0f;
			}
			return num6;
		}
		public static void DrawTile(ref SaveTileData Data, TileDrawInfo info, int i, int j, int h, int k, Vector2 offset, int pass)
		{
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Tile otherTile = Main.tile[i, j];
			Tile myTile = Main.tile[h, k];
			int oType = otherTile.TileType;
			if (pass == 2)
			{
				Data.OntoTile(myTile);
			}
			else if (pass == 1 || pass == 3)
			{
				if (otherTile.LiquidAmount > 0 && pass == 3)
				{
					int liquidType = 0;
					bool water = false;
					switch (otherTile.LiquidType)
					{
						case 0:
							water = true;
							break;
						case 1:
							liquidType = 1;
							break;
						case 2:
							liquidType = 11;
							break;
					}
					if (liquidType == 0)
						liquidType = Main.waterStyle;
					bool flag7 = false;
					float waterAlpha = WaterAlphaMult(info, i, j, liquidType);
					bool isTopLiquid = (otherTile.LiquidAmount != 0 && Main.tile[i, j - 1].LiquidAmount == 0) && (!SOTSWorldgenHelper.TrueTileSolid(i, j - 1, true) || otherTile.LiquidAmount < 250);
					float liquidPercent = otherTile.LiquidAmount / 255f;
					Rectangle frame = isTopLiquid ? new Rectangle(0, 0, 16, (int)(16 * liquidPercent)) : new Rectangle(0, 8, 16, 8);
					float yScale = isTopLiquid ? 1 : 2 * liquidPercent;
					Vector2 origin = isTopLiquid ? new Vector2(0, 0) : new Vector2(0, 8);
					int tileOffset = isTopLiquid ? 16 - frame.Height : 16;
					if (water)
					{
						for (int a = 0; a < 13; a++)
						{
							if (Main.IsLiquidStyleWater(a) && Main.liquidAlpha[a] > 0f && a != liquidType)
							{
								Main.spriteBatch.Draw(TextureAssets.Liquid[a].Value, new Vector2(h * 16, k * 16 + tileOffset) - Main.screenPosition, frame, Color.White * waterAlpha, 0f, origin, new Vector2(1, yScale), 0, 0f);
								flag7 = true;
								break;
							}
						}
					}
					Main.spriteBatch.Draw(TextureAssets.Liquid[liquidType].Value, new Vector2(h * 16, k * 16 + tileOffset) - Main.screenPosition, frame, Color.White * waterAlpha * (flag7 ? Main.liquidAlpha[liquidType] : 1f), 0f, origin, new Vector2(1, yScale), 0, 0f);
				}
				else if (pass != 3 && otherTile.HasTile && !TileID.Sets.IsATreeTrunk[otherTile.TileType] && !TileID.Sets.CountsAsGemTree[otherTile.TileType] && oType != TileID.PalmTree && oType != ModContent.TileType<Items.Furniture.Functional.Hydroponics>() && oType != ModContent.TileType<Items.Otherworld.Furniture.SkyChainTile>())
				{
					if (TileLoader.PreDraw(h, k, oType, Main.spriteBatch))
					{
						Run_DrawSingleTile(Main.instance.TilesRenderer, info, true, -1, unscaledPosition + offset, Vector2.Zero, i, j);
					}
					TileLoader.PostDraw(h, k, oType, Main.spriteBatch);
				}
			}
			else
			{
				if (!TextureAssets.Tile[oType].IsLoaded)
				{
					Main.instance.LoadTiles(oType);
				}
				Data.IntoSave(myTile);
				Data.CopyTileToTile(otherTile, myTile);
			}
		}
		public static void PlaceDownAnomalies()
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
				return;
			int padding = 50;
			Vector2 firstPosition = Vector2.Zero;
			int checks = 0;
			bool valid = false;
			int AttemptedYLayer = padding + (int)(Math.Pow(Main.rand.NextFloat(1), 4) * (Main.maxTilesY - padding)); //Weighted towards the top of the map
			while (checks < 160 && !valid)
			{
				int randX = Main.rand.Next(padding, Main.maxTilesX - padding);
				int randY = (int)MathHelper.Lerp(AttemptedYLayer, Main.rand.Next(padding, Main.maxTilesY / 2), Math.Clamp(checks / 120f, 0, 1));//Weighted towards the top of the map
				firstPosition = new Vector2(randX * 16 + 8, randY * 16 + 8);
				valid = isThisPlacementValid(new Point(randX, randY));
				checks++;
			}
			Projectile.NewProjectile(new EntitySource_Misc("SOTS:ArchaeologistPortals"), firstPosition, Vector2.Zero, ModContent.ProjectileType<VoidAnomaly>(), 0, 0, Main.myPlayer, -1, -60);
			Vector2 secondPosition = Vector2.Zero;
			checks = 0;
			valid = false;
			AttemptedYLayer = Main.rand.Next(padding, Main.maxTilesY - padding);
			while (checks < 160 && !valid)
			{
				int randX = Main.rand.Next(padding, Main.maxTilesX - padding);
				int randY = (int)MathHelper.Lerp(AttemptedYLayer, Main.rand.Next(padding, Main.maxTilesY - padding), Math.Clamp(checks / 120f, 0, 1));
				secondPosition = new Vector2(randX * 16 + 8, randY * 16 + 8);
				if (Vector2.Distance(secondPosition, firstPosition) < 6400)
					valid = false; //Not a valid spot unless the distances are far from each other
				else
					valid = isThisPlacementValid(new Point(randX, randY));
				checks++;
			}
			Projectile.NewProjectile(new EntitySource_Misc("SOTS:ArchaeologistPortals"), secondPosition, Vector2.Zero, ModContent.ProjectileType<VoidAnomaly>(), 0, 0, Main.myPlayer, -2, -60);
		}
		public static bool isThisPlacementValid(Point point)
		{
			bool isAtLeastOneTileBelow = false;
			int countOfValid = 0;
			for (int j = -1; j <= 3; j++)
			{
				for (int i = -2; i <= 2; i++)
				{
					if (j <= 1 && i == 0)
						if (SOTSWorldgenHelper.TrueTileSolid(point.X + i, point.Y + j))
						{
							return false;
						}
						else
							countOfValid++;
					else if (SOTSWorldgenHelper.TrueTileSolid(point.X + i, point.Y + j))
					{
						isAtLeastOneTileBelow = true;
						if (countOfValid >= 3)
						{
							return true;
						}
					}
				}
			}
			// x x o x x
			// x x o x x
			// x x o x x
			// x x x x x
			// x x x x x
			// Looks for a tile in X and a lack of tile on O
			if (isAtLeastOneTileBelow)
			{
				return true;
			}
			return false;
		}
		public static void KillOtherAnomalies()
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
				return;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.active && proj.type == ModContent.ProjectileType<VoidAnomaly>())
				{
					proj.ai[0] = -3;
					proj.netUpdate = true;
				}
			}
		}
		public static void PrepareLocalPlayerShader()
		{
			Player localPlayer = Main.LocalPlayer;
			bool foundAnAnomaly = false;
			float alphaMult = 0f;
			Vector2 position = Vector2.Zero;
			float maxDist = 1280f;
			float dist = maxDist;
			for (int i = 0; i < 1000; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.active && proj.type == ModContent.ProjectileType<VoidAnomaly>())
				{
					if (proj.ModProjectile is VoidAnomaly vAnom)
					{
						Vector2 toCenter = proj.Center - localPlayer.Center;
						if (toCenter.Length() < dist)
						{
							dist = toCenter.Length();
							alphaMult = vAnom.alphaMult;
							foundAnAnomaly = true;
							position = proj.Center;
						}
					}
				}
			}
			if (foundAnAnomaly)
			{
				float percent = (maxDist - dist) / maxDist;
				percent *= alphaMult;
				percent = (float)Math.Pow(percent, 4);
				AnomalyShaderPosition = position;
				if (AnomalyShaderProgress < percent)
				{
					AnomalyShaderProgress = MathHelper.Lerp(AnomalyShaderProgress, percent, 0.4f);
				}
				else
				{
					AnomalyShaderProgress = MathHelper.Lerp(AnomalyShaderProgress, percent, 0.01f);
				}
				AnomalyIntesity = AnomalyShaderProgress;
			}
			else
			{
				if (Vector2.Distance(AnomalyShaderPosition, localPlayer.Center) > maxDist * 3)
				{
					AnomalyShaderPosition = Vector2.Zero;
					AnomalyShaderProgress = 0f;
					AnomalyIntesity = 0f;
				}
				else
				{
					AnomalyShaderProgress = MathHelper.Lerp(AnomalyShaderProgress, 0, 0.045f);
					AnomalyIntesity = MathHelper.Lerp(AnomalyIntesity, 0, 0.045f);
					if (AnomalyShaderProgress <= 0.001f && AnomalyIntesity <= 0.001f)
					{
						AnomalyShaderPosition = Vector2.Zero;
						AnomalyShaderProgress = 0f;
						AnomalyIntesity = 0f;
					}
				}
			}
		}
		public static Vector2 AnomalyShaderPosition = Vector2.Zero;
		public static float AnomalyShaderProgress = 0f;
		public static float APortalIsAccepting = 0f;
		public static float AnomalyIntesity = 0f;
		public const int CloseToSize = 20;
		public const int Radius = 6; 
		public float alphaBarrier = 0;
		private bool canIMove = false;
		private SaveTileData[] Data = null;
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(APortalIsAccepting);
			writer.Write(hasGrownToFull);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			APortalIsAccepting = reader.ReadSingle();
			hasGrownToFull = reader.ReadBoolean();
		}
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			behindNPCs.Add(index);
		}
		public void DrawTilesFromOtherPortal(float currentRadius)
		{
			TileDrawInfo value = RunGet_currentTileDrawInfo(Main.instance.TilesRenderer);
			int x = (int)(positionOfOtherPortal.X / 16);
			int y = (int)(positionOfOtherPortal.Y / 16);
			int x2 = (int)(Projectile.Center.X / 16);
			int y2 = (int)(Projectile.Center.Y / 16);
			Vector2 offset = positionOfOtherPortal - Projectile.Center;
			int maxSize = (int)Math.Pow((Radius * 2 + 1), 2);
			if (Data == null)
				Data = new SaveTileData[maxSize];
			if (WorldGen.InWorld(x, y, 40))
			{
				if (WorldGen.InWorld(x2, y2, 40))
				{
					Main.drawToScreen = true;
					Main.gameMenu = true;
					canIMove = false;
					for (int k = 0; k < 4; k++)
					{
						int currentIndex = 0;
						for (int j = Radius; j >= -Radius; j--)
						{
							for (int i = -Radius; i <= Radius; i++)
							{
								float dist = i * i + j * j;
								if (dist < currentRadius * currentRadius)
								{
									if (k == 0)
									{
										DrawTile(ref Data[currentIndex], value, x + i, y + j, x2 + i, y2 + j, offset, 0);
										DrawWall(ref Data[currentIndex], x + i, y + j, x2 + i, y2 + j, offset, 0);
									}
									if (k == 1)
									{
										DrawWall(ref Data[currentIndex], x + i, y + j, x2 + i, y2 + j, offset, 1);
										DrawTile(ref Data[currentIndex], value, x + i, y + j, x2 + i, y2 + j, offset, 3);
									}
									if (k == 2)
									{
										DrawTile(ref Data[currentIndex], value, x + i, y + j, x2 + i, y2 + j, offset, 1);
									}
									if (k == 3)
									{
										DrawTile(ref Data[currentIndex], value, x + i, y + j, x2 + i, y2 + j, offset, 2);
										DrawWall(ref Data[currentIndex], x + i, y + j, x2 + i, y2 + j, offset, 2);
									}
								}
								currentIndex++;
							}
						}
					}
					canIMove = true;
					Main.gameMenu = false;
					Main.drawToScreen = false;
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			DrawWaves();
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 drawPos = Projectile.Center;
			for (int j = 1; j <= 4; j++)
				for (int i = -1; i <= 1; i += 2)
				{
					float rotation = j * MathHelper.PiOver4 / 2f;
					Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, Color.Black, i * Projectile.rotation + rotation, texture.Size() / 2, 0.25f * (Projectile.ai[1] > 0 ? 1 : 0) + 2.25f * alphaMult, i == -1 ? SpriteEffects.FlipHorizontally : 0, 0f);
				}
			return false;
		}
		public void DrawWaves()
		{
			float currentRadius = Radius * alphaMult;
			Texture2D auraTexture = ModContent.Request<Texture2D>("SOTS/Common/GlobalNPCs/FreezeSpiral3").Value;
			Color color = new Color(80, 35, 120, 0) * 2f;
			float size = (currentRadius * 32f + 120) / auraTexture.Width;
			for (int i = 0; i < 6; i++)
			{
				Main.spriteBatch.Draw(auraTexture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, color * alphaMult, Projectile.rotation * (i % 2 * 2 - 1) + i * MathHelper.Pi / 3f, auraTexture.Size() / 2, size, (SpriteEffects)(i % 2), 0f);
			}
		}
		public void DrawBarrier(Color barrierColor, ref float bonusWidth, ref float AlphaPercent, bool draw = true)
		{
			float sinusoidalBonus = 2 * (float)Math.Sin(MathHelper.ToRadians(bonusWidth + SOTSWorld.GlobalCounter));
			if (AlphaPercent <= 0.05f)
            {
				return;
            }
			if (draw)
			{
				barrierColor.A = (byte)(barrierColor.A * 0.5f);
				float barrierWidth = 256;
				float scaleMult = (bonusWidth + sinusoidalBonus) / barrierWidth;
				Texture2D BarrierTexture = TextureAssets.Extra[195].Value;
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				SOTS.BarrierShader.Parameters["size"].SetValue(scaleMult);
				SOTS.BarrierShader.Parameters["pixelSize"].SetValue(12);
				SOTS.BarrierShader.CurrentTechnique.Passes[0].Apply();
				Main.spriteBatch.Draw(BarrierTexture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, barrierColor * (AlphaPercent - 0.05f), Projectile.rotation, BarrierTexture.Size() / 2f, scaleMult, 0, 0f);
				SOTS.BarrierShader.Parameters["size"].SetValue(scaleMult);
				SOTS.BarrierShader.Parameters["pixelSize"].SetValue(6);
				SOTS.BarrierShader.CurrentTechnique.Passes[0].Apply();
				Main.spriteBatch.Draw(BarrierTexture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, Color.Lerp(new Color(210, 202, 222, 0), barrierColor, 0.2f) * (AlphaPercent - 0.05f), Projectile.rotation, BarrierTexture.Size() / 2f, scaleMult, 0, 0f);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			}
			AlphaPercent *= 0.9f;
			AlphaPercent -= 0.06f;
			bonusWidth += 18 + sinusoidalBonus;
		}
		public override void PostDraw(Color lightColor)
		{
			float currentRadius = Radius * alphaMult;
			if ((Projectile.ai[0] == -1 || Projectile.ai[0] == -2) && Projectile.ai[1] > 8)
			{
				DrawTilesFromOtherPortal(currentRadius);
			}
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 drawPos = Projectile.Center;
			Color color = Color.Lerp(new Color(160, 120, 180, 0), Color.Black, 0.15f + 0.85f * (1 - alphaMult));
			for (int k = 0; k < 2; k++)
				for (int j = 1; j <= 2; j++)
					for (int i = -1; i <= 1; i += 2)
					{
						float rotation = j * MathHelper.PiOver4;
						Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, color * 0.125f * j, i * Projectile.rotation * -1 + rotation, texture.Size() / 2, (1.75f + j * 0.5f + k * 0.25f) * alphaMult, i == -1 ? SpriteEffects.FlipHorizontally : 0, 0f);
					}
			Texture2D borderTexture = ModContent.Request<Texture2D>("SOTS/NPCs/Town/VoidAnomalyBorder").Value;
			Texture2D borderTextureG = ModContent.Request<Texture2D>("SOTS/NPCs/Town/VoidAnomalyBorderGlow").Value;
			Vector2 origin = borderTexture.Size() / 2;
			int direction = 1;
			for (int j = 0; j < 5; j++)
			{
				float total = 35;
				color = Color.Lerp(ColorHelpers.VoidAnomaly, Color.Black, MathHelper.Lerp(0.1f, 1f, 1 - alphaMult));
				float sizeM = 0.75f;
				if (j == 2)
				{
					sizeM = 1.5f;
					color = Color.Lerp(ColorHelpers.VoidAnomaly, Color.Black, MathHelper.Lerp(0.56f, 1f, 1 - alphaMult));
					total = 40;
				}
				if (j == 3)
				{
					total = 45;
					sizeM = 2.3f;
					color = Color.Lerp(ColorHelpers.VoidAnomaly, Color.Black, MathHelper.Lerp(0.425f, 1f, 1 - alphaMult));
				}
				if (j == 4 || j == 0)
				{
					total = 50;
					sizeM = 3f;
					color = Color.Lerp(ColorHelpers.VoidAnomaly, Color.Black, MathHelper.Lerp(0.21f, 1f, 1 - alphaMult));
				}
				total = 30 + (total - 30) * alphaMult;
				float sinusoid = (float)(Math.Sin(MathHelper.ToRadians(SOTSWorld.GlobalCounter + sizeM * 22.5f)));
				for (int i = 0; i < total; i++)
				{
					float rotation = i / total * MathHelper.TwoPi + Projectile.rotation * ((float)Math.Sqrt(j) / 2f + 0.5f) * direction * 0.2f;
					Vector2 circular = new Vector2((currentRadius + sizeM * 0.625f) * 16 + 7f / (sizeM + 1) * sinusoid * alphaMult - 20, 0).RotatedBy(rotation);
					if (j == 0)
					{
						if (alphaMult > 0)
						{
							Main.spriteBatch.Draw(borderTextureG, drawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + circular, null, new Color(160, 120, 180, 0), rotation + MathHelper.PiOver2, origin, new Vector2(1.65f, 1.0f * alphaMultRoot), SpriteEffects.None, 0f);
							if (!SOTS.Config.lowFidelityMode)
							{
								Main.spriteBatch.Draw(borderTextureG, drawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + circular, null, new Color(140, 100, 160, 0) * 0.4f, rotation + MathHelper.PiOver2, origin, new Vector2(2.45f, 1.1f * alphaMultRoot), SpriteEffects.None, 0f);
								Main.spriteBatch.Draw(borderTextureG, drawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + circular, null, new Color(120, 80, 140, 0) * 0.15f, rotation + MathHelper.PiOver2, origin, new Vector2(3.05f, 1.2f * alphaMultRoot), SpriteEffects.None, 0f);
							}
						}
					}
					else
					{
						float verticalSize = 1f;
						if (j == 4)
							verticalSize = 0.65f;
						Main.spriteBatch.Draw(borderTexture, drawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + circular, null, color, rotation + MathHelper.PiOver2, origin, new Vector2(0.25f + 0.75f * alphaMult, verticalSize * alphaMultRoot), SpriteEffects.None, 0f);
					}
				}
				direction *= -1;
			}
			float barrierSize = Radius * 16 * alphaMult * 2 + 80;
			float sinusoidalBonus = 2 * (float)Math.Sin(MathHelper.ToRadians(barrierSize + SOTSWorld.GlobalCounter));
			barrierSize += sinusoidalBonus;
			float alphaMultBarrier = alphaBarrier * 1.05f * Projectile.timeLeft / 120f;
			if (!SOTSWorld.DiamondKeySlotted)
				DrawBarrier(ColorHelpers.DiamondColor, ref barrierSize, ref alphaMultBarrier);
			if (!SOTSWorld.RubyKeySlotted)
				DrawBarrier(ColorHelpers.RubyColor, ref barrierSize, ref alphaMultBarrier);
			if (!SOTSWorld.EmeraldKeySlotted)
				DrawBarrier(ColorHelpers.EmeraldColor, ref barrierSize, ref alphaMultBarrier);
			if (!SOTSWorld.SapphireKeySlotted)
				DrawBarrier(ColorHelpers.SapphireColor, ref barrierSize, ref alphaMultBarrier);
			if (!SOTSWorld.TopazKeySlotted)
				DrawBarrier(ColorHelpers.TopazColor, ref barrierSize, ref alphaMultBarrier);
			if (!SOTSWorld.AmethystKeySlotted)
				DrawBarrier(ColorHelpers.AmethystColor, ref barrierSize, ref alphaMultBarrier);
			if (!SOTSWorld.AmberKeySlotted)
				DrawBarrier(ColorHelpers.AmberColor, ref barrierSize, ref alphaMultBarrier);
		}
		public override void SetDefaults()
		{
			Projectile.height = 82;
			Projectile.width = 82;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.timeLeft = 100000;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.hide = true;
			Projectile.netImportant = true;
		}
		public float alphaMultRoot
		{
			get
			{
				float mult = (Projectile.ai[1] / 60f);
				if (mult < 0)
					return 0;
				else
					return (float)Math.Sqrt(mult);
			}
		}
		public float alphaMult
		{
			get
			{
				float mult = (Projectile.ai[1] / 60f);
				if (mult < 0)
					return 0;
				else
					return (float)Math.Pow(mult, 2);
			}
		}
		public Vector2 positionOfOtherPortal
		{
			get
			{
				if (Projectile.ai[0] == -1)
					return Archaeologist.AnomalyPosition3;
				if (Projectile.ai[0] == -2)
					return Archaeologist.AnomalyPosition2;
				return Vector2.Zero;
			}
		}
		public bool hasGrownToFull = false;
		public float growthTotal = 0f;
		private bool runOnce = true;
		public void FramePortalBlocks()
        {
			int i = (int)Projectile.Center.X / 16;
			int j = (int)Projectile.Center.Y / 16;
			int startX = i - 8;
			int startY = j - 8;
			int endX = i + 8;
			int endY = j + 8;
			WorldGen.SectionTileFrameWithCheck(startX, startY, endX, endY);
		}
        public override void AI()
        {
			Update();
        }
        public void Update()
		{
			Color color = ColorHelpers.VoidAnomaly;
			color.A = 0;
			if (Main.netMode == NetmodeID.Server && SOTSWorld.GlobalCounter % 10 == 0)
			{
				for (int k = 0; k < 255; k++)
				{
					if (Main.player[k].active)
					{
						RemoteClient.CheckSection(k, Projectile.Center);
					}
				}
				if (runOnce)
				{
					Projectile.netUpdate = true;
					runOnce = false;
				}
			}
			else if(Main.netMode == NetmodeID.SinglePlayer)
			{
				if (runOnce)
				{
					FramePortalBlocks();
					runOnce = false;
				}
			}
			if (alphaBarrier < 1)
				alphaBarrier += 1f / 240f;
			if (alphaBarrier > 1)
				alphaBarrier = 1;
			if (APortalIsAccepting > 0)
			{
				if (Projectile.ai[1] > CloseToSize)
				{
					Projectile.ai[1]--;
					Projectile.ai[1] *= 0.85f;
				}
				if (Projectile.ai[1] < CloseToSize)
					Projectile.ai[1] = CloseToSize;
				if (Main.netMode == NetmodeID.Server)
					Projectile.netUpdate = true;
			}
			else if (Projectile.ai[1] < 60 && Projectile.ai[0] != -3)
			{
				float growthMult = 1f;
				if (hasGrownToFull)
					growthMult = 0.05f;
				if (Projectile.ai[1] < 8)
				{
					if (Projectile.ai[1] >= 0)
					{
						if (Projectile.ai[1] == 0)
							Projectile.ai[1] = 6;
						for (int i = 0; i < 32; i++)
						{
							float radius = Radius * 16 * alphaMult;
							Vector2 circular = new Vector2(radius + 12 + Main.rand.NextFloat(8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
							Dust dust = Dust.NewDustDirect(Projectile.Center + circular - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, Color.Lerp(color, Color.Black, Main.rand.NextFloat(0.3f)), 1.1f);
							dust.fadeIn = 1;
							dust.noGravity = true;
							dust.velocity *= 0.5f;
							dust.velocity += circular.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(5);
						}
					}
					Projectile.ai[1]++;
					if (Projectile.ai[1] == 8)
					{
						SOTSUtils.PlaySound(SoundID.Item78, Projectile.Center, 1.5f, -0.8f, 0.1f);
					}
				}
				else if (Projectile.ai[1] < 8 + (float)(Math.Pow(totalKeysSlotted / 7f, 0.4f) * 52))
                {
					if (Main.netMode == NetmodeID.Server)
						Projectile.netUpdate = true;
					Projectile.ai[1] += growthMult;
				}
				if (Projectile.ai[1] >= 60)
				{
					if (Main.netMode == NetmodeID.Server && SOTSWorld.GlobalCounter % 150 == 0)
						Projectile.netUpdate = true;
					Projectile.ai[1] = 60;
					if(!hasGrownToFull)
					{
						hasGrownToFull = true;
						if (Main.netMode == NetmodeID.Server)
							Projectile.netUpdate = true;
					}
				}
				growthTotal = Projectile.ai[1];
			}
			if (Projectile.ai[0] == -1)
			{
				if (Projectile.ai[1] >= 0 && Projectile.timeLeft >= 119)
					Archaeologist.AnomalyPosition2 = Projectile.Center;
			}
			if (Projectile.ai[0] == -2)
			{
				if (Projectile.ai[1] >= 0 && Projectile.timeLeft >= 119)
					Archaeologist.AnomalyPosition3 = Projectile.Center;
			}
			if (Projectile.ai[0] == -3) // start dying
			{
				if (Main.netMode == NetmodeID.Server)
					Projectile.netUpdate = true;
				if (Projectile.timeLeft > 120)
					Projectile.timeLeft = 120;
				Projectile.ai[1] = MathHelper.Lerp(growthTotal, 7, 1 - Projectile.timeLeft / 120f);
			}
			else
				Projectile.timeLeft = 120;
			Projectile.alpha = (int)(255 * (1 - alphaMult));
			Projectile.rotation += MathHelper.ToRadians(3.5f);
			float count = Main.rand.NextFloat(4) * alphaMult + 1;
			for (int i = 0; i < count; i++)
			{
				float radius = Radius * 16 * alphaMult;
				Vector2 circular = new Vector2(radius + 16 + Main.rand.NextFloat(8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
				Dust dust = Dust.NewDustDirect(Projectile.Center + circular - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, color * (0.5f + 0.5f * alphaMult), 1);
				dust.fadeIn = 8;
				dust.velocity *= 0.3f;
				dust.velocity += circular.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(2) * (2 * (alphaMult - 0.5f));
			}
			EntityCollision();
		}
		public float GetBarrierWidth()
		{
			float point = 1.05f * alphaBarrier * Projectile.timeLeft / 120f;
			float barrierSize = Radius * 16 * alphaMult * 2 + 80;
			float sinusoidalBonus = 2 * (float)Math.Sin(MathHelper.ToRadians(barrierSize + SOTSWorld.GlobalCounter));
			barrierSize += sinusoidalBonus;
			if (!SOTSWorld.DiamondKeySlotted)
				DrawBarrier(ColorHelpers.DiamondColor, ref barrierSize, ref point, false);
			if (!SOTSWorld.RubyKeySlotted)
				DrawBarrier(ColorHelpers.RubyColor, ref barrierSize, ref point, false);
			if (!SOTSWorld.EmeraldKeySlotted)
				DrawBarrier(ColorHelpers.EmeraldColor, ref barrierSize, ref point, false);
			if (!SOTSWorld.SapphireKeySlotted)
				DrawBarrier(ColorHelpers.SapphireColor, ref barrierSize, ref point, false);
			if (!SOTSWorld.TopazKeySlotted)
				DrawBarrier(ColorHelpers.TopazColor, ref barrierSize, ref point, false);
			if (!SOTSWorld.AmethystKeySlotted)
				DrawBarrier(ColorHelpers.AmethystColor, ref barrierSize, ref point, false);
			if (!SOTSWorld.AmberKeySlotted)
				DrawBarrier(ColorHelpers.AmberColor, ref barrierSize, ref point, false);
			return barrierSize / 2;
		}
		public int totalKeysSlotted
		{
			get
			{
				return SOTSWorld.DiamondKeySlotted.ToInt() + SOTSWorld.RubyKeySlotted.ToInt() + SOTSWorld.EmeraldKeySlotted.ToInt() + SOTSWorld.SapphireKeySlotted.ToInt() + SOTSWorld.TopazKeySlotted.ToInt() + SOTSWorld.AmethystKeySlotted.ToInt() + SOTSWorld.AmberKeySlotted.ToInt();
			}
		}
		public void EntityCollision()
		{
			bool allKeysSlotted = false;
			if (totalKeysSlotted >= 7)
				allKeysSlotted = true;
			float barrier = GetBarrierWidth();
			if (allKeysSlotted)
				barrier = -1;
			for (int i = 0; i < Main.maxItems; i++)
			{
				Item item = Main.item[i];
				if (item.active)
				{
					GlobalEntityItem gen = item.GetGlobalItem<GlobalEntityItem>();
					if (!gen.RecentlyTeleported)
					{
						if (barrier == -1)
							AcceptEntity(item, i);
						else
							RejectEntity(item, barrier);
					}
				}
				if (i < Main.maxNPCs)
				{
					NPC npc = Main.npc[i];
					if (npc.active && !npc.noTileCollide && !npc.boss)
					{
						GlobalEntityNPC gen = npc.GetGlobalNPC<GlobalEntityNPC>();
						if (!gen.RecentlyTeleported)
						{
							if (barrier == -1)
								AcceptEntity(npc, i);
							else
								RejectEntity(npc, barrier);
						}
					}
				}
				if (i < Main.maxPlayers)
				{
					Player p = Main.player[i];
					if (p.active)
					{
						if (!p.HasBuff(ModContent.BuffType<Skipped>()))
						{
							if (barrier == -1)
								AcceptEntity(p, i);
							else
								RejectEntity(p, barrier);
						}
					}
				}
			}
		}
		public void TeleportEntity(Entity entity, int whoAmI)
		{
			if (entity is NPC npc)
			{
				GlobalEntityNPC gen = npc.GetGlobalNPC<GlobalEntityNPC>();
				if (gen.RecentlyTeleported)
				{
					return;
				}
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					gen.RecentlyTeleported = true;
					Projectile.NewProjectile(new EntitySource_Misc("SOTS:VoidAnomaly"), npc.Center, Vector2.Zero, ModContent.ProjectileType<PortalDustProjectile>(), 0, 0, Main.myPlayer, whoAmI, 1);
					entity.Center = positionOfOtherPortal;
					entity.velocity *= 0.5f;
					entity.velocity += new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f));
					if (Main.netMode == NetmodeID.Server)
					{
						GlobalEntity.SendServerChanges(Mod, npc, true, whoAmI); //Packet must be sent after center is modified to send the correct position
						npc.netUpdate = true;
					}
				}
			}
			else
			{
				if (entity is Item item)
				{
					GlobalEntityItem gen = item.GetGlobalItem<GlobalEntityItem>();
					if (gen.RecentlyTeleported)
					{
						return;
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						gen.TeleportCounter = 1;
						Projectile.NewProjectile(new EntitySource_Misc("SOTS:VoidAnomaly"), item.Center, Vector2.Zero, ModContent.ProjectileType<PortalDustProjectile>(), 0, 0, Main.myPlayer, whoAmI, -1);
						entity.Center = positionOfOtherPortal;
						entity.velocity *= 0.5f;
						entity.velocity += new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f));
						if (Main.netMode == NetmodeID.Server)
						{
							gen.NetUpdate(whoAmI);
						}
					}
				}
				else if (entity is Player player)
				{
					player.AddBuff(ModContent.BuffType<Skipped>(), 300);
					if (Main.netMode != NetmodeID.Server)
						DustCircle(player.Center, player.width * 0.5f, player.height * 0.5f);
					entity.Center = positionOfOtherPortal;
					entity.velocity *= 0.5f;
					entity.velocity += new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f));
				}
			}
		}
		public void AcceptEntity(Entity entity, int whoAmI)
		{
			Vector2 center = Projectile.Center;
			Vector2 toCenter = center - entity.Center;
			float dist = toCenter.Length();
			float vacuumRadius = 640f;
			float acceptRadius = 24;
			if(dist < 12)
            {
				if(APortalIsAccepting <= 0 && Projectile.ai[1] >= 60)
					APortalIsAccepting += 40;
			}
			if (dist < acceptRadius)
			{
				if (Projectile.ai[1] <= CloseToSize && APortalIsAccepting != 0)
				{
					TeleportEntity(entity, whoAmI);
				}
			}
			bool valid = true;
			if(entity is Player p)
            {
				if (SOTSPlayer.ModPlayer(p).normalizedGravity)
					valid = false;
			}
			if (dist < vacuumRadius && valid)
			{
				//if (SOTSWorld.GlobalCounter % 10 == 0 && entity is Item item)
				//{
				//	GlobalEntityItem gen = item.GetGlobalItem<GlobalEntityItem>();
				//	if (Main.netMode != NetmodeID.Server)
				//		Main.NewText(whoAmI + "-" + gen.RecentlyTeleported + ": " + gen.TeleportCounter);
				//	if (Main.netMode == NetmodeID.Server)
				//		WorldGen.BroadcastText(NetworkText.FromLiteral(whoAmI + "-" + gen.RecentlyTeleported + ": " + gen.TeleportCounter), Color.Red);
				//}
				float distancePercent = (vacuumRadius - dist) / vacuumRadius * (APortalIsAccepting > 0 ? 1 : alphaMult);
				Vector2 outward = toCenter.SafeNormalize(Vector2.Zero) * (float)Math.Pow(distancePercent, 3) * 0.625f;
				Vector2 pullVelocity = Vector2.Zero;
				if (entity.velocity.Y != 0 || entity is Item || dist < 80)
				{
					entity.velocity.X *= 1 - 0.3f * distancePercent;
					entity.velocity.X += outward.X * 2.5f;
					pullVelocity.X += outward.X * 3f;
					if (outward.Y < 0)
					{
						if (entity.velocity.Y > 0)
							entity.velocity.Y *= 1f - distancePercent;
						entity.velocity.Y += outward.Y * 1.375f * distancePercent;
                    }
                }
				else
				{
					entity.velocity.X *= 1 - 0.1f * distancePercent;
					entity.velocity.X += outward.X * 1f;
					pullVelocity.X += outward.X * 6f;
				}
				float lengthForSuperSuck = 96f * alphaMultRoot;
				if(dist < lengthForSuperSuck)
                {
					entity.velocity = Vector2.Lerp(entity.velocity, toCenter * 0.1f * alphaMult, 0.5f * (lengthForSuperSuck - dist) / lengthForSuperSuck * alphaMult);
                }
				if(dist > 32)
				{
					pullVelocity = Collision.TileCollision(entity.position, pullVelocity, entity.width, entity.height, false, false, 1);
					entity.Center = entity.Center + pullVelocity;
					for (int i = 0; i < 6 * alphaMult; i++)
					{
						Vector2 pos = entity.position;
						int height = entity.height;
						float veloMultY = 1f;
						if(Math.Abs(entity.velocity.Y) <= 0.05f && i <= 1)
                        {
							pos.Y += entity.height + 2;
							height = 10;
							veloMultY = 0.1f;
                        }
						if (Main.rand.NextBool(2 + (int)Math.Sqrt(dist)))
						{
							int type = ModContent.DustType<PixelDust>();
							if(Main.rand.NextBool())
                            {
								type = ModContent.DustType<CopyDust4>();
                            }
							Dust dust = Dust.NewDustDirect(pos - new Vector2(3, 5), entity.width + 3, height, type, 0, 0, 0, Color.Lerp(ColorHelpers.VoidAnomaly, Color.Black, Main.rand.NextFloat(0.3f)), type == ModContent.DustType<CopyDust4>() ? 1.3f : 1.0f);
							dust.fadeIn = type == ModContent.DustType<CopyDust4>() ? 0.2f : 7;
							dust.noGravity = true;
							dust.velocity *= Main.rand.NextFloat(1) * Main.rand.NextFloat(1) * 0.3f;
							dust.velocity += toCenter.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(4f, 5f) * distancePercent * dist / 120f;
							dust.velocity *= veloMultY;
							dust.color.A = (byte)Main.rand.Next(200);
						}
					}
				}
            }
		}
		public void RejectEntity(Entity entity, float barrierSize)
        {
			Vector2 awayFromCenter = entity.Center - Projectile.Center;
			float dist = awayFromCenter.Length();
			Vector2 velocity = awayFromCenter.SafeNormalize(Vector2.Zero) * barrierSize;
			Vector2 edgeOfCircle = Projectile.Center + velocity;
			if (dist < barrierSize || entity.Hitbox.Contains(new Point((int)edgeOfCircle.X, (int)edgeOfCircle.Y)))
			{
				Vector2 edgeToCenter = (entity.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
				velocity += new Vector2(entity.Hitbox.Width * edgeToCenter.X, entity.Hitbox.Height * edgeToCenter.Y) * 0.5f;
				Vector2 finalPos = Projectile.Center + velocity;
				if (Math.Abs(awayFromCenter.X) >= Math.Abs(velocity.X))
				{
					finalPos.X = entity.Center.X;
				}
				finalPos.X += Math.Sign(velocity.X);
				finalPos.Y = entity.Center.Y - awayFromCenter.Y + velocity.Y;
				Vector2 toFinal = finalPos - entity.Center;
				toFinal = Collision.TileCollision(entity.position, toFinal, entity.width, entity.height, false, false, 1);
				entity.Center = entity.Center + toFinal;
            }
        }
        public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 32; i++)
			{
				float radius = Radius * 16 * alphaMult;
				Vector2 circular = new Vector2(radius + 12 + Main.rand.NextFloat(8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
				Dust dust = Dust.NewDustDirect(Projectile.Center + circular * 0.5f - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, Color.Lerp(ColorHelpers.VoidAnomaly, Color.Black, Main.rand.NextFloat(0.5f)), 1.5f);
				dust.fadeIn = 0.4f;
				dust.noGravity = true;
				dust.velocity *= 0.5f;
				dust.velocity += circular.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(0.4f, 3);
				dust.color.A = (byte)Main.rand.Next(200);
			}
			Color c = ColorHelpers.VoidAnomaly;
			c.A = 0;
			for (int i = 0; i < 48; i++)
			{
				float radius = Radius * 16 * alphaMult;
				Vector2 circular = new Vector2(radius + 14, 0).RotatedBy(i * MathHelper.TwoPi / 48f);
				Dust dust = Dust.NewDustDirect(Projectile.Center + circular - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, Color.Lerp(c, Color.Black, Main.rand.NextFloat(0.6f, 1f)), 1.0f);
				dust.fadeIn = 6;
				dust.noGravity = true;
				dust.velocity *= 0.6f;
				dust.velocity += circular.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(0.5f, 1.5f);
			}
			SOTSUtils.PlaySound(SoundID.Item74, Projectile.Center, 1.5f, -0.4f, 0.1f);
		}
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
	public static class PortalDrawingHelper
	{
		public static void DustCircle(Vector2 center, float radiusX, float radiusY)
		{
			SOTSUtils.PlaySound(SoundID.Item117, center, 1.5f, -0.8f, 0.1f);
			Color color = ColorHelpers.VoidAnomaly;
			for (int j = 0; j < 3; j++)
			{
				int type = ModContent.DustType<CopyDust4>();
				if (j == 1)
					type = ModContent.DustType<PixelDust>();
				int count = 10 + (int)Math.Sqrt(radiusX * radiusX + radiusY * radiusY);
				for (int i = 0; i < count; i++)
				{
					Vector2 circular = new Vector2(Main.rand.NextFloat(1.2f, 1.5f), 0).RotatedBy(i * MathHelper.TwoPi / (float)count + j);
					circular.X *= radiusX;
					circular.Y *= radiusY;
					Dust dust = Dust.NewDustDirect(center + circular - new Vector2(5, 5), 0, 0, type, 0, 0, 0, Color.Lerp(color, Color.Black, Main.rand.NextFloat(0.3f)), j == 0 ? 2.2f : 1.5f);
					dust.fadeIn = 1 + (j == 1 ? 5 : 0);
					dust.noGravity = true;
					dust.velocity *= 0.5f;
					dust.velocity += circular.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(3, 4) * (j * 0.25f + 0.75f);
				}
			}
		}
		public struct SaveTileData
		{
			bool saveActive;
			ushort saveTileType;
			short saveTileFrameX;
			short saveTileFrameY;
			byte saveTileColor;
			ushort saveWallType;
			int saveWallFrameX;
			int saveWallFrameY;
			byte saveWallColor;
			byte saveLiquid;
			int saveLiquidT;
			public void IntoSaveW(Tile data)
			{
				saveWallType = data.WallType;
				saveWallFrameX = data.WallFrameX;
				saveWallFrameY = data.WallFrameY;
				saveWallColor = data.WallColor;
			}
			public void OntoTileW(Tile data)
			{
				data.WallType = saveWallType;
				data.WallFrameX = saveWallFrameX;
				data.WallFrameY = saveWallFrameY;
				data.WallColor = saveWallColor;
			}
			public void CopyTileToTileW(Tile CopyFrom, Tile CopyOnto)
			{
				CopyOnto.WallType = CopyFrom.WallType;
				CopyOnto.WallFrameX = CopyFrom.WallFrameX;
				CopyOnto.WallFrameY = CopyFrom.WallFrameY;
				CopyOnto.WallColor = CopyFrom.WallColor;
			}
			public void IntoSave(Tile data)
			{
				saveActive = data.HasTile;
				saveTileType = data.TileType;
				saveTileFrameX = data.TileFrameX;
				saveTileFrameY = data.TileFrameY;
				saveTileColor = data.TileColor;
				saveLiquid = data.LiquidAmount;
				saveLiquidT = data.LiquidType;
			}
			public void OntoTile(Tile data)
            {
				data.HasTile = saveActive;
				data.TileType = saveTileType;
				data.TileFrameX = saveTileFrameX;
				data.TileFrameY = saveTileFrameY;
				data.TileColor = saveTileColor;
				data.LiquidAmount = saveLiquid;
				data.LiquidType = saveLiquidT;
			}
			public void CopyTileToTile(Tile CopyFrom, Tile CopyOnto)
			{
				CopyOnto.HasTile = CopyFrom.HasTile;
				CopyOnto.TileType = CopyFrom.TileType;
				CopyOnto.TileFrameX = CopyFrom.TileFrameX;
				CopyOnto.TileFrameY = CopyFrom.TileFrameY;
				CopyOnto.TileColor = CopyFrom.TileColor;
				CopyOnto.LiquidAmount = CopyFrom.LiquidAmount;
				CopyOnto.LiquidType = CopyFrom.LiquidType;
			}
        }
		public static Texture2D Run_GetTileDrawTexture(WallDrawing wDrawer, Tile tile, int tileX, int tileY)
		{
			Type type = wDrawer.GetType();
			MethodInfo method = type.GetMethod("GetTileDrawTexture", BindingFlags.NonPublic | BindingFlags.Instance);
			if (method == null)
				return null;
			return (Texture2D)method.Invoke(wDrawer, new object[] { tile, tileX, tileY });
		}
		public static void DrawWallM(SpriteBatch spriteBatch, Vector2 offset, Tile tile, int i, int j, int wall)
		{
			Vector2 value = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				value = Vector2.Zero;
			}
			value += offset;
			float gfxQuality = Main.gfxQuality;
			int num21 = (int)(120f * (1f - gfxQuality) + 40f * gfxQuality);
			int num13 = (int)(num21 * 0.4f);
			int num14 = (int)(num21 * 0.35f);
			int num15 = (int)(num21 * 0.3f);
			Color color = Lighting.GetColor(i, j);
			if (tile.WallColor == 31)
			{
				color = Color.White;
			}
			if (color.R == 0 && color.G == 0 && color.B == 0 && j < Main.UnderworldLayer)
			{
				return;
			}
			Rectangle value2 = new Rectangle(0, 0, 32, 32);
			value2.X = tile.WallFrameX;
			value2.Y = tile.WallFrameY + Main.wallFrame[wall] * 180;
			if ((uint)(tile.WallType - 242) <= 1u)
			{
				int num11 = 20;
				int num12 = (Main.wallFrameCounter[wall] + i * 11 + j * 27) % (num11 * 8);
				value2.Y = tile.WallFrameY + 180 * (num12 / num11);
			}
			if (Lighting.NotRetro && !Main.wallLight[wall] && tile.WallType != 241 && (tile.WallType < 88 || tile.WallType > 93) && !WorldGen.SolidTile(tile))
			{
				Texture2D tileDrawTexture = Run_GetTileDrawTexture(Main.instance.WallsRenderer, tile, i, j);
				if (tile.WallType == 44)
				{
					color = new Color((int)(byte)Main.DiscoR, (int)(byte)Main.DiscoG, (int)(byte)Main.DiscoB);
				}
				Vector2 pos = new Vector2((float)(i * 16 - (int)Main.screenPosition.X - 8), (float)(j * 16 - (int)Main.screenPosition.Y - 8));
				spriteBatch.Draw(tileDrawTexture, pos + value, value2, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				Color color2 = color;
				if (wall == 44)
				{
					color2 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
				}
				Texture2D tileDrawTexture2 = Run_GetTileDrawTexture(Main.instance.WallsRenderer, tile, i, j);
				Vector2 pos = new Vector2((float)(i * 16 - (int)Main.screenPosition.X - 8), (float)(j * 16 - (int)Main.screenPosition.Y - 8));
				spriteBatch.Draw(tileDrawTexture2, pos + value, value2, color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			if (color.R > num13 || color.G > num14 || color.B > num15)
			{
				bool num22 = Main.tile[i - 1, j].WallType > 0 && Main.wallBlend[Main.tile[i - 1, j].WallType] != Main.wallBlend[tile.WallType];
				bool flag = Main.tile[i + 1, j].WallType > 0 && Main.wallBlend[Main.tile[i + 1, j].WallType] != Main.wallBlend[tile.WallType];
				bool flag2 = Main.tile[i, j - 1].WallType > 0 && Main.wallBlend[Main.tile[i, j - 1].WallType] != Main.wallBlend[tile.WallType];
				bool flag3 = Main.tile[i, j + 1].WallType > 0 && Main.wallBlend[Main.tile[i, j + 1].WallType] != Main.wallBlend[tile.WallType];
				if (num22)
				{
					spriteBatch.Draw(TextureAssets.WallOutline.Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + value, (Rectangle?)new Rectangle(0, 0, 2, 16), color, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
				}
				if (flag)
				{
					spriteBatch.Draw(TextureAssets.WallOutline.Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X + 14), (float)(j * 16 - (int)Main.screenPosition.Y)) + value, (Rectangle?)new Rectangle(14, 0, 2, 16), color, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
				}
				if (flag2)
				{
					spriteBatch.Draw(TextureAssets.WallOutline.Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + value, (Rectangle?)new Rectangle(0, 0, 16, 2), color, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
				}
				if (flag3)
				{
					spriteBatch.Draw(TextureAssets.WallOutline.Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y + 14)) + value, (Rectangle?)new Rectangle(0, 14, 16, 2), color, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
				}
			}
		}
    }
	public class PortalDustProjectile : ModProjectile
	{
        public override string Texture => "SOTS/Items/Chaos/VoidAnomaly";
        public void SpawnDust(Item item)
		{
			if (NetmodeID.Server == Main.netMode)
				return;
			SpawnDustEntity(item);
			DustCircle(Projectile.Center, item.width / 2f, item.height / 2f);
		}
		public void SpawnDust(NPC npc)
		{
			if (NetmodeID.Server == Main.netMode)
				return;
			SpawnDustEntity(npc);
			DustCircle(Projectile.Center, npc.width / 2f, npc.height / 2f);
		}
		public void SpawnDustEntity(Entity ent)
		{
			if (NetmodeID.Server == Main.netMode)
				return;
			Texture2D texture = null;
			int frameCount = 1;
			int spriteDirection = 1;
			int frame = 0;
			float rotation = 0;
			Rectangle? frameRect = null;
			if (ent is Item item)
			{
				texture = TextureAssets.Item[item.type].Value;
				DrawAnimation anim = Main.itemAnimations[item.type];
				if (anim != null)
				{
					frameCount = anim.FrameCount;
					frame = anim.Frame;
				}
			}
			if (ent is NPC npc)
			{
				texture = TextureAssets.Npc[npc.type].Value;
				frameCount = Main.npcFrameCount[npc.type];
				frameRect = npc.frame;
				spriteDirection = -npc.spriteDirection;
				rotation = npc.rotation;
			}
			if (texture != null)
			{
				int width = texture.Width;
				int height = texture.Height / frameCount;
				Color[] data = new Color[texture.Width * texture.Height];
				int startAt = width * height * frame;
				if (frameRect != null)
				{
					startAt = frameRect.Value.X + frameRect.Value.Y * width;
				}
				texture.GetData(data);
				int localX = 0;
				int localY = 0;
				for (int i = startAt; i < startAt + width * height; i++)
				{
					localX++;
					if (localX > width)
					{
						localX -= width;
						localY++;
					}
					if (data[i].A >= 255 && Main.rand.NextBool(5))
					{
						Vector2 offset = -new Vector2(width / 2, height / 2) + new Vector2(localX, localY);
						offset.X *= spriteDirection;
						offset = offset.RotatedBy(rotation);
						Vector2 velocity = new Vector2(offset.X / width, offset.Y / height) * 2f;
						velocity = velocity.RotatedBy(rotation);
						Color color = data[i];
						Dust dust = Dust.NewDustDirect(Projectile.Center + offset - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0, 0, Color.Lerp(ColorHelpers.VoidAnomaly, color, 0.5f));
						dust.velocity = velocity;
						dust.fadeIn = 3;
						dust.noGravity = true;
						dust.color.A = 0;
					}
				}
			}
		}
		public override void SetDefaults() //Do you enjoy how all my net sycning is done via projectiles?
		{
			Projectile.alpha = 255;
			Projectile.timeLeft = 24;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.netImportant = true;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.ignoreWater = true;
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override void AI()
		{
			Projectile.alpha = 255;
			Projectile.Kill();
		}
		public override void Kill(int timeLeft)
		{
			int whoAmI = (int)Projectile.ai[0];
			bool isAnItem = Projectile.ai[1] == -1;
			if(isAnItem)
            {
				Item item = Main.item[whoAmI];
				SpawnDust(item);
            }
			else
			{
				NPC npc = Main.npc[whoAmI];
				SpawnDust(npc);
			}
		}
	}
}