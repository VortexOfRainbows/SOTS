
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Items.Secrets.IceCream
{
	public class IceCreamBottleTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			minPick = 80; 
			dustType = 72;
			drop = mod.ItemType("IceCreamOre2");
			AddMapEntry(new Color(212, 150, 180));
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 2.5f;
			g = 2.5f;
			b = 2.5f;
		}
	}
}