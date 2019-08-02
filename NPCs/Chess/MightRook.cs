using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Chess
{
	public class MightRook : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Might Rook");
		}
		public override void SetDefaults()
		{
			
			npc.CloneDefaults(73);
            aiType = 73; //18 is the demon scythe style
            npc.lifeMax = 900;   //boss life
            npc.damage = 35;  //boss damage
            npc.defense = 9;    //boss defense
            npc.knockBackResist = 0.2f;
            npc.width = 46;
            npc.height = 44;
			animationType = 3;
			 Main.npcFrameCount[npc.type] = 3;    //boss frame/animation
            npc.value = 900;
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
			
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.SoulofMight), 1);
		
			if((Main.rand.Next(19) == 0))
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("SoulSingularity")), 1);
				
			if((Main.rand.Next(199) == 0))
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("MightyFlood")), 1);
		
			if((Main.rand.Next(299) == 0))
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("MightyDart")), 1);	
				
			
		}
		public override void AI()
		{
		 Player player  = Main.player[npc.target];
			npc.ai[0]++;
				if(npc.ai[0] >= 150)
				{
					npc.ai[0] = 0;
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -10, 0, mod.ProjectileType("MightyBlade"), 36, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 10, 0, mod.ProjectileType("MightyBlade"), 36, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 10, mod.ProjectileType("MightyBlade"), 36, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -10, mod.ProjectileType("MightyBlade"), 36, 1, 0);
				}
			
			
		}
				
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
		player.AddBuff(BuffID.Obstructed, 300);
}
		
			
			
			
}
	}
