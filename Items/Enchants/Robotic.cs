using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class Robotic : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Relic XV : Robotic");
			Tooltip.SetDefault("S");
		}
		public override void SetDefaults()
		{
      
            item.width = 24;     
            item.height = 32;   
            item.value = 1000000000;
            item.rare = 3;
			item.expert = true;
			item.accessory = true;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"SMaterial", 1);
			recipe.AddIngredient(null,"BladeMaterial", 1);
			recipe.AddIngredient(null,"PurgeBringer", 1);
			recipe.AddIngredient(null,"Gemblem", 1);
			recipe.AddIngredient(null,"AntimaterialMandible", 15);
			recipe.AddIngredient(null,"TheHardCore", 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.thrownDamage += 0.2f;
			player.thrownCrit += 15;
			timer++;
			if(timer == 360)
			{
				timer = 0;
				
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("SaturnBlade"), 1, 0, 0);
			}
			
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if(modPlayer.needle == true)
			{
					player.AddBuff(mod.BuffType("Needle"), 300);
			}
				  
		}
		
	}
}
