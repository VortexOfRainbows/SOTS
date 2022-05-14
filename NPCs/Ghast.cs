using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Pyramid;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class Ghast : ModNPC
	{	float ai1 = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghast");
		}
		public override void SetDefaults()
		{
            NPC.lifeMax = 80;   
            NPC.damage = 30; 
            NPC.defense = 10;  
            NPC.knockBackResist = 0f;
            NPC.width = 34;
            NPC.height = 38;
			Main.npcFrameCount[NPC.type] = 4;  
            npc.value = 1000;
            npc.npcSlots = 0.6f;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.netUpdate = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = true;
			Banner = NPC.type;
			BannerItem = ItemType<GhastBanner>();
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[npc.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 8);
			Vector2 drawPos = npc.Center - Main.screenPosition;
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, npc.width, npc.height), npc.GetAlpha(drawColor), npc.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			texture = GetTexture("SOTS/NPCs/GhastGlow");
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, npc.width, npc.height), Color.White, npc.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			return false;
		}
		public override void AI()
		{
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			bool lineOfSight = Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height);
			float speed = 0.65f + 0.35f * (float)Math.Sin(MathHelper.ToRadians(ai1 * 3));
			Vector2 toPlayer = player.Center - npc.Center;
			float length = toPlayer.Length();
			if (lineOfSight || length <= 640)
			{
				if (length > 320 && !lineOfSight)
					speed *= 0.5f;
				toPlayer = toPlayer.SafeNormalize(Vector2.Zero);
				npc.velocity.Y *= 0.9775f;
				npc.velocity += toPlayer.SafeNormalize(Vector2.Zero) * 0.1475f * speed;
			}
			if((!lineOfSight && length < 160) || SOTSWorldgenHelper.TrueTileSolid((int)npc.Center.X / 16, (int)npc.Center.Y / 16))
            {
				npc.alpha += 2;
            }
			else
            {
				npc.alpha -= 3;
            }
			npc.alpha = (int)MathHelper.Clamp(npc.alpha, 0, 200);
			bool tileCollide = true;
			if(npc.alpha >= 180)
            {
				tileCollide = false;
            }
			npc.velocity.Y += 0.01f * (float)Math.Sin(MathHelper.ToRadians(ai1 * 6));
			for (int i = 0; i < 1 + Main.rand.Next(2); i++)
			{
				int num1 = Dust.NewDust(npc.position, npc.width - 4, 12, DustType<CurseDust>());
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity.X = npc.velocity.X;
				Main.dust[num1].velocity.Y = -2 + i * 1.0f;
				Main.dust[num1].scale *= 1.25f + i * 0.15f;
			}
			int damage2 = npc.damage / 2;
			if (Main.expertMode)
			{
				damage2 = (int)(damage2 / Main.expertDamage);
			}

			ai1++;
			if (ai1 >= 600)
			{
				ai1 = 0;
			}
			//Main.NewText(ai1);
			/*if ((int)ai1 % 600 == 480 && (lineOfSight || length <= 320))
			{
				//Main.NewText("bruh");
				if (Main.netMode != 1)
					for (int i = 0; i < 8; i ++)
					{
						Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileType<GhastDrop>(), damage2, 1f, Main.myPlayer, i * 45f, npc.whoAmI);
					}
            }
			else 
			{
			}*/
			if (Main.netMode != 1 && Main.rand.NextBool(75))
			{
				Vector2 spawn = (npc.position + new Vector2(4, 4) + new Vector2(Main.rand.Next(npc.width - 8), Main.rand.Next(npc.height - 8)));
				Projectile.NewProjectile(spawn, npc.velocity * Main.rand.NextFloat(-0.1f, 0.1f), ProjectileType<GhastDrop>(), damage2, 1f, Main.myPlayer, -3, -1f);
			}
			if(tileCollide)
				npc.velocity = Collision.TileCollision(npc.position + new Vector2(8, 8), npc.velocity, npc.width - 16, npc.height - 16, true);
		}
		public override void FindFrame(int frameHeight) 
		{
			npc.frameCounter++;
			if (npc.frameCounter >= 5f) 
			{
				npc.frameCounter -= 5f;
				NPC.frame.Y += frameHeight;
				if(NPC.frame.Y >= 4 * frameHeight)
				{
					NPC.frame.Y = 0;
				}
			}
		}
		public override void NPCLoot()
		{
			if(SOTSWorld.downedCurse && Main.rand.NextBool(3))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<CursedMatter>(), Main.rand.Next(2) + 1);	
			else
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<SoulResidue>(), Main.rand.Next(2) + 1);
			if (Main.rand.NextBool(2))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<CursedTumor>(), Main.rand.Next(3) + 4);
		}
		public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life > 0)
			{
				int num = 0;
				while (num < damage / npc.lifeMax * 50.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustType<CurseDust>(), (float)(2 * hitDirection), -2f);
					num++;
				}
			}
            else
			{
				for (int k = 0; k < 30; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustType<CurseDust>(), (float)(2 * hitDirection), -2f);
				}
			}		
		}
	}
}





















