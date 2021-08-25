using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class MaligmorChild : ModNPC
	{
		private float ownerID
		{
			get => npc.ai[0];
			set => npc.ai[0] = value;
		}
		private float aiCounter
		{
			get => npc.ai[1];
			set => npc.ai[1] = value;
		}
		private float aiCounter2
		{
			get => npc.ai[2];
			set => npc.ai[2] = value;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(npc.frame.Y);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			npc.frame.Y = reader.ReadInt32();
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Maligmor");
		}
		public override void SetDefaults()
		{
            npc.lifeMax = 70;  
            npc.damage = 10; 
            npc.defense = 0;  
            npc.knockBackResist = 0.5f;
            npc.width = 26;
            npc.height = 26;
			Main.npcFrameCount[npc.type] = 3;  
            npc.value = 0;
            npc.npcSlots = 0f;
			npc.noGravity = true;
			npc.alpha = 0;
			npc.HitSound = SoundID.NPCHit19;
			npc.DeathSound = SoundID.NPCDeath1;
			//banner = npc.type;
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
			npc.lifeMax = 100;
            base.ScaleExpertStats(numPlayers, bossLifeScale);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 3 * 0.5f);
			Vector2 drawPos = npc.Center - Main.screenPosition;
			spriteBatch.Draw(texture, drawPos, npc.frame, drawColor, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			texture = GetTexture("SOTS/NPCs/MaligmorChildEye");
			spriteBatch.Draw(texture, drawPos, npc.frame, Color.White, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			return false;
		}
		bool runOnce = true;
		public override bool PreAI()
		{
			if(runOnce && Main.netMode != 1)
            {
				npc.frame.Y = Main.rand.Next(3) * npc.height;
				npc.netUpdate = true;
				runOnce = false;
            }
			npc.TargetClosest(true);
			return true;
		}
		public override void AI()
		{
			//Player player = Main.player[npc.target];
			NPC owner = Main.npc[(int)ownerID];
			aiCounter2++;
			aiCounter++;
			if (owner.type != mod.NPCType("Maligmor") || !owner.active)
			{
				npc.active = false;
			}
			float bonusSpeedMult = 1.0f + ((npc.whoAmI % 11 - 5) * 0.05f);
			Vector2 circular = owner.Center + new Vector2(32 + (float)Math.Sin(MathHelper.ToRadians(aiCounter)) * 3.5f, 0).RotatedBy(MathHelper.ToRadians(aiCounter2 * bonusSpeedMult));
			Vector2 goTo = circular - npc.Center;
			float length = goTo.Length();
			float speed = 5f + length * 0.0007f;
			if (speed > length)
				speed = length;
			npc.velocity *= 0.75f;
			npc.velocity += goTo.SafeNormalize(Vector2.Zero) * speed;
		}
		public override void HitEffect(int hitDirection, double damage)
		{

		}
		public override bool PreNPCLoot()
		{
			return false;
		}
	}
}