using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{	
	public class Sandwich : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandwich");
			Tooltip.SetDefault("Increases healing recieved from potions by 40\nKilling enemies will drop baguette crumbs\nSummons a pet Putrid Pinky to assist in combat\nLatches onto enemies, slowing them down and draining life\nIncreases life regeneration by 1");
		}
		public override void SetDefaults()
		{
			item.damage = 20;
			item.summon = true;
            item.width = 40;     
            item.height = 34;   
            item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.lifeRegen += 1;
			modPlayer.petPinky += (int)(item.damage * (1f + (player.minionDamage - 1f) + (player.allDamage - 1f)));
			modPlayer.baguetteDrops = true;
			modPlayer.additionalHeal += 40;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Baguette", 1);
			recipe.AddIngredient(null, "RoyalJelly", 1);
			recipe.AddIngredient(null, "PeanutButter", 1);
			recipe.AddTile(TileID.CookingPots);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}