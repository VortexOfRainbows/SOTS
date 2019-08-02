using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles 
{    
    public class StarGate : ModProjectile 
    {	int split;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zap");
			
		}
		
        public override void SetDefaults()
        {	
			int projAM = 5;
			if(NPC.downedBoss1)
			{//10
				projAM += 5;
				
			}
			
			if(NPC.downedBoss2)
			{
				//20
				projAM += 10;
			
			}
			if(NPC.downedBoss3)
			{
				//30
				projAM += 10;
			}
			
			if(Main.hardMode)
			{
				//50
				projAM += 20;
				//17
			}
			
			if(NPC.downedMechBoss1)
			{
				//65
				projAM += 15;
				//16
			}
			
			if(NPC.downedMechBoss2)
			{
				//80
				projAM += 15;
				//15
			}
			
			
			if(NPC.downedMechBoss3)
			{
				//95
				projAM += 15;
				//14
			}
			
			if(NPC.downedMechBoss3 && NPC.downedMechBoss2 && NPC.downedMechBoss1)
			{
				//120
				projAM += 25;
				//12
			}
			if(NPC.downedPlantBoss)
			{
				//180
				projAM += 50;
				//10
			}
			
			if(NPC.downedGolemBoss)
			{
				//250
				projAM += 70;
				//8
			}
			if(NPC.downedMoonlord)
			{
				//350
				projAM += 100;
				//5
			}
		
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.ranged = true; 
			projectile.width = 100;
			projectile.tileCollide = false;
			projectile.height = 105;
			projectile.timeLeft = projAM * 2;
            Main.projFrames[projectile.type] = 5;
			projectile.alpha = 0;
		}
		public override void AI()
        {	
		split++;
			if(split >= 2)
			{
				Projectile.NewProjectile((projectile.Center.X) + Main.rand.Next(-24, 25), (projectile.Center.Y) + Main.rand.Next(-24, 25), Main.rand.Next(-1, 2), 21, 521 , projectile.damage, 1, 0);
				
		split = 0;
			}
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y),100, 105, 65);
			
			if (++projectile.frameCounter >= 5)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 5)
				{
					projectile.frame = 0;
				}
}
		}
		
		
}
}
			