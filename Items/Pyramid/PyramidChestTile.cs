using Microsoft.Xna.Framework;
using SOTS.Items.Furniture;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Pyramid
{
	public class PyramidChestTile : ContainerType
	{
		protected override int ChestDrop => ModContent.ItemType<PyramidChest>();
		protected override int DustType => 7;
		protected override void AddMapEntires()
		{
			AddMapEntry(new Color(194, 138, 138), this.GetLocalization("MapEntry0"), MapChestName);
		}
	}
}