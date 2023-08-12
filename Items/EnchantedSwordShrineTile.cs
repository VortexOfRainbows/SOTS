using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace SOTS.Items
{
	public class EnchantedSwordShrineTile : ModTile
    {
        public override void SetStaticDefaults()
        {
		    MineResist = 0.01f;
		    MinPick = 0;
            Main.tileNoFail[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.addTile(Type);
            DustType = 32;
            LocalizedText name = CreateMapEntryName();
		    AddMapEntry(new Color(170, 150, 85), name);
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 1200;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
             if(frameX == 0)
             {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, ItemID.EnchantedSword);
             }
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