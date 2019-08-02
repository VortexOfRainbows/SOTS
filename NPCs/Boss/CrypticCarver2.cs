using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{[AutoloadBossHead]
	public class CrypticCarver2 : ModNPC
	{	int despawn = 0;
		int fireDelay = 0;
		int fireDelay2 = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Cryptic Carver");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 14;  //5 is the flying AI
            npc.lifeMax = 3900;   //boss life
            npc.damage = 32;  //boss damage
            npc.defense = 7;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 44;
            npc.height = 44;
            animationType = NPCID.SkeletronHead;   //this boss will behavior like the DemonEye
            Main.npcFrameCount[npc.type] = 1;    //boss frame/animation
            npc.value = 20000;
            npc.npcSlots = 1f;
            npc.boss = true;
            npc.lavaImmune = false;
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit41;
            npc.DeathSound = SoundID.NPCDeath3;
            music = MusicID.Boss1;
            npc.netAlways = true;
			
			bossBag = mod.ItemType("BossBagMargrit");
		}
		 public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)((npc.lifeMax * bossLifeScale * 0.6f));  //boss life scale in expertmode
            npc.damage = (int)(npc.damage * 1.2f);  //boss damage increase in expermode
        }
		public override void BossLoot(ref string name, ref int potionType)
		{ 
		SOTSWorld.downedCarver = true;
		potionType = ItemID.HealingPotion;
	
		if(Main.expertMode)
		{ 
npc.DropBossBags();
		} 
		else 
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MargritCore"), 1); 
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 3081, Main.rand.Next(50,90)); 
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 3086, Main.rand.Next(50,90)); 
			
				}
				}
		public override void AI()
		{	
		npc.rotation += 0.25f;
			fireDelay++;
			fireDelay2++;
			
			if(Main.expertMode)
			{
			fireDelay++;
			fireDelay2++;
				
			}
			npc.timeLeft = 600;
			if(Main.player[npc.target].dead)
			{
			 despawn++;
			}
			if(despawn >= 720)
			{
			npc.active = false;
			}
				   Player target = Main.player[npc.target];

				   float shootToX = target.position.X + (float)target.width * 0.5f - npc.Center.X;
				   float shootToY = target.position.Y + (float)target.height * 0.5f  - npc.Center.Y;
				   float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

				  
						   //Dividing the factor of 3f which is the desired velocity by distance
						   distance = 1f / distance;
						  
						   shootToX *= distance * 5;
						   shootToY *= distance * 5;
			   
				  		 Projectile.NewProjectile(npc.Center.X + (shootToX * 12), npc.Center.Y + (shootToY * 12), shootToX * 8, shootToY * 8, mod.ProjectileType("CarverShield"), 0, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
				   	 
					   if(fireDelay >= 60) 
					   {
							  
							  Projectile.NewProjectile(npc.Center.X, npc.Center.Y, shootToX, shootToY, mod.ProjectileType("MargritBolt"), 23, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
							  
							  
								 
							fireDelay = 0;
					   }
						if(fireDelay2 >= 1080)
						{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, shootToX * 2, shootToY * 2, mod.ProjectileType("CarverSingularity"), 1, 0, Main.myPlayer, 0f, 0f); //Spawning a projectile
							  
							fireDelay2 = 0;
						}
			
			
		}
		public override void PostDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/LaserTargeting");    //this where the chain of grappling hook is drawn
                                                      //change YourModName with ur mod name/ and CustomHookPr_Chain with the name of ur one
            Vector2 position = Main.player[npc.target].MountedCenter;
            Vector2 mountedCenter = npc.Center;
            Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            float num1 = (float)texture.Height;
            Vector2 vector2_4 = mountedCenter - position;
            float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
            bool flag = true;
            if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                flag = false;
            if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                flag = false;
            while (flag)
            {
                if ((double)vector2_4.Length() < (double)num1 + 1.0)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * num1;
                    vector2_4 = mountedCenter - position;
                    Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
                    color2 = npc.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
                }
            }
        }
	}
}





















