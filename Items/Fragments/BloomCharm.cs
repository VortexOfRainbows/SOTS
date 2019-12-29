using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;


namespace SOTS.Items.Fragments
{
	public class BloomCharm : ModItem
	{	float boost = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloom Charm");
			Tooltip.SetDefault("Powers up the dirt rod");
		}
		public override void SetDefaults()
		{
			item.damage = 23;
			item.summon = true;
			item.maxStack = 1;
            item.width = 28;     
            item.height = 28;   
            item.value = Item.sellPrice(0, 0, 75, 0);
            item.rare = 3;
			item.accessory = true;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "FragmentOfNature", 6);
			recipe.AddIngredient(ItemID.JungleSpores, 6);
			recipe.AddIngredient(ItemID.Vine, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		int counter = 0;
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			counter++;
			for(int j = 0; j < 1000; j++)
			{
				Projectile projectile = Main.projectile[j];
				if(projectile.type == 17 && player == Main.player[projectile.owner] && projectile.active)
				{
					for(int i = 0; i < 5; i++)
					{
					int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 2);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 1.25f;
					}
					float minDist = 175;
					int target2 = -1;
					float dX = 0f;
					float dY = 0f;
					float distance = 0;
					float speed = 12f;
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
					if(target2 != -1 && counter >= 35)
					{
						NPC toHit = Main.npc[target2];
						if(toHit.active)
						{
							
						dX = toHit.Center.X - projectile.Center.X;
						dY = toHit.Center.Y - projectile.Center.Y;
						distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
						speed /= distance;
					   
						Vector2 fireTo = new Vector2(dX * speed, dY * speed);
						int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, fireTo.X, fireTo.Y, 206, (int)(item.damage * (1 + (player.minionDamage - 1f) + (player.allDamage - 1f))), 4.5f, projectile.owner, 0, 0);
						Main.projectile[proj].minion = true;
						Main.projectile[proj].magic = false;
						counter = 0;
						}
					}
				}
			}
		}
	}
}
