using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Pyramid
{
	public class SpiritGlove : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Glove");
			Tooltip.SetDefault("Increases void regeneration speed by 5% and melee crit chance by 8%");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 26;     
            Item.height = 30;   
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegenSpeed += 0.05f;
			player.meleeCrit += 8;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<SoulResidue>(), 32);
			recipe.AddIngredient(ItemID.Emerald, 1);
			recipe.AddRecipeGroup("SOTS:GoldBar", 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}