using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{[AutoloadBossHead]
	public class PutridPinky1 : ModNPC
	{	int despawn = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Pinky");
		}
		public override void SetDefaults()
		{
            npc.aiStyle = 10; 
            npc.lifeMax = 300;   
            npc.damage = 30; 
            npc.defense = 0;   
            npc.knockBackResist = 0f;
            npc.width = 84;
            npc.height = 84;
            animationType = NPCID.SkeletronHead;  
            Main.npcFrameCount[npc.type] = 1; 
            npc.value = 10000;
            npc.npcSlots = 1f;
            npc.boss = false;
            npc.lavaImmune = false;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.netAlways = true;
		}
		Vector2 aimTo = new Vector2(-1, -1);
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Player player = Main.player[npc.target];
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/PutridEye2");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = npc.Center - Main.screenPosition;
			aimTo = player.Center;

			float shootToX = aimTo.X - npc.Center.X;
			float shootToY = aimTo.Y - npc.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			distance = 1.7f / distance;

			shootToX *= distance * 5;
			shootToY *= distance * 5;

			drawColor = npc.GetAlpha(drawColor);
			drawPos.X += shootToX;
			drawPos.Y += -1 + shootToY + (texture.Height * 0.5f);
			drawPos.X += ((float)npc.lifeMax - npc.life) / (float)npc.lifeMax * (float)Main.rand.Next(-6, 7);
			drawPos.Y += ((float)npc.lifeMax - npc.life) / (float)npc.lifeMax * (float)Main.rand.Next(-6, 7);
			if (npc.scale == 1)
				spriteBatch.Draw(texture, drawPos, null, drawColor, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
		}
		public void Explode()
		{
			NPC.NewNPC((int)npc.position.X + npc.width/2, (int)npc.position.Y + npc.height, mod.NPCType("PutridPinkyPhase2"));
			Main.PlaySound(15, (int)(npc.Center.X), (int)(npc.Center.Y), 0, 1.25f);
			for (int i = 0; i < 12; i++)
			{
				Vector2 rotation = new Vector2(40, 0).RotatedBy(MathHelper.ToRadians(i * 30));
				if (Main.netMode != NetmodeID.MultiplayerClient)
					Projectile.NewProjectile(npc.Center + rotation, new Vector2(0, 0), mod.ProjectileType("PinkExplosion"), 0, 0, Main.myPlayer);
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
			Player player = Main.player[npc.target];
			aimTo = player.Center;
			counter++;
			npc.rotation = MathHelper.ToRadians((((float)(npc.lifeMax - npc.life) / (float)npc.lifeMax * 9f) + 1f) * counter);
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 1.9f / 255f, (255 - npc.alpha) * 0.4f / 255f, (255 - npc.alpha) * 0.7f / 255f);
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
		}
	}
}





















