using Microsoft.Xna.Framework;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Planetarium.FromChests
{	
	[AutoloadEquip(EquipType.Waist)]
	public class Calculator : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 30;     
            Item.height = 36;   
            Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
			Item.hasVanityEffects = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<PrecariousCluster>(1).AddIngredient<HardlightAlloy>(10).AddTile(ModContent.TileType<Furniture.HardlightFabricatorTile>()).Register();
		}
        public override void EquipFrameEffects(Player player, EquipType type)
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