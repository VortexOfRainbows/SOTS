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
			NPC.aiStyle =1;
            NPC.lifeMax = 100;  
            NPC.damage = 24; 
            NPC.defense = 10;  
            NPC.knockBackResist = 1f;
            NPC.width = 36;
            NPC.height = 28;
            AnimationType = NPCID.BlueSlime;
			Main.npcFrameCount[NPC.type] = 2;  
            NPC.value = 1800;
            NPC.npcSlots = .5f;
			NPC.alpha = 70;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			Banner = NPC.type;
			BannerItem = ItemType<FluxSlimeBanner>();
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/PinkyGrappleSpike");
			Vector2 drawPos = new Vector2(NPC.Center.X, NPC.position.Y + NPC.height - 10) - Main.screenPosition;
			drawColor = NPC.GetAlpha(drawColor);
			if (initiateSize == -1)
			{
				for (int i = 0; i < 7; i++)
				{
					counterArr[i] += randSeed1[i];
					
					Vector2 circular = new Vector2(-17 * NPC.scale, 0).RotatedBy(MathHelper.ToRadians(i * 30));
					int direction = 1;
					if(circular.X < 0)
                    {
						direction = -1;
                    }
					if(i == 3)
                    {
						direction = 0;
                    }
					if(NPC.frame.Y == 0)
					{
						if(i == 0 || i == 6)
							circular.Y -= 2 * NPC.scale;
						circular.Y += 2 * NPC.scale;
					}
					else
					{
						circular.X -= 2 * NPC.scale * direction;
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
					spriteBatch.Draw(texture, drawPos + circular, FrameSize, drawColor, MathHelper.ToRadians(i * 30) + MathHelper.ToRadians(90), new Vector2(texture.Width / 2, 3.5f), NPC.scale * 0.75f, SpriteEffects.None, 0f);
				}
			}
			return true;
		}
		float[] counterArr = new float[7];
		float[] randSeed1 = new float[7];
		public override bool PreAI()
		{
			NPC.TargetClosest(true);
			if(initiateSize == 1)
			{
				if (Main.netMode != 1)
					NPC.netUpdate = true;
				for (int i = 0; i < counterArr.Length; i++)
				{
					counterArr[i] = 0;
					randSeed1[i] = Main.rand.NextFloat(0.8f, 1.2f);
				}
				NPC.Center = NPC.position;
				initiateSize = -1;
				NPC.scale = 1.25f;
				NPC.width = (int)(NPC.width * NPC.scale);
				NPC.height = (int)(NPC.height * NPC.scale);
				NPC.position = NPC.Center;
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
				if (pet.type == Mod.Find<ModNPC>("FluxSlimeBall") .Type&& (int)pet.ai[0] == NPC.whoAmI && pet.active)
				{
					total++;
				}
			}
			if (Main.netMode != NetmodeID.MultiplayerClient && counter >= 150 * (1 + total)) 
			{
				counter = 0;
				if (total < 3)
				{
					int npc1 = NPC.NewNPC((int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y + NPC.height, Mod.Find<ModNPC>("FluxSlimeBall").Type, 0, NPC.whoAmI, Main.rand.NextFloat(360), 0, Main.rand.NextFloat(0.8f, 1.2f));
				}
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)NPC.lifeMax * 110.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.PinkSlime, (float)hitDirection, -1f, NPC.alpha);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 45; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.PinkSlime, (float)(2 * hitDirection), -2f, NPC.alpha);
				}
			}
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(6) == 0)
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("FoulConcoction").Type, Main.rand.Next(2) + 1);
			}
			if(!Main.rand.NextBool(5) || Main.expertMode)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("VialofAcid").Type, Main.rand.Next(2) + 1);
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.PinkGel, Main.rand.Next(4, 7));
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("FragmentOfNature").Type, 1);
		}
	}
}