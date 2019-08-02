using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class ShardEnchant : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shard Relic");
			Tooltip.SetDefault("10% increased mining speed\nGetting hit will deal some damage back to the attacker\nDecreases all damage done by 1%");
		}
		public override void SetDefaults()
		{
      
            item.width = 30;     
            item.height = 30;   
            item.value = 50000;
            item.rare = 7;
			item.accessory = true;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AntimaterialMandible", 5);
			recipe.AddIngredient(ItemID.GoldBar, 8);
			recipe.AddIngredient(ItemID.IronOre, 28);
			recipe.AddIngredient(ItemID.Minishark, 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			player.minionDamage -= 0.01f;
			player.magicDamage -= 0.01f;
			player.meleeDamage -= 0.01f;
			player.rangedDamage -= 0.01f;
			player.thrownDamage -= 0.01f;
			player.pickSpeed -= 0.1f;
			player.thorns = 0.75f;
			Vector2 vector14;
						if (player.gravDir == 1f)
					{
					vector14.Y = (float)Main.mouseY + Main.screenPosition.Y;
					}
					else
					{
					vector14.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
						vector14.X = (float)Main.mouseX + Main.screenPosition.X;
						//Dust.NewDust(new Vector2(vector14.X , vector14.Y), 1, 1, 269);
            
					
		}
		
	}
}
