using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles;
using System;
using Terraria;
using Terraria.Audio;
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
            NPC.aiStyle =1; 
            NPC.lifeMax = 300;   
            NPC.damage = 30; 
            NPC.defense = 0;   
            NPC.knockBackResist = 0f;
            NPC.width = 62;
            NPC.height = 42;
            AnimationType = NPCID.BlueSlime;  
            Main.npcFrameCount[NPC.type] = 2; 
            NPC.value = 0;
            NPC.npcSlots = 1f;
            NPC.boss = false;
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
			NPC.alpha = 70;
		}
		Vector2 aimTo = new Vector2(-1, -1);
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Player player = Main.player[NPC.target];
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/PutridPinkyPupil");
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/PutridPinky1Eye");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.25f);
			Vector2 drawPos = NPC.Center - screenPos;
			aimTo = player.Center;

			float shootToX = aimTo.X - NPC.Center.X;
			float shootToY = aimTo.Y - NPC.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			distance = 2.5f / distance;

			shootToX *= distance;
			shootToY *= distance;
			Color color = drawColor;
			drawPos.X += shootToX;
			drawPos.Y += shootToY + 6 + (NPC.frame.Y > 0 ? -2 : 0);
			drawPos.X += ((float)NPC.lifeMax - NPC.life) / (float)NPC.lifeMax * (float)Main.rand.NextFloat(-1, 1);
			drawPos.Y += ((float)NPC.lifeMax - NPC.life) / (float)NPC.lifeMax * (float)Main.rand.NextFloat(-1, 1);
			spriteBatch.Draw(texture2, NPC.Center - screenPos + new Vector2(0, 4), NPC.frame, color, NPC.rotation, drawOrigin2, NPC.scale, SpriteEffects.None, 0f);
			if (NPC.scale == 1)
				spriteBatch.Draw(texture, drawPos, null, color, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
		}
		public void Explode()
		{
			NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + NPC.width/2, (int)NPC.Center.Y, ModContent.NPCType<PutridPinkyPhase2>());
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, (int)NPC.Center.X, (int)NPC.Center.Y, 0, 1.25f);
			for (int i = 0; i < 12; i++)
			{
				Vector2 rotation = new Vector2(50, 0).RotatedBy(MathHelper.ToRadians(i * 30));
				rotation += new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
				if (Main.netMode != NetmodeID.MultiplayerClient)
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(0, NPC.height) + rotation, new Vector2(0, 0), ModContent.ProjectileType<PinkExplosion>(), 0, 0, Main.myPlayer, MathHelper.ToRadians(Main.rand.NextFloat(360)));
			}
			NPC.active = false;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life < 20)
			{
				Explode();
			}
		}
		public override void AI()
		{
			NPC.TargetClosest(true);
			NPC.ai[0] += 3; //speed up jumping speed
			Player player = Main.player[NPC.target];
			aimTo = player.Center;
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 1.0f / 255f, (255 - NPC.alpha) * 0.2f / 255f, (255 - NPC.alpha) * 0.34f / 255f);
			if (NPC.life < 20)
			{
				Explode();
			}
			NPC.timeLeft = 100;
			if(Main.player[NPC.target].dead)
			{
				despawn++;
			}
			if(despawn >= 720)
			{
				NPC.active = false;
			}
			if(Main.rand.NextBool((int)Math.Sqrt(NPC.life) + 5))
			{
				Dust dust = Dust.NewDustDirect(NPC.position + new Vector2(5), NPC.width - 8, NPC.height - 8, DustID.PinkSlime, 0, 0, 120);
				dust.velocity *= 0.05f;
				dust.scale *= 1.5f;
				dust.noGravity = false;
			}
		}
	}
}





















