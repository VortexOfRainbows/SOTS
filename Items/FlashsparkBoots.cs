using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class FlashsparkBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flashspark Boots");
			Tooltip.SetDefault("Allows flight, super fast running, and extra mobility on ice\nIncreases movement speed greatly\nProvides the ability to walk on water and lava\nGrants immunity to fire blocks and 10 seconds of immunity to lava");
		}
		public override void SetDefaults()
		{
            item.width = 42;     
            item.height = 36;   
            item.value = Item.sellPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Yellow;
			item.accessory = true;
			item.expert = false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
		    recipe.AddIngredient(ItemID.FrostsparkBoots, 1);
			recipe.AddIngredient(ItemID.LavaWaders, 1);
			recipe.AddIngredient(null, "AbsoluteBar", 12);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.buffImmune[BuffID.Burning] = true;
			player.waterWalk = true; 
			player.fireWalk = true; 
			player.lavaMax += 600; 
			player.rocketBoots = 2; 
			player.iceSkate = true;
			player.moveSpeed += 0.2f;
			player.accRunSpeed = 8f;
		}
	}
}
