using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{    
    public class StormArrow : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tracer Arrow");
		}
		
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(1);
            AIType = 1;
			Projectile.alpha = 100;
			Projectile.width = 18;
			Projectile.height = 38;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 20;
		}
		public void RegisterPhantoms(Player player)
		{
			int npcIndex = -1;
			int npcIndex1 = -1;
			int npcIndex2 = -1;
			int npcIndex3 = -1;
			for(int j = 0; j < 4; j++)
			{
				double distanceTB = 900;
				for(int i = 0; i < 200; i++) //find first enemy
				{
					NPC npc = Main.npc[i];
					if(!npc.friendly && npc.lifeMax > 5 && npc.active && npc.CanBeChasedBy())
					{
						if(npcIndex != i && npcIndex1 != i && npcIndex2 != i && npcIndex3 != i)
						{
							float disX = Projectile.Center.X - npc.Center.X;
							float disY = Projectile.Center.Y - npc.Center.Y;
							double dis = Math.Sqrt(disX * disX + disY * disY);
							if(dis < distanceTB && j == 0)
							{
								distanceTB = dis;
								npcIndex = i;
							}
							if(dis < distanceTB && j == 1)
							{
								distanceTB = dis;
								npcIndex1 = i;
							}
							if(dis < distanceTB && j == 2)
							{
								distanceTB = dis;
								npcIndex2 = i;
							}
							if(dis < distanceTB && j == 3)
							{
								distanceTB = dis;
								npcIndex3 = i;
							}
						}
					}
				}
			}
			if(npcIndex != -1)
			{
				NPC npc = Main.npc[npcIndex];
				float spawnPosX = npc.Center.X;
				float spawnPosY = npc.position.Y - 24;
				
				for(int i = 0; i < 25; i++)
				{
					int num2 = Dust.NewDust(new Vector2(spawnPosX - 10, spawnPosY - 10), 20, 20, 16);
					Main.dust[num2].noGravity = true;
					Main.dust[num2].alpha = 125;
					Main.dust[num2].scale = 1.75f;
				}
				
				if(!npc.friendly && npc.lifeMax > 5 && npc.active && Main.myPlayer == Projectile.owner)
				{
					int newIndex = Projectile.NewProjectile(spawnPosX, spawnPosY, npc.velocity.X * 0.8f, 15 + npc.velocity.Y * 0.8f, (int)(Projectile.ai[1]), Projectile.damage, Projectile.knockBack, player.whoAmI);
				}
			}
			if(npcIndex1 != -1)
			{
				NPC npc = Main.npc[npcIndex1];
				float spawnPosX = npc.Center.X;
				float spawnPosY = npc.position.Y - 24;
				
				for(int i = 0; i < 25; i++)
				{
					int num2 = Dust.NewDust(new Vector2(spawnPosX - 10, spawnPosY - 10), 20, 20, 16);
					Main.dust[num2].noGravity = true;
					Main.dust[num2].alpha = 125;
					Main.dust[num2].scale = 1.75f;
				}
				
				if(!npc.friendly && npc.lifeMax > 5 && npc.active && Main.myPlayer == Projectile.owner)
				{
					int newIndex1 = Projectile.NewProjectile(spawnPosX, spawnPosY, npc.velocity.X * 0.8f, 15 + npc.velocity.Y * 0.8f, (int)(Projectile.ai[1]), Projectile.damage, Projectile.knockBack, player.whoAmI);
				}
			}
			if(npcIndex2 != -1)
			{
				NPC npc = Main.npc[npcIndex2];
				float spawnPosX = npc.Center.X;
				float spawnPosY = npc.position.Y - 24;
				
				for(int i = 0; i < 25; i++)
				{
					int num2 = Dust.NewDust(new Vector2(spawnPosX - 10, spawnPosY - 10), 20, 20, 16);
					Main.dust[num2].noGravity = true;
					Main.dust[num2].alpha = 125;
					Main.dust[num2].scale = 1.75f;
				}
				
				if(!npc.friendly && npc.lifeMax > 5 && npc.active && Main.myPlayer == Projectile.owner)
				{
					int newIndex2 = Projectile.NewProjectile(spawnPosX, spawnPosY, npc.velocity.X * 0.8f, 15 + npc.velocity.Y * 0.8f, (int)(Projectile.ai[1]), Projectile.damage, Projectile.knockBack, player.whoAmI);
				}
			}
			if(npcIndex3 != -1)
			{
				NPC npc = Main.npc[npcIndex3];
				float spawnPosX = npc.Center.X;
				float spawnPosY = npc.position.Y - 24;
				
				for(int i = 0; i < 25; i++)
				{
					int num2 = Dust.NewDust(new Vector2(spawnPosX - 10, spawnPosY - 10), 20, 20, 16);
					Main.dust[num2].noGravity = true;
					Main.dust[num2].alpha = 125;
					Main.dust[num2].scale = 1.75f;
				}
				
				if(!npc.friendly && npc.lifeMax > 5 && npc.active && Main.myPlayer == Projectile.owner)
				{
					int newIndex2 = Projectile.NewProjectile(spawnPosX, spawnPosY, npc.velocity.X * 0.8f, 15 + npc.velocity.Y * 0.8f, (int)(Projectile.ai[1]), Projectile.damage, Projectile.knockBack, player.whoAmI);
				}
			}
		}
		public override void Kill(int timeLeft)
        {
			RegisterPhantoms(Main.player[Projectile.owner]);
		}
		public override void AI()
		{
			Projectile.alpha += 6;
			if(Projectile.timeLeft < 3)
			{
				for(int i = 0; i < 20; i++)
				{
					int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 18, 38, 16);
					Main.dust[num2].noGravity = true;
					Main.dust[num2].alpha = 200;
				}
			}
			int num1 = Dust.NewDust(new Vector2(Projectile.Center.X - 2, Projectile.Center.Y - 2), 4, 4, 16);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			Main.dust[num1].alpha = Projectile.alpha;
		}
	}
}
		
			