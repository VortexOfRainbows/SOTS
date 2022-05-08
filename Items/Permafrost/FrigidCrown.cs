using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	[AutoloadEquip(EquipType.Head)]
	
	public class FrigidCrown : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 20;
            Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 3;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Crown");
			Tooltip.SetDefault("Adds an additional burst to the Shard Staff and Storm Spell");
		}
		public override void UpdateEquip(Player player)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			modPlayer.shardSpellExtra++;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "FrigidBar", 8);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}