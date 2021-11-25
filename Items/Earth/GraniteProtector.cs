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
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
            item.width = 30;     
            item.height = 30;   
            item.value = Item.sellPrice(0, 0, 20, 0);
            item.rare = ItemRarityID.Blue;
			item.defense = 1;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.endurance += 0.06f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.GraniteBlock, 50); //smooth granite
			recipe.AddIngredient(ModContent.ItemType<FragmentOfEarth>(), 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}