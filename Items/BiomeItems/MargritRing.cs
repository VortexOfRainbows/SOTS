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


namespace SOTS.Items.BiomeItems
{
	public class MargritRing : ModItem
	{ 	int firerate = 0;
		bool overheated = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Margrit Ring");
			Tooltip.SetDefault("Fires the weapon from your last inventory slot in random directions");
		}
		public override void SetDefaults()
		{
            
            item.width = 26;     
            item.height = 26;     
            item.value = 110000;
            item.rare = 6;
			item.accessory = true;
			
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			firerate++;
			Item item1 = player.inventory[49];
			int projectileType = item1.shoot;
			int damage = item1.damage;
			int reload = item1.useTime;
			int useAmmo = item1.useAmmo;
			float knockBack = item1.knockBack;
			
			if(useAmmo != 0)
			{
				projectileType = mod.ProjectileType("FireProj");
				for(int i = 0; i < 58; i++)
				{
			Item item2 = player.inventory[i];
				if(useAmmo == item2.ammo)
					{
					int projectileAmmo = item2.shoot;
					projectileType = projectileAmmo;
					damage += item2.damage;
					break;
					}
				}
				
			}
			
			if(item1.summon == false && item1.type != mod.ItemType("Obliterator") && item1.type != mod.ItemType("TrinityScatter") && item1.type != mod.ItemType("TrinityCrossbow") && item1.type != mod.ItemType("HallowedCrossbow") && item1.type != mod.ItemType("HallowedScatter") && item1.type != mod.ItemType("WormWoodScatter") && item1.type != mod.ItemType("WormWoodCrossbow") &&  item1.type != mod.ItemType("MargritBlaster") && item1.type != 71  && item1.type != 72 && item1.type != 73 && item1.type != 74)
			{	
				if(firerate >= reload && item1.channel == false)
				{
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-8,9), Main.rand.Next(-8,9), projectileType, damage, knockBack, player.whoAmI);
			
				firerate = 0;
				}
			}
			
			
			
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "MargritCore", 1);
			recipe.AddIngredient(null, "ObsidianScale", 32);
			recipe.AddIngredient(3081, 24);
			recipe.AddIngredient(3086, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
