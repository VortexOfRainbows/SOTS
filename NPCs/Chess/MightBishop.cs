using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Chess
{
	public class MightBishop : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Might Bishop");
		}
		public override void SetDefaults()
		{
			
			npc.CloneDefaults(49);
            aiType = 49; //18 is the demon scythe style
            npc.lifeMax = 275;   //boss life
            npc.damage = 34;  //boss damage
            npc.defense = 12;    //boss defense
            npc.knockBackResist = 0.2f;
            npc.width = 78;
            npc.height = 78;
			animationType = NPCID.Probe;
			 Main.npcFrameCount[npc.type] = 1;    //boss frame/animation
            npc.value = 1200;
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
			
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -3, -3, mod.ProjectileType("MightEssence"), 1, 0, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 3, -3, mod.ProjectileType("MightEssence"), 1, 0, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -3, 3, mod.ProjectileType("MightEssence"), 1, 0, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 3, 3, mod.ProjectileType("MightEssence"), 1, 0, 0);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.SoulofMight), 1);
		
			if((Main.rand.Next(19) == 0))
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("SoulSingularity")), 1);
				
			if((Main.rand.Next(199) == 0))
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("MightyTrail")), 1);	
				
			
		}
		
			
			
			
}
	}
