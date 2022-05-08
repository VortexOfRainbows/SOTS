using SOTS.Items.Fragments;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class ArcaneAqueduct : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Aqueduct");
			Tooltip.SetDefault("Surrounds you with 2 orbital projectiles");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 5));
		}
		public override void SetDefaults()
		{
			Item.damage = 14;
			Item.magic = true;
            Item.width = 28;     
            Item.height = 44;   
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WaterBolt, 1);
			recipe.AddIngredient(ItemID.AquaScepter, 1);
			recipe.AddIngredient(ItemID.MarbleBlock, 25);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfTide>(), 4);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.aqueductDamage += (int)(Item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f)));
			modPlayer.aqueductNum += 2;
		}
	}
}