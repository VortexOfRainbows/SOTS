using Microsoft.Xna.Framework;
using SOTS.Items;
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
				shop[nextSlot++] = ModContent.ItemType<SafetySwitch>();
			}
			if (Main.rand.NextBool(4))
			{
				shop[nextSlot++] = ModContent.ItemType<CrushingCapacitor>();
			}
			if (Main.hardMode && Main.rand.NextBool(4))
			{
				shop[nextSlot++] = ModContent.ItemType<BoreBullet>();
			}
			if (Main.rand.NextBool(10) && NPC.downedPlantBoss)
			{
				shop[nextSlot++] = ModContent.ItemType<Traingun>();
			}
			if (Main.rand.NextBool(10))
			{
				shop[nextSlot++] = ModContent.ItemType<RecursiveBow>();
            }
            if (Main.rand.NextBool(5))
            {
                shop[nextSlot++] = ItemID.EnchantedBoomerang;
            }
            if (Main.rand.NextBool(500))
			{
				shop[nextSlot++] = ModContent.ItemType<SupremSticker>();
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
				shop.Add(ModContent.ItemType<KeepersBox>(), Condition.TimeNight);
                shop.Add(ModContent.ItemType<PlasticBait>());
                shop.Add(ModContent.ItemType<BlackFlare>(), Condition.PlayerCarriesItem(ModContent.ItemType<FlareDetonator>()));
			}
		}
    }
}
