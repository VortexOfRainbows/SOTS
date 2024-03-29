using SOTS.Items.ChestItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class CrestofDasuver : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 36;     
            Item.height = 38;     
            Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
			Item.defense = 3;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetCritChance(DamageClass.Generic) += 6;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<ShieldofDesecar>(1).AddIngredient<ShieldofStekpla>(1).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}
