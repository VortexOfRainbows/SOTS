using Microsoft.Xna.Framework;
using SOTS.Items.Furniture;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.Furniture
{
	public class LockedSkywareChest : ContainerType
	{
		protected override string ChestName => "Skyware Chest";
		protected override int ChestDrop => ItemID.SkywareChest;
		protected override int ChestKey => ModContent.ItemType<Otherworld.SkywareKey>();
		protected override int DustType => 116;
		protected override void AddMapEntires()
		{
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Skyware Chest");
			AddMapEntry(new Color(233, 207, 94), name, MapChestName);

			name = CreateMapEntryName(Name + "_Locked");
			name.SetDefault("Locked Skyware Chest");
			AddMapEntry(new Color(233, 207, 94), name, MapChestName);
		}
	}
}