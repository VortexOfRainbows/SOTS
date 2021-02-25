using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.ChestItems
{	[AutoloadEquip(EquipType.Shield)]
	public class ShieldofStekpla : ModItem
	{ 	
		int critbonus = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shield of Stekpla");
			Tooltip.SetDefault("'More is More'\nGrants 1% bonus crit chance for every 4 full inventory slots");
		}
		public override void SetDefaults()
		{
            item.width = 28;     
            item.height = 36;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			critbonus = 0;
			for(int i = 0; i < 50; i++)
			{
				Item inventoryItem = player.inventory[i];
				if(inventoryItem.type != 0)
				{
					critbonus++;
				}
			}
			player.meleeCrit += (int)(critbonus * 0.25f);
			player.rangedCrit += (int)(critbonus * 0.25f);
			player.magicCrit += (int)(critbonus * 0.25f);
			player.thrownCrit += (int)(critbonus * 0.25f);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DissolvingAurora", 1);
			recipe.AddIngredient(ItemID.PlatinumBar, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
