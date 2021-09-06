using Microsoft.Xna.Framework;
using SOTS.Items.SoldStuff;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.NPCs
{
    public class VanillaNPCShop : GlobalNPC
    {
        public override void SetupTravelShop(int[] shop, ref int nextSlot)
        {
			if (Main.rand.NextBool(5))
			{
				shop[nextSlot] = ModContent.ItemType<SafetySwitch>();
				nextSlot++;
			}
			else if(Main.rand.NextBool(500))
			{
				shop[nextSlot] = ModContent.ItemType<SupremSticker>();
				nextSlot++;
			}
		}
		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{	
            switch (type)
            {
                case NPCID.Merchant: 
					if (Main.LocalPlayer.HasItem(mod.ItemType("FlareDetonator")))
					{
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<BlackFlare>());
						nextSlot++;
					}
                    break;
            }
		}
    }
}
