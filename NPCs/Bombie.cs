using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs
{
	public class Bombie : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Bombie");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 14; 
            npc.lifeMax = 65;  
            npc.damage = 22; 
            npc.defense = 4;  
            npc.knockBackResist = 0.25f;
            npc.width = 62;
            npc.height = 32;
            animationType = NPCID.Pixie;   
			 Main.npcFrameCount[npc.type] = 4;   
            npc.value = 200;
            npc.npcSlots = 1f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.buffImmune[24] = true;
            npc.netAlways = false;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
			
			return SpawnCondition.Meteor.Chance * 0.16f;
        }
		
		public override void NPCLoot()
		{
		
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.Bomb), 1);
			for(int i = 0; i < 8; i++)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), mod.ProjectileType("GeodeBomb"), (int)(npc.damage * 0.75) + 1, 0, 0);
			}
				
		}	
		public override void AI()
		{
			Dust.NewDust(new Vector2(npc.position.X, npc.position.Y),62,32, 6);
		}
	
	}
}