using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;

namespace SOTS.Items.Celestial
{
	public class GalaxyBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Galaxy Blade");
			Tooltip.SetDefault("Slices through space and time\nCan hit up to 2 enemies at a time");
		}
		public override void SetDefaults()
		{

			item.damage = 66;
			item.melee = true;
			item.width = 42;
			item.height = 42;
			item.useTime = 18;
			item.useAnimation = 18;
			item.useStyle = 1;
			item.knockBack = 4.5f;
			item.value = Item.sellPrice(0, 9, 0, 0);
			item.rare = 8;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("SpiritBlade"); 
            item.shootSpeed = 22.5f;
			item.noMelee = false;
		}
		public void RegisterPhantoms(Player player)
		{
			int npcIndex = -1;
			int npcIndex1 = -1;
			for(int j = 0; j < 2; j++)
			{
				double distanceTB = 720;
				for(int i = 0; i < 200; i++) //find first enemy
				{
					NPC npc = Main.npc[i];
					if(!npc.friendly && npc.lifeMax > 5 && npc.active)
					{
						if(npcIndex != i && npcIndex1 != i)
						{
							float disX = player.Center.X - npc.Center.X;
							float disY = player.Center.Y - npc.Center.Y;
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
						}
					}
				}
			}
			for(int projNum = 0; projNum < 1; projNum++)
			{
				if(npcIndex != -1)
				{
					NPC npc = Main.npc[npcIndex];
					int randRot = Main.rand.Next(360);
					Vector2 rotatePos = new Vector2(-npc.width - 124, 0).RotatedBy(MathHelper.ToRadians(randRot));
					Vector2 attackPos = new Vector2(item.shootSpeed, 0).RotatedBy(MathHelper.ToRadians(randRot));
					
					float spawnPosX = npc.Center.X + rotatePos.X; //size of projectile stats
					float spawnPosY = npc.Center.Y + rotatePos.Y;
					
					if(!npc.friendly && npc.lifeMax > 5 && npc.active)
					{
						int newIndex = Projectile.NewProjectile(spawnPosX, spawnPosY, attackPos.X, attackPos.Y, item.shoot, item.damage, item.knockBack, player.whoAmI);
					}
				}
				if(npcIndex1 != -1)
				{
					NPC npc = Main.npc[npcIndex1];
					int randRot = Main.rand.Next(360);
					Vector2 rotatePos = new Vector2(-npc.width - 124, 0).RotatedBy(MathHelper.ToRadians(randRot));
					Vector2 attackPos = new Vector2(item.shootSpeed, 0).RotatedBy(MathHelper.ToRadians(randRot));
					
					float spawnPosX = npc.Center.X + rotatePos.X;
					float spawnPosY = npc.Center.Y + rotatePos.Y;
					
					if(!npc.friendly && npc.lifeMax > 5 && npc.active)
					{
						int newIndex1 = Projectile.NewProjectile(spawnPosX, spawnPosY, attackPos.X, attackPos.Y, item.shoot, item.damage, item.knockBack, player.whoAmI);
					}
				}
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			RegisterPhantoms(player);
			return false; 
		}
		public override void AddRecipes()	
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "StarShard", 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}