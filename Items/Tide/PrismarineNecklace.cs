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
<<<<<<< Updated upstream
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(3, 20));
		}
		public override void SetDefaults()
		{
            item.width = 26;     
            item.height = 44;   
            item.value = Item.sellPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.accessory = true;
=======
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 20));
		}
		public override void SetDefaults()
		{
            Item.width = 26;     
            Item.height = 44;   
            Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
>>>>>>> Stashed changes
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			modPlayer.rippleBonusDamage += 4;
			if(!hideVisual)
				modPlayer.rippleEffect = true;
			player.statLifeMax2 += 20;
			modPlayer.rippleTimer++;
			player.armorPenetration += 8;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<HeartOfTheSea>(), 1);
			recipe.AddIngredient(ItemID.SharkToothNecklace, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}