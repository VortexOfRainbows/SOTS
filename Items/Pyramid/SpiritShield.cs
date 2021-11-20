using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Pyramid
{	[AutoloadEquip(EquipType.Shield)]
	public class SpiritShield : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Shield");
			Tooltip.SetDefault("Increases void regen by 2, life regen by 1, and reduces damage taken by 2%");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
            item.width = 26;     
            item.height = 40;   
            item.value = Item.sellPrice(0, 3, 50, 0);
            item.rare = ItemRarityID.LightPurple;
			item.defense = 2;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegen += 0.2f;
			player.lifeRegen += 1;
			player.endurance += 0.02f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<SoulResidue>(), 25);
			recipe.AddIngredient(ModContent.ItemType<EmeraldBracelet>(), 1);
			recipe.AddIngredient(ItemID.BandofRegeneration, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}