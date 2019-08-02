using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class MinerEnchantment : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Miner Relic");
			Tooltip.SetDefault("Creates a light source where your cursor is located\n7% increased melee and throwing damage\n50% increased mining speed\nPermanent shine, spelunker, and hunter potion buffs");
		}
		public override void SetDefaults()
		{
      
            item.width = 30;     
            item.height = 30;   
            item.value = 50000;
            item.rare = 7;
			item.expert = true;
			item.accessory = true;
			item.defense = 3;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Mushgun", 1);
			recipe.AddIngredient(null, "AntimaterialMandible", 5);
			recipe.AddIngredient(null, "SteelBar", 8);
			recipe.AddIngredient(ItemID.MiningHelmet,1);
			recipe.AddIngredient(ItemID.Sickle,1);
			recipe.AddIngredient(ItemID.BoneGlove,1);
			recipe.AddIngredient(ItemID.ShinePotion,10);
			recipe.AddIngredient(ItemID.BugNet,1);
			recipe.AddIngredient(ItemID.MoltenPickaxe,1);
			recipe.AddIngredient(ItemID.ReaverShark,1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			player.meleeDamage += 0.07f;
			player.thrownDamage += 0.07f;
			player.pickSpeed -= 0.50f;
			
					player.AddBuff(BuffID.Spelunker, 300);
					player.AddBuff(BuffID.Shine, 300);
					player.AddBuff(BuffID.Hunter, 300);
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
