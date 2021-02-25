using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{	
	[AutoloadBossHead]
	public class PutridPinky1 : ModNPC
	{	int despawn = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Pinky");
		}
		public override void SetDefaults()
		{
            npc.aiStyle = 1; 
            npc.lifeMax = 300;   
            npc.damage = 30; 
            npc.defense = 0;   
            npc.knockBackResist = 0f;
            npc.width = 62;
            npc.height = 42;
            animationType = NPCID.BlueSlime;  
            Main.npcFrameCount[npc.type] = 2; 
            npc.value = 0;
            npc.npcSlots = 1f;
            npc.boss = false;
            npc.lavaImmune = false;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.netAlways = true;
			npc.alpha = 70;
		}
		Vector2 aimTo = new Vector2(-1, -1);
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Player player = Main.player[npc.target];
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/PutridPinkyPupil");
			Texture2D texture2 = ModContent.GetTexture("SOTS/NPCs/Boss/PutridPinky1Eye");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.25f);
			Vector2 drawPos = npc.Center - Main.screenPosition;
			aimTo = player.Center;

			float shootToX = aimTo.X - npc.Center.X;
			float shootToY = aimTo.Y - npc.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			distance = 2.5f / distance;

			shootToX *= distance;
			shootToY *= distance;
			Color color = drawColor;
			drawPos.X += shootToX;
			drawPos.Y += shootToY + 6 + (npc.frame.Y > 0 ? -2 : 0);
			drawPos.X += ((float)npc.lifeMax - npc.life) / (float)npc.lifeMax * (float)Main.rand.NextFloat(-1, 1);
			drawPos.Y += ((float)npc.lifeMax - npc.life) / (float)npc.lifeMax * (float)Main.rand.NextFloat(-1, 1);
			spriteBatch.Draw(texture2, npc.Center - Main.screenPosition + new Vector2(0, 4), npc.frame, color, npc.rotation, drawOrigin2, npc.scale, SpriteEffects.None, 0f);
			if (npc.scale == 1)
				spriteBatch.Draw(texture, drawPos, null, color, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
		}
		public void Explode()
		{
			NPC.NewNPC((int)npc.position.X + npc.width/2, (int)npc.Center.Y, mod.NPCType("PutridPinkyPhase2"));
			Main.PlaySound(15, (int)(npc.Center.X), (int)(npc.Center.Y), 0, 1.25f);
			for (int i = 0; i < 12; i++)
			{
				Vector2 rotation = new Vector2(50, 0).RotatedBy(MathHelper.ToRadians(i * 30));
				rotation += new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
				if (Main.netMode != NetmodeID.MultiplayerClient)
					Projectile.NewProjectile(npc.Center - new Vector2(0, npc.height) + rotation, new Vector2(0, 0), mod.ProjectileType("PinkExplosion"), 0, 0, Main.myPlayer, MathHelper.ToRadians(Main.rand.NextFloat(360)));
			}
			npc.active = false;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life < 20)
			{
				Explode();
			}
		}
		int counter;
		public override void AI()
		{
			npc.TargetClosest(true);
			npc.ai[0] += 3; //speed up jumping speed
			Player player = Main.player[npc.target];
			aimTo = player.Center;
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 1.0f / 255f, (255 - npc.alpha) * 0.2f / 255f, (255 - npc.alpha) * 0.34f / 255f);
			if (npc.life < 20)
			{
				Explode();
			}
			npc.timeLeft = 100;
			if(Main.player[npc.target].dead)
			{
				despawn++;
			}
			if(despawn >= 720)
			{
				npc.active = false;
			}
			if(Main.rand.NextBool((int)Math.Sqrt(npc.life) + 5))
			{
				Dust dust = Dust.NewDustDirect(npc.position + new Vector2(5), npc.width - 8, npc.height - 8, DustID.PinkSlime, 0, 0, 120);
				dust.velocity *= 0.05f;
				dust.scale *= 1.5f;
				dust.noGravity = false;
			}
		}
	}
}





















