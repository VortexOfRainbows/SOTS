using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using System;

namespace SOTS.Items.Celestial
{
	public class BloodstainedJewel : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodstained Jewel");
			Tooltip.SetDefault("Chance to heal for 100 health after taking damage\nThe chance increases after taking consecutive hits of damage\nWorks while in the inventory\nDecreases void regen by 2.5 and max void by 50");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.maxStack = 1;
			item.consumable = false; 
            item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 8;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SanguiteBar", 15);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}
		float healChance = 1.5f;
		public override void UpdateInventory(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			if(!modPlayer.bloodstainedJewel)
			{
				voidPlayer.voidRegen -= 0.25f;
				voidPlayer.voidMeterMax2 -= 50;
				modPlayer.bloodstainedJewel = true;
				if(modPlayer.onhit == 1)
				{
					if(healChance >= 25f)
					{
						healChance = 25f;
					}
					if(Main.rand.Next(1000000) / 1000000f <= (double)(healChance / 100f))
					{
						healChance = 0.5f;
						player.statLife += 100;
						player.HealEffect(100);
					}
					else
					{
						healChance += 0.75f;
						healChance += 0.025f * modPlayer.onhitdamage;
						if(modPlayer.onhitdamage > 75)
						{
						healChance += 3.55f;
						}
					}
				}
			}
		}
	}
}