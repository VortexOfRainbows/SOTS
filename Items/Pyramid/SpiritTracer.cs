using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;

namespace SOTS.Items.Pyramid
{
	public class SpiritTracer : VoidItem
	{
		int currentIndex = -1;
		bool inInventory = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Tracer");
			Tooltip.SetDefault("Fires phantom arrows\nCan hit up to 3 enemies at a time");
		}
		public override void SafeSetDefaults()
		{

			item.damage = 33;
			item.ranged = true;
			item.width = 30;
			item.height = 74;
			item.useTime = 18;
			item.useAnimation = 18;
			item.useStyle = 5;
			item.knockBack = 1.5f;
			item.value = Item.sellPrice(0, 7, 25, 0);
			item.rare = 5;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;            
			item.shoot = 1; 
            item.shootSpeed = 21.5f;
			item.useAmmo = AmmoID.Arrow;
			item.noMelee = true;
			item.expert = true;
		}
		public override void GetVoid(Player player)
		{
				voidMana = 6;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(6, 0);
		}
        public override bool BeforeUseItem(Player player)
		{
			if(inInventory)
				return true;
			return false;
		}
		public void RegisterPhantoms(Player player)
		{
			if(currentIndex != -1)
			{
				Projectile proj = Main.projectile[currentIndex];
				proj.tileCollide = false;
				proj.alpha += 20;
				proj.friendly = false;
				if(proj.alpha >= 200)
				{
					proj.active = false;
					currentIndex = -1;
					
					int npcIndex = -1;
					int npcIndex1 = -1;
					int npcIndex2 = -1;
					for(int j = 0; j < 3; j++)
					{
						double distanceTB = 600;
						for(int i = 0; i < 200; i++) //find first enemy
						{
							NPC npc = Main.npc[i];
							if(!npc.friendly && npc.lifeMax > 5 && npc.active)
							{
								if(npcIndex != i && npcIndex1 != i && npcIndex2 != i)
								{
									float disX = proj.Center.X - npc.Center.X;
									float disY = proj.Center.Y - npc.Center.Y;
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
								}
							}
						}
					}
					if(npcIndex != -1)
					{
						NPC npc = Main.npc[npcIndex];
						int randRot = Main.rand.Next(360);
						Vector2 rotatePos = new Vector2(-npc.width - 124, 0).RotatedBy(MathHelper.ToRadians(randRot));
						Vector2 attackPos = new Vector2(18, 0).RotatedBy(MathHelper.ToRadians(randRot));
						
						float spawnPosX = npc.Center.X + rotatePos.X - proj.width/2;
						float spawnPosY = npc.Center.Y + rotatePos.Y - proj.height/2;
						
						if(!npc.friendly && npc.lifeMax > 5 && npc.active)
						{
							int newIndex = Projectile.NewProjectile(spawnPosX, spawnPosY, attackPos.X, attackPos.Y, proj.type, proj.damage, proj.knockBack, player.whoAmI);
							Projectile newProj = Main.projectile[newIndex];
							newProj.alpha = 125;
							newProj.tileCollide = false;
							newProj.timeLeft = 125;
						}
					}
					if(npcIndex1 != -1)
					{
						NPC npc = Main.npc[npcIndex1];
						int randRot = Main.rand.Next(360);
						Vector2 rotatePos = new Vector2(-npc.width - 124, 0).RotatedBy(MathHelper.ToRadians(randRot));
						Vector2 attackPos = new Vector2(18, 0).RotatedBy(MathHelper.ToRadians(randRot));
						
						float spawnPosX = npc.Center.X + rotatePos.X - proj.width/2;
						float spawnPosY = npc.Center.Y + rotatePos.Y - proj.height/2;
						
						if(!npc.friendly && npc.lifeMax > 5 && npc.active)
						{
							int newIndex1 = Projectile.NewProjectile(spawnPosX, spawnPosY, attackPos.X, attackPos.Y, proj.type, proj.damage, proj.knockBack, player.whoAmI);
							Projectile newProj = Main.projectile[newIndex1];
							newProj.alpha = 125;
							newProj.tileCollide = false;
							newProj.timeLeft = 125;
						}
					}
					if(npcIndex2 != -1)
					{
						NPC npc = Main.npc[npcIndex2];
						int randRot = Main.rand.Next(360);
						Vector2 rotatePos = new Vector2(-npc.width - 124, 0).RotatedBy(MathHelper.ToRadians(randRot));
						Vector2 attackPos = new Vector2(18, 0).RotatedBy(MathHelper.ToRadians(randRot));
						
						float spawnPosX = npc.Center.X + rotatePos.X - proj.width/2;
						float spawnPosY = npc.Center.Y + rotatePos.Y - proj.height/2;
						
						if(!npc.friendly && npc.lifeMax > 5 && npc.active)
						{
							int newIndex2 = Projectile.NewProjectile(spawnPosX, spawnPosY, attackPos.X, attackPos.Y, proj.type, proj.damage, proj.knockBack, player.whoAmI);
							Projectile newProj = Main.projectile[newIndex2];
							newProj.alpha = 125;
							newProj.tileCollide = false;
							newProj.timeLeft = 125;
						}
					}
				}
			}
		}
		public override void UpdateInventory(Player player)
		{
			inInventory = true;
			RegisterPhantoms(player);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			inInventory = false;
			int numberProjectiles = 1;
			for (int i = 0; i < numberProjectiles; i++)
			{
				if(currentIndex != -1)
				{
					Main.projectile[currentIndex].alpha = 255;
					RegisterPhantoms(player);
				}
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(7));
				currentIndex = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				Main.projectile[currentIndex].tileCollide = false;
			}
			return false; 
		}
	}
}