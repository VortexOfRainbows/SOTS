using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;

namespace SOTS.Items.Secrets
{
	public class ForgottenLampTile : ModTile
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
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            DustType = DustID.Cloud;
            ModTranslation name = CreateMapEntryName();
		    AddMapEntry(new Color(255, 255, 239), name);
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
             if(frameX == 0)
             {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, ModContent.ItemType<DreamLamp>());
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