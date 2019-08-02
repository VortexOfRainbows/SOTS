
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class EmptyPlanetariumBlock : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			minPick = 60; 
			dustType = 160;
			drop = mod.ItemType("EmptyPlanetariumOrb");
			AddMapEntry(new Color(200, 200, 200));
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem) 
		{
			
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 1.5f;
			g = 1.5f;
			b = 1.5f;
		}
	}
}