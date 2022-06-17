using Microsoft.Xna.Framework;
using SOTS.Items.Furniture;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.Furniture
{
	public class LockedMeteoriteChest : ContainerType
	{
		protected override string ChestName => "Meteorite Chest";
		protected override int ChestDrop => ItemID.MeteoriteChest;
		protected override int ChestKey => ModContent.ItemType<Otherworld.MeteoriteKey>();
        protected override int DustType => -1;
		protected override void AddMapEntires()
		{
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Meteorite Chest");
			AddMapEntry(new Color(174, 129, 92), name, MapChestName);

			name = CreateMapEntryName(Name + "_Locked"); 
			name.SetDefault("Locked Meteorite Chest");
			AddMapEntry(new Color(174, 129, 92), name, MapChestName);
		}
	}
}