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
			writer.Write(NPC.frame.Y);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			NPC.frame.Y = reader.ReadInt32();
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Maligmor");
		}
		public override void SetDefaults()
		{
            NPC.lifeMax = 40;  
            NPC.damage = 24; 
            NPC.defense = 4;  
            NPC.knockBackResist = 0.6f;
            NPC.width = 16;
            NPC.height = 16;
			Main.npcFrameCount[NPC.type] = 3;  
            npc.value = 0;
            npc.npcSlots = 0f;
			npc.noGravity = true;
			npc.alpha = 0;
			npc.HitSound = SoundID.NPCHit19;
			npc.DeathSound = SoundID.NPCDeath1;
			//Banner = NPC.type;
		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !fallen;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
			NPC.lifeMax = 60;
            base.ScaleExpertStats(numPlayers, bossLifeScale);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[npc.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 3 * 0.5f);
			Vector2 drawPos = npc.Center - Main.screenPosition;
			spriteBatch.Draw(texture, drawPos, npc.frame, drawColor, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			texture = GetTexture("SOTS/NPCs/MaligmorChildEye");
			spriteBatch.Draw(texture, drawPos, npc.frame, Color.White, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			return false;
		}
		bool runOnce = true;
		bool fallen = false;
		public override bool PreAI()
		{
			if(runOnce && Main.netMode != 1)
            {
				NPC.frame.Y = Main.rand.Next(3) * 26;
				npc.netUpdate = true;
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
			if(!fallen)
			{
				if (owner.type != mod.NPCType("Maligmor") || !owner.active)
				{
					fallen = true;
					npc.velocity.X += Main.rand.NextFloat(-4f, 4f);
					npc.velocity.Y += Main.rand.NextFloat(-4f, 4f);
					runOnce = false;
					return;
				}
				float bonusSpeedMult = 1.0f + ((npc.whoAmI % 11 - 5) * 0.05f);
				Vector2 circular = owner.Center + new Vector2(36 + npc.ai[3] + (float)Math.Sin(MathHelper.ToRadians(aiCounter)) * 4f, 0).RotatedBy(MathHelper.ToRadians(aiCounter2 * bonusSpeedMult));
				Vector2 goTo = circular - npc.Center;
				float length = goTo.Length();
				float speed = 4f + length * 0.0005f;
				if (speed > length)
					speed = length;
				npc.velocity *= 0.75f;
				npc.velocity += goTo.SafeNormalize(Vector2.Zero) * speed;
				npc.rotation = circular.Y * 0.06f;
				if (runOnce)
				{
					for (int k = 0; k < 20; k++)
					{
						Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, ModContent.DustType<CurseDust>(), 0, 0, 0, default, 1.0f);
						dust.scale *= 1.2f;
						dust.velocity *= 0.6f;
						dust.velocity += npc.velocity * 1.5f;
						dust.noGravity = true;
					}
					runOnce = false;
				}
				if (npc.ai[3] > 0)
					npc.ai[3] *= 0.993f;
				else
					npc.ai[3] = 0;
			}
			else
            {
				npc.velocity.X *= 1f;
				npc.velocity.Y += 0.11f;
				npc.rotation += npc.velocity.X * 0.0006f;
				//npc.noTileCollide = false;
            }
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while (num < damage / npc.lifeMax * 30.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<CurseDust>(), (float)(2 * hitDirection), -2f, 0, default, 1.2f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 30; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<CurseDust>(), (float)(2 * hitDirection), -2f, 0, default, 1.2f);
				}
			}
		}
		public override bool PreNPCLoot()
		{
			return false;
		}
	}
}