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
            item.width = 30;     
            item.height = 36;   
            item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
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
			modPlayer.typhonRange = 120;
			if(!hideVisual)
				modPlayer.petAdvisor = true;
		}
	}
}