using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{
	public class FrostbiteLaser : ModNPC
	{	int aiRate = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Frostbite Laser");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = -1; 
            npc.lifeMax = 240;   
            npc.damage = 56; 
            npc.defense = 5;  
            npc.knockBackResist = 0f;
            npc.width = 34;
            npc.height = 34;
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
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.9f / 255f, (255 - npc.alpha) * 0.1f / 255f, (255 - npc.alpha) * 0.3f / 255f);
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
				aiRate++;
				if(aiRate >= 30)
				{
					aiRate = 0;
					npc.rotation += MathHelper.ToRadians(9);
					npc.ai[1] += 9;
				
					Vector2 rotateVelocity = new Vector2(-4, 0).RotatedBy(MathHelper.ToRadians(npc.ai[1]));
					Vector2 rotateVelocity2 = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(npc.ai[1]));
					Vector2 rotateVelocity3 = new Vector2(0, 4).RotatedBy(MathHelper.ToRadians(npc.ai[1]));
					Vector2 rotateVelocity4 = new Vector2(0, -4).RotatedBy(MathHelper.ToRadians(npc.ai[1]));
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, rotateVelocity.X, rotateVelocity.Y, mod.ProjectileType("MargritBolt"), 32, 0, Main.myPlayer, 0f, 0f);
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, rotateVelocity2.X, rotateVelocity2.Y, mod.ProjectileType("MargritBolt"), 32, 0, Main.myPlayer, 0f, 0f);
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, rotateVelocity3.X, rotateVelocity3.Y, mod.ProjectileType("MargritBolt"), 32, 0, Main.myPlayer, 0f, 0f);
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, rotateVelocity4.X, rotateVelocity4.Y, mod.ProjectileType("MargritBolt"), 32, 0, Main.myPlayer, 0f, 0f);
					
						
				}
			}
			else
			{
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





















