using Microsoft.Xna.Framework;
using SOTS.Items.Furniture;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Planetarium.Furniture
{
	public class LockedSkywareChest : ContainerType
	{
		protected override int ChestDrop => ItemID.SkywareChest;
		protected override int ChestKey => ModContent.ItemType<Planetarium.SkywareKey>();
		protected override int DustType => 116;
		protected override void AddMapEntires()
		{
			AddMapEntry(new Color(233, 207, 94), this.GetLocalization("MapEntry0"), MapChestName);
			AddMapEntry(new Color(233, 207, 94), this.GetLocalization("MapEntry1"), MapChestName);
		}
	}
}