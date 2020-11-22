using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;

namespace SOTS.Projectiles.Crushers
{    
    public class WormWoodArm : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wormwood Arm");
			
		}
		
        public override void SetDefaults()
        {
			projectile.width = 26;
			projectile.height = 22;
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.timeLeft = 6004;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 0;
		}
		
		///These are for modification
		private float maxDamage = 5; //total damage %, 1 for no increase, 5 for 500% at max charge, etc
		private int chargeTime = 150; //charge time in frames
		private int explosiveCountMin = 3; //amount of explosions minimum
		private int explosiveCountMax = 4; //amount of explosions max
		private float explosiveRange = 48; //distance between each explosion
		private float releaseTime = 180; //how long in frames until auto-release
		///Make sure to change the released projectile down near the bottom
		
		///These are also for modification (but not recommended)
		private float accSpeed = 0.3f; //speed of the retraction, scales exponentially
		private float initialExplosiveRange = 48; //distance between player and first explosion (also the distance between the player and the crusher)
		
		///DO NOT MODIFY
		private bool released = false; 
		private float rotationTimer = 0; //assume 170 is full rotation, 0 has not been rotated
		private int explosive = 1;
		float currentCharge = 0; //how close are we to chargeTime?
		float initiateTimer = 0; //how close are we to releaseTime?
		private bool flip = false; //direction?
		private bool initiate = true; //initiate some variables
		private int initialDamage; //what is the initial damage?
		private float accelerateAmount = 0;
		
		public override bool PreAI()
		{
			VoidPlayer modPlayer = VoidPlayer.ModPlayer(Main.player[projectile.owner]);
			if (initiate)
			{
				initiate = false;
				initialDamage = projectile.damage;
				if(projectile.knockBack == 1) //knockback has been used on the item to store the projectile's direction
				{
					flip = true;
					projectile.spriteDirection = -1;
					projectile.knockBack = -1;
				}
				if(projectile.knockBack == 0)
				{
					projectile.knockBack = -1;
				}
			}
			if(currentCharge < chargeTime && !released) //not charged and not released
			{
				currentCharge += 1 * (1f / Main.player[projectile.owner].meleeSpeed + modPlayer.voidSpeed - 1);
				explosive = explosiveCountMin + (int)(((float)currentCharge/(float)chargeTime) * (explosiveCountMax + 1 - explosiveCountMin)); //count
				explosive = explosive < explosiveCountMin ? explosiveCountMin : explosiveCountMax < explosive ? explosiveCountMax : explosive; //making sure the explosive amount is within range
				/** Here's an example of the maxExplosion/minExplosion system
				* Say your min is 3 and max is 5, we start by finding the possible numbers, 3, 4, and 5 (3 numbers total)
				* Now we split those numbers among 3 charge percentages, 0-33 : 3, 34-66 : 4, 67 - 100 : 5
				* The various code above MAY not be optimal, but it works
				*/
				rotationTimer = ((float)currentCharge/(float)chargeTime) * 170; //making the rotation timer proportional to the charge time completed
				float increaseDamage = 1 + ((maxDamage - 1f)/170f) * rotationTimer;
				projectile.damage = (int)(initialDamage * increaseDamage);
			}
			else if(!released) //after full charge, before release
			{
				initiateTimer += 1 * (1f / Main.player[projectile.owner].meleeSpeed + modPlayer.voidSpeed - 1);
				projectile.damage = (int)(initialDamage * maxDamage);
			}
				
			return projectile.active;
		}
		public override void AI()
		{
			projectile.ai[0]++;
			Player player  = Main.player[projectile.owner];
			if(player.whoAmI == Main.myPlayer)
			{
				if((int)projectile.ai[0] % 3 == 0)
				{
					projectile.netUpdate = true;
				}
				Vector2 cursorArea = Main.MouseWorld;
					
				float shootToX = cursorArea.X - player.Center.X;
				float shootToY = cursorArea.Y - player.Center.Y;
				double direction = Math.Atan2((double)-shootToY, (double)-shootToX);
				double degDirection = direction	* 180/Math.PI;
							
				if(initiateTimer >= releaseTime)
				{
					released = true;
				}
				if(player.channel || projectile.timeLeft > 6001)
				{
					projectile.timeLeft = 6000;
					projectile.alpha = 0;
				}
				else
				{
					released = true;
				}
				if(released)
				{
					accelerateAmount += accSpeed;
					if(rotationTimer <= 0) //collision
					{
						if(projectile.owner == Main.myPlayer)
						{
							double rad1 = direction;
							for(int i = 0; i < explosive; i++)
							{
								double distance = (explosiveRange * i) + initialExplosiveRange;
								float positionX = player.Center.X - (int)(Math.Cos(rad1) * distance);
								float positionY = player.Center.Y - (int)(Math.Sin(rad1) * distance);
								Projectile.NewProjectile(positionX, positionY, 0, 0, mod.ProjectileType("PinkCrush"), projectile.damage, initialDamage, Main.myPlayer, 0f, 0f);
							}
						}
						projectile.Kill();
					}
					rotationTimer -= accelerateAmount; //start to close the crushers
				}
				if(flip)
				{
					projectile.ai[1] = rotationTimer + 5 + (float)degDirection; //add rotate
					projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 315);
				}
				else //when not flip
				{
					projectile.ai[1] = -rotationTimer - 5 + (float)degDirection; //subtract rotate
					projectile.rotation = MathHelper.ToRadians(projectile.ai[1] + 225);
				}
				double deg = (double) projectile.ai[1]; 
				double rad = deg * (Math.PI / 180);
				projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * initialExplosiveRange) - projectile.width/2;
				projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * initialExplosiveRange) - projectile.height/2;
			}
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(projectile.rotation);
			writer.Write(projectile.spriteDirection);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			projectile.rotation = reader.ReadSingle();
			projectile.spriteDirection = reader.ReadInt32();
		}
	}
}
			