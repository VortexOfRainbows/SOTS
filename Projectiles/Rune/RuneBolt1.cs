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

namespace SOTS.Projectiles.Rune
{    
    public class RuneBolt1 : ModProjectile 
    {	int wait = 0;
		bool oil = false;
		bool child = false;
		int childPosition = 0;
		bool boomerang = false;
		bool paralysis = false;
		bool turret = false;
		int turretRate = 0;
		float lifesteal = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rune Bolt");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; //18 is the demon scythe style
			projectile.alpha = 0;
			projectile.timeLeft = 900;
			projectile.ranged = false;
			projectile.width = 12;
			projectile.height = 12;


		}
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			if(child)
			{
				Item rune = player.inventory[childPosition];
				if(rune.type == mod.ItemType("SingleShotRune"))
				{
					int damage = (int)(rune.stack * 3f);
					float newVelocityX = projectile.velocity.X;
					float newVelocityY = projectile.velocity.Y;
					
					if(boomerang)
					{
						newVelocityX *= -1;
						newVelocityY *= -1;
					}
                  Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, newVelocityX, newVelocityY, projectile.type, (int)(projectile.damage * 0.5f) + damage, childPosition, player.whoAmI);
					
				}
				if(rune.type == mod.ItemType("LaserRune"))
				{
					int damage = (int)(rune.stack * 5f);
					float newVelocityX = projectile.velocity.X * 2;
					float newVelocityY = projectile.velocity.Y * 2; 
					
					if(boomerang)
					{
						newVelocityX *= -1;
						newVelocityY *= -1;
					}
                  Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, newVelocityX, newVelocityY, projectile.type, (int)(projectile.damage * 0.5f) + damage, childPosition, player.whoAmI);
					
				}
				if(rune.type == mod.ItemType("ShotgunRune"))
				{
					int damage = (int)(rune.stack * -2f);
					int numberProjectiles = rune.stack * 2;
					float newVelocityX = projectile.velocity.X;
					float newVelocityY = projectile.velocity.Y;
					if(boomerang)
					{
						newVelocityX *= -1;
						newVelocityY *= -1;
					}
					for(int i = 0; i < numberProjectiles; i++)
					{
					Vector2 perturbedSpeed = new Vector2(newVelocityX, newVelocityY).RotatedByRandom(MathHelper.ToRadians((int)(numberProjectiles * 3f))); // This defines the projectiles random spread . 30 degree spread.
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, projectile.type, (int)(projectile.damage * 0.5f) + damage, childPosition, player.whoAmI);
					}
				}
				if(rune.type == mod.ItemType("FlowerRune"))
				{
					int damage = (int)(rune.stack * -1f);
					int numberProjectiles = rune.stack * 4;
					float newVelocityX = projectile.velocity.X;
					float newVelocityY = projectile.velocity.Y;
					if(boomerang)
					{
						newVelocityX *= -1;
						newVelocityY *= -1;
					}
					for(int i = 0; i < numberProjectiles; i++)
					{
					Vector2 perturbedSpeed = new Vector2(newVelocityX, newVelocityY).RotatedByRandom(MathHelper.ToRadians((int)(numberProjectiles * 7.5f))); // This defines the projectiles random spread . 30 degree spread.
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, projectile.type, (int)(projectile.damage * 0.5f) + damage, childPosition, player.whoAmI);
					}
				}
				
				
				
			}
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			knockback = 0;
			if(oil)
			{
			damage += (int)(target.defense * 0.5f);
			}
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player owner = Main.player[projectile.owner];
			if(lifesteal > 0)
			{
			owner.statLife += (int)((damage - (0.5f * target.defense)) * lifesteal);
			owner.HealEffect((int)((damage - (0.5f * target.defense)) * lifesteal));
			}
			if(paralysis)
			{
				target.AddBuff(mod.BuffType("FrozenThroughTime"), 30, false);
			}
		}
		public override void AI()
		{
			
			if(projectile.damage < 1)
			{
				projectile.Kill();
			}
			int maxStack1 = 2;
			int maxStack2 = 1;
			int maxStack3 = 4;
			int homing = 0;
			lifesteal = 0;
			Player player = Main.player[projectile.owner];
				for(int i = (int)(projectile.knockBack); i < 50; i++)
				{
					Item rune = player.inventory[i];
					if(rune.type == mod.ItemType("RocketRune"))//Interferes with D
					{	
						for(int stack = rune.stack; stack > 0; stack--)
						{
							maxStack1--;
							if(maxStack1 >= 0)
							{
								homing++;
							}
						}
					}
					if(rune.type == mod.ItemType("LaserRune"))//Interferes with	D
					{	
						for(int stack = rune.stack; stack > 0; stack--)
						{
						maxStack1--;
						}
					}
					if(rune.type == mod.ItemType("CloudRune"))//Interferes with D
					{	
						for(int stack = rune.stack; stack > 0; stack--)
						{
						maxStack1 -= 2;
								if(maxStack1 >= 0)
								{
								homing += 3;
								}
						}
					}
					if(rune.type == mod.ItemType("SingleShotRune"))//Interferes with B
					{	
						for(int stack = rune.stack; stack > 0; stack--)
						{
						maxStack2--;
						}
					}
					if(rune.type == mod.ItemType("OilRune"))//Interferes with B
					{	
						maxStack2--;
						if(maxStack2 >= 0)
						{
						oil = true;
						}
					}
					if(rune.type == mod.ItemType("RailRune"))//Interferes with C
					{	
						maxStack3 -= 4;
						if(maxStack3 >= 0)
						{
							projectile.velocity.X *= 1.03f;
							projectile.velocity.Y *= 1.03f;
							if(Main.rand.Next(30) == 0)
							{
							projectile.damage++;
							}
						}
					}
					if(rune.type == mod.ItemType("ThunderRune"))//Interferes with C
					{	
						maxStack3 -= 4;
						if(maxStack3 >= 0)
						{
							paralysis = true;
						}
					}
					if(rune.type == mod.ItemType("VampiricRune"))//Interferes with C
					{	
						for(int stack = rune.stack; stack > 0; stack--)
						{
							maxStack3 -= 1;
							if(maxStack3 >= 0)
							{
								lifesteal += 0.05f;
							}
						}
					}
					if(rune.type == mod.ItemType("BoomerangRune"))//Child opportunity
					{	
						child = true;
						childPosition = i + 1;
						boomerang = true;
						i += 100;
						
					}
					if(rune.type == mod.ItemType("TurretRune"))//Child opportunity
					{	
						childPosition = i + 1;
						turret = true;
						i += 100;
						
					}
				}
			
			
			
			if(turret == true)
			{
				turretRate++;
				if(turretRate >= (1.5f * childPosition) + 30)
				{
					turretRate = 0;
					float minDist = 270;
					int target2 = -1;
					float dX = 0f;
					float dY = 0f;
					float distance = 0;
					float speed = (float)Math.Sqrt((double)(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y));
					if(projectile.friendly == true && projectile.hostile == false)
					{
						for(int i = 0; i < Main.npc.Length - 1; i++)
						{
							NPC target = Main.npc[i];
							if(!target.friendly && target.dontTakeDamage == false)
							{
								dX = target.Center.X - projectile.Center.X;
								dY = target.Center.Y - projectile.Center.Y;
								distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
								if(distance < minDist)
								{
									minDist = distance;
									target2 = i;
								}
							}
						}
							
						if(target2 != -1)
						{
						NPC toHit = Main.npc[target2];
							if(toHit.active == true)
							{
								
							dX = toHit.Center.X - projectile.Center.X;
							dY = toHit.Center.Y - projectile.Center.Y;
							distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
							speed /= distance;
							   Item rune = player.inventory[childPosition];
									if(rune.type == mod.ItemType("SingleShotRune"))
									{
										int damage = (int)(rune.stack * 3f);
										float newVelocityX = dX * speed;
										float newVelocityY = dY * speed;
										
									  Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, newVelocityX, newVelocityY, projectile.type, (int)(projectile.damage * 0.25f) + damage, childPosition, player.whoAmI);
										
									}
									if(rune.type == mod.ItemType("LaserRune"))
									{
										int damage = (int)(rune.stack * 5f);
										float newVelocityX = dX * 2;
										float newVelocityY = dY * 2; 
										
									  Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, newVelocityX, newVelocityY, projectile.type, (int)(projectile.damage * 0.25f) + damage, childPosition, player.whoAmI);
										
									}
									if(rune.type == mod.ItemType("ShotgunRune"))
									{
										int damage = (int)(rune.stack * -2f);
										int numberProjectiles = rune.stack * 2;
										float newVelocityX = dX * speed;
										float newVelocityY = dY * speed;
									
										for(int i = 0; i < numberProjectiles; i++)
										{
										Vector2 perturbedSpeed = new Vector2(newVelocityX, newVelocityY).RotatedByRandom(MathHelper.ToRadians((int)(numberProjectiles * 3f))); // This defines the projectiles random spread . 30 degree spread.
										Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, projectile.type, (int)(projectile.damage * 0.25f) + damage, childPosition, player.whoAmI);
										}
									}
									if(rune.type == mod.ItemType("FlowerRune"))
									{
										int damage = (int)(rune.stack * -1f);
										int numberProjectiles = rune.stack * 4;
										float newVelocityX = dX * speed;
										float newVelocityY = dY * speed;
									
										for(int i = 0; i < numberProjectiles; i++)
										{
										Vector2 perturbedSpeed = new Vector2(newVelocityX, newVelocityY).RotatedByRandom(MathHelper.ToRadians((int)(numberProjectiles * 7.5f))); // This defines the projectiles random spread . 30 degree spread.
										Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, projectile.type, (int)(projectile.damage * 0.25f) + damage, childPosition, player.whoAmI);
										}
									}
							}
						}
							
							
							
					}
					
				}
			}
								if(homing >= 3)
								{
										float minDist = 240;
										int target2 = -1;
										float dX = 0f;
										float dY = 0f;
										float distance = 0;
										float speed = (float)Math.Sqrt(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y);
										if(projectile.friendly == true && projectile.hostile == false)
										{
											for(int j = 0; j < Main.npc.Length - 1; j++)
											{
												NPC target = Main.npc[j];
												if(!target.friendly && target.dontTakeDamage == false)
												{
													dX = target.Center.X - projectile.Center.X;
													dY = (target.position.Y - target.height) - projectile.Center.Y;
													distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
													if(distance < minDist)
													{
														minDist = distance;
														target2 = j;
													}
												}
											}
											
											if(target2 != -1)
											{
											NPC toHit = Main.npc[target2];
												if(toHit.active == true)
												{
													
												dX = toHit.Center.X - projectile.Center.X;
												dY = (toHit.position.Y - toHit.height) - projectile.Center.Y;
												distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
												speed /= distance;
											   
												projectile.velocity = new Vector2(dX * speed, dY * speed);
												projectile.tileCollide = false;
												}
											}
										}
								}
								else if(homing >= 2)
								{
										float minDist = 180;
										int target2 = -1;
										float dX = 0f;
										float dY = 0f;
										float distance = 0;
										float speed = (float)Math.Sqrt(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y);
										if(projectile.friendly == true && projectile.hostile == false)
										{
											for(int j = 0; j < Main.npc.Length - 1; j++)
											{
												NPC target = Main.npc[j];
												if(!target.friendly && target.dontTakeDamage == false)
												{
													dX = target.Center.X - projectile.Center.X;
													dY = target.Center.Y - projectile.Center.Y;
													distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
													if(distance < minDist)
													{
														minDist = distance;
														target2 = j;
													}
												}
											}
											
											if(target2 != -1)
											{
											NPC toHit = Main.npc[target2];
												if(toHit.active == true)
												{
													
												dX = toHit.Center.X - projectile.Center.X;
												dY = toHit.Center.Y - projectile.Center.Y;
												distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
												speed /= distance;
											   
												projectile.velocity = new Vector2(dX * speed, dY * speed);
												projectile.tileCollide = false;
												}
											}
										}
								}
								else if(homing >= 1)
								{
										float minDist = 270;
										int target2 = -1;
										float dX = 0f;
										float dY = 0f;
										float distance = 0;
										float speed = 0.04f * (float)Math.Sqrt(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y);
										if(projectile.friendly == true && projectile.hostile == false)
										{
											for(int j = 0; j < Main.npc.Length - 1; j++)
											{
												NPC target = Main.npc[j];
												if(!target.friendly && target.dontTakeDamage == false)
												{
													dX = target.Center.X - projectile.Center.X;
													dY = target.Center.Y - projectile.Center.Y;
													distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
													if(distance < minDist)
													{
														minDist = distance;
														target2 = j;
													}
												}
											}
											
											if(target2 != -1)
											{
											NPC toHit = Main.npc[target2];
												if(toHit.active == true)
												{
													
												dX = toHit.Center.X - projectile.Center.X;
												dY = toHit.Center.Y - projectile.Center.Y;
												distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
												speed /= distance;
											   
												projectile.velocity += new Vector2(dX * speed, dY * speed);
												projectile.tileCollide = false;
												}
											}
										}
									}
		}
	}
}

		
			