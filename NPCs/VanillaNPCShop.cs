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
						if(Main.rand.Next(4) == 0)
						{
                        shop[nextSlot] = (mod.ItemType("BackAttachment"));  //this is an example of how to add your item
                        nextSlot++;
						}
						if(Main.rand.Next(3) == 0)
						{
                        shop[nextSlot] = (mod.ItemType("DartBlaster"));  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (mod.ItemType("SupremSticker"));  //this is an example of how to add your item
                        nextSlot++;
						}
						if(Main.rand.Next(20) == 0)
						{
							
                        shop[nextSlot] = (mod.ItemType("TheBarrel"));  //this is an example of how to add your item
                        nextSlot++;
						}
						if(Main.rand.Next(12) == 0 && NPC.downedBoss2)
						{
						shop[nextSlot] = (ItemID.SawtoothShark);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.ReaverShark);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.Swordfish);  //this is an example of how to add your item
                        nextSlot++;
						}
						if(Main.rand.Next(15) == 0 && NPC.downedBoss1)
						{
						shop[nextSlot] = (ItemID.BladedGlove);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.Arkhalis);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.EnchantedSword);  //this is an example of how to add your item
                        nextSlot++;
						}
						if(Main.rand.Next(7) == 0 && NPC.downedBoss1)
						{
						shop[nextSlot] = (ItemID.LesserHealingPotion);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.LesserManaPotion);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.StrangeBrew);  //this is an example of how to add your item
                        nextSlot++;
						}
						if(Main.rand.Next(7) == 0 && NPC.downedBoss2)
						{
						shop[nextSlot] = (ItemID.BattlePotion);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.CalmingPotion);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.EndurancePotion);  //this is an example of how to add your item
                        nextSlot++;
						}
						if(Main.rand.Next(7) == 0 && NPC.downedBoss3)
						{
						shop[nextSlot] = (ItemID.IronskinPotion);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.InfernoPotion);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.LifeforcePotion);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.MagicPowerPotion);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.ManaRegenerationPotion);  //this is an example of how to add your item
                        nextSlot++;
						}
						if(Main.rand.Next(30) == 0)
						{
						shop[nextSlot] = (ItemID.Amethyst);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.Topaz);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.Sapphire);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.Emerald);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.Ruby);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.Diamond);  //this is an example of how to add your item
                        nextSlot++;
						}
						if(Main.rand.Next(30) == 0)
						{
						shop[nextSlot] = (ItemID.CopperOre);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.IronOre);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.SilverOre);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.GoldOre);  //this is an example of how to add your item
                        nextSlot++;
						
						}
						if(Main.rand.Next(30) == 0)
						{
						shop[nextSlot] = (ItemID.TinOre);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.LeadOre);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.TungstenOre);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.PlatinumOre);  //this is an example of how to add your item
                        nextSlot++;
						
						}
						if(Main.rand.Next(12) == 0)
						{
						shop[nextSlot] = (ItemID.FlyingCarpet);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = 857;  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.PharaohsRobe);  //this is an example of how to add your item
                        nextSlot++;
						
						shop[nextSlot] = (ItemID.PharaohsMask);  //this is an example of how to add your item
                        nextSlot++;
						
						}
						
						if(Main.rand.Next(5) == 0)
						{
						shop[nextSlot] = (ItemID.IllegalGunParts);  //this is an example of how to add your item
                        nextSlot++;
						}
						if(Main.rand.Next(9) == 0 && NPC.downedBoss3)
						{
						shop[nextSlot] = (mod.ItemType("PulsePistol"));  //this is an example of how to add your item
                        nextSlot++;
						}
						
						if(Main.rand.Next(7) == 0)
						{
						shop[nextSlot] = (mod.ItemType("BiggHands"));  //this is an example of how to add your item
                        nextSlot++;
						}
		  }public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
			
			
            switch (type)
            {
                case NPCID.Merchant:  //change Dryad whith what NPC you want
 
                    if (NPC.downedBoss1) //if it's hardmode the NPC will sell this
                    {
                        shop.item[nextSlot].SetDefaults(mod.ItemType("MidasMaker"));  //this is an example of how to add your item
                        nextSlot++;
						
						if(Main.player[Main.myPlayer].ZoneSnow)
						{
						shop.item[nextSlot].SetDefaults(mod.ItemType("Ribbon"));  //this is an example of how to add your item
                        nextSlot++;
						}
					
                        shop.item[nextSlot].SetDefaults(mod.ItemType("EnchantedGoggles"));  //this is an example of how to add your item
                        nextSlot++;
                    } if (NPC.downedPlantBoss) //if it's hardmode the NPC will sell this
                    {
						
                        shop.item[nextSlot].SetDefaults(mod.ItemType("CheckeredBoard"));  //this is an example of how to add your item
                        nextSlot++;
						
						if(Main.eclipse)
						{
                        shop.item[nextSlot].SetDefaults(ItemID.Ectoplasm);  //this is an example of how to add your item
                        nextSlot++;
						}
					}
                    break;
            }
			
			
            switch (type)
            {
                case NPCID.ArmsDealer:  //change Dryad whith what NPC you want
 
                    if (Main.hardMode) //if it's hardmode the NPC will sell this
                    {
                        shop.item[nextSlot].SetDefaults(mod.ItemType("IchorSprayer"));  //this is an example of how to add your item
                        nextSlot++;
                    }
                    if (NPC.downedBoss3) //if it's hardmode the NPC will sell this
                    {
                        shop.item[nextSlot].SetDefaults(mod.ItemType("BulletSnapper"));  //this is an example of how to add your item
                        nextSlot++;
                    }
 
 
                    break;
            }
			
        }
        }
    }
