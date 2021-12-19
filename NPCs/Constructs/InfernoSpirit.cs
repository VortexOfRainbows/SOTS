using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Evil;
using SOTS.Projectiles.Tide;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Constructs
{
	public class InfernoSpirit : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[npc.type] = 1;
			DisplayName.SetDefault("Inferno Spirit");
			NPCID.Sets.TrailCacheLength[npc.type] = 5;  
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(phase);
			writer.Write(counter);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			phase = reader.ReadInt32();
			counter = reader.ReadInt32();
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 10;
            npc.lifeMax = 4000; 
            npc.damage = 100; 
            npc.defense = 0;   
            npc.knockBackResist = 0f;
            npc.width = 70;
            npc.height = 70;
            npc.value = Item.buyPrice(0, 18, 0, 0);
            npc.npcSlots = 10f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = false;
			npc.rarity = 2;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.damage = 135;
			npc.lifeMax = 6000;
		}
		private int InitiateHealth = 15000;
		private float ExpertHealthMult = 1.5f; //22500
		int phase = 1;
		int counter = 0;
		public int startEyes = 0;
		public const int range = 96;
		public override void AI()
		{
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.15f / 255f, (255 - npc.alpha) * 0.25f / 255f, (255 - npc.alpha) * 0.65f / 255f);
			Player player = Main.player[npc.target];
			float mult = (100 + npc.ai[2]) / 100f;
			if (phase == 3)
			{
				npc.aiStyle = -1;
				npc.dontTakeDamage = false;
				int damage = npc.damage / 2;
				if (Main.expertMode)
				{
					damage = (int)(damage / Main.expertDamage);
				}
			}
			if (phase == 2)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
					npc.netUpdate = true;
				npc.dontTakeDamage = false;
				npc.aiStyle = -1;
				npc.ai[0] = 0;
				npc.ai[1] = 0;
				npc.ai[2] = 0;
				npc.ai[3] = 0;
				phase = 3;
			}
			else if(phase == 1)
			{
				counter++;
			}
			if(Main.player[npc.target].dead)
			{
				counter++;
			}
			else if(phase != 1 && counter > 0)
			{
				counter--;
			}
			if(counter >= 1440)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.netUpdate = true;
				}
				phase = 1;
				npc.aiStyle = -1;
				npc.velocity.Y -= 0.014f;
				npc.dontTakeDamage = true;
			}
			int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.RainbowMk2);
			Dust dust = Main.dust[dust2];
			dust.color = new Color(255, 130, 15);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 2f;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			for (int k = 0; k < npc.oldPos.Length; k++) {
				Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
				Color color = new Color(150, 80, 70, 0) * ((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, npc.rotation, drawOrigin, npc.scale * 1.1f, SpriteEffects.None, 0f);
			}
			return false;
		}	
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for(int i = 0; i < 50; i ++)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.RainbowMk2);
					dust.color = new Color(239, 139, 18);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 2f;
					dust.velocity *= 5f;
				}
				if(phase == 1)
				{
					phase = 2;
					npc.lifeMax = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
					npc.life = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
				}
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				Main.spriteBatch.Draw(texture, npc.Center + Main.rand.NextVector2Circular(4f, 4f) - Main.screenPosition, null, color, 0f, drawOrigin, npc.scale * 1.1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<DissolvingNether>(), 1);	
		}	
	}
}
