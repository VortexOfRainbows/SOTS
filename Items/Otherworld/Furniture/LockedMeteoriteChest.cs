using Microsoft.Xna.Framework;
using SOTS.Items.Furniture;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Otherworld.Furniture
{
	public class LockedMeteoriteChest : ContainerType
	{
		protected override int ChestDrop => ItemID.MeteoriteChest;
		protected override int ChestKey => ModContent.ItemType<Otherworld.MeteoriteKey>();
        protected override int DustType => -1;
		protected override void AddMapEntires()
		{
			AddMapEntry(new Color(174, 129, 92), this.GetLocalization("MapEntry0"), MapChestName);
			AddMapEntry(new Color(174, 129, 92), this.GetLocalization("MapEntry1"), MapChestName);
		}
	}
}