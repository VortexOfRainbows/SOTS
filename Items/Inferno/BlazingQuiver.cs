using SOTS.Items.Fragments;
using SOTS.Items.Otherworld.FromChests;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Inferno
{
	[AutoloadEquip(EquipType.Back)]
	public class BlazingQuiver : ModItem	
	{	
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases ranged damage by 10% and greatly increases arrow speed\n20% chance not to consume arrows\nConverts wooden arrows into Blazing Arrows\nBlazing Arrows ignore gravity, pierce once, and explode for 15% damage");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 32;     
            Item.height = 42;
            Item.value = Item.sellPrice(0, 15, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.magicQuiver = true;
			player.GetDamage(DamageClass.Ranged) += 0.1f;
			SOTSPlayer.ModPlayer(player).BlazingQuiver = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.MoltenQuiver, 1).AddIngredient(ModContent.ItemType<DissolvingNether>(), 1).AddIngredient(ModContent.ItemType<FragmentOfInferno>(), 5).AddIngredient(ItemID.SoulofFright, 10).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}