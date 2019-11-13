using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.HolyRelics
{    
    public class DeoxysAttack : ModProjectile 
    {	int level = 5;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deoxys");
			
		}
		
        public override void SetDefaults()
        {
		
			
            projectile.CloneDefaults(ProjectileID.Raven);

            aiType = ProjectileID.Raven;
			projectile.netImportant = true;
            projectile.width = 59;
            projectile.height = 84; 
            projectile.timeLeft = 255;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.magic = true; 
			projectile.minion = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = false;
			projectile.minionSlots = 0;		
			


		}
        public override bool PreAI()
        {
			
            Player player = Main.player[projectile.owner];
			player.bunny = false; // Relic from aiType
            return true;
        }
		public override void AI() //The projectile's AI/ what the projectile does
		{
            projectile.tileCollide = false;
			projectile.damage = 8;
			level = 5;
			if(NPC.downedBoss1)
			{//4
				projectile.damage += (int)(projectile.damage * 0.25);
				level += 2;
			}
			
			if(NPC.downedBoss2)
			{
				//5
				projectile.damage += (int)(projectile.damage * 0.33);
				level += 3;
				//19
			}
			if(NPC.downedBoss3)
			{
				//7
				projectile.damage += (int)(projectile.damage * 0.33);
				level += 4;
				//18
			}
			if(NPC.downedSlimeKing)
			{
				//11
				projectile.damage += (int)(projectile.damage * 0.2);
				level += 2;
				//17
			}
			if(NPC.downedQueenBee)
			{
				//11
				projectile.damage += (int)(projectile.damage * 0.33);
				level += 4;
				//17
			}
			
			if(Main.hardMode)
			{
				//11
				projectile.damage += (int)(projectile.damage * 0.40);
				level += 5;
				//17
			}
			
			if(NPC.downedMechBoss1)
			{
				//16
				projectile.damage += (int)(projectile.damage * 0.3);
				level += 4;
				//16
			}
			
			if(NPC.downedMechBoss2)
			{
				//21
				projectile.damage += (int)(projectile.damage * 0.3);
				level += 4;
				//15
			}
			
			
			if(NPC.downedMechBoss3)
			{
				//26
				projectile.damage += (int)(projectile.damage * 0.3);
				level += 4;
				//14
			}
			
			
			if(NPC.downedMechBoss3 && NPC.downedMechBoss2 && NPC.downedMechBoss1)
			{
				//32
				projectile.damage += (int)(projectile.damage * 0.3);
				level += 5;
				//12
			}
			if(NPC.downedPlantBoss)
			{
				//39
				projectile.damage += (int)(projectile.damage * 0.3);
				level += 14;
				//10
			}
			
			if(NPC.downedGolemBoss)
			{
				//50
				projectile.damage += (int)(projectile.damage * 0.3);
				level += 8;
				//8
			}
			if(NPC.downedMoonlord)
			{
				//65
				projectile.damage += (int)(projectile.damage * 1);
				level += 12;
				//5
			}
			if(NPC.downedAncientCultist)
			{
				
				projectile.damage += (int)(projectile.damage * 0.4);
				level += 5;
			}
			if(NPC.downedGoblins)
			{
				projectile.damage += (int)(projectile.damage * 0.2);
				level += 5;
				
			}
			if(NPC.downedPirates)
			{
				projectile.damage += (int)(projectile.damage * 0.2);
				level += 7;
				
			}
			if(NPC.downedMartians)
			{
				projectile.damage += (int)(projectile.damage * 0.2);
				level += 7;
				
			}
			string myString = level.ToString();


			Player player = Main.player[projectile.owner];
            SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.6f, 0.9f, 0.3f);
		
			if(modPlayer.deoxysPet == false)
			{
				Main.NewText("Deoxys's current level is", 255, 255, 255);
				Main.NewText(myString, 255, 255, 255);
				projectile.Kill();
			}
            if (player.dead)
            {
                modPlayer.deoxysPet = false;
            }
            if (modPlayer.deoxysPet)
            {
				
                projectile.timeLeft = 2;
            }
		}
	}
	
}
