using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
namespace SOTS.Projectiles.Minions
{    
    public class CursedBlade : ModProjectile 
    {
		int initiate = -1;
		double startingRotation;
		float rotationAreaX;
		float rotationAreaY;
		float goToCursorX;
		float goToCursorY;
		float cursorDistance;
		float eternalBetweenPlayer = 196;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Blade");
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(projectile.rotation);
			writer.Write(projectile.spriteDirection);
			writer.Write(initiate);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			projectile.rotation = reader.ReadSingle();
			projectile.spriteDirection = reader.ReadInt32();
			initiate = reader.ReadInt32();
		}
        public override void SetDefaults()
        {
			projectile.height = 34;
			projectile.width = 34;
			projectile.minion = true;
			projectile.penetrate = -1;
			projectile.friendly = false;
            Main.projFrames[projectile.type] = 1;
			projectile.timeLeft = 10000;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.netImportant = true;
		}
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 2.2f / 255f, (255 - projectile.alpha) * 0.75f / 255f, (255 - projectile.alpha) * 2.45f / 255f);
			if(initiate == -1)
			{
				startingRotation = Main.rand.Next(360);
				initiate = 1;
			}
			projectile.alpha = 0;
			Player player  = Main.player[projectile.owner];
			if(player.whoAmI == Main.myPlayer)
			{
				projectile.netUpdate = true;
				if(player.FindBuffIndex(mod.BuffType("CursedBlade")) > -1)
				{
					projectile.timeLeft = 6;
				}
					
				double deg = (double)startingRotation; 
				double rad = deg * (Math.PI / 180);
				double dist = eternalBetweenPlayer;
				rotationAreaX = player.Center.X - (int)(Math.Cos(rad) * dist);
				rotationAreaY = player.Center.Y - (int)(Math.Sin(rad) * dist);
				
				
			
				projectile.friendly = false;
				projectile.ai[0]++;
				
				Vector2 cursorArea = Main.MouseWorld;
				
				goToCursorX = cursorArea.X - projectile.Center.X;
				goToCursorY = cursorArea.Y - projectile.Center.Y;
				cursorDistance = (float)System.Math.Sqrt((double)(goToCursorX * goToCursorX + goToCursorY * goToCursorY));

				float cursorDistance2 = 2.45f / cursorDistance;
			   
				goToCursorX *= cursorDistance2 * 5;
				goToCursorY *= cursorDistance2 * 5;
						   
				double startingDirection = Math.Atan2((double)-goToCursorY, (double)-goToCursorX);
				startingDirection *= 180/Math.PI;
				
				
				
				projectile.ai[1] = (float)startingDirection;
			
				projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 225);
			
				startingRotation += 1.2f;
				if(projectile.ai[0] <= 150 )
				{
					float shootToX = rotationAreaX - projectile.Center.X;
					float shootToY = rotationAreaY - projectile.Center.Y;
					float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
				   
					distance = .75f / distance;
				
					shootToX *= distance * 5;
					shootToY *= distance * 5;
				   
					projectile.velocity.X = shootToX;
					projectile.velocity.Y = shootToY; 
				}
				if(projectile.ai[0] == 151 && cursorDistance > 90f)
				{
					projectile.velocity.X = goToCursorX;
					projectile.velocity.Y = goToCursorY;
					projectile.ai[0] = 150;			   
				}
				if(projectile.ai[0] >= 152 && projectile.ai[0] <= 155)
				{
					projectile.velocity.X *= 0.5f;
					projectile.velocity.Y *= 0.5f;
				}
				if(projectile.ai[0] >= 155 && projectile.ai[0] < 160)
				{
					projectile.velocity.X *= 0.94f;
					projectile.velocity.Y *= 0.94f;
					float newRotation = MathHelper.ToRadians((projectile.ai[0] - 174) * 60);
					newRotation -= MathHelper.ToRadians(50);
					projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 225) + newRotation;
					
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("CurseExplosion"), projectile.damage, 2, Main.myPlayer, 0f, 0f);
					Main.PlaySound(SoundID.Item1, (int)(projectile.Center.X), (int)(projectile.Center.Y));
				}
				if(projectile.ai[0] >= 160 && projectile.ai[0] < 165)
				{
					projectile.velocity.X *= 1.3f;
					projectile.velocity.Y *= 1.3f;
					projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 225 + Main.rand.Next(360));
					
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("CurseExplosion"), projectile.damage, 2, Main.myPlayer, 0f, 0f);
					Main.PlaySound(SoundID.Item1, (int)(projectile.Center.X), (int)(projectile.Center.Y));
				}
				if(projectile.ai[0] >= 165 && projectile.ai[0] < 170)
				{
					projectile.velocity.X *= 0.94f;
					projectile.velocity.Y *= 0.94f;
					projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 225 + Main.rand.Next(360));
					
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("CurseExplosion"), projectile.damage, 2, Main.myPlayer, 0f, 0f);
					Main.PlaySound(SoundID.Item1, (int)(projectile.Center.X), (int)(projectile.Center.Y));
				}
				if(projectile.ai[0] >= 170 && projectile.ai[0] < 175)
				{
					projectile.velocity.X *= 1.3f;
					projectile.velocity.Y *= 1.3f;
					projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 225 + Main.rand.Next(360));
					
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("CurseExplosion"), projectile.damage, 2, Main.myPlayer, 0f, 0f);
					Main.PlaySound(SoundID.Item1, (int)(projectile.Center.X), (int)(projectile.Center.Y));
				}
				if(projectile.ai[0] >= 200)
				{
					projectile.ai[0] = -30;
				}
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //target.immune[projectile.owner] = 15;
        }
	}
}
		