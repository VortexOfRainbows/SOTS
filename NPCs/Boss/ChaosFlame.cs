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
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}
		public override void SetDefaults()
		{
            NPC.aiStyle =14; 
			NPC.lifeMax = 1200;
            NPC.damage = 70; 
            NPC.defense = 60;  
            NPC.knockBackResist = 0f;
            NPC.width = 72;
            NPC.height = 118;
            AnimationType = NPCID.CaveBat;
            Main.npcFrameCount[NPC.type] = 5;
            NPC.npcSlots = 1f;
            NPC.boss = false;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
			NPC.netUpdate = true;
			NPC.netAlways = true;
            NPC.HitSound = SoundID.NPCHit3;
            NPC.DeathSound = SoundID.NPCDeath6;
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.damage = (int)(NPC.damage * 0.75f);  
        }
		public void GenerateDust()
		{
			for(int j = 0; j < 3; j ++)
			{
				for(int i = 0; i < 360; i += 15)
				{
					Vector2 circularLocation = new Vector2(48 + j * 12, 0).RotatedBy(MathHelper.ToRadians(i + j * 10));
					
					int num1 = Dust.NewDust(new Vector2(NPC.Center.X + circularLocation.X - 4, NPC.Center.Y + circularLocation.Y - 4), 4, 4, 134);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 0.1f;
				}
			}
		}
		public override void AI()
		{
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 2.5f / 255f, (255 - NPC.alpha) * 1.6f / 255f, (255 - NPC.alpha) * 2.4f / 255f);
			float rotateAmount = 170 * 0.012f;
			int dist = 360;
			
			Player target = Main.player[NPC.target];
			
            NPC.TargetClosest(false);
			
			if(Main.rand.NextBool(3) && NPC.ai[0] % 40 == 0)
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + Main.rand.Next(-50,51), NPC.Center.Y + Main.rand.Next(-50,51), Main.rand.Next(-2,3), Main.rand.Next(-2,3), Mod.Find<ModProjectile>("plusBeam").Type, 40, 0, 0);
				
			if(Main.rand.NextBool(3) && NPC.ai[0] % 40 == 20)
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + Main.rand.Next(-50,51), NPC.Center.Y + Main.rand.Next(-50,51), Main.rand.Next(-2,3), Main.rand.Next(-2,3), Mod.Find<ModProjectile>("XBeam").Type, 40, 0, 0);

			double deg = (double)rotateTimer; 
			double rad = deg * (Math.PI / 180);
			rotateTimer += rotateAmount;
			
			float rotateToX = target.Center.X - (int)(Math.Cos(rad) * dist) - NPC.width/2;
			float rotateToY = target.Center.Y - (int)(Math.Sin(rad) * dist) - NPC.height/2;
	
			
			int originY = (int)target.position.Y;
			int originX = (int)target.position.X;
			
			NPC.ai[0] += 1;
			
			if(NPC.ai[0] == 20)
			GenerateDust();
		
			if(NPC.ai[0] >= 30 && NPC.ai[0] < 120)
			{
				NPC.position.X = rotateToX;
				NPC.position.Y = rotateToY;
			}
			if(NPC.ai[0] >= 240)
			{
				NPC.ai[0] = 0;
				
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
				
				NPC.position.X = originX + randX - NPC.width/2;
				NPC.position.Y = originY + randY - NPC.height/2;
				GenerateDust();
				
				float travelToX = target.position.X + (float)target.width * 0.5f - NPC.Center.X;
				float travelToY = target.position.Y + (float)target.height * 0.5f  - NPC.Center.Y;
				float distance = (float)System.Math.Sqrt((double)(travelToX * travelToX + travelToY * travelToY));

				distance = 2.25f / distance;
						
				travelToX *= distance * 5;
				travelToY *= distance * 5;
				NPC.velocity.X = travelToX;
				NPC.velocity.Y = travelToY;
			}
			if(Main.player[NPC.target].dead)
			{
				despawn++;
			}
			if(despawn >= 360)
			{
				NPC.active = false;
			}
			else
			{
				NPC.timeLeft = 1000;
			}
		}
	}
}