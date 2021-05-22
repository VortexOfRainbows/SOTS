using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{
	public class FrostbiteProbe : ModNPC
	{	bool finishedRotating = false;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Frostbite Probe");
		}
		public override void SetDefaults()
		{
            npc.aiStyle = -1; 
            npc.lifeMax = 200;   
            npc.damage = 20; 
            npc.defense = 0;  
            npc.knockBackResist = 0f;
            npc.width = 48;
            npc.height = 28;
            npc.value = 0;
            npc.npcSlots = 1f;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = true;
            npc.buffImmune[44] = true;
		}
		public override void AI()
		{	
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.9f / 255f, (255 - npc.alpha) * 0.1f / 255f, (255 - npc.alpha) * 0.3f / 255f);
			int counter = 0;
			
				for(int i = 0; i < 200; i++)
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
					npc.rotation = MathHelper.ToRadians(90);
					npc.ai[1] = 90;
				}
				if(counter == 3)
				{
					npc.rotation = MathHelper.ToRadians(180);
					npc.ai[1] = 180;
				}
				if(counter == 4)
				{
					npc.rotation = MathHelper.ToRadians(270);
					npc.ai[1] = 270;
				}
				finishedRotating = true;
			}
			
			
			if(counter >= 5)
			{
				npc.active = false;
			}
			npc.ai[0]++;
			
			Vector2 rotateVelocity = new Vector2(-4, 0).RotatedBy(MathHelper.ToRadians(npc.ai[1]));
			Vector2 tripletVelocity = rotateVelocity.RotatedBy(MathHelper.ToRadians(15));
			Vector2 tripletVelocity2 = rotateVelocity.RotatedBy(MathHelper.ToRadians(-15));
			
			if(npc.ai[0] == 30 || npc.ai[0] == 60 || npc.ai[0] == 90 || npc.ai[0] == 120 || npc.ai[0] == 150 || npc.ai[0] == 180 || npc.ai[0] == 210 || npc.ai[0] == 240 || npc.ai[0] == 270 || npc.ai[0] == 300 || npc.ai[0] == 330)
			{
				if(Main.netMode != 1)
				{
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, rotateVelocity.X, rotateVelocity.Y, mod.ProjectileType("MargritBolt"), 22, 0, Main.myPlayer, 0f, 0f);
					if(Main.expertMode && Main.rand.Next(2) == 0)
					{
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, tripletVelocity.X, tripletVelocity.Y, mod.ProjectileType("MargritBolt"), 15, 0, Main.myPlayer, 0f, 0f);	
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, tripletVelocity2.X, tripletVelocity2.Y, mod.ProjectileType("MargritBolt"), 15, 0, Main.myPlayer, 0f, 0f);	
					}
				}
			}
			if(npc.ai[0] >= 360 && npc.ai[0] < 380)
			{
				npc.ai[1] += 3.25f;
				npc.rotation += MathHelper.ToRadians(3.25f);
				if(Main.netMode != 1)
				{
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, rotateVelocity.X * 0.5f, rotateVelocity.Y * 0.5f, mod.ProjectileType("MargritBolt"), 35, 0, Main.myPlayer, 0f, 0f);
				}
			}
			if(npc.ai[0] >= 380)
			{
				npc.ai[1] += 3.25f;
				npc.rotation += MathHelper.ToRadians(3.25f);
				if(Main.netMode != 1)
				{
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, rotateVelocity.X * 0.9f, rotateVelocity.Y * 0.9f, mod.ProjectileType("MargritBolt"), 35, 0, Main.myPlayer, 0f, 0f);
				}				
			}
			if(npc.ai[0] >= 400)
			{
				npc.ai[0] = 0;
			}
				
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
				double dist = 96;
				npc.position.X = IcyAbom.Center.X - (int)(Math.Cos(rad) * dist) - npc.width/2;
				npc.position.Y = IcyAbom.Center.Y - (int)(Math.Sin(rad) * dist) - npc.height/2;
			}
			else
			{
				npc.ai[0] = 0;
				npc.scale -= 0.008f;
				npc.rotation += 0.3f;
				npc.life--;
				if(npc.life < 1 || npc.scale < 0)
				{
					npc.active = false;
				}
			}
		}
	}
}