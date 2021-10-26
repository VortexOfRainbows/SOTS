using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Tide;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Constructs
{
	public class EvilSpirit : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[npc.type] = 1;
			DisplayName.SetDefault("Evil Spirit");
			NPCID.Sets.TrailCacheLength[npc.type] = 5;  
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(phase);
			writer.Write(counter);
			writer.Write(direction);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			phase = reader.ReadInt32();
			counter = reader.ReadInt32();
			direction = reader.ReadInt32();
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 10;
            npc.lifeMax = 3000; 
            npc.damage = 80; 
            npc.defense = 0;   
            npc.knockBackResist = 0f;
            npc.width = 58;
            npc.height = 58;
            npc.value = Item.sellPrice(0, 10, 0, 0);
            npc.npcSlots = 7f;
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
			npc.damage = 80;
			npc.lifeMax = 5000;
		}
		List<EvilEye> eyes = new List<EvilEye>();
		private int InitiateHealth = 8000;
		private float ExpertHealthMult = 1.5f;
		int phase = 1;
		int counter = 0;
		int direction = 1;
		public void UpdateEyes(bool draw = false)
        {
			for(int i = 0; i < eyes.Count; i++)
            {
				EvilEye eye = eyes[i];
				if (draw)
					eye.Draw(npc.Center, npc.rotation);
				else
					eye.Update();
            }
        }
		public override void AI()
		{
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.15f / 255f, (255 - npc.alpha) * 0.25f / 255f, (255 - npc.alpha) * 0.65f / 255f);
			Player player = Main.player[npc.target];
			UpdateEyes();
			if (phase == 3)
			{
				npc.dontTakeDamage = false;
				npc.velocity *= 0.95f;
				if (npc.ai[0] >= 0)
                {
					int counter = (int)(npc.ai[0]);
					if(counter < 180)
                    {
						if(counter % 10 == 0)
                        {
							Vector2 circular = new Vector2(96, 0).RotatedBy(MathHelper.ToRadians(counter * 2));
							eyes.Add(new EvilEye(circular));
                        }
                    }
					Vector2 toPlayer = player.Center - npc.Center;
					float speed = 12 + toPlayer.Length() * 0.005f;
					if(counter % 200 == 0)
                    {
						npc.velocity += toPlayer.SafeNormalize(Vector2.Zero) * speed;
					}
					npc.rotation += npc.velocity.X * 0.01f;
				}
				npc.ai[0]++;
			}
			if (phase == 2)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
					npc.netUpdate = true;
				direction = Main.rand.Next(2) * 2 - 1;
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
			if(counter >= 1440)
			{
				if (Main.netMode != 1)
				{
					npc.netUpdate = true;
				}
				phase = 1;
				npc.aiStyle = -1;
				npc.velocity.Y -= 0.014f;
				npc.dontTakeDamage = true;
			}
			int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 267);
			Dust dust = Main.dust[dust2];
			dust.color = new Color(VoidPlayer.EvilColor.R, VoidPlayer.EvilColor.G, VoidPlayer.EvilColor.B);
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
				Color color = npc.GetAlpha(Color.Black) * ((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			}
			return false;
		}	
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for(int i = 0; i < 50; i ++)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 267);
					dust.color = new Color(64, 72, 178);
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
			Color color = VoidPlayer.EvilColor * 1.3f;
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				Main.spriteBatch.Draw(texture,
				npc.Center + Main.rand.NextVector2Circular(4f, 4f) - Main.screenPosition,
				null, color, 0f, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			UpdateEyes(true);
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<DissolvingUmbra>(), 1);	
		}	
	}
	public class EvilEye
    {
		public Texture2D texture;
		public Texture2D texturePupil;
		public Vector2 offset;
		public float counter = 0;
		public EvilEye(Vector2 offset)
        {
			texture = ModContent.GetTexture("SOTS/NPCs/Constructs/EvilEye");
			texturePupil = ModContent.GetTexture("SOTS/NPCs/Constructs/EvilEyePupil");
			this.offset = offset;
        }
		public void Update()
        {
			counter++;
        }
		public void Draw(Vector2 center, float rotation)
        {
			Vector2 drawPosition = center + offset.RotatedBy(rotation) - Main.screenPosition;
			Vector2 origin = texture.Size() / 2;
			float alpha = counter / 40f;
			if (alpha > 1)
				alpha = 1;
			for(int i = 0; i < 5; i++)
			{
				int length = 0;
				if (i != 0)
					length = 1;
				Vector2 circular = new Vector2(length, 0).RotatedBy(i * MathHelper.Pi / 2f);
				Color color = VoidPlayer.EvilColor;
				color.A = 20;
				Main.spriteBatch.Draw(texture, drawPosition + circular, null, color * alpha, 0f, origin, 1f, SpriteEffects.None, 0f);
			}
		}
    }
}
