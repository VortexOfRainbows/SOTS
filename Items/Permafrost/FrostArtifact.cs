using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class FrostArtifact : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 44;
			Item.height = 26;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<FrostArtifactTile>();
			Item.rare = ItemRarityID.Cyan;
        }
    }
}