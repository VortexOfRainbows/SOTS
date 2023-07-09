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

namespace SOTS.NPCs.Town
{
	public class Archaeologist : ModNPC
	{
		public static Vector2 AnomalyPosition = Vector2.Zero;
		public static float AnomalyAlphaMult = 0f;
		public static float FinalAnomalyAlphaMult = 0f;
		public const int timeToGoToSetPiece = 600; //This is five minutes
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(locationTimer);
			writer.Write(InitialDirection);
			writer.Write(currentLocationType);
			writer.Write(AnomalyAlphaMult);
			writer.Write(AnomalyPosition.X);
			writer.Write(AnomalyPosition.Y);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			locationTimer = reader.ReadInt32();
			InitialDirection = reader.ReadInt32();
			currentLocationType = reader.ReadInt32();
			AnomalyAlphaMult = reader.ReadSingle();
			AnomalyPosition.X = reader.ReadSingle();
			AnomalyPosition.Y = reader.ReadSingle();
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
		}
		public bool playerNearby = false;
		public int AnimCycles = 0;
		public int FrameY = 0;
		public int FrameSpeed = 5;
		public int TotalIdleFrameCycles = 6;
		public int TotalLookUpFrameCycles = 3;
		public const int MinimumIdleCycles = 6;
		public const int MaximumIdleCycles = 22;
		public const int MinimunLookCycles = 5;
		public const int MaximumLookCycles = 14;
		public int DrawTimer = 0;
		public int aiTimer = 0;
		public int locationTimer = 100000;
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
        public override bool PreAI()
        {
			if(NPC.CountNPCS(Type) > 1)
            {
				NPC.active = false;
				return false;
            }
			else
			{
				AnomalyAlphaMult += 1 / 60f;
				AnomalyPosition = NPC.Center;
			}
			if (InitialDirection == 0)
            {
				InitialDirection = 1;
            }
			int directionToGoTo = InitialDirection;
			playerNearby = false;
			bool playerWithinSecondRange = false;
			for(int i = 0; i < Main.player.Length; i++)
            {
				Player player = Main.player[i];
				if (player.active)
                {
					if(player.Distance(NPC.Center) < 1600)
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
			if(!playerWithinSecondRange)
            {
				if(locationTimer > timeToGoToSetPiece)
                {
					aiTimer = 0;
					locationTimer = 0;
					if(Main.netMode != NetmodeID.MultiplayerClient)
					{
						FindALocationToGoTo();
						InitialDirection = NPC.direction;
					}
                }
				else if(locationTimer > timeToGoToSetPiece - 60)
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
				AnomalyAlphaMult = 0;
				if (Main.netMode == NetmodeID.Server)
					Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("The location is at: " + currentLocationType), Color.Gray);
				else
					Main.NewText("The location is at: " + currentLocationType);
			}
			else
            {
				currentLocationType = olderLocationType;
            }
		}
	}
}