using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Banners;
using SOTS.Items.Fragments;
using SOTS.Items.Pyramid;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.AbandonedVillage
{
	public class Fistfull : ModNPC
	{
		private static int DustType => Main.rand.NextBool() ? DustType<FamishedDustCorruption>() : DustType<FamishedDustCrimson>();
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
        }
        public override void SetDefaults()
		{
			NPC.aiStyle = 3;
			NPC.width = 32;
			NPC.height = 60;
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
			int height = texture.Height / Main.npcFrameCount[NPC.type];
			Vector2 drawOrigin = new Vector2(texture.Width / 2, height / 2);
			Vector2 drawPos = NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY);
			Rectangle frame = new Rectangle(0, NPC.frame.Y, texture.Width, height);
			spriteBatch.Draw(texture, drawPos, frame, drawColor, NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			//texture = GetTexture("SOTS/NPCs/TeratomaGlow");
			//spriteBatch.Draw(texture, drawPos, frame, Color.White, npc.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			return false;
		}
        public override bool PreAI()
        {
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
			NPC.TargetClosest(true);
		}
		public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 6f)
            {
                NPC.frameCounter -= 6f;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
                {
                    NPC.frame.Y = 1 * frameHeight;
                }
            }
        }
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life > 0)
			{
				int num = 0;
				if (Main.netMode != NetmodeID.Server)
					while (num < hit.Damage / NPC.lifeMax * 40.0)
					{
						Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType, (float)(2.4f * hit.HitDirection), -2f, 0, default, 1.6f);
						num++;
					}
			}
			else
            {
                if (Main.netMode != NetmodeID.Server)
                    for (int k = 0; k < 45; k++)
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType, (float)(2.4f * hit.HitDirection), -2.1f, 0, default, 1.6f);
                    }
            }
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