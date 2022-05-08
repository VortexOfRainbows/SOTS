using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Items.Earth;
using SOTS.Items.Pyramid;
using SOTS.Items.ChestItems;

namespace SOTS.Items
{	[AutoloadEquip(EquipType.Shield)]
	public class OlympianAegis : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Olympian Aegis");
			Tooltip.SetDefault("Increases void gain by 2 and life regen by 1\nReduces damage taken by 7% and increases crit chance by 4%");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 34;     
            Item.height = 42;   
            Item.value = Item.sellPrice(0, 4, 75, 0);
            Item.rare = ItemRarityID.LightRed;
			Item.defense = 3;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.bonusVoidGain += 2;
			player.lifeRegen += 1;
			player.endurance += 0.07f;
			player.meleeCrit += 4;
			player.rangedCrit += 4;
			player.magicCrit += 4;
			player.thrownCrit += 4;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<GraniteProtector>(), 1);
			recipe.AddIngredient(ModContent.ItemType<SpiritShield>(), 1);
			recipe.AddIngredient(ModContent.ItemType<CrestofDasuver>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}