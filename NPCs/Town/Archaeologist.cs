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

namespace SOTS.NPCs.Town
{
	public class Archaeologist : ModNPC
	{
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
			NPC.noGravity = true;
		}
        public override bool PreAI()
        {
			NPC.dontTakeDamage = true;
			NPC.dontTakeDamageFromHostiles = true;
			NPC.velocity *= 0;
            return base.PreAI();
        }
        public override void PostAI()
        {
            base.PostAI();
			NPC.velocity *= 0f;
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
				new FlavorTextBestiaryInfoElement("WIP Bestiary Entry"),
				new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.ArchaeologistLore")
			});
		}
		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();

			// These are things that the NPC has a chance of telling you when you talk to it.
			chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogue1"));
			chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogue2"));
			chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogue3"));
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
		public override void SetupShop(Chest shop, ref int nextSlot)
		{

		}
	}
}