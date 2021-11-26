using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Constructs
{
	public class PermafrostConstruct : ModNPC
	{
		int timer = 0;
		int ai1 = 0;
		float dir = 0f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Permafrost Construct");
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 26; //unicorn AI
			npc.lifeMax = 275;  
			npc.damage = 25; 
			npc.defense = 10;  
			npc.knockBackResist = 0.1f;
			npc.width = 90;
			npc.height = 90;
			Main.npcFrameCount[npc.type] = 1;  
			npc.value = Item.buyPrice(0, 1, 0, 0);
			npc.npcSlots = 3f;
			npc.boss = false;
			npc.lavaImmune = false;
			npc.noGravity = false;
			npc.noTileCollide = false;
			npc.netAlways = true;
			npc.alpha = 255;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.rarity = 5;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D textureG = ModContent.GetTexture("SOTS/NPCs/Constructs/PermafrostConstructHeadGlow");
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Constructs/PermafrostConstructHead");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = npc.Center - Main.screenPosition + new Vector2(0f, npc.gfxOffY);
			Texture2D texture2 = Main.npcTexture[npc.type];
			Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			Color color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.25f;
				float y = Main.rand.Next(-10, 11) * 0.25f;
				Main.spriteBatch.Draw(texture2, drawPos + new Vector2(x, y), null, color * (1f - (npc.alpha / 255f)), npc.rotation, drawOrigin2, npc.scale, SpriteEffects.None, 0f);
			}
			bool flip = false;
			if (Math.Abs(MathHelper.WrapAngle(dir)) <= MathHelper.ToRadians(90))
			{
				flip = true;
			}
			float bonusDir = !flip ? MathHelper.ToRadians(180) : 0;
			spriteBatch.Draw(texture, drawPos, null, drawColor, dir - bonusDir, drawOrigin, 1f, !flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			spriteBatch.Draw(textureG, drawPos, null, Color.White, dir - bonusDir, drawOrigin, 1f, !flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int k = 0; k < 20; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 82, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
				}
				for (int i = 0; i < 30; i++)
				{
					int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModContent.DustType<BigPermafrostDust>());
					Main.dust[dust].velocity *= 5f;
				}
				for (int i = 1; i <= 7; i++)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PermafrostConstruct/PermafrostConstructGore" + i), 1f);
				for (int i = 0; i < 9; i++)
					Gore.NewGore(npc.position, npc.velocity, Main.rand.Next(61, 64), 1f);
			}
		}
		public override void PostAI()
		{
			npc.velocity.X *= 1f - (npc.alpha / 255f * 0.01f);
			if (npc.velocity.X > 0)
				npc.spriteDirection = 1;
			else
				npc.spriteDirection = -1;
			base.PostAI();
		}
		public override bool PreAI()
		{
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.15f / 155f, (255 - npc.alpha) * 0.25f / 155f, (255 - npc.alpha) * 0.65f / 155f);
			npc.scale = 1.4f;
			Player player = Main.player[npc.target];
			dir = (float)Math.Atan2(player.Center.Y - npc.Center.Y, player.Center.X - npc.Center.X);
			npc.rotation += 0.05f * npc.velocity.X;
			timer++;
			if(timer % 2 == 0)
				npc.alpha--;
			if(npc.alpha < 0)
			{
				npc.alpha = 0;
			}
			float distToPlayerX = player.Center.X - npc.Center.X;
			if((Math.Abs(distToPlayerX) < 320 && npc.alpha <= 10) || ai1 > 0)
			{
				npc.noGravity = true;
				npc.aiStyle = 0;
				ai1++;
				if (ai1 < 20)
				{
					npc.velocity.Y -= 0.12f;
					npc.velocity.Y *= 0.97f;
				}
				if (ai1 > 60)
				{
					npc.noGravity = false;
					npc.aiStyle = 26;
					ai1 = 0;
					npc.alpha = 255;
					int damage = npc.damage / 2;
					if (Main.expertMode)
					{
						damage = (int)(damage / Main.expertDamage);
					}
					for (int i = 0; i < 8; i++)
					{
						Vector2 velocity = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(45 * i) + npc.rotation);
						if (Main.netMode != 1)
							Projectile.NewProjectile(npc.Center, velocity, mod.ProjectileType("HostileJavelin"), damage, 0, Main.myPlayer, npc.Center.X, npc.Center.Y);
					}
				}
			}
			if(npc.velocity.Y > 0)
			{
				npc.velocity.Y *= 0.8f;
				if (ai1 > 30)
				{
					npc.velocity.Y *= 0.25f;
				}
			}
			return true;
		}
		public override void NPCLoot()
		{
			int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("PermafrostSpirit"));	
			Main.npc[n].velocity.Y = -10f;
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("FragmentOfPermafrost"), Main.rand.Next(4) + 4);	
		}	
	}
}