using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Constructs
{
	public class Bridgeburner : ModNPC
	{
        public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
		}
        public override void SetDefaults()
		{
			NPC.aiStyle = 0;
			NPC.lifeMax = 500;  
			NPC.damage = 50; 
			NPC.defense = 50;  
			NPC.knockBackResist = 0.1f;
			NPC.width = 76;
			NPC.height = 74;
			NPC.value = 3330;
			NPC.npcSlots = 3f;
			NPC.boss = false;
			NPC.lavaImmune = false;
			NPC.noGravity = false;
			NPC.noTileCollide = false;
			NPC.netAlways = true;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.rarity = 5;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Player player = Main.player[NPC.target];
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[Type].Value;
			Texture2D texture2 = ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/BridgeburnerHead").Value;
			Vector2 origin = texture.Size() / 2;
            spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), null, drawColor, 0, origin, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            spriteBatch.Draw(texture2, NPC.Center - screenPos + new Vector2(-3, NPC.gfxOffY - 39), null, drawColor, 0, texture2.Size() / 2, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return false;
        }
        public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{
				for (int k = 0; k < 20; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Lead, 2.5f * (float)hit.HitDirection, -2.5f, 0, default(Color), 0.7f);
				}
				//for(int i = 1; i < 8; i++)
				//	Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/NatureConstructGore" + i), 1f);
				//for(int i = 0; i < 9; i++)
				//	Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Main.rand.Next(61,64), 1f);	
			}
		}
		public override void FindFrame(int frameHeight) 
		{
			float speed = Math.Abs(NPC.velocity.X * 0.7f);
			if (speed > 1.67f)
				speed = 1.67f;
			else if (speed <= 0.1f)
            {
				speed = 0;
				NPC.frame.Y = 0;
            }
			NPC.frameCounter += speed;
			if (NPC.frameCounter > 10f) 
			{
				NPC.frame.Y = (NPC.frame.Y + frameHeight);
				if(NPC.frame.Y >= frameHeight * 3)
				{
					NPC.frame.Y = 0;
				}
				NPC.frameCounter = 0;
			}
		}
		public override void AI()
		{
			Player player = Main.player[NPC.target];
		}
        public override void OnKill()
		{
			//int n = NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<NatureSpirit>());
			//Main.npc[n].velocity.Y = -10f;
			//Main.npc[n].netUpdate = true;
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			//npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfNature>(), 1, 4, 7));
		}
	}
}