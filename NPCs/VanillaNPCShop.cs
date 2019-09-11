using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.NPCs
{
    public class VanillaNPCShop : GlobalNPC
    {
        public override void SetupTravelShop(int[] shop, ref int nextSlot)
        {
            //switch (type)
          //  {
						if(Main.rand.Next(3) == 0)
						{
						
						shop[nextSlot] = (mod.ItemType("SupremSticker"));  
                        nextSlot++;
						}
						if(Main.rand.Next(12) == 0 && NPC.downedBoss2)
						{
						shop[nextSlot] = (ItemID.SawtoothShark);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.ReaverShark);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.Swordfish);  
                        nextSlot++;
						}
						if(Main.rand.Next(15) == 0 && NPC.downedBoss1)
						{
						shop[nextSlot] = (ItemID.BladedGlove);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.Arkhalis);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.EnchantedSword);  
                        nextSlot++;
						}
						if(Main.rand.Next(7) == 0 && NPC.downedBoss1)
						{
						shop[nextSlot] = (ItemID.LesserHealingPotion);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.LesserManaPotion);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.StrangeBrew);  
                        nextSlot++;
						}
						if(Main.rand.Next(7) == 0 && NPC.downedBoss2)
						{
						shop[nextSlot] = (ItemID.BattlePotion);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.CalmingPotion);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.EndurancePotion);  
                        nextSlot++;
						}
						if(Main.rand.Next(7) == 0 && NPC.downedBoss3)
						{
						shop[nextSlot] = (ItemID.IronskinPotion);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.InfernoPotion);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.LifeforcePotion);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.MagicPowerPotion);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.ManaRegenerationPotion);  
                        nextSlot++;
						}
						if(Main.rand.Next(30) == 0)
						{
						shop[nextSlot] = (ItemID.Amethyst);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.Topaz);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.Sapphire);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.Emerald);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.Ruby);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.Diamond);  
                        nextSlot++;
						}
						if(Main.rand.Next(30) == 0)
						{
						shop[nextSlot] = (ItemID.CopperOre);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.IronOre);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.SilverOre);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.GoldOre);  
                        nextSlot++;
						
						}
						if(Main.rand.Next(30) == 0)
						{
						shop[nextSlot] = (ItemID.TinOre);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.LeadOre);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.TungstenOre);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.PlatinumOre);  
                        nextSlot++;
						
						}
						if(Main.rand.Next(12) == 0)
						{
						shop[nextSlot] = (ItemID.FlyingCarpet);  
                        nextSlot++;
						
						shop[nextSlot] = 857;  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.PharaohsRobe);  
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.PharaohsMask);  
                        nextSlot++;
						
						}
						
						if(Main.rand.Next(5) == 0)
						{
						shop[nextSlot] = (ItemID.IllegalGunParts);  
                        nextSlot++;
						}
		}
		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			
			
            switch (type)
            {
                case NPCID.Merchant: 
					if (NPC.downedPlantBoss) 
                    {

					}
                    break;
            }
			
			
            switch (type)
            {
                case NPCID.ArmsDealer:
 
                    if (Main.hardMode) 
                    {
						
                    }
                    if (NPC.downedBoss3) 
                    {
						
                    }
                    break;
            }
		}
    }
}
