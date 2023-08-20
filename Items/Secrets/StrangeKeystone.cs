using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using Terraria.DataStructures;

namespace SOTS.Items.Secrets
{
	public class StrangeKeystone : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 32;
			Item.height = 42;
			Item.rare = ItemRarityID.Orange;
			Item.createTile = ModContent.TileType<StrangeKeystoneTile>();
		}
		public override void UpdateInventory(Player player)
		{
			SOTSPlayer.ModPlayer(player).weakerCurse = true;
		}
	}
	public class StrangeKeystoneBroken : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 32;
			Item.height = 20;
			Item.rare = ItemRarityID.Orange;
			Item.createTile = ModContent.TileType<StrangeKeystoneTile>();
			Item.placeStyle = 1;
		}
	}
	public class StrangeKeystoneTile : ModTile
    {
        public override void SetStaticDefaults()
        {
		    MineResist = 0.01f;
		    MinPick = 0;
		    Main.tileSolid[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.addTile(Type);
            DustType = 32;
            LocalizedText name = CreateMapEntryName();
		    AddMapEntry(new Color(90, 80, 45), name);
        }
	    public override bool CanExplode(int i, int j)
		{
			return true;
		}
    	public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			offsetY = 2;
        }
    }
}