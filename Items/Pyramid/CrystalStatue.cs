using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;
 
namespace SOTS.Items.Pyramid        
{
    public class CrystalStatue : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Crystal Statue");		
			AddMapEntry(new Color(175, 0, 0), name);
            soundType = 21;
            soundStyle = 2;
            TileObjectData.addTile(Type);
			mineResist = 2.5f;
 
        }
 
 
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 32, ItemID.LifeCrystal);//this defines what to drop when this tile is destroyed
        }
    }
}