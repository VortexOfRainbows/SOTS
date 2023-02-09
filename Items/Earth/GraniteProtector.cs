using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth
{	[AutoloadEquip(EquipType.Shield)]
	public class GraniteProtector : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Protector");
			Tooltip.SetDefault("Reduces damage taken by 6%");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 30;     
            Item.height = 30;   
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Blue;
			Item.defense = 1;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.endurance += 0.06f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Granite, 50).AddIngredient(ModContent.ItemType<FragmentOfEarth>(), 4).AddTile(TileID.Anvils).Register();
		}
	}
}