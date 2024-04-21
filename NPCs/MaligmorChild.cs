using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
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
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		private float aiCounter
		{
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}
		private float aiCounter2
		{
			get => NPC.ai[2];
			set => NPC.ai[2] = value;
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
            Main.npcFrameCount[NPC.type] = 3;
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}
		public override void SetDefaults()
		{
            NPC.lifeMax = 40;  
            NPC.damage = 24; 
            NPC.defense = 4;  
            NPC.knockBackResist = 0.6f;
            NPC.width = 16;
            NPC.height = 16;
            NPC.value = 0;
            NPC.npcSlots = 0f;
			NPC.noGravity = true;
			NPC.alpha = 0;
			NPC.HitSound = SoundID.NPCHit19;
			NPC.DeathSound = SoundID.NPCDeath1;
			//Banner = NPC.type;
		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !fallen;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
			NPC.lifeMax = NPC.lifeMax * 3 / 4;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 3 * 0.5f);
			Vector2 drawPos = NPC.Center - screenPos;
			spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			texture = (Texture2D)Request<Texture2D>("SOTS/NPCs/MaligmorChildEye");
			spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			return false;
		}
		bool runOnce = true;
		bool fallen = false;
		public override bool PreAI()
		{
			if(runOnce && Main.netMode != NetmodeID.MultiplayerClient)
            {
				NPC.frame.Y = Main.rand.Next(3) * 26;
				NPC.netUpdate = true;
			}
			NPC.TargetClosest(true);
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
				if (owner.type != ModContent.NPCType<Maligmor>() || !owner.active)
				{
					fallen = true;
					NPC.velocity.X += Main.rand.NextFloat(-4f, 4f);
					NPC.velocity.Y += Main.rand.NextFloat(-4f, 4f);
					runOnce = false;
					return;
				}
				float bonusSpeedMult = 1.0f + ((NPC.whoAmI % 11 - 5) * 0.05f);
				Vector2 circular = owner.Center + new Vector2(36 + NPC.ai[3] + (float)Math.Sin(MathHelper.ToRadians(aiCounter)) * 4f, 0).RotatedBy(MathHelper.ToRadians(aiCounter2 * bonusSpeedMult));
				Vector2 goTo = circular - NPC.Center;
				float length = goTo.Length();
				float speed = 4f + length * 0.0005f;
				if (speed > length)
					speed = length;
				NPC.velocity *= 0.75f;
				NPC.velocity += goTo.SafeNormalize(Vector2.Zero) * speed;
				NPC.rotation = circular.Y * 0.06f;
				if (runOnce)
				{
					for (int k = 0; k < 20; k++)
					{
						Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustType<CurseDust>(), 0, 0, 0, default, 1.0f);
						dust.scale *= 1.2f;
						dust.velocity *= 0.6f;
						dust.velocity += NPC.velocity * 1.5f;
						dust.noGravity = true;
					}
					runOnce = false;
				}
				if (NPC.ai[3] > 0)
					NPC.ai[3] *= 0.993f;
				else
					NPC.ai[3] = 0;
			}
			else
            {
				NPC.velocity.X *= 1f;
				NPC.velocity.Y += 0.11f;
				NPC.rotation += NPC.velocity.X * 0.0006f;
				//npc.noTileCollide = false;
            }
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life > 0)
			{
				int num = 0;
				while (num < hit.Damage / NPC.lifeMax * 30.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CurseDust>(), (float)(2 * hit.HitDirection), -2f, 0, default, 1.2f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 30; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CurseDust>(), (float)(2 * hit.HitDirection), -2f, 0, default, 1.2f);
				}
			}
		}
        public override bool PreKill()
        {
			return false;
		}
	}
}