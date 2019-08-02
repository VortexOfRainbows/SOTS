using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class ExplosiveEnchant : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosive Relic");
			Tooltip.SetDefault("Fatal hits will now return your health to your current mana\nEach time this happens, your max mana will be cut down depending on how much damage you take\nIf you take more damage than your max mana can handle, the fatal hit will not be cancelled\nYour max mana will return to normal after death or 5 minutes\nWhen mana is below 10, paralysis is induced\nWhen mana is below 190, enemies will drop heal hearts and mana stars, an extra projectile will also be shot\n25% increased magic speed, 50% increased mana cost\n10% decrease to all damage");
		}
		public override void SetDefaults()
		{
      
            item.width = 34;     
            item.height = 38;   
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
			recipe.AddIngredient(null,"MeguminHat", 1);
			recipe.AddIngredient(null,"MeguminShirt", 1);
			recipe.AddIngredient(null,"MeguminLeggings", 1);
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
				player.manaCost += 0.5f;
			for(player.statMana = player.statMana; player.statMana < 0; player.statMana++)
			{
				
			}
                
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
                modPlayer.megSet = true;
			if(player.statMana < 180)
			{
                modPlayer.megShirt = true;
                modPlayer.megHat = true;
			}
			else
			{
                modPlayer.megShirt = false;
                modPlayer.megHat = false;
			}
				
			if(player.statMana < 10)
			{
				player.AddBuff(mod.BuffType("FrozenThroughTime"), 30, false);
				
			}
					
		}
		
	}
}
