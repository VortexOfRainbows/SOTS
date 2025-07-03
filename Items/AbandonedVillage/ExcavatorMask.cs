using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	[AutoloadEquip(EquipType.Head)]
	public class ExcavatorMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
			//SetupDrawing();
        }
        //private void SetupDrawing()
        //{
        //    if (Main.netMode == NetmodeID.Server)
        //        return;
        //    int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
        //    ArmorIDs.Head.Sets.
        //}
        public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 20;
			Item.vanity = true;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
		}
	}
}