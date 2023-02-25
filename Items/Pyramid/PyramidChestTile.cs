using Microsoft.Xna.Framework;
using SOTS.Items.Furniture;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Pyramid
{
	public class PyramidChestTile : ContainerType
	{
		protected override string ChestName => Language.GetTextValue("Mods.SOTS.MapObject.PyramidChestTile");
		protected override int ChestDrop => ModContent.ItemType<PyramidChest>();
		protected override int DustType => 7;
		protected override void AddMapEntires()
		{
			ModTranslation name = CreateMapEntryName();
			AddMapEntry(new Color(194, 138, 138), name, MapChestName);
		}
	}
}