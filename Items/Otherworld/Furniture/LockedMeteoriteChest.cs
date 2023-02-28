using Microsoft.Xna.Framework;
using SOTS.Items.Furniture;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Otherworld.Furniture
{
	public class LockedMeteoriteChest : ContainerType
	{
		protected override string ChestName => Language.GetTextValue("Mods.SOTS.ItemName.LockedMeteoriteChest");
		protected override int ChestDrop => ItemID.MeteoriteChest;
		protected override int ChestKey => ModContent.ItemType<Otherworld.MeteoriteKey>();
        protected override int DustType => -1;
		protected override void AddMapEntires()
		{
			ModTranslation name = CreateMapEntryName();
			AddMapEntry(new Color(174, 129, 92), name, MapChestName);

			name = CreateMapEntryName(Name + "_Locked"); 
			AddMapEntry(new Color(174, 129, 92), name, MapChestName);
		}
	}
}