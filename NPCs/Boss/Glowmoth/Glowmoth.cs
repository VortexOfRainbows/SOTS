using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Earth.Glowmoth;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Pyramid;
using SOTS.WorldgenHelpers;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.Boss.Glowmoth
{
	[AutoloadBossHead]
	public class Glowmoth : ModNPC
	{
		private float AI0
		{
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		private float AI1
		{
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}
		private float AI2
		{
			get => NPC.ai[2];
			set => NPC.ai[2] = value;
		}
		private float AI3
		{
			get => NPC.ai[3];
			set => NPC.ai[3] = value;
		}
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 10;
		}
        public override void SetDefaults()
		{
            NPC.lifeMax = 2000;   
            NPC.damage = 20; 
            NPC.defense = 12;  
            NPC.knockBackResist = 0f;
            NPC.width = 62;
            NPC.height = 94;
            NPC.value = 3000;
            NPC.npcSlots = 5f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netUpdate = true;
            NPC.HitSound = SoundID.NPCHit32;
            NPC.DeathSound = SoundID.NPCDeath5;
            NPC.netAlways = true;
			NPC.boss = true;
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * bossLifeScale * 14 / 20); //140%, 210%
			NPC.damage = (int)(NPC.damage * 0.75f); //150%, 225%
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / Main.npcFrameCount[NPC.type] / 2);
			Vector2 drawPos = NPC.Center - screenPos;
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, texture.Width, NPC.height), NPC.GetAlpha(drawColor), NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			texture = (Texture2D)Request<Texture2D>("SOTS/NPCs/Boss/Glowmoth/GlowmothGlow");
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, texture.Width, NPC.height), Color.White, NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			return false;
		}
        public override bool PreAI()
        {
            return base.PreAI();
        }
        public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
		}
        public override void PostAI()
        {
			float sinusoid = (float)Math.Sin(MathHelper.ToRadians(AI3 * 5));
			NPC.position += new Vector2(0, sinusoid * 0.75f);
			AI3++;
        }
        public override void FindFrame(int frameHeight) 
		{
			NPC.frameCounter++;
			if (NPC.frameCounter >= 3.5f) 
			{
				NPC.frameCounter -= 3.5f;
				NPC.frame.Y += frameHeight;
				if(NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
				{
					NPC.frame.Y = 0;
				}
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.BossBag(ItemType<GlowmothBag>()));
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life > 0)
			{
				int num = 0;
				while (num < damage / NPC.lifeMax * 50.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<CurseDust>(), (float)(2 * hitDirection), -2f);
					num++;
				}
			}
            else
			{
				for (int k = 0; k < 30; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<CurseDust>(), (float)(2 * hitDirection), -2f);
				}
			}		
		}
	}
}





















