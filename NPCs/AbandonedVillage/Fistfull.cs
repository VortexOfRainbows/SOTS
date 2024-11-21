using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Fragments;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.AbandonedVillage
{
	public class Fistfull : ModNPC
	{
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(fistPosition);
            writer.WriteVector2(fistVelo);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            fistPosition = reader.ReadVector2();
            fistVelo = reader.ReadVector2();
        }
        private Vector2 fistPosition;
        private Vector2 fistVelo;
		private static int DustType => Main.rand.NextBool() ? DustType<FamishedDustCorruption>() : DustType<FamishedDustCrimson>();
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
        }
        public override void SetDefaults()
		{
			NPC.aiStyle = 3;
            NPC.width = 24; //Has to be smaller than the sprite size to allow jumping over blocks properly
            NPC.height = 40; //Has to be shorter than the sprite height to prevent falling through platforms erroneously 
            //Very similar stats to face monster
            NPC.damage = 25;
            NPC.lifeMax = 70;
			NPC.defense = 10;
            NPC.knockBackResist = 0.4f;
            NPC.value = Item.buyPrice(0, 0, 2, 50);
			NPC.scale = 1.0f;
			NPC.HitSound = SoundID.NPCHit19;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.noTileCollide = false;
			//Banner = NPC.type;
			//BannerItem = ItemType<TeratomaBanner>();
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            Texture2D textureF = Request<Texture2D>(this.Texture + "Hand").Value;
            Texture2D textureIdle = Request<Texture2D>(this.Texture + "Punch").Value;
			int height = texture.Height / Main.npcFrameCount[NPC.type];
            Vector2 drawOrigin = new Vector2(texture.Width / 2, height / 2 + 8);
			Vector2 drawPos = NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY);
			Rectangle frame = new Rectangle(0, NPC.frame.Y, texture.Width, height);
            if (fistPosition != NPC.Center && !runOnce && NPC.localAI[3] > 60)
            {
                spriteBatch.Draw(textureIdle, drawPos, null, drawColor, NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
                Player player = Main.player[NPC.target];
                Vector2 toPlayer = player.Center - fistPosition;
                float scale = 0.75f + 0.25f * Math.Min(1, fistPosition.Distance(NPC.Center) / 80f);
                spriteBatch.Draw(textureF, fistPosition - screenPos, null, Lighting.GetColor((int)fistPosition.X / 16, (int)fistPosition.Y / 16), toPlayer.ToRotation() + MathF.PI / 2f, textureF.Size() / 2, NPC.scale * scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
            else
            {
			    spriteBatch.Draw(texture, drawPos, frame, drawColor, NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }
            //texture = GetTexture("SOTS/NPCs/TeratomaGlow");
            //spriteBatch.Draw(texture, drawPos, frame, Color.White, npc.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
            return false;
		}
        private bool runOnce = true;
        public override bool PreAI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            if (runOnce)
            {
                fistPosition = NPC.Center;
                runOnce = false;
            }
            fistPosition += fistVelo + NPC.velocity * 0.5f;
            NPC.localAI[3]++;
            if (NPC.localAI[3] > 60)
            {
                float speedM = MathF.Min(1, (NPC.localAI[3] - 60f) / 30f);
                NPC.velocity.X *= 0.1f;
                Vector2 toPlayer = player.Center - fistPosition;
                fistVelo *= 0.925f;
                fistVelo += toPlayer.SNormalize() * 0.2f * speedM;
                Vector2 toNPC = NPC.Center - fistPosition;
                fistPosition = Vector2.Lerp(fistPosition, NPC.Center, 0.012f);
                fistVelo += toNPC * 0.00005f * NPC.localAI[3] / 120f;
                if (NPC.localAI[3] < 280)
                {
                    if (NPC.localAI[3] % 60 == 0)
                    {
                        fistVelo += toPlayer * 0.0125f + toPlayer.SNormalize() * 7f;
                    }
                    if (NPC.localAI[3] > 90 && NPC.localAI[3] % 60 > 30)
                    {
                        speedM = MathF.Sin(NPC.localAI[3] % 60 / 60f * MathF.PI);
                        fistVelo += toNPC * 0.0015f * speedM + toNPC.SNormalize() * 0.12f;
                    }
                }
                else
                {
                    fistPosition = Vector2.Lerp(fistPosition, NPC.Center, (NPC.localAI[3] - 280) / 120f);
                    if(fistPosition.Distance(NPC.Center) < 10)
                    {
                        NPC.localAI[3] = -30;
                    }
                }
            }
            else
            {
                fistPosition = NPC.Center;
            }
            return base.PreAI();
        }
        public override void AI()
		{
			if (NPC.velocity.Y == 0 && Math.Abs(NPC.velocity.X) > 0.5f && !Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(NPC.position + new Vector2(0, (NPC.height - 2) * NPC.scale) - new Vector2(5), (int)(NPC.width * NPC.scale), 4, DustType, 0, 0, 0, default, 0.8f);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
				NPC.velocity.X *= 0.97125f;
			}
            NPC.spriteDirection = NPC.direction;
		}
		public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.5f + MathF.Sqrt(MathF.Abs(NPC.velocity.X * 0.5f));
            if (NPC.frameCounter >= 8f)
            {
                NPC.frameCounter -= 8f;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
                {
                    NPC.frame.Y = 1 * frameHeight;
                }
            }
        }
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life > 0)
            {
                for (int num = 0; num < hit.Damage / NPC.lifeMax * 40f; num++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType, (float)(2.4f * hit.HitDirection), -2f, 0, default, 1.6f);
            }
			else
            {
                for (int k = 0; k < 45; k++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType, (float)(2.4f * hit.HitDirection), -2.1f, 0, default, 1.6f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position - new Vector2(0, 20), NPC.velocity, ModGores.GoreType("Gores/Fistfull/FistfullGore1"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 4), NPC.velocity, ModGores.GoreType("Gores/Fistfull/FistfullGore2"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(6, 24), NPC.velocity, ModGores.GoreType("Gores/Fistfull/FistfullGore3"), 1f);
            }
        }
        public override bool ModifyCollisionData(Rectangle victimHitbox, ref int immunityCooldownSlot, ref MultipliableFloat damageMultiplier, ref Rectangle npcHitbox)
        {
            bool colliding = NPC.Hitbox.Intersects(victimHitbox);
            if (colliding)
                return true;
            int width = 32;
            Rectangle hitBox = new Rectangle((int)fistPosition.X - width/2, (int)fistPosition.Y - width/2, width, width);
            if (hitBox.Intersects(victimHitbox))
            {
                npcHitbox = victimHitbox;
            }
            return false;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Vertebrae, 5));
            npcLoot.Add(ItemDropRule.Common(ItemID.RottenChunk, 5));
            npcLoot.Add(ItemDropRule.Common(ItemType<FragmentOfEvil>(), 5));
            npcLoot.Add(ItemDropRule.Common(ItemType<OldKey>(), 100));
        }
	}
}