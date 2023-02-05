using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tide
{
	public class PlasmaShrimp : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Shrimp");
			Tooltip.SetDefault("When above 40% mana, magic hits fire hot, shrimpy plasma for 50% damage\nReduces mana cost by 5%\n'Not so shrimple now'");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 50;     
            Item.height = 46;   
            Item.value = 0;
            Item.rare = ItemRarityID.Lime;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.accessory = true;
			//Item.canBePlacedInVanityRegardlessOfConditions = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			//if(!hideVisual)
			modPlayer.PlasmaShrimpVanity = true;
			modPlayer.PlasmaShrimp = true;
			player.manaCost -= 0.05f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PistolShrimp>(), 1).AddIngredient(ItemID.SorcererEmblem, 1).AddIngredient<DissolvingDeluge>(1).AddTile(TileID.TinkerersWorkbench).Register();
		}	
	}
}

