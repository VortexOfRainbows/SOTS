using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Pyramid;
using SOTS.WorldgenHelpers;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class Ghast : ModNPC
	{	float ai1 = 0;
		public override void SetDefaults()
		{
            NPC.lifeMax = 80;   
            NPC.damage = 30; 
            NPC.defense = 10;  
            NPC.knockBackResist = 0f;
            NPC.width = 34;
            NPC.height = 38;
			Main.npcFrameCount[NPC.type] = 4;  
            NPC.value = 1000;
            NPC.npcSlots = 0.6f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netUpdate = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.netAlways = true;
			Banner = NPC.type;
			BannerItem = ItemType<GhastBanner>();
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 8);
			Vector2 drawPos = NPC.Center - screenPos;
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height), NPC.GetAlpha(drawColor), NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			texture = (Texture2D)Request<Texture2D>("SOTS/NPCs/GhastGlow");
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height), Color.White, NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			return false;
		}
		public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			bool lineOfSight = Collision.CanHitLine(player.position, player.width, player.height, NPC.position, NPC.width, NPC.height);
			float speed = 0.65f + 0.35f * (float)Math.Sin(MathHelper.ToRadians(ai1 * 3));
			Vector2 toPlayer = player.Center - NPC.Center;
			float length = toPlayer.Length();
			if (lineOfSight || length <= 640)
			{
				if (length > 320 && !lineOfSight)
					speed *= 0.5f;
				toPlayer = toPlayer.SafeNormalize(Vector2.Zero);
				NPC.velocity.Y *= 0.9775f;
				NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero) * 0.1475f * speed;
			}
			if((!lineOfSight && length < 160) || SOTSWorldgenHelper.TrueTileSolid((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16))
            {
				NPC.alpha += 2;
            }
			else
            {
				NPC.alpha -= 3;
            }
			NPC.alpha = (int)MathHelper.Clamp(NPC.alpha, 0, 200);
			bool tileCollide = true;
			if(NPC.alpha >= 180)
            {
				tileCollide = false;
            }
			NPC.velocity.Y += 0.01f * (float)Math.Sin(MathHelper.ToRadians(ai1 * 6));
			for (int i = 0; i < 1 + Main.rand.Next(2); i++)
			{
				int num1 = Dust.NewDust(NPC.position, NPC.width - 4, 12, DustType<CurseDust>());
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity.X = NPC.velocity.X;
				Main.dust[num1].velocity.Y = -2 + i * 1.0f;
				Main.dust[num1].scale *= 1.25f + i * 0.15f;
			}
			int damage2 = SOTSNPCs.GetBaseDamage(NPC) / 2;

			ai1++;
			if (ai1 >= 600)
			{
				ai1 = 0;
			}
			//Main.NewText(ai1);
			/*if ((int)ai1 % 600 == 480 && (lineOfSight || length <= 320))
			{
				//Main.NewText("bruh");
				if (Main.netMode != NetmodeID.MultiplayerClient)
					for (int i = 0; i < 8; i ++)
					{
						Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileType<GhastDrop>(), damage2, 1f, Main.myPlayer, i * 45f, npc.whoAmI);
					}
            }
			else 
			{
			}*/
			if (Main.netMode != NetmodeID.MultiplayerClient && Main.rand.NextBool(75))
			{
				Vector2 spawn = (NPC.position + new Vector2(4, 4) + new Vector2(Main.rand.Next(NPC.width - 8), Main.rand.Next(NPC.height - 8)));
				Projectile.NewProjectile(NPC.GetSource_FromAI(), spawn, NPC.velocity * Main.rand.NextFloat(-0.1f, 0.1f), ProjectileType<GhastDrop>(), damage2, 1f, Main.myPlayer, -3, -1f);
			}
			if(tileCollide)
				NPC.velocity = Collision.TileCollision(NPC.position + new Vector2(8, 8), NPC.velocity, NPC.width - 16, NPC.height - 16, true);
		}
		public override void FindFrame(int frameHeight) 
		{
			NPC.frameCounter++;
			if (NPC.frameCounter >= 5f) 
			{
				NPC.frameCounter -= 5f;
				NPC.frame.Y += frameHeight;
				if(NPC.frame.Y >= 4 * frameHeight)
				{
					NPC.frame.Y = 0;
				}
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemType<CursedTumor>(), 2, 4, 6));
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life > 0)
			{
				int num = 0;
				while (num < hit.Damage / NPC.lifeMax * 50.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<CurseDust>(), (float)(2 * hit.HitDirection), -2f);
					num++;
				}
			}
            else
			{
				for (int k = 0; k < 30; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<CurseDust>(), (float)(2 * hit.HitDirection), -2f);
				}
			}		
		}
	}
}





















