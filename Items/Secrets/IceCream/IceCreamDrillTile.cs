using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Secrets.IceCream
{
	public class IceCreamDrillTile : ModTile
{
    public override void SetDefaults()
    {
        Main.tileFrameImportant[Type] = true;
		Main.tileSolid[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
        TileObjectData.newTile.CoordinateHeights = new[]{16, 16};
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
        TileObjectData.addTile(Type);
		AddMapEntry(new Color(255, 180, 188));
		dustType = 72;
		disableSmartCursor = true;
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        Item.NewItem(i * 16, j * 16, 32, 16, mod.ItemType("IceCreamDrill"));
    }
	public override void HitWire(int i, int j)
	{
		int mine = 3;
			for(mine = 3; mine < 23; mine++)
			{
				if(Main.tile[i, j + mine].type != mod.TileType("IceCreamBrickTile") && Main.tile[i, j + mine].type != mod.TileType("PlanetariumBlock") && Main.tile[i, j + mine].type != 226)
				{
				Projectile.NewProjectile(i * 16 + 8, (j + mine) * 16 + 8, 0, 0, mod.ProjectileType("AlphaBlockBreak"), 0, 0, 1);
				}
				else
				{
				mine = 23;
				}
				
			}
			
	}
}}