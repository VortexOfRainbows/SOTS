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
		float ai1 = 60;
        public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 10;
		}
        public override void SetDefaults()
		{
            NPC.lifeMax = 100;   
            NPC.damage = 40; 
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
			if (NPC.ai[0] > 0)
			{
				DrawLaserTelegraph(spriteBatch, screenPos);
			}
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height), NPC.GetAlpha(drawColor), NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			return false;
		}
		public void DrawLaserTelegraph(SpriteBatch spriteBatch, Vector2 screenPos)
		{
			Texture2D texture = ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/PixelLaser").Value;
			Vector2 drawOrigin = new Vector2(0, 1);
			Vector2 drawPos = NPC.Center - screenPos;
			float percent = NPC.ai[0] / ai1;
			if (percent > 1f)
				percent = 1f;
			for(int i = -4; i <= 4; i++)
            {
				Color color = Color.Lerp(new Color(213, 66, 232), new Color(191, 190, 238), 0.5f + i * 0.125f);
				color.A = 0;
				float degrees = i * 10f;
				float antiDegrees = Math.Sign(i) * 40f * percent * percent;
				if (Math.Abs(antiDegrees) > Math.Abs(degrees))
				{
					antiDegrees = degrees;
				}
				degrees -= antiDegrees;
				float rotation = MathHelper.ToRadians(degrees);
				Vector2 offset = new Vector2(-i * 0.75f, 0);
				float size = SearchForTileLength(NPC.Center + offset, NPC.Center + new Vector2(0, 1200).RotatedBy(rotation));
				spriteBatch.Draw(texture, drawPos + offset, null, color * (float)Math.Sqrt(percent) * (1 - Math.Abs(degrees) / 40f) * 0.75f, MathHelper.ToRadians(degrees + 90), drawOrigin, new Vector2(size / texture.Width, 1f), SpriteEffects.None, 0f);
			}
		}
		public float SearchForTileLength(Vector2 start, Vector2 end)
		{
			Vector2 currentPosition = start;
			float total = (start - end).Length();
			float iterator = 2f / total;
			for (float b = 0; b <= 1f; b += iterator)
			{
				currentPosition = Vector2.Lerp(start, end, b);
				int i = (int)currentPosition.X / 16;
				int j = (int)currentPosition.Y / 16;
				if (WorldgenHelpers.SOTSWorldgenHelper.TrueTileSolid(i, j))
				{
					return (start - currentPosition).Length();
				}
			}
			return (start - currentPosition).Length();
		}
		public override void AI()
		{
			ai1 = 80;
			NPC.TargetClosest(false);
			Player player = Main.player[NPC.target];
			Vector2 toPlayer = player.Center + new Vector2(0, -128) - NPC.Center;
			float length = toPlayer.Length();
			float speed = 0.85f + 0.35f * (float)Math.Sin(MathHelper.ToRadians(NPC.ai[0] * 3)) + length * 0.0076f;
			toPlayer = toPlayer.SafeNormalize(Vector2.Zero);
			if (NPC.ai[0] > 0)
			{
				NPC.velocity *= 0.9f;
				if (NPC.ai[0] > ai1 + 10)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int damage2 = SOTSNPCs.GetBaseDamage(NPC) / 2;
						Vector2 spawn = NPC.Center - new Vector2(0, 4);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), spawn, new Vector2(0, 4), ProjectileType<UltraLaser>(), damage2, 1f, Main.myPlayer, -3, -1f);
					}
					NPC.velocity.Y -= 4f;
					NPC.ai[0] = 0;
					if (Main.netMode == NetmodeID.Server)
						NPC.netUpdate = true;
				}
				else
					NPC.ai[0]++;
				NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero) * 0.05f * speed;
				NPC.velocity.Y += 0.0025f * (float)Math.Sin(MathHelper.ToRadians(NPC.ai[1] * 6));
				NPC.ai[1]++;
			}
			else 
			{
				if (length < 24f || Main.rand.NextBool(240))
                {
					NPC.ai[0]++;
					if (Main.netMode == NetmodeID.Server)
						NPC.netUpdate = true;
                }
				else
				{
					NPC.velocity.Y *= 0.9775f;
					NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero) * 0.1475f * speed;
					NPC.velocity.Y += 0.01f * (float)Math.Sin(MathHelper.ToRadians(NPC.ai[1] * 6));
					NPC.ai[1]++;
				}
			}
			if (!Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(NPC.position + new Vector2(-4, NPC.height - 17), NPC.width, 12, DustType<PixelDust>(), 0, 0, 0, Color.Lerp(ColorHelpers.VoidAnomaly, Color.Black, Main.rand.NextFloat(1f)), 1f);
				dust.color.A = 0;
				dust.noGravity = true;
				dust.velocity.X = NPC.velocity.X + Main.rand.NextFloat(-1, 1f);
				dust.velocity.Y += 2.5f;
				dust.fadeIn = 8;
			}
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





















