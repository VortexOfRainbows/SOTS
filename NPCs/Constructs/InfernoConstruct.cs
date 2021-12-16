using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Celestial;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Constructs
{
	public class InfernoConstruct : ModNPC
	{
		float dir = 0f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Inferno Construct");
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 0;
			npc.lifeMax = 5000;  
			npc.damage = 70; 
			npc.defense = 26;  
			npc.knockBackResist = 0.1f;
			npc.width = 98;
			npc.height = 78;
			Main.npcFrameCount[npc.type] = 1;
			npc.value = Item.buyPrice(0, 10, 0, 0);
			npc.npcSlots = 4f;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.netAlways = true;
			npc.alpha = 0;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.rarity = 5;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.damage = 100;
			npc.lifeMax = 8000;
		}
		List<FireParticle> particleList = new List<FireParticle>();
		public void cataloguePos()
		{
			for (int i = 0; i < particleList.Count; i++)
			{
				FireParticle particle = particleList[i];
				particle.Update();
				if (!particle.active)
				{
					particleList.RemoveAt(i);
					i--;
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Vector2 origin = new Vector2(npc.width / 2, npc.height / 2);
			Texture2D texture = Main.npcTexture[npc.type];
			dir = (float)Math.Atan2(aimTo.Y - npc.Center.Y, aimTo.X - npc.Center.X);
			float rotation = dir + (npc.spriteDirection - 1) * 0.5f * -MathHelper.ToRadians(180); 
			DrawFire();
			Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), null, drawColor, rotation, origin, npc.scale, npc.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}
		public void DrawFire()
		{
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color;
			for (int i = 0; i < particleList.Count; i++)
			{
				color = new Color(255, 69, 0, 0);
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color = npc.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-2f, 2f);
					float y = Main.rand.NextFloat(-2f, 2f);
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.1f, SpriteEffects.None, 0f);
				}
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{

		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int k = 0; k < 20; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 82, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
				}
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructs/OtherworldlyConstructGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructs/OtherworldlyConstructGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructs/OtherworldlyConstructGore4"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructs/OtherworldlyConstructGore5"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructs/OtherworldlyConstructGore6"), 1f);
				for (int i = 0; i < 9; i++)
					Gore.NewGore(npc.position, npc.velocity, Main.rand.Next(61, 64), 1f);
			}
		}
		Vector2 aimTo = new Vector2(-1, -1);
		public override bool PreAI()
		{
			Player player = Main.player[npc.target];
			npc.TargetClosest(true);
			aimTo = player.Center;
			return true;
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.45f / 155f, (255 - npc.alpha) * 0.25f / 155f, (255 - npc.alpha) * 0.45f / 155f);
			if(npc.ai[0] < 0)
			{
				npc.velocity *= 0.98f;
				return;
			}
			Vector2 toPlayer = player.Center - npc.Center;
			float distToPlayer = toPlayer.Length();
			float speed = 12 + distToPlayer * 0.0005f;
			if(speed > distToPlayer)
            {
				speed = distToPlayer;
			}
			if(distToPlayer > 880)
            {
				speed *= 2.8f;
            }
			if(distToPlayer < 380 && distToPlayer > 320)
            {
				speed *= 0.1f;
            }
			if(distToPlayer < 320)
			{
				speed = -2 + distToPlayer * -0.001f;
			}
			npc.velocity = Vector2.Lerp(npc.velocity, toPlayer.SafeNormalize(Vector2.Zero) * speed, 0.1f);
			dir = (float)Math.Atan2(aimTo.Y - npc.Center.Y, aimTo.X - npc.Center.X);
			npc.rotation = dir;
			if (Main.netMode != NetmodeID.Server)
			{
				for (int i = 0; i < (SOTS.Config.lowFidelityMode ? 2 : 3); i++)
				{
					Vector2 rotational = new Vector2(-5f, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-30f, 30f)));
					if (i <= 1)
					{
						rotational.X *= 1f;
						rotational.Y *= 0.6f;
					}
					else
                    {
						rotational.X *= 0.4f;
						rotational.Y *= 1.1f;
					}
					rotational = rotational.RotatedBy(npc.rotation);
					particleList.Add(new FireParticle(npc.Center + new Vector2(-30, 0).RotatedBy(npc.rotation), rotational, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(1.6f, 2f)));
				}
				cataloguePos();
			}
			npc.spriteDirection = npc.direction;
		}
		public override void NPCLoot()
		{
			int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("OtherworldlySpirit"));	
			Main.npc[n].velocity.Y = -10f;
			if (Main.netMode != NetmodeID.MultiplayerClient)
				Main.npc[n].netUpdate = true;
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FragmentOfInferno>(), Main.rand.Next(4) + 4);
		}	
	}
}