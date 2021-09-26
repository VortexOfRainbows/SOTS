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
using System.IO;
using Terraria.Graphics.Shaders;
using SOTS.Items.GelGear;

namespace SOTS.NPCs.TreasureSlimes
{
	public abstract class TreasureSlime : ModNPC
	{
		public struct TreasureSlimeItem
		{
			public TreasureSlimeItem(int item, int amount, int amountCap, float failChance = 1f)
			{
				Type = item;
				Amount = amount;
				AmountCap = amountCap;
				FailChance = failChance;
			}
			public float FailChance { get; }
			public int Type { get; }
			public int Amount { get; }
			public int AmountCap { get; }
		}
		public int treasure = 0;
		public int LootAmt = 3;
		public Color gelColor = new Color(255, 255, 133, 100);
        public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(timeToUpdate);
			writer.Write(indexes[0]);
			writer.Write(indexes[1]);
			writer.Write(indexes[2]);
			writer.Write(indexes[3]);
			writer.Write(indexes[4]);
			writer.Write(indexes[5]);
			writer.Write(indexes[6]);
			writer.Write(indexes[7]);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			timeToUpdate = reader.ReadBoolean();
			indexes[0] = reader.ReadInt32();
			indexes[1] = reader.ReadInt32();
			indexes[2] = reader.ReadInt32();
			indexes[3] = reader.ReadInt32();
			indexes[4] = reader.ReadInt32();
			indexes[5] = reader.ReadInt32();
			indexes[6] = reader.ReadInt32();
			indexes[7] = reader.ReadInt32();
		}
		bool runOnce = true;
		public bool timeToUpdate = false;
		public List<TreasureSlimeItem> items = new List<TreasureSlimeItem>() { new TreasureSlimeItem(ItemID.Torch, 10, 19),
		new TreasureSlimeItem(ItemID.IronBar, 5, 12),
		new TreasureSlimeItem(ItemID.SilverBar, 4, 11),
		new TreasureSlimeItem(ItemID.GoldBar, 3, 10)};
		public int[] indexes = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };
		public List<TreasureSlimeItem> possibleItems = new List<TreasureSlimeItem>();
		public void SetupLootTable()
		{
			if (items.Count < LootAmt)
				LootAmt = items.Count;
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				for (int i = 0; i < LootAmt; i++)
				{
					int rand = Main.rand.Next(items.Count);
					if (Main.rand.NextFloat(1) <= items[rand].FailChance)
					{
						possibleItems.Add(items[rand]);
						indexes[i] = rand;
					}
					else
						i--;
					items.RemoveAt(rand);
				}
				timeToUpdate = true;
				runOnce = false;
				npc.netUpdate = true;
			}
			else
            {
				if(timeToUpdate)
                {
					List<TreasureSlimeItem> itemsTemp = new List<TreasureSlimeItem>();
					for(int i = 0; i < items.Count; i++)
                    {
						itemsTemp.Add(items[i]);
                    }
					possibleItems = new List<TreasureSlimeItem>();
					timeToUpdate = false;
					for (int i = 0; i < LootAmt; i++)
					{
						int indexOfItem = indexes[i];
						if(indexOfItem != -1)
						{
							possibleItems.Add(itemsTemp[indexOfItem]);
							itemsTemp.RemoveAt(indexOfItem);
						}
					}
					runOnce = false;
				}
            }
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Slime");
			NPCID.Sets.TrailCacheLength[npc.type] = 6;
			NPCID.Sets.TrailingMode[npc.type] = 2;
		}
        public override Color? GetAlpha(Color drawColor)
        {
			return drawColor * ((255f - npc.alpha) / 255f);
        }
        public override void SetDefaults()
		{
			Color temp = npc.color;
			npc.CloneDefaults(NPCID.GreenSlime);
			aiType = NPCID.GreenSlime;
			animationType = NPCID.BlueSlime;
			npc.alpha = 50;
			npc.color = temp;
			npc.rarity = 1;
			Main.npcFrameCount[npc.type] = 2;
		}
		public float runAwayCounter = 0;
        public sealed override bool PreAI()
        {
			if(runOnce)
            {
				SetupLootTable();
			}
			if (possibleItems.Count != 0)
				doTreasureTimer();
			if(npc.life < npc.lifeMax / 2)
            {
				npc.TargetClosest(true);
				Player player = Main.player[npc.target];
				if (player.Center.X > npc.Center.X)
				{
					npc.direction = -1;
				}
				else
					npc.direction = 1;
				npc.ai[2] = -1;
				npc.ai[0] += 2.5f;
				/*if(npc.velocity.Y < 0)
				{
					npc.position.Y += npc.velocity.Y * 0.05f;
				}
				npc.position.X += npc.velocity.X * 0.05f;*/
				runAwayCounter += 1 + 0.5f * (1 - (float)npc.life / (npc.lifeMax / 2));
            }
			return true;
        }
		public int treasureSpeed = 38;
		float treasureCounter = 0;
		public void doTreasureTimer()
		{
			int itemMax = possibleItems.Count - 1;
			treasureCounter++;
			if (treasureCounter % treasureSpeed == 0)
			{
				if(Main.netMode == NetmodeID.Server)
                {
					npc.netUpdate = true;
				}
				treasure++;
			}
			if (treasure > itemMax)
			{
				treasure = 0;
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if(Main.rand.NextBool(2))
				Main.PlaySound(SoundID.NPCHit, (int)npc.Center.X, (int)npc.Center.Y, 4, 0.6f, 0.2f);
			if (npc.life > 0)
			{
				int num = 0;
				while (num < damage / npc.lifeMax * 100.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 4, (float)hitDirection, -1f, npc.alpha, gelColor, 1f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 50; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 4, (float)(2 * hitDirection), -2f, npc.alpha, gelColor, 1f);
				}
			}
		}
		public sealed override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (runOnce)
				return false;
			int itemMax = possibleItems.Count - 1;
			int otherId = (int)treasure - 1;
			if(otherId < 0)
            {
				otherId = itemMax;
            }
			TreasureSlimeItem item = possibleItems[(int)treasure];
			TreasureSlimeItem item2 = possibleItems[otherId];
			float firstAlpha = 1;
			float secondAlpha = 0;
			if (treasureCounter % treasureSpeed <= 7)
            {
				secondAlpha = 1 - ((treasureCounter % treasureSpeed) + 1) / 8f;
				firstAlpha -= secondAlpha;
			}
			Vector2 drawPos = npc.oldPos[3] + new Vector2(0, -20 + (float)(Math.Cos((float)treasureCounter / treasureSpeed) * 2) + npc.gfxOffY) + (npc.Size / 2) - Main.screenPosition;
			Texture2D texture = Main.itemTexture[item.Type];
			float scale = 1.2f * npc.scale / (float)Math.Sqrt(texture.Width * texture.Width + texture.Height * texture.Height) * npc.width;
			scale = MathHelper.Clamp(scale, 0.4f, 1.1f);
			//Texture2D textureGlow = ModContent.GetTexture("SOTS/Assets/TreasureSlimeBloom");
			Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height);
			//spriteBatch.Draw(textureGlow, new Vector2(npc.Center.X, npc.position.Y + npc.gfxOffY + 12) - Main.screenPosition, null, new Color(glowColor.R, glowColor.G, glowColor.B, 0), 0, new Vector2(textureGlow.Width/2, textureGlow.Height), 2f / (float)Math.Sqrt(textureGlow.Width * textureGlow.Width + textureGlow.Height * textureGlow.Height) * npc.width, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, drawPos, frame, drawColor * firstAlpha, MathHelper.ToRadians(npc.velocity.X * 1.2f), texture.Size() / 2, scale, SpriteEffects.None, 0f);
			texture = Main.itemTexture[item2.Type];
			frame = new Rectangle(0, 0, texture.Width, texture.Height);
			scale = 1.2f * npc.scale / (float)Math.Sqrt(texture.Width * texture.Width + texture.Height * texture.Height) * npc.width;
			scale = MathHelper.Clamp(scale, 0.4f, 1.1f);
			spriteBatch.Draw(texture, drawPos, frame, drawColor * secondAlpha, MathHelper.ToRadians(npc.velocity.X * 1.2f), texture.Size() / 2, scale, SpriteEffects.None, 0f);
			DrawSlime(spriteBatch, drawColor);
			return false;
		}
		public void DrawSlime(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, npc.GetAlpha(drawColor), npc.rotation, new Vector2(texture.Width / 2, npc.height / 2), npc.scale, SpriteEffects.None, 0f);
		}
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0;
		}
		public sealed override void NPCLoot()
		{
			TreasureSlimeItem item = possibleItems[(int)treasure];
			Item.NewItem(npc.Hitbox, item.Type, Main.rand.Next(item.Amount, item.AmountCap + 1));
			Item.NewItem(npc.Hitbox, ItemType<Peanut>(), 5 + Main.rand.Next(6));
			Item.NewItem(npc.Hitbox, ItemID.Gel, 5 + Main.rand.Next(6));
			AdditionalLoot();
		}
		public virtual void AdditionalLoot()
        {

        }
    }
}