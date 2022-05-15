using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Permafrost;
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
			NPC.aiStyle =26; //unicorn AI
			NPC.lifeMax = 275;  
			NPC.damage = 25; 
			NPC.defense = 10;  
			NPC.knockBackResist = 0.1f;
			NPC.width = 90;
			NPC.height = 90;
			Main.npcFrameCount[NPC.type] = 1;  
			NPC.value = Item.buyPrice(0, 1, 0, 0);
			NPC.npcSlots = 3f;
			NPC.boss = false;
			NPC.lavaImmune = false;
			NPC.noGravity = false;
			NPC.noTileCollide = false;
			NPC.netAlways = true;
			NPC.alpha = 255;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.rarity = 5;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D textureG = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/PermafrostConstructHeadGlow");
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/PermafrostConstructHead");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY);
			Texture2D texture2 = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			Color color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.25f;
				float y = Main.rand.Next(-10, 11) * 0.25f;
				Main.spriteBatch.Draw(texture2, drawPos + new Vector2(x, y), null, color * (1f - (NPC.alpha / 255f)), NPC.rotation, drawOrigin2, NPC.scale, SpriteEffects.None, 0f);
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
			if (NPC.life <= 0)
			{
				for (int k = 0; k < 20; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 82, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
				}
				for (int i = 0; i < 30; i++)
				{
					int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<BigPermafrostDust>());
					Main.dust[dust].velocity *= 5f;
				}
				for (int i = 1; i <= 7; i++)
					Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/PermafrostConstruct/PermafrostConstructGore" + i), 1f);
				for (int i = 0; i < 9; i++)
					Gore.NewGore(NPC.position, NPC.velocity, Main.rand.Next(61, 64), 1f);
			}
		}
		public override void PostAI()
		{
			NPC.velocity.X *= 1f - (NPC.alpha / 255f * 0.01f);
			if (NPC.velocity.X > 0)
				NPC.spriteDirection = 1;
			else
				NPC.spriteDirection = -1;
			base.PostAI();
		}
		public override bool PreAI()
		{
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.15f / 155f, (255 - NPC.alpha) * 0.25f / 155f, (255 - NPC.alpha) * 0.65f / 155f);
			NPC.scale = 1.4f;
			Player player = Main.player[NPC.target];
			dir = (float)Math.Atan2(player.Center.Y - NPC.Center.Y, player.Center.X - NPC.Center.X);
			NPC.rotation += 0.05f * NPC.velocity.X;
			timer++;
			if(timer % 2 == 0)
				NPC.alpha--;
			if(NPC.alpha < 0)
			{
				NPC.alpha = 0;
			}
			float distToPlayerX = player.Center.X - NPC.Center.X;
			if((Math.Abs(distToPlayerX) < 320 && NPC.alpha <= 10) || ai1 > 0)
			{
				NPC.noGravity = true;
				NPC.aiStyle =0;
				ai1++;
				if (ai1 < 20)
				{
					NPC.velocity.Y -= 0.12f;
					NPC.velocity.Y *= 0.97f;
				}
				if (ai1 > 60)
				{
					NPC.noGravity = false;
					NPC.aiStyle =26;
					ai1 = 0;
					NPC.alpha = 255;
					int damage = NPC.damage / 2;
					if (Main.expertMode)
					{
						damage = (int)(damage / Main.expertDamage);
					}
					for (int i = 0; i < 8; i++)
					{
						Vector2 velocity = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(45 * i) + NPC.rotation);
						if (Main.netMode != 1)
							Projectile.NewProjectile(NPC.Center, velocity, ModContent.ProjectileType<HostileJavelin>(), damage, 0, Main.myPlayer, NPC.Center.X, NPC.Center.Y);
					}
				}
			}
			if(NPC.velocity.Y > 0)
			{
				NPC.velocity.Y *= 0.8f;
				if (ai1 > 30)
				{
					NPC.velocity.Y *= 0.25f;
				}
			}
			return true;
		}
		public override void NPCLoot()
		{
			int n = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<PermafrostSpirit>());	
			Main.npc[n].velocity.Y = -10f;
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<FragmentOfPermafrost>(), Main.rand.Next(4) + 4);	
		}	
	}
}