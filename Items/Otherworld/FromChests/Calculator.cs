using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{	
	[AutoloadEquip(EquipType.Waist)]
	public class Calculator : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Calculator");
			Tooltip.SetDefault("Summons a pet Advisor to assist in combat\nInstead of attacking enemies directly, the Advisor assists with your aim\n'He really wants to help!'");
		}
		public override void SetDefaults()
		{
            Item.width = 30;     
            Item.height = 36;   
            Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(null, "PrecariousCluster", 1);
			recipe.AddIngredient(null, "HardlightAlloy", 10);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
        public override void UpdateVanity(Player player, EquipType type)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.petAdvisor = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.typhonRange = 96;
			if(!hideVisual)
				modPlayer.petAdvisor = true;
		}
	}
}