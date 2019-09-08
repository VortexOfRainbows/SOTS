using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{
	public class FrostbiteSpike : ModNPC
	{	bool finishedRotating = false;
		int thruster = 0;
		int thrusterDistance = -500;
				float thrusterBoost = 0;
				double dist = 128;
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(finishedRotating);
			writer.Write(thruster);
			writer.Write(thrusterDistance);
			writer.Write(thrusterBoost);
			writer.Write(dist);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			finishedRotating = reader.ReadBoolean();
			thruster = reader.ReadInt32();
			thrusterDistance = reader.ReadInt32();
			thrusterBoost = reader.ReadSingle();
			dist = reader.ReadDouble();
		}
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Frostbite Spike");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = -1; 
            npc.lifeMax = 1000;   
            npc.damage = 56; 
            npc.defense = 0;  
            npc.knockBackResist = 0f;
            npc.width = 50;
            npc.height = 50;
            npc.value = 0;
            npc.npcSlots = 1f;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = true;
			
		}
		public override void AI()
		{	
				if(Main.expertMode)
				{
					npc.dontTakeDamage = true;
				}
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.9f / 255f, (255 - npc.alpha) * 0.1f / 255f, (255 - npc.alpha) * 0.3f / 255f);
			int counter = 0;
			
				for(int i = 200; i > 0; i--)
				{
					NPC probe = Main.npc[i];
					if(probe.type == npc.type && probe.active && npc.active)
					{
						counter++;
						if(probe == npc)
						{
							break;
						}
					}
				}
				
			if(!finishedRotating)
			{
				if(counter == 1)
				{
					npc.rotation = MathHelper.ToRadians(0);
					npc.ai[1] = 0;
				}
				if(counter == 2)
				{
					npc.rotation = MathHelper.ToRadians(45);
					npc.ai[1] = 45;
				}
				if(counter == 3)
				{
					npc.rotation = MathHelper.ToRadians(90);
					npc.ai[1] = 90;
				}
				if(counter == 4)
				{
					npc.rotation = MathHelper.ToRadians(135);
					npc.ai[1] = 135;
				}
				if(counter == 5)
				{
					npc.rotation = MathHelper.ToRadians(180);
					npc.ai[1] = 180;
				}
				if(counter == 6)
				{
					npc.rotation = MathHelper.ToRadians(225);
					npc.ai[1] = 225;
				}
				if(counter == 7)
				{
					npc.rotation = MathHelper.ToRadians(270);
					npc.ai[1] = 270;
				}
				if(counter == 8)
				{
					npc.rotation = MathHelper.ToRadians(315);
					npc.ai[1] = 315;
				}
			finishedRotating = true;
			}
			
			thruster += Main.rand.Next(3);
			
				if(counter >= 9)
				{
					npc.active = false;
				}
			
				
				if(thruster > 120)
				{
					if(thrusterDistance == -500)
					{
						thrusterDistance = Main.rand.Next(120,361);
					}
					if(dist >= 128)
					{
						thrusterDistance--;
						thrusterBoost = thrusterDistance * 0.01f;
					}
					if(dist <= 127)
					{
						thrusterDistance = -500;
						thruster = -30;
						thrusterBoost = 0;
						dist = 128;
					}
				}
			
			
				dist += thrusterBoost;
			Vector2 rotateVelocity = new Vector2(-4, 0).RotatedBy(MathHelper.ToRadians(npc.ai[1]));
			
	
				
			int IcyAbomInt = -1;
			for(int i = 0; i < 200; i++)
			{
				NPC IcyAbom = Main.npc[i];
				if(IcyAbom.type == mod.NPCType("ShardKing") && IcyAbom.active)
				{
					IcyAbomInt = i;
					break;
				}
			}
			if(IcyAbomInt >= 0)
			{
				npc.rotation += MathHelper.ToRadians(1);
				npc.ai[1] += 1;
				
				NPC IcyAbom  = Main.npc[IcyAbomInt];
				double deg = (double) npc.ai[1]; 
				double rad = deg * (Math.PI / 180);
				npc.position.X = IcyAbom.Center.X - (int)(Math.Cos(rad) * dist) - npc.width/2;
				npc.position.Y = IcyAbom.Center.Y - (int)(Math.Sin(rad) * dist) - npc.height/2;
			}
			else
			{
				npc.scale -= 0.008f;
				npc.rotation += 0.3f;
				npc.life -= 3;
				if(npc.life < 1 || npc.scale < 0)
				{
					npc.active = false;
				}
			}
			
			
			
			
			
		}
		/*
		public override void PostDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
				double deg = (double) npc.ai[1]; 
				double rad = deg * (Math.PI / 180);
				double dist = 960;
				float positionX = npc.Center.X - (int)(Math.Cos(rad) * dist) - npc.width/2;
				float positionY = npc.Center.Y - (int)(Math.Sin(rad) * dist) - npc.height/2;
				
            Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/LaserTargeting");    //this where the chain of grappling hook is drawn
                                                      //change YourModName with ur mod name/ and CustomHookPr_Chain with the name of ur one
            Vector2 position = new Vector2(positionX, positionY);
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
		*/
	
	}
}





















