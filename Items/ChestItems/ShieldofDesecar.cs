using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.ChestItems
{	[AutoloadEquip(EquipType.Shield)]
	public class ShieldofDesecar : ModItem
	{
		float shield = 0;
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 28;     
            Item.height = 36;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			Item.defense = 0;
			shield = 0;
			for(int i = 0; i < 50; i++)
			{
				Item inventoryItem = player.inventory[i];
				if(inventoryItem.type == ItemID.None)
				{
					shield += 0.25f;
				}
			}
			Item.defense += (int)shield;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<DissolvingEarth>(1).AddIngredient(ItemID.GoldBar, 20).AddTile(TileID.Anvils).Register();
		}
	}
}
