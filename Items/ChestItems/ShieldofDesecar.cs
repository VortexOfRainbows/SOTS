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
			DisplayName.SetDefault("Shield of Desecar");
			Tooltip.SetDefault("'Less is More'\nGrants 1 defense for every 4 empty inventory slots");
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
				if(inventoryItem.type == 0)
				{
					shield += 0.25f;
				}
			}
			Item.defense += (int)shield;
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(null, "DissolvingEarth", 1);
			recipe.AddIngredient(ItemID.GoldBar, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
