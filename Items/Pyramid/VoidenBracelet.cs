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
			this.SetResearchCost(1);
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
			player.GetDamage<VoidGeneric>() += 0.08f;
			voidPlayer.voidCost -= 0.08f;
			player.GetDamage(DamageClass.Magic) += 0.08f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<CursedMatter>(), 4).AddIngredient(ItemID.Ruby, 1).AddTile(TileID.Anvils).Register();
		}
	}
}