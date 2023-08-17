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
using SOTS.Items.Slime;
using SOTS.Projectiles.Slime;
using Terraria.GameContent.ItemDropRules;

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
					if(possibleItems.Count() == 0 && (NPC.type == NPCType<CorruptionTreasureSlime>() || NPC.type == NPCType<CrimsonTreasureSlime>()))
                    {
						rand = 0;
                    }
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
				NPC.netUpdate = true;
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
			NPCID.Sets.TrailCacheLength[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 2;
		}
        public override Color? GetAlpha(Color drawColor)
        {
			return drawColor * ((255f - NPC.alpha) / 255f);
        }
        public override void SetDefaults()
		{
			Color temp = NPC.color;
			NPC.CloneDefaults(NPCID.GreenSlime);
			AIType = NPCID.GreenSlime;
			AnimationType = NPCID.BlueSlime;
			NPC.alpha = 50;
			NPC.color = temp;
			NPC.rarity = 1;
			Main.npcFrameCount[NPC.type] = 2;
		}
		public float runAwayCounter = 0;
		public float runAwayDelay = 0;
		public int runAwayTime = 480;
        public sealed override bool PreAI()
        {
			if(runOnce)
            {
				SetupLootTable();
			}
			if (possibleItems.Count != 0)
				doTreasureTimer();
			if (NPC.life < NPC.lifeMax / 2)
			{
				NPC.TargetClosest(true);
				Player player = Main.player[NPC.target];
				if (player.Center.X > NPC.Center.X)
				{
					NPC.direction = -1;
				}
				else
					NPC.direction = 1;
				NPC.ai[2] = -1;
				NPC.ai[0] += 2.5f;
				/*if(npc.velocity.Y < 0)
				{
					npc.position.Y += npc.velocity.Y * 0.05f;
				}
				npc.position.X += npc.velocity.X * 0.05f;*/
				//Main.NewText(runAwayCounter + " : " + runAwayDelay);
				bool returnV = true;
				if (runAwayCounter >= runAwayTime)
                {
					float percent = (runAwayCounter - runAwayTime) / 100f;
					if (percent > 1)
						percent = 1;
					NPC.velocity.Y -= 0.5f;
					NPC.velocity.X *= 1f - percent;
					NPC.velocity.Y *= 1f - percent;
					NPC.noGravity = true;
					returnV = false;
				}
				if (runAwayCounter >= (runAwayTime + 100))
                {	
					if (Main.netMode != NetmodeID.MultiplayerClient && (int)runAwayDelay == 0)
                    {
						int type = 0;
						if (NPC.type == NPCType<BasicTreasureSlime>())
							type = 0;   
						if (NPC.type == NPCType<GoldenTreasureSlime>())
							type = 1;   
						if (NPC.type == NPCType<IceTreasureSlime>())
							type = 2;   
						if (NPC.type == NPCType<PyramidTreasureSlime>())
							type = 3;   
						if (NPC.type == NPCType<ShadowTreasureSlime>())
							type = 4;
						if (NPC.type == NPCType<CorruptionTreasureSlime>())
							type = 5;
						if (NPC.type == NPCType<CrimsonTreasureSlime>())
							type = 6;
						if (NPC.type == NPCType<JungleTreasureSlime>())
							type = 7;
						if (NPC.type == NPCType<HallowTreasureSlime>())
							type = 8;
						if (NPC.type == NPCType<DungeonTreasureSlime>())
							type = 9;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, 4), Vector2.Zero, ProjectileType<TreasureStarPortal>(), 0, 0, Main.myPlayer, 0, type);
					}
					runAwayDelay++;
					if(runAwayDelay >= 70)
                    {
						NPC.active = false;
						return false;
                    }
                }
				else
					runAwayCounter += 1 + 0.5f * (1 - (float)NPC.life / (NPC.lifeMax / 2));
				return returnV;
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
					NPC.netUpdate = true;
				}
				treasure++;
			}
			if (treasure > itemMax)
			{
				treasure = 0;
			}
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (Main.rand.NextBool(2))
				SOTSUtils.PlaySound(SoundID.NPCHit4, (int)NPC.Center.X, (int)NPC.Center.Y, 0.6f, 0.2f);
			if (NPC.life > 0)
			{
				int num = 0;
				while (num < hit.Damage / NPC.lifeMax * 100.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, (float)hit.HitDirection, -1f, NPC.alpha, gelColor, 1f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 50; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, (float)(2 * hit.HitDirection), -2f, NPC.alpha, gelColor, 1f);
				}
			}
		}
        public sealed override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			bool canDrawItems = true;
			if (runOnce)
				canDrawItems = false;
			if(canDrawItems)
			{
				int itemMax = possibleItems.Count - 1;
				int otherId = (int)treasure - 1;
				if (otherId < 0)
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
				Vector2 drawPos = NPC.oldPos[3] + new Vector2(0, -20 + (float)(Math.Cos((float)treasureCounter / treasureSpeed) * 2) + NPC.gfxOffY) + (NPC.Size / 2) - screenPos;
				if(!Terraria.GameContent.TextureAssets.Item[item.Type].IsLoaded)
				{
					Main.instance.LoadItem(item.Type);
				}
				Texture2D texture = Terraria.GameContent.TextureAssets.Item[item.Type].Value;
				float scale = 1.2f * NPC.scale / (float)Math.Sqrt(texture.Width * texture.Width + texture.Height * texture.Height) * NPC.width;
				scale = MathHelper.Clamp(scale, 0.4f, 1.1f);
				//Texture2D textureGlow = (Texture2D)ModContent.Request<Texture2D>("SOTS/Assets/TreasureSlimeBloom");
				Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height);
				//spriteBatch.Draw(textureGlow, new Vector2(npc.Center.X, npc.position.Y + npc.gfxOffY + 12) - screenPos, null, new Color(glowColor.R, glowColor.G, glowColor.B, 0), 0, new Vector2(textureGlow.Width/2, textureGlow.Height), 2f / (float)Math.Sqrt(textureGlow.Width * textureGlow.Width + textureGlow.Height * textureGlow.Height) * npc.width, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, drawPos, frame, drawColor * firstAlpha, MathHelper.ToRadians(NPC.velocity.X * 1.2f), texture.Size() / 2, scale, SpriteEffects.None, 0f);
				texture = Terraria.GameContent.TextureAssets.Item[item2.Type].Value;
				frame = new Rectangle(0, 0, texture.Width, texture.Height);
				scale = 1.2f * NPC.scale / (float)Math.Sqrt(texture.Width * texture.Width + texture.Height * texture.Height) * NPC.width;
				scale = MathHelper.Clamp(scale, 0.4f, 1.1f);
				spriteBatch.Draw(texture, drawPos, frame, drawColor * secondAlpha, MathHelper.ToRadians(NPC.velocity.X * 1.2f), texture.Size() / 2, scale, SpriteEffects.None, 0f);
			}
			DrawSlime(spriteBatch, screenPos, drawColor);
			return false;
		}
		public void DrawSlime(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(texture.Width / 2, NPC.height / 2), NPC.scale, SpriteEffects.None, 0f);
		}
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0;
		}
        public sealed override void OnKill()
        {
			int itemMax = possibleItems.Count - 1;
			int otherId = (int)treasure - 1;
			if (otherId < 0)
			{
				otherId = itemMax;
			}
			int itemID = treasure;
			if (treasureCounter % treasureSpeed <= 7)
				itemID = otherId;
			TreasureSlimeItem item = possibleItems[(int)itemID];
			Item.NewItem(NPC.GetSource_Loot("SOTS:TreasureSlimePriorityItem"), NPC.Hitbox, item.Type, Main.rand.Next(item.Amount, item.AmountCap + 1));
			AdditionalOnKill();
		}
        public sealed override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(ItemType<Peanut>(), 1, 10, 20));
			npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 5, 10));
			ModifyAdditionalLoot(npcLoot);
        }
		public virtual void ModifyAdditionalLoot(NPCLoot npcLoot)
		{

        }
        public virtual void AdditionalOnKill()
        {

        }
    }
}