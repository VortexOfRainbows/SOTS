using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Chess
{
	public class FrightPawn : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Fright Pawn");
		}
		public override void SetDefaults()
		{
			
			npc.CloneDefaults(73);
            aiType = 73; //18 is the demon scythe style
            npc.lifeMax = 150;   //boss life
            npc.damage = 24;  //boss damage
            npc.defense = 4;    //boss defense
            npc.knockBackResist = 0.2f;
            npc.width = 28;
            npc.height = 40;
			animationType = 3;
			 Main.npcFrameCount[npc.type] = 3;    //boss frame/animation
            npc.value = 1000;
            npc.npcSlots = 0f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath7;
            npc.netAlways = false;
		}
		public override void NPCLoot()
		{
			
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.SoulofFright), 1);
		
			if((Main.rand.Next(19) == 0))
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("SoulSingularity")), 1);
				
			if((Main.rand.Next(199) == 0))
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("FrightBlade")), 1);	
				
			
		}
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, 292, 43, 1, 0);
}
		
			
			
			
}
	}
