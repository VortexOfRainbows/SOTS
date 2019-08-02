using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Chess
{
	public class FrightBishop : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Fright Bishop");
		}
		public override void SetDefaults()
		{
			
			npc.CloneDefaults(49);
            aiType = 49; //18 is the demon scythe style
            npc.lifeMax = 250;   //boss life
            npc.damage = 44;  //boss damage
            npc.defense = 4;    //boss defense
            npc.knockBackResist = 0.2f;
            npc.width = 62;
            npc.height = 70;
			animationType = NPCID.Probe;
			 Main.npcFrameCount[npc.type] = 1;    //boss frame/animation
            npc.value = 1200;
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
			
				Projectile.NewProjectile(npc.Center.X + 60, npc.Center.Y + 60, 0, 0, 292, 27, 0, 0);
				Projectile.NewProjectile(npc.Center.X + 60, npc.Center.Y - 60, 0, 0, 292, 27, 0, 0);
				Projectile.NewProjectile(npc.Center.X - 60, npc.Center.Y + 60, 0, 0, 292, 27, 0, 0);
				Projectile.NewProjectile(npc.Center.X - 60, npc.Center.Y - 60, 0, 0, 292, 27, 0, 0);
				Projectile.NewProjectile(npc.Center.X + 160, npc.Center.Y + 160, 0, 0, 292, 27, 0, 0);
				Projectile.NewProjectile(npc.Center.X + 160, npc.Center.Y - 160, 0, 0, 292, 27, 0, 0);
				Projectile.NewProjectile(npc.Center.X - 160, npc.Center.Y + 160, 0, 0, 292, 27, 0, 0);
				Projectile.NewProjectile(npc.Center.X - 160, npc.Center.Y - 160, 0, 0, 292, 27, 0, 0);
				Projectile.NewProjectile(npc.Center.X + 260, npc.Center.Y + 260, 0, 0, 292, 27, 0, 0);
				Projectile.NewProjectile(npc.Center.X + 260, npc.Center.Y - 260, 0, 0, 292, 27, 0, 0);
				Projectile.NewProjectile(npc.Center.X - 260, npc.Center.Y + 260, 0, 0, 292, 27, 0, 0);
				Projectile.NewProjectile(npc.Center.X - 260, npc.Center.Y - 260, 0, 0, 292, 27, 0, 0);
				Projectile.NewProjectile(npc.Center.X + 360, npc.Center.Y + 360, 0, 0, 292, 27, 0, 0);
				Projectile.NewProjectile(npc.Center.X + 360, npc.Center.Y - 360, 0, 0, 292, 27, 0, 0);
				Projectile.NewProjectile(npc.Center.X - 360, npc.Center.Y + 360, 0, 0, 292, 27, 0, 0);
				Projectile.NewProjectile(npc.Center.X - 360, npc.Center.Y - 360, 0, 0, 292, 27, 0, 0);
				Projectile.NewProjectile(npc.Center.X + 460, npc.Center.Y + 460, 0, 0, 292, 27, 0, 0);
				Projectile.NewProjectile(npc.Center.X + 460, npc.Center.Y - 460, 0, 0, 292, 27, 0, 0);
				Projectile.NewProjectile(npc.Center.X - 460, npc.Center.Y + 460, 0, 0, 292, 27, 0, 0);
				Projectile.NewProjectile(npc.Center.X - 460, npc.Center.Y - 460, 0, 0, 292, 27, 0, 0);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.SoulofFright), 1);
		
			if((Main.rand.Next(19) == 0))
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("SoulSingularity")), 1);
				
			if((Main.rand.Next(199) == 0))
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("FrightBattery")), 1);	
				
			
		}
		
			
			
			
}
	}
