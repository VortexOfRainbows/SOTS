using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Conduit;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Anomaly;
using SOTS.Projectiles.Pyramid;
using SOTS.WorldgenHelpers;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.Tide
{
	public class PhantarayBig : ModNPC
	{
		public const float AttackWindup = 75;
        public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 11;
		}
        public override void SetDefaults()
		{
            NPC.lifeMax = 900;   
            NPC.damage = 75; 
            NPC.defense = 24;  
            NPC.knockBackResist = 0f;
            NPC.width = 152;
            NPC.height = 126;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.npcSlots = 1f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netUpdate = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.netAlways = true;
			Banner = NPC.type;
			BannerItem = ItemType<UltracapBanner>();
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / (2 * Main.npcFrameCount[NPC.type]));
			Vector2 drawPos = NPC.Center - screenPos;
			spriteBatch.Draw(texture, drawPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			return false;
		}
		public override void AI()
		{
			NPC.TargetClosest(false);
			Player player = Main.player[NPC.target];
			Vector2 toPlayer = player.Center - NPC.Center;
			float length = toPlayer.Length();
			float speed = 4f + length * 0.00005f;
			toPlayer = toPlayer.SafeNormalize(Vector2.Zero) * speed;
			float sinusoid = 0.5f + 0.5f * (float)Math.Sin(NPC.ai[0]++ * MathHelper.TwoPi / 180f);
			if(NPC.wet)
            {
                NPC.velocity += toPlayer;
                NPC.velocity *= (0.1f + sinusoid) * 0.5f;
            }
            NPC.rotation = toPlayer.ToRotation() + MathHelper.PiOver2;
            Tile tile = Framing.GetTileSafely((NPC.Center / 16).ToPoint());
			if (tile.LiquidAmount > 0)
			{
				NPC.wet = true;
			}
			else
				NPC.wet = false;
			if(!NPC.wet)
			{
				NPC.velocity.Y += 0.05f;
			}
			NPC.alpha = (int)MathHelper.Lerp(205, 50, sinusoid);
		}
		public void CheckOtherCollision()
        {
			Vector2 nudge = Vector2.Zero;
			for(int i = 0; i < Main.maxNPCs; i++)
            {
				NPC npc = Main.npc[i];
				if (npc.active && npc.type == Type && npc.Hitbox.Intersects(NPC.Hitbox))
                {
					Vector2 away = NPC.Center - npc.Center;
					nudge += away * 0.01f;
                }
            }
			NPC.velocity += nudge;
        }
		public override void FindFrame(int frameHeight) 
		{
			NPC.frameCounter++;
			if (NPC.frameCounter >= 6f) 
			{
				NPC.frameCounter -= 6f;
				NPC.frame.Y += frameHeight;
				if(NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
				{
					NPC.frame.Y = 0;
				}
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			//npcLoot.Add(ItemDropRule.Common(ItemType<SkipSoul>(), 1, 1, 2));
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{
				for (int k = 0; k < 30; k++)
				{
					Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustType<CopyDust4>(), (float)(2 * hit.HitDirection), -2f);
					d.velocity *= 1.0f;
					d.fadeIn = 0.2f;
					d.noGravity = true;
					d.scale *= 1.5f;
					d.color = ColorHelpers.TideColor;
				}
			}		
		}
	}
}





















