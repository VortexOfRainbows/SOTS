using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class MythicalEnchant2 : ModItem
	{	int Probe = -1;
	int timer = -1;
	int mr = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mythical Relic");
			Tooltip.SetDefault("Disabled Martian Probe\nCrazy healing and and 30% reduced mana usage\nMagic healing and damaging orbs spawn as you do magic damage");
		}
		public override void SetDefaults()
		{
      
            item.width = 28;     
            item.height = 30;   
            item.value = 50000;
            item.rare = 11;
			item.expert = true;
			item.accessory = true;
			item.defense = 3;
			item.shootSpeed = 24;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BatEnchant", 1);
			recipe.AddIngredient(null, "SpaceEnchant", 1);
			recipe.AddIngredient(null, "MagicalEnchant", 1);
			recipe.AddIngredient(null, "MartianCore", 1);
			recipe.AddIngredient(null, "AntimaterialMandible", 5);
			recipe.AddIngredient(ItemID.SpectreHood,1);
			recipe.AddIngredient(ItemID.SpectreRobe,1);
			recipe.AddIngredient(ItemID.SpectrePants,1);
			recipe.AddIngredient(ItemID.SpectreMask,1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(null, "MythicalEnchant", 1);
			recipe1.AddTile(TileID.MythrilAnvil);
			recipe1.SetResult(this);
			recipe1.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			int choise = Main.rand.Next(6);
			if(choise == 0)
			{
				player.statLife += 1;
				
			int choise2 = Main.rand.Next(22);
			if(choise2 == 0)
			{
				player.statLife += 1;
				
			}
			}
		
		
		timer += 1; //Magical----------------------------Space
			player.manaCost -= 100;
			Vector2 vector14;
			
		
			player.ghostHurt = true;
			player.ghostHeal = true;
	}
		
		
		
		
		
		}
		
	}

