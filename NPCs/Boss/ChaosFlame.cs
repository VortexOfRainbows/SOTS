using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{//[AutoloadBossHead]
	public class ChaosFlame : ModNPC
	{
		int despawn = 0;
		float rotateTimer = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Chaos Flame");
		}
		public override void SetDefaults()
		{
            npc.aiStyle = 14; 
			npc.lifeMax = 600;
            npc.damage = 70; 
            npc.defense = 10;  
            npc.knockBackResist = 0f;
            npc.width = 72;
            npc.height = 118;
            animationType = NPCID.CaveBat;
            Main.npcFrameCount[npc.type] = 5;
            npc.npcSlots = 1f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
			npc.netUpdate = true;
			npc.netAlways = true;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath6;
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = (int)(npc.damage * 0.75f);  
        }
		public void GenerateDust()
		{
			for(int j = 0; j < 3; j ++)
			{
				for(int i = 0; i < 360; i += 15)
				{
					Vector2 circularLocation = new Vector2(48 + j * 12, 0).RotatedBy(MathHelper.ToRadians(i + j * 10));
					
					int num1 = Dust.NewDust(new Vector2(npc.Center.X + circularLocation.X - 4, npc.Center.Y + circularLocation.Y - 4), 4, 4, 134);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 0.1f;
				}
			}
		}
		public override void AI()
		{
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 2.5f / 255f, (255 - npc.alpha) * 1.6f / 255f, (255 - npc.alpha) * 2.4f / 255f);
			float rotateAmount = 170 * 0.012f;
			int dist = 360;
			
			Player target = Main.player[npc.target];
			
            npc.TargetClosest(false);
			
			if(Main.rand.Next(3) == 0 && npc.ai[0] % 40 == 0)
					Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-50,51), npc.Center.Y + Main.rand.Next(-50,51), Main.rand.Next(-2,3), Main.rand.Next(-2,3), mod.ProjectileType("plusBeam"), 40, 0, 0);
				
			if(Main.rand.Next(3) == 0 && npc.ai[0] % 40 == 20)
					Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-50,51), npc.Center.Y + Main.rand.Next(-50,51), Main.rand.Next(-2,3), Main.rand.Next(-2,3), mod.ProjectileType("XBeam"), 40, 0, 0);

			double deg = (double)rotateTimer; 
			double rad = deg * (Math.PI / 180);
			rotateTimer += rotateAmount;
			
			float rotateToX = target.Center.X - (int)(Math.Cos(rad) * dist) - npc.width/2;
			float rotateToY = target.Center.Y - (int)(Math.Sin(rad) * dist) - npc.height/2;
	
			
			int originY = (int)target.position.Y;
			int originX = (int)target.position.X;
			
			npc.ai[0] += 1;
			
			if(npc.ai[0] == 20)
			GenerateDust();
		
			if(npc.ai[0] >= 30 && npc.ai[0] < 120)
			{
				npc.position.X = rotateToX;
				npc.position.Y = rotateToY;
			}
			if(npc.ai[0] >= 240)
			{
				npc.ai[0] = 0;
				
				GenerateDust();
				int randY = Main.rand.Next(-500,501);
				int randX = Main.rand.Next(-500,501);
				
				if(randX > 0)
				{
					randX += 350;
				}
				if(randX < 0)
				{
					randX -= 350;
				}
				
				npc.position.X = originX + randX - npc.width/2;
				npc.position.Y = originY + randY - npc.height/2;
				GenerateDust();
				
				float travelToX = target.position.X + (float)target.width * 0.5f - npc.Center.X;
				float travelToY = target.position.Y + (float)target.height * 0.5f  - npc.Center.Y;
				float distance = (float)System.Math.Sqrt((double)(travelToX * travelToX + travelToY * travelToY));

				distance = 2.25f / distance;
						
				travelToX *= distance * 5;
				travelToY *= distance * 5;
				npc.velocity.X = travelToX;
				npc.velocity.Y = travelToY;
			}
			if(Main.player[npc.target].dead)
			{
				despawn++;
			}
			if(despawn >= 360)
			{
				npc.active = false;
			}
			else
			{
				npc.timeLeft = 1000;
			}
		}
	}
}