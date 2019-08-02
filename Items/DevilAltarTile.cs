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
	public class DevilAltarTile : ModTile
{
    public override void SetDefaults()
    {
		minPick = 100000; 
				Main.tileSolid[Type] = false;
        Main.tileFrameImportant[Type] = true;
        Main.tileLavaDeath[Type] = false;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newTile.StyleWrapLimit = 36;
        TileObjectData.addTile(Type);
        dustType = 14;
		ModTranslation name = CreateMapEntryName();
		name.SetDefault("Libra's Altar");		
		AddMapEntry(new Color(255, 0, 0), name);
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 1200;
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
     if(frameX == 0)
       {
       Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("DevilAltar"));
     }
    }
	public override bool CanExplode(int i, int j)
		{
			if (Main.tile[i, j].type == mod.TileType("DevilAltarTile"))
			{
				return false;
			}
			return false;
		}
	public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return false;
			
		}
		   public override void RightClick(int i, int j)
        {
            Player player = Main.player[Main.myPlayer];
            Tile tile = Main.tile[i, j];
            Main.mouseRightRelease = false;
			Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, -4, mod.ProjectileType("DevilDonut"), 1, 1, 0);
		}  public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 0;
			
}
		public override bool Slope(int i, int j)
		{
			return false;
		}
public override void PlaceInWorld(int i, int j, Item item)
		{
				Main.tileSolid[Type] = true;
		}
		public override void RandomUpdate(int i, int j)
		{
				Main.tileSolid[Type] = true;

	
}
}
}