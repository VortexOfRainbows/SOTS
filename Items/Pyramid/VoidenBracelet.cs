using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Pyramid
{
	public class VoidenBracelet : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Voiden Bracelet");
			Tooltip.SetDefault("Increases void damage and magic damage by 8%\nReduces void cost by 8%");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 26;     
            Item.height = 24;   
            Item.value = Item.sellPrice(0, 2, 25, 0);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidDamage += 0.08f;
			voidPlayer.voidCost -= 0.08f;
			player.magicDamage += 0.08f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CursedMatter>(), 4);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}