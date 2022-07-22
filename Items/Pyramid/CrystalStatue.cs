using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
 
namespace SOTS.Items.Pyramid        
{
    public class CrystalStatue : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 4;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
			name.SetDefault("Crystal Statue");		
			AddMapEntry(new Color(175, 0, 0), name);
            HitSound = SoundID.Tink;
			MineResist = 2.5f;
			DustType = 12;
            AnimationFrameHeight = 36;
        }
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter > 5)
            {
                frameCounter = 0;
                frame++;
                if (frame >= 11)
                {
                    frame = 0;
                }
            }
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.LifeCrystal);//this defines what to drop when this tile is destroyed
        }
    }
}