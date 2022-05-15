using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Projectiles.Ores;

namespace SOTS.Items.OreItems
{
	public class PlatinumSoulStaff : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platinum Soul Staff");
			Tooltip.SetDefault("Absorb life from up to 3 enemies at a time");
		}
		public override void SafeSetDefaults()
		{

			Item.damage = 8;
			Item.DamageType = DamageClass.Magic;
			Item.width = 28;
			Item.height = 32;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<SoulLock>(); 
            Item.shootSpeed = 5.5f; //arbitrary
			Item.noMelee = true;
			Item.staff[Item.type] = true; //this makes the useStyle animate as a staff
			Item.crit = 6;
		}
		public void RegisterPhantoms(Player player)
		{
			int npcIndex = -1;
			int npcIndex1 = -1;
			int npcIndex2 = -1;
			for(int j = 0; j < 3; j++)
			{
				double distanceTB = 180;
				Vector2 cursorArea = Main.MouseWorld;
				for(int i = 0; i < 200; i++) //find first enemy
				{
					NPC npc = Main.npc[i];
					if(!npc.friendly && npc.lifeMax > 5 && npc.active)
					{
						if(npcIndex != i && npcIndex1 != i && npcIndex2 != i)
						{
							float disX = cursorArea.X - npc.Center.X;
							float disY = cursorArea.Y - npc.Center.Y;
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
			for(int projNum = 0; projNum < 1; projNum++)
			{
				if(npcIndex != -1)
				{
					NPC npc = Main.npc[npcIndex];
					
					if(!npc.friendly && npc.lifeMax > 5 && npc.active)
					{
						int newIndex = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, Item.shoot, Item.damage, Item.knockBack, player.whoAmI, npc.whoAmI);
					}
				}
				if(npcIndex1 != -1)
				{
					NPC npc = Main.npc[npcIndex1];
					
					if(!npc.friendly && npc.lifeMax > 5 && npc.active)
					{
						int newIndex1 = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, Item.shoot, Item.damage, Item.knockBack, player.whoAmI, npc.whoAmI);
					}
				}
				if(npcIndex2!= -1)
				{
					NPC npc = Main.npc[npcIndex2];
					
					if(!npc.friendly && npc.lifeMax > 5 && npc.active)
					{
						int newIndex2 = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, Item.shoot, Item.damage, Item.knockBack, player.whoAmI, npc.whoAmI);
					}
				}
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			RegisterPhantoms(player);
			return false; 
		}
		public override int GetVoid(Player player)
		{
			return 4;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.PlatinumBar, 15).AddTile(TileID.Anvils).Register();
		}
	}
}