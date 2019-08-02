using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class CreationEnchant : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Creation Relic");
			Tooltip.SetDefault("Creates a light source where your cursor is located\n25% increased melee and throwing damage\n100% increased mining speed\nPermanent shine, spelunker, dangersense, sonar, builder, and hunter potion buffs");
		}
		public override void SetDefaults()
		{
      
            item.width = 30;     
            item.height = 30;   
            item.value = 50000;
            item.rare = 7;
			item.expert = true;
			item.accessory = true;
			item.defense = 6;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "TechEnchant", 1);
			recipe.AddIngredient(null, "MinerEnchantment", 1);
			recipe.AddIngredient(null, "RazorEnchant", 1);
			recipe.AddIngredient(null, "AntimaterialMandible", 5);
			recipe.AddIngredient(null, "CoreOfCreation", 2);
			recipe.AddIngredient(ItemID.BuilderPotion, 5);
			recipe.AddIngredient(ItemID.SilverBar, 5);
			recipe.AddIngredient(ItemID.TinBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			player.meleeDamage += 0.25f;
			player.thrownDamage += 0.25f;
			player.pickSpeed -= 1f;
			
					player.AddBuff(BuffID.Spelunker, 300);
					player.AddBuff(BuffID.Shine, 300);
					player.AddBuff(BuffID.Hunter, 300);
					player.AddBuff(BuffID.Builder, 300);
					player.AddBuff(BuffID.Dangersense, 300);
					player.AddBuff(BuffID.Sonar, 300);
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
						Dust.NewDust(new Vector2(vector14.X , vector14.Y), 1, 1, 269);
            
					
		}
		
	}
}
