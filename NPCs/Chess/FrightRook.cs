using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Chess
{
	public class FrightRook : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Fright Rook");
		}
		public override void SetDefaults()
		{
			
			npc.CloneDefaults(73);
            aiType = 73; //18 is the demon scythe style
            npc.lifeMax = 1000;   //boss life
            npc.damage = 1;  //boss damage
            npc.defense = 4;    //boss defense
            npc.knockBackResist = 0.2f;
            npc.width = 46;
            npc.height = 44;
			animationType = 3;
			 Main.npcFrameCount[npc.type] = 3;    //boss frame/animation
            npc.value = 1000;
            npc.npcSlots = 0f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
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
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("FrightEruption")), 1);
		
			if((Main.rand.Next(299) == 0))
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("FrightBlood")), 1);	
				
			
		}
		public override void AI()
		{
		 Player player  = Main.player[npc.target];
			npc.ai[0]++;
			if(npc.ai[0] >= 30)
			{
				if(player.position.X - 80 > npc.position.X && npc.ai[0] == 30)
				{
					npc.velocity.X += 40;
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -5, 0, mod.ProjectileType("Brimstone"), 22, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 5, 0, mod.ProjectileType("Brimstone"), 22, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 5, mod.ProjectileType("Brimstone"), 22, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -5, mod.ProjectileType("Brimstone"), 22, 1, 0);
				}
				if(player.position.X + 80 < npc.position.X && npc.ai[0] == 30)
				{
					npc.velocity.X -= 40;
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -5, 0, mod.ProjectileType("Brimstone"), 22, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 5, 0, mod.ProjectileType("Brimstone"), 22, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 5, mod.ProjectileType("Brimstone"), 22, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -5, mod.ProjectileType("Brimstone"), 22, 1, 0);
				}
				if(npc.ai[0] == 45)
				{
					npc.velocity.X = 0;
				}
				if(npc.ai[0] == 60)
				{
					
				if(player.position.Y -80 > npc.position.Y)
				{
					npc.velocity.Y += 30;
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -5, 0, mod.ProjectileType("Brimstone"), 22, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 5, 0, mod.ProjectileType("Brimstone"), 22, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 5, mod.ProjectileType("Brimstone"), 22, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -5, mod.ProjectileType("Brimstone"), 22, 1, 0);
				}
				if(player.position.Y +80 < npc.position.Y)
				{
					npc.velocity.Y -= 30;
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -5, 0, mod.ProjectileType("Brimstone"), 22, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 5, 0, mod.ProjectileType("Brimstone"), 22, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 5, mod.ProjectileType("Brimstone"), 22, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -5, mod.ProjectileType("Brimstone"), 22, 1, 0);
				}
				
					npc.ai[0] = 0;
					
				
				
				
				
				
				
				
				}
			}
			
				if(npc.ai[0] == 5)
				{
					npc.velocity.Y = 0;
				}
				
				
			
		}
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
		player.AddBuff(mod.BuffType("BloodTapped"), 300);
}
		
			
			
			
}
	}
