using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Chess
{
	public class SightRook : ModNPC
	{	int summon = 1;
	int spawn = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Sight Rook");
		}
		public override void SetDefaults()
		{
			
			
			npc.CloneDefaults(73);
            aiType = 73; //18 is the demon scythe style
            npc.lifeMax = 900;   //boss life
            npc.damage = 50;  //boss damage
            npc.defense = 12;    //boss defense
            npc.knockBackResist = 0.2f;
            npc.width = 46;
            npc.height = 44;
			animationType = NPCID.Harpy;
			 Main.npcFrameCount[npc.type] = 6;    //boss frame/animation
            npc.value = 1000;
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
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("SightSnap")), 1);
		
			if((Main.rand.Next(299) == 0))
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("SightFountain")), 1);	
				
			
		}
		public override void AI()
		{
			npc.rotation = 0;
				if( Main.rand.Next(360) == 0 && summon == 1)
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
			if(spawn >= 60)
			{
				
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -5, mod.ProjectileType("SightEssence"), 0, 0, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 5, mod.ProjectileType("SightEssence"), 0, 0, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -5, 0, mod.ProjectileType("SightEssence"), 0, 0, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 5, 0, mod.ProjectileType("SightEssence"), 0, 0, 0);
				summon = 1;
				spawn = 0;
			}
				
			
		}
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
		player.AddBuff(mod.BuffType("Catalyst"), 300);
}
		
			
			
			
}
	}
