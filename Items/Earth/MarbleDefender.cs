using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth
{	[AutoloadEquip(EquipType.Shield)]
	public class MarbleDefender : ModItem	
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Marble Defender");
			Tooltip.SetDefault("Launches attackers away from you with javelins");
		}
		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.maxStack = 1;
            Item.width = 28;     
            Item.height = 36;   
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Blue;
			Item.defense = 1;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer.ModPlayer(player).PushBack = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ItemID.MarbleBlock, 50);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfEarth>(), 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}