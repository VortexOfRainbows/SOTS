using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Permafrost;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Constructs
{
	public class PermafrostSpirit : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			NPCID.Sets.TrailCacheLength[NPC.type] = 5;  
			NPCID.Sets.TrailingMode[NPC.type] = 0;
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
			NPC.aiStyle =10;
            NPC.lifeMax = 700; 
            NPC.damage = 60; 
            NPC.defense = 0;   
            NPC.knockBackResist = 0f;
            NPC.width = 58;
            NPC.height = 58;
			Main.npcFrameCount[NPC.type] = 1;   
            NPC.value = 35075;
            NPC.npcSlots = 4f;
            NPC.boss = false;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.netAlways = false;
			NPC.rarity = 2;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.damage = NPC.damage * 2 / 3;
			NPC.lifeMax = NPC.lifeMax * 6 / 7;
		}
		private int InitiateHealth = 2000;
		private float ExpertHealthMult = 1.5f;
		private float MasterHealthMult = 2f;
		int phase = 1;
		int counter = 0;
		public void SpellLaunch(Vector2 velocity)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				int damage = NPC.GetBaseDamage() / 2;
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<PermafrostSpike>(), damage, 0, Main.myPlayer, 180 - NPC.ai[1]);
			}
			//Terraria.Audio.SoundEngine.PlaySound(SoundID.Item92, (int)(npc.Center.X), (int)(npc.Center.Y));
		}
		public override void AI()
		{
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.15f / 255f, (255 - NPC.alpha) * 0.25f / 255f, (255 - NPC.alpha) * 0.65f / 255f);
			Player player = Main.player[NPC.target];
			if(phase == 3)
			{
				NPC.dontTakeDamage = false;
				if (Main.netMode != 1)
				{
					NPC.netUpdate = true;
				}
				if ((int)NPC.ai[0] % 270 == 269)
				{
					NPC.ai[1]++;
					if ((int)NPC.ai[1] % 10 == 0)
					{
						Vector2 toPlayer = player.Center - NPC.Center;
						toPlayer.Normalize();
						toPlayer *= Main.rand.Next(6,10);
						toPlayer.X += Main.rand.Next(-1, 2);
						toPlayer.Y += Main.rand.Next(-1, 2);
						SpellLaunch(toPlayer);
					}
					if (NPC.ai[1] >= 180)
					{
						SOTSUtils.PlaySound(SoundID.Item46, (int)NPC.Center.X, (int)NPC.Center.Y);
						NPC.ai[1] = 0;
						NPC.ai[0]++;
					}
				}
				else
				{
					NPC.ai[0]++;
				}
				Vector2 circleGen = new Vector2(45, 0).RotatedBy(MathHelper.ToRadians(NPC.ai[0]));
				Vector2 rotatePos = new Vector2(-360, 0).RotatedBy(MathHelper.ToRadians(circleGen.X + 90));


				Vector2 rotateAround = new Vector2(NPC.ai[1], 0).RotatedBy(MathHelper.ToRadians(NPC.ai[1] * 2));

				Vector2 toCircle = rotateAround + rotatePos + player.Center - NPC.Center;
				toCircle.Normalize();
				toCircle *= 4f;

				NPC.velocity = toCircle;
			}
			if(phase == 2)
			{
				if (Main.netMode != 1)
				{
					NPC.netUpdate = true;
				}
				NPC.dontTakeDamage = false;
				NPC.aiStyle =-1;
				NPC.ai[0] = 0;
				NPC.ai[1] = 0;
				phase = 3;
			}
			else if(phase == 1)
			{
				counter++;
			}
			if(Main.player[NPC.target].dead)
			{
				counter++;
			}
			if(counter >= 1320)
			{
				if (Main.netMode != 1)
				{
					NPC.netUpdate = true;
				}
				phase = 1;
				NPC.aiStyle =-1;
				NPC.velocity.Y -= 0.014f;
				NPC.dontTakeDamage = true;
			}
			int dust2 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 267);
			Dust dust = Main.dust[dust2];
			dust.color = new Color(65, 136, 164);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 2f;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			for (int k = 0; k < NPC.oldPos.Length; k++) {
				Vector2 drawPos = NPC.oldPos[k] - screenPos + drawOrigin + new Vector2(0f, NPC.gfxOffY);
				Color color = NPC.GetAlpha(drawColor) * ((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			}
			return false;
		}	
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				if (Main.netMode != NetmodeID.Server)
					for (int i = 0; i < 50; i++)
					{
						int dust3 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 267);
						Dust dust4 = Main.dust[dust3];
						dust4.velocity *= 2.5f;
						dust4.color = new Color(65, 136, 164);
						dust4.noGravity = true;
						dust4.fadeIn = 0.1f;
						dust4.scale *= 2.5f;
					}
				if (phase == 1)
				{
					phase = 2;
					NPC.lifeMax = (int)(InitiateHealth * (Main.masterMode ? MasterHealthMult : Main.expertMode ? ExpertHealthMult : 1));
					NPC.life = NPC.lifeMax;
				}
			}
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.45f;
				float y = Main.rand.Next(-10, 11) * 0.45f;
				spriteBatch.Draw(texture,
				new Vector2((float)(NPC.Center.X - (int)screenPos.X) + x, (float)(NPC.Center.Y - (int)screenPos.Y) + y),
				null, color, 0f, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DissolvingAurora>()));
		}
	}
}
