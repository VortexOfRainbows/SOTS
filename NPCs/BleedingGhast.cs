using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Banners;
using SOTS.Projectiles.Pyramid;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class BleedingGhast : ModNPC
	{	float ai1 = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bleeding Ghast");
		}
		public override void SetDefaults()
		{
            npc.lifeMax = 150;   
            npc.damage = 70; 
            npc.defense = 16;  
            npc.knockBackResist = 0f;
            npc.width = 48;
            npc.height = 56;
			Main.npcFrameCount[npc.type] = 4;  
            npc.value = 4450;
            npc.npcSlots = 0.6f;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.netUpdate = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = true;
			banner = npc.type;
			bannerItem = ItemType<BleedingGhastBanner>();
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 8);
			Vector2 drawPos = npc.Center - Main.screenPosition;
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, npc.frame.Y, 48, 56), drawColor, npc.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			texture = GetTexture("SOTS/NPCs/BleedingGhastGlow");
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, npc.frame.Y, 48, 56), Color.White, npc.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
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
				npc.velocity.Y *= 0.98f;
				npc.velocity += toPlayer.SafeNormalize(Vector2.Zero) * 0.15f * speed;
			}
			npc.velocity.Y += 0.02f * (float)Math.Sin(MathHelper.ToRadians(ai1 * 6));
			for (int i = 0; i < 2; i++)
			{
				int num1 = Dust.NewDust(npc.position, npc.width - 4, 24, mod.DustType("CurseDust"));
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity.X = npc.velocity.X;
				Main.dust[num1].velocity.Y = -3 + i * 1.5f;
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
				Vector2 spawn = (npc.position + new Vector2(8, 8) + new Vector2(Main.rand.Next(npc.width - 16), Main.rand.Next(npc.height - 16)));
				Projectile.NewProjectile(spawn, npc.velocity * Main.rand.NextFloat(-0.1f, 0.1f), ProjectileType<GhastDrop>(), damage2, 1f, Main.myPlayer, -2, -1f);
			}
			npc.velocity = Collision.TileCollision(npc.position + new Vector2(8, 8), npc.velocity, npc.width - 16, npc.height - 16, true);
		}
		public override void FindFrame(int frameHeight) 
		{
			npc.frameCounter++;
			if (npc.frameCounter >= 5f) 
			{
				npc.frameCounter -= 5f;
				npc.frame.Y += frameHeight;
				if(npc.frame.Y >= 4 * frameHeight)
				{
					npc.frame.Y = 0;
				}
			}
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("CursedMatter"), Main.rand.Next(2) + 2);	
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ItemID.Ichor, Main.rand.Next(4) + 2);
			if (Main.rand.NextBool(10))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CursedCaviar"), 1);
		}
		public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life > 0)
			{
				int num = 0;
				while (num < damage / npc.lifeMax * 50.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("CurseDust"), (float)(2 * hitDirection), -2f);
					num++;
				}
			}
            else
			{
				for (int k = 0; k < 30; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("CurseDust"), (float)(2 * hitDirection), -2f);
				}
			}		
		}
	}
}





















