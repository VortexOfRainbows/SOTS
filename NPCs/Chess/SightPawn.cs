using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Chess
{
	public class SightPawn : ModNPC
	{	int summon = 1;
	int spawn = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Sight Pawn");
		}
		public override void SetDefaults()
		{
			
			npc.CloneDefaults(73);
            aiType = 73; //18 is the demon scythe style
            npc.lifeMax = 125;   //boss life
            npc.damage = 11;  //boss damage
            npc.defense = 12;    //boss defense
            npc.knockBackResist = 0.2f;
            npc.width = 40;
            npc.height = 40;
			animationType = 3;
			 Main.npcFrameCount[npc.type] = 3;    //boss frame/animation
            npc.value = 750;
            npc.npcSlots = 0f;
            npc.boss = false;
            npc.lavaImmune = false;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath7;
            npc.netAlways = false;
		}
		public override void NPCLoot()
		{
			
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.SoulofSight), 1);
		
			if((Main.rand.Next(19) == 0))
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("SoulSingularity")), 1);
				
			if((Main.rand.Next(199) == 0))
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("SightChalice")), 1);	
				
			
		}
		public override void AI()
		{
			
			if( Main.rand.Next(340) == 0 && summon == 1)
			{
				summon = 0;
			}
			if(summon == 0)
			{
				Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 40, 40, 75);
				npc.velocity.X = 0;
				npc.velocity.Y = 0;
				spawn++;
			}
			if(spawn >= 90)
			{
				
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -5, mod.ProjectileType("SightEssence"), 0, 0, 0);
				summon = 1;
				spawn = 0;
			}
		}
		
			
			
			
}
	}
