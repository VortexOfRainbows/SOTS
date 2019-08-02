using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class Recycler : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Recycler");
			Tooltip.SetDefault("Increases damage when damage is done\nCaps at 60% damage up\nDamage returns to normal overtime");
		}
		public override void SetDefaults()
		{
      
            item.width = 34;     
            item.height = 36; 
            item.rare = 5;
			item.value = 750000;
			item.accessory = true;
			item.defense = 6;
			item.expert = true;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Gemblem", 1);
			recipe.AddIngredient(null, "CoreOfExpertise", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			modPlayer.recycleOn = true;
			player.meleeDamage += modPlayer.recycleDamageBoost;
			player.rangedDamage += modPlayer.recycleDamageBoost;
			player.magicDamage +=  modPlayer.recycleDamageBoost;
			player.thrownDamage +=  modPlayer.recycleDamageBoost;
			if(modPlayer.recycleDamageBoost > 0.6f)
			{
				 modPlayer.recycleDamageBoost = 0.6f;
			}
			
		}
	}
}
