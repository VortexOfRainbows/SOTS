using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Banners;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class FluxSlime : ModNPC
	{	int initiateSize = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flux Slime");
		}
		public override void SetDefaults()
		{
			//npc.CloneDefaults(NPCID.BlackSlime);
			npc.aiStyle = 1;
            npc.lifeMax = 100;  
            npc.damage = 24; 
            npc.defense = 10;  
            npc.knockBackResist = 1f;
            npc.width = 36;
            npc.height = 28;
            animationType = NPCID.BlueSlime;
			Main.npcFrameCount[npc.type] = 2;  
            npc.value = 1800;
            npc.npcSlots = .5f;
			npc.alpha = 70;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
			banner = npc.type;
			bannerItem = ItemType<NatureSlimeBanner>();
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/PinkyGrappleSpike");
			Vector2 drawPos = new Vector2(npc.Center.X, npc.position.Y + npc.height - 10) - Main.screenPosition;
			drawColor = npc.GetAlpha(drawColor);
			if (initiateSize == -1)
			{
				for (int i = 0; i < 7; i++)
				{
					counterArr[i] += randSeed1[i];
					
					Vector2 circular = new Vector2(-17 * npc.scale, 0).RotatedBy(MathHelper.ToRadians(i * 30));
					int direction = 1;
					if(circular.X < 0)
                    {
						direction = -1;
                    }
					if(i == 3)
                    {
						direction = 0;
                    }
					if(npc.frame.Y == 0)
					{
						if(i == 0 || i == 6)
							circular.Y -= 2 * npc.scale;
						circular.Y += 2 * npc.scale;
					}
					else
					{
						circular.X -= 2 * npc.scale * direction;
						//circular.Y -= 2 * npc.scale;
					}
					int frame = 0;
					if (counterArr[i] >= 10)
					{
						frame = 1;
					}
					if (counterArr[i] >= 20)
					{
						frame = 2;
					}
					if (counterArr[i] >= 40)
					{
						frame = 3;
					}
					if (counterArr[i] >= 50)
					{
						frame = 0;
						counterArr[i] = 0;
						randSeed1[i] = Main.rand.NextFloat(0.8f, 1.2f);
					}
					Rectangle FrameSize = new Rectangle(0, texture.Height / 4 * frame, texture.Width, texture.Height / 4);
					spriteBatch.Draw(texture, drawPos + circular, FrameSize, drawColor, MathHelper.ToRadians(i * 30) + MathHelper.ToRadians(90), new Vector2(texture.Width / 2, 3.5f), npc.scale * 0.75f, SpriteEffects.None, 0f);
				}
			}
			return true;
		}
		float[] counterArr = new float[7];
		float[] randSeed1 = new float[7];
		public override bool PreAI()
		{
			npc.TargetClosest(true);
			if(initiateSize == 1)
			{
				if (Main.netMode != 1)
					npc.netUpdate = true;
				for (int i = 0; i < counterArr.Length; i++)
				{
					counterArr[i] = 0;
					randSeed1[i] = Main.rand.NextFloat(0.8f, 1.2f);
				}
				npc.Center = npc.position;
				initiateSize = -1;
				npc.scale = 1.25f;
				npc.width = (int)(npc.width * npc.scale);
				npc.height = (int)(npc.height * npc.scale);
				npc.position = npc.Center;
			}
			return true;
		}
		int counter = 0;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(counter);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			counter = reader.ReadInt32();
		}
		public override void AI()
		{
			counter++;
			int total = 0;
			for(int i = 0; i < Main.maxNPCs; i++)
			{
				NPC pet = Main.npc[i];
				if (pet.type == mod.NPCType("FluxSlimeBall") && (int)pet.ai[0] == npc.whoAmI && pet.active)
				{
					total++;
				}
			}
			if (Main.netMode != NetmodeID.MultiplayerClient && counter >= 150 * (1 + total)) 
			{
				counter = 0;
				if (total < 3)
				{
					int npc1 = NPC.NewNPC((int)npc.position.X + npc.width / 2, (int)npc.position.Y + npc.height, mod.NPCType("FluxSlimeBall"), 0, npc.whoAmI, Main.rand.NextFloat(360), 0, Main.rand.NextFloat(0.8f, 1.2f));
				}
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 110.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.PinkSlime, (float)hitDirection, -1f, npc.alpha);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 45; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.PinkSlime, (float)(2 * hitDirection), -2f, npc.alpha);
				}
			}
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(6) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FoulConcoction"), Main.rand.Next(2) + 1);
			}
			if(!Main.rand.NextBool(5) || Main.expertMode)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("VialofAcid"), Main.rand.Next(2) + 1);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.PinkGel, Main.rand.Next(4, 7));
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FragmentOfNature"), 1);
		}
	}
}