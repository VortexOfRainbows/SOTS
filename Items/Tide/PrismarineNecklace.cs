using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tide
{	
	public class PrismarineNecklace : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismarine Necklace");
			Tooltip.SetDefault("Increases armor penetration by 8 and max life by 20\nRelease waves of damage periodically\nRelease more waves at lower health\nWaves ignore up to 16 defense total\nWaves disabled when hidden");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 20));
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 44;
			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.rippleBonusDamage += 4;
			if(!hideVisual)
				modPlayer.rippleEffect = true;
			player.statLifeMax2 += 20;
			modPlayer.rippleTimer++;
			player.GetArmorPenetration(DamageClass.Generic) += 8;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<HeartOfTheSea>(), 1).AddIngredient(ItemID.SharkToothNecklace, 1).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}