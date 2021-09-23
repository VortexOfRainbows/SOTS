using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;
using System;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.NPCs.TreasureSlimes
{
	public abstract class TreasureSlime : ModNPC
	{
		public float treasureTimer = 0;

		public struct TreasureSlimeItem
		{
			public TreasureSlimeItem(int item, int amount, int amountCap)
			{
				Type = item;
				Amount = amount;
				AmountCap = amountCap;
			}

			public int Type { get; }
			public int Amount { get; }
			public int AmountCap { get; }
		}

		public List<TreasureSlimeItem> items = new List<TreasureSlimeItem>() { new TreasureSlimeItem(ItemID.Torch, 10, 19),
		new TreasureSlimeItem(ItemID.IronBar, 5, 12),
		new TreasureSlimeItem(ItemID.SilverBar, 4, 11),
		new TreasureSlimeItem(ItemID.GoldBar, 3, 10)};

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Slime");
			NPCID.Sets.TrailCacheLength[npc.type] = 6;
			NPCID.Sets.TrailingMode[npc.type] = 1;
		}
		public override void SetDefaults()
		{
			npc.CloneDefaults(NPCID.GreenSlime);
			npc.color = Color.White;
			aiType = NPCID.GreenSlime;
			animationType = NPCID.BlueSlime;
			Main.npcFrameCount[npc.type] = 2;
			npc.lifeMax = 350;
			npc.damage = 24;
		}
		public override void AI()
		{
			float itemMax = items.Count - 1;

			npc.ai[1]++;

			if (npc.ai[1] % 30 == 0) treasureTimer++;
			if (treasureTimer > itemMax) treasureTimer = 0;

			npc.TargetClosest(true);
		}
		public override void HitEffect(int hitDirection, double damage)
		{

		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			TreasureSlimeItem item = items[(int)treasureTimer];
			Texture2D texture = Main.itemTexture[item.Type];

			spriteBatch.Draw(texture, npc.oldPos[3] + new Vector2(0, -20 + (float)(Math.Cos(npc.ai[1] / 20) * 2)) + (npc.Size / 2) - Main.screenPosition,
			new Rectangle(0, 0, texture.Width, texture.Height), drawColor, MathHelper.ToRadians(npc.velocity.X * 1.2f),
			texture.Size() / 2, 1f / (float)((float)Math.Sqrt(Math.Pow(texture.Width, 2) + Math.Pow(texture.Height, 2))) * 30f, SpriteEffects.None, 0f);

			return true;
		}
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0;
		}
		public override void NPCLoot()
		{
			TreasureSlimeItem item = items[(int)treasureTimer];
			Item.NewItem(npc.Hitbox, item.Type, Main.rand.Next(item.Amount, item.AmountCap));
		}
        public override void FindFrame(int frameHeight)
        {
			
        }
    }
}