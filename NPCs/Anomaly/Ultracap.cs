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

namespace SOTS.NPCs.Anomaly
{
	public class Ultracap : ModNPC
	{
		float ai1 = 600;
        public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 10;
		}
        public override void SetDefaults()
		{
            NPC.lifeMax = 80;   
            NPC.damage = 30; 
            NPC.defense = 16;  
            NPC.knockBackResist = 0f;
            NPC.width = 36;
            NPC.height = 26;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.npcSlots = 1f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netUpdate = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.netAlways = true;
			//Banner = NPC.type;
			//BannerItem = ItemType<GhastBanner>();
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 20);
			Vector2 drawPos = NPC.Center - screenPos;
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height), NPC.GetAlpha(drawColor), NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			return false;
		}
		public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			bool lineOfSight = Collision.CanHitLine(player.position, player.width, player.height, NPC.position, NPC.width, NPC.height);
			Vector2 toPlayer = player.Center + new Vector2(0, -128) - NPC.Center;
			float length = toPlayer.Length();
			float speed = 0.85f + 0.35f * (float)Math.Sin(MathHelper.ToRadians(NPC.ai[0] * 3)) + length * 0.01f;
			if (lineOfSight || length <= 640)
			{
				if (length > 320 && !lineOfSight)
					speed *= 0.5f;
				toPlayer = toPlayer.SafeNormalize(Vector2.Zero);
				NPC.velocity.Y *= 0.9775f;
				NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero) * 0.1475f * speed;
			}
			NPC.velocity.Y += 0.01f * (float)Math.Sin(MathHelper.ToRadians(NPC.ai[0] * 6));
			if(!Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(NPC.position + new Vector2(-4, NPC.height - 17), NPC.width, 12, DustType<PixelDust>(), 0, 0, 0, Color.Lerp(ColorHelpers.VoidAnomaly, Color.Black, Main.rand.NextFloat(1f)), 1f);
				dust.color.A = 0;
				dust.noGravity = true;
				dust.velocity.X = NPC.velocity.X + Main.rand.NextFloat(-1, 1f);
				dust.velocity.Y += 2.5f;
				dust.fadeIn = 8;
			}
			int damage2 = SOTSNPCs.GetBaseDamage(NPC) / 2;

			NPC.ai[0]++;
			if (NPC.ai[0] >= 600)
			{
				NPC.ai[0] = 0;
			}
			if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[0] % 60 == 0)
			{
				Vector2 spawn = NPC.Center + new Vector2(0, 16);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), spawn, new Vector2(0, 4), ProjectileType<UltraLaser>(), damage2, 1f, Main.myPlayer, -3, -1f);
			}
			//if(tileCollide)
			//	NPC.velocity = Collision.TileCollision(NPC.position + new Vector2(8, 8), NPC.velocity, NPC.width - 16, NPC.height - 16, true);
		}
		public override void FindFrame(int frameHeight) 
		{
			NPC.frameCounter++;
			if (NPC.frameCounter >= 3f) 
			{
				NPC.frameCounter -= 3f;
				NPC.frame.Y += frameHeight;
				if(NPC.frame.Y >= 10 * frameHeight)
				{
					NPC.frame.Y = 0;
				}
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemType<SkipSoul>(), 1, 1, 1));
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





















