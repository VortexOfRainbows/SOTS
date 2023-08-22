using Microsoft.Xna.Framework;
using SOTS.Items.Crushers;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Items.SoldStuff;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Common.GlobalNPCs
{
    public class SOTSNPCShop : GlobalNPC
    {
        public override void SetupTravelShop(int[] shop, ref int nextSlot)
        {
			if (Main.rand.NextBool(5))
			{
				shop[nextSlot] = ModContent.ItemType<SafetySwitch>();
				nextSlot++;
			}
			if (Main.rand.NextBool(4))
			{
				shop[nextSlot] = ModContent.ItemType<CrushingCapacitor>();
				nextSlot++;
			}
			if (Main.hardMode && Main.rand.NextBool(4))
			{
				shop[nextSlot] = ModContent.ItemType<BoreBullet>();
				nextSlot++;
			}
			if (Main.rand.NextBool(10) && NPC.downedPlantBoss)
			{
				shop[nextSlot] = ModContent.ItemType<Traingun>();
				nextSlot++;
			}
			if (Main.rand.NextBool(10))
			{
				shop[nextSlot] = ModContent.ItemType<RecursiveBow>();
				nextSlot++;
			}
			if (Main.rand.NextBool(500))
			{
				shop[nextSlot] = ModContent.ItemType<SupremSticker>();
				nextSlot++;
			}
		}
        public override void ModifyShop(NPCShop shop)
        {
			if(shop.NpcType == NPCID.Mechanic)
			{
				shop.Add(ModContent.ItemType<CrushingTransformer>());
			}
			if (shop.NpcType == NPCID.Merchant)
			{
				shop.Add(ModContent.ItemType<BlackFlare>(), Condition.PlayerCarriesItem(ModContent.ItemType<FlareDetonator>()));
			}
		}
    }
}
