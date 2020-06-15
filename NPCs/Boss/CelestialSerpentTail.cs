using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.NPCs.Boss
{   
    public class CelestialSerpentTail : ModNPC
    {	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Celestial Serpent");
		}
        public override void SetDefaults()
        {
            npc.width = 44;             
            npc.height = 50;         
            npc.damage = 40;
            npc.defense = 0;
            npc.lifeMax = 12312412;  //arbitrary
            Main.npcFrameCount[npc.type] = 22;  
            npc.knockBackResist = 0.0f;
            //npc.behindTiles = true;
            npc.noTileCollide = true;
            npc.boss = true;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.dontCountMe = true;
            npc.value = 100000;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath32;
			music = MusicID.Boss2;
            npc.buffImmune[69] = true;
            npc.buffImmune[70] = true;
            npc.buffImmune[39] = true;
            npc.buffImmune[24] = true;
            npc.buffImmune[BuffID.Frostburn] = true;
        }
		float ai2 = 0;
        public override bool PreAI()
        {
            if (npc.ai[3] > 0)
                npc.realLife = (int)npc.ai[3];
            if (npc.target < 0 || npc.target == byte.MaxValue || Main.player[npc.target].dead)
                npc.TargetClosest(true);
 
            if (Main.netMode != 1)
            {
                if (!Main.npc[(int)npc.ai[1]].active)
                {
                    npc.life = 0;
                    npc.HitEffect(0, 10.0);
                    npc.active = false;
                    NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f, 0.0f, 0.0f, 0, 0, 0);
                }
            }
 
            if (npc.ai[1] < (double)Main.npc.Length)
            {
                // We're getting the center of this NPC.
                Vector2 npcCenter = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                // Then using that center, we calculate the direction towards the 'parent NPC' of this NPC.
                float dirX = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - npcCenter.X;
                float dirY = Main.npc[(int)npc.ai[1]].position.Y + (float)(Main.npc[(int)npc.ai[1]].height / 2) - npcCenter.Y;
                // We then use Atan2 to get a correct rotation towards that parent NPC.
                npc.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                // We also get the length of the direction vector.
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                // We calculate a new, correct distance.
                float dist = (length - (float)npc.width) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
 
                // Reset the velocity of this NPC, because we don't want it to move on its own
                npc.velocity = Vector2.Zero;
                // And set this NPCs position accordingly to that of this NPCs parent NPC.
                npc.position.X = npc.position.X + posX;
                npc.position.Y = npc.position.Y + posY;
            }
            return false;
        }
		float ai1 = 0;
		int frame = 0;
		public override void FindFrame(int frameHeight) 
		{
			Player player  = Main.player[npc.target];
			frame = frameHeight;
			
                if (Main.npc[(int)npc.ai[3]].velocity.X == 0 && Main.npc[(int)npc.ai[3]].velocity.Y == 0)
				{
					ai1 += 4.75f;
				}
					
				ai1 += 0.75f;
				if (ai1 >= 5f) 
				{
					ai1 -= 5f;
					npc.frame.Y += frame;
					if(npc.frame.Y >= 22 * frame)
					{
						npc.frame.Y = 0;
					}
					if(npc.frame.Y == frame * 10)
					{
					
						float shootToX = player.Center.X - npc.Center.X;
						float shootToY = player.Center.Y - npc.Center.Y;
						float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

						distance = 5f / distance;
								  
						shootToX *= distance * 5;
						shootToY *= distance * 5;
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, shootToX, shootToY, mod.ProjectileType("BlueCellBlast"), 30, 0f, 0);
					}
					if(npc.frame.Y == 0)
					{
					
						float shootToX = player.Center.X - npc.Center.X;
						float shootToY = player.Center.Y - npc.Center.Y;
						float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

						distance = 5f / distance;
								  
						shootToX *= distance * 5;
						shootToY *= distance * 5;
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, shootToX, shootToY, mod.ProjectileType("PurpleCellBlast"), 30, 0f, 0);
					}
				}
				
			
		}
       // public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
       // {
      //      Texture2D texture = Main.npcTexture[npc.type];
      //      Vector2 origin = new Vector2((texture.Width * 0.5f), (texture.Height * 0.5f));
      //      Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, new Rectangle?(), drawColor, npc.rotation, origin, npc.scale, SpriteEffects.None, 0);
      //      return false;
      //  }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
 
            return false;       //this make that the npc does not have a health bar
        }
		public override void PostAI()
		{
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 2.5f / 255f, (255 - npc.alpha) * 1.6f / 255f, (255 - npc.alpha) * 2.4f / 255f);
			npc.timeLeft = 10000000;
		}
    }
}