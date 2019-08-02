using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.MPLunar
{[AutoloadBossHead]
	public class Demos : ModNPC
	{	int despawn = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Demos");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 5;  //5 is the flying AI
            npc.lifeMax = 750000;   //boss life
            npc.damage = 1000;  //boss damage
            npc.defense = 0;    //boss defense
            npc.knockBackResist = 0.25f;
            npc.width = 110;
            npc.height = 122;
            animationType = NPCID.Probe;   //this boss will behavior like the DemonEye
			 Main.npcFrameCount[npc.type] = 1;    //boss frame/animation
            npc.value = 200;
            npc.npcSlots = 1f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath61;
            npc.buffImmune[24] = true;
            npc.netAlways = false;
			
            music = MusicID.LunarBoss;
		}
		
		
	public override void AI()
	{
		 Player player  = Main.player[npc.target];
		npc.position.X = player.position.X - 750;
		Main.time = 27000;
		
		npc.position.Y = player.position.Y - 0;
		
		
				if( Main.rand.Next(50) == 0)
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 4, 0, mod.ProjectileType("PurPyre"), 100, 0, Main.myPlayer);
		   if(Main.player[npc.target].dead)
			{
			 despawn++;
			}
			if(despawn >= 720)
			{
			npc.active = false;
			}
	}public override void BossLoot(ref string name, ref int potionType)
		{ 
	//	potionType = ItemID.SuperHealingPotion;
	
	//	if(Main.expertMode)
		
	//	{ 
	//	Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("NightmareFuel"), Main.rand.Next(1, 4)); 
	//	Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("NightmareManipulator"), 1); 
	//	} 
	//	else 
		//	{
			//	Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("NightmareFuel"), Main.rand.Next(1, 4)); 
			if(!NPC.AnyNPCs(mod.NPCType("Phobos")))
			{
		Main.NewText("Ancient text is inscribed on screen", 255, 0, 0);
		Main.NewText("'Come back next update'", 255, 0, 0);
			}
		
	//}
}
	
	}
}