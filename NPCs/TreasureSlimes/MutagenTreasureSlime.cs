using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;
using SOTS.Items.Fragments;
using SOTS.Items.Inferno;
using SOTS.Items.Tools;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Void;
using SOTS.Items;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using SOTS.Items.Whips;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.NPCs.TreasureSlimes
{
	public class MutagenTreasureSlime : TreasureSlime
	{
        //public override void SetStaticDefaults()
        //{
        //	NPCID.Sets.TrailCacheLength[NPC.type] = 6;
        //	NPCID.Sets.TrailingMode[NPC.type] = 2;
        //}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Player player = Main.player[NPC.target];
            Texture2D eye = Request<Texture2D>("SOTS/NPCs/TreasureSlimes/MutagenTreasureSlimeEye").Value;
            Texture2D pupil = Request<Texture2D>("SOTS/NPCs/TreasureSlimes/MutagenTreasureSlimePupil").Value;
            Vector2 drawOrigin = new Vector2(eye.Width * 0.5f, eye.Height * 0.5f);
            Vector2 drawOrigin2 = new Vector2(pupil.Width * 0.5f, pupil.Height * 0.5f);
            Vector2 drawPos = NPC.Center - screenPos + new Vector2(0, 11);
            Color color = drawColor;
			Vector2 toPlayer = player.Center - NPC.Center;
			if (NPC.frame.Y > 0)
				drawPos.Y -= 2;
			spriteBatch.Draw(eye, drawPos, null, color, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(pupil, drawPos + toPlayer.SNormalize() * 1.25f, null, color, NPC.rotation, drawOrigin2, NPC.scale * 0.8f, SpriteEffects.None, 0f);
        }
        public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.lifeMax = 225;
			NPC.damage = 45;
			NPC.defense = 12;
			NPC.knockBackResist = 0.05f;
			NPC.value = Item.buyPrice(0, 2, 75, 0);
			NPC.Size = new Vector2(32, 42);
			NPC.npcSlots = 1f;
			//Banner = NPC.type;
			//BannerItem = ItemType<CorruptionTreasureSlimeBanner>();
			LootAmt = 3;
			gelColor = new Color(222, 73, 170, 100);
            items = new List<TreasureSlimeItem>()
			{
				new TreasureSlimeItem(ItemType<OldKey>(), 1, 1, 1f), //guaranteed
				new TreasureSlimeItem(ItemType<AncientSteelBar>(), 9, 15, 1f),
				new TreasureSlimeItem(ItemType<CharredWood>(), 45, 60, 0.5f),
				new TreasureSlimeItem(ItemType<SootBlock>(), 45, 60, 0.5f),
				new TreasureSlimeItem(ItemType<VisionAmulet>(), 1, 1, 1f),
                new TreasureSlimeItem(ItemType<FizzleStar>(), 1, 1, 1f),
				new TreasureSlimeItem(ItemType<Lockpick>(), 1, 1, 1f),
				new TreasureSlimeItem(ItemType<AutoClicker>(), 1, 1, 1f),
				new TreasureSlimeItem(ItemType<BrassWhip>(), 1, 1, 1f),
				new TreasureSlimeItem(ItemType<HandCannon>(), 1, 1, 1f),
				new TreasureSlimeItem(ItemType<MineralSpewer>(), 1, 1, 1f),
				new TreasureSlimeItem(ItemType<BackupBow>(), 1, 1, 1f),
				new TreasureSlimeItem(ItemType<PixelBlaster>(), 1, 1, 0.25f),
				new TreasureSlimeItem(ItemType<AcidicInjection>(), 1, 1, 0.25f),
			};
        }
        public override void ModifyAdditionalLoot(NPCLoot npcLoot)
		{
			//npcLoot.Add(ItemDropRule.Common(ItemType<ExplosiveKnife>(), 1, 10, 20));
			npcLoot.Add(ItemDropRule.Common(ItemID.PinkGel, 1, 10, 20));
        }
    }
}