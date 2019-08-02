using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items
{
	public class EnchantedPickShrineTile : ModTile
{
    public override void SetDefaults()
    {
		mineResist = 0.01f;
		minPick = 0;
				Main.tileSolid[Type] = false;
        Main.tileFrameImportant[Type] = true;
        Main.tileLavaDeath[Type] = false;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newTile.StyleWrapLimit = 36;
        TileObjectData.addTile(Type);
        dustType = 1;
		ModTranslation name = CreateMapEntryName();
		name.SetDefault("Enchanted Pickaxe Shrine");		
		AddMapEntry(new Color(255, 255, 225), name);
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 1200;
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
     if(frameX == 0)
       {
       Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("EnchantedPickaxe"));
     }
    }
	public override bool CanExplode(int i, int j)
		{
			return true;
		}
	public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
			
}
}
}