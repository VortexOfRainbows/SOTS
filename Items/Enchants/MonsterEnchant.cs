using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class MonsterEnchant : ModItem
	{	int timer = 1;
		int up = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Monster Relic");
			Tooltip.SetDefault("Press down to phase through walls\nIncreased speed\n25% increased melee speed\n10% decrease to all damage");
		}
		public override void SetDefaults()
		{
      
            item.width = 46;     
            item.height = 34;   
            item.value = 50000;
            item.rare = 6;
			item.accessory = true;
			item.defense = 0;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"AntimaterialMandible", 5);
			recipe.AddIngredient(null,"DevilHelmet", 1);
			recipe.AddIngredient(null,"DevilChestplate", 1);
			recipe.AddIngredient(null,"DevilLeggings", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
				player.rangedDamage -= 0.1f;
				player.meleeDamage -= 0.1f;
				player.magicDamage -= 0.1f;
				player.minionDamage -= 0.1f;
				player.thrownDamage -= 0.1f;
				SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
                modPlayer.devilHat = true;
				if(player.controlLeft) 
				  {
					  if(player.velocity.X > 0)
					  {
						  player.velocity.X = -1;
						  }
				  player.velocity.X -= 0.24f;
				  
				  if(player.controlDown)
				  player.position.X -= 1.5f;
			  
				  }
				  if(player.controlRight)
				  {
					  if(player.velocity.X < 0)
					  {
						  player.velocity.X = 1;
						  }
				  player.velocity.X += 0.24f;
				  
				  if(player.controlDown)
				  player.position.X += 1.5f;
			  
				  }
				  if(player.controlJump && up <= 9 && up >= 1 && player.controlDown) 
				  {
					  up = 0;
					  player.velocity.Y -= 1.2f;
					  player.position.Y -= 16;
			
				  }
				  if(up > 0)
				  {
				  up--;
				  }
				if(player.controlJump) 
				  {
					  up = 10;
				  }
					
		}
		
	}
}
