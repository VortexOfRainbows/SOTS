using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
 
namespace SOTS.Items.SpecialDrops         
{
    public class PyramidCrateTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = true;    
            Main.tileFrameImportant[Type] = true;
            Main.tileTable[Type] = true;    
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.CoordinateHeights = new int[]{ 16,18 };
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Pyramid Crate");		
			AddMapEntry(new Color(200, 180, 100), name);
            TileObjectData.addTile(Type);
 
        }
 
 
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 16, mod.ItemType("PyramidCrate")); //this defines what to drop when this tile is destroyed
        }
    }
}