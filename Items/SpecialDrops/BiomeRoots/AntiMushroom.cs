using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.SpecialDrops.BiomeRoots
{
	public class AntiMushroom : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anti Material Mushroom");
			
			Tooltip.SetDefault("Makes you go all trippy and balls\nAKA, gives random buffs or debuffs");
		}
		public override void SetDefaults()
		{
			
		item.CloneDefaults(ItemID.SwiftnessPotion);
			item.width = 22;
			item.height = 30;
			item.value = 0;
			item.rare = 4;
			item.maxStack = 1;
			item.consumable = false;
            item.buffType = 1;    //this is where you put your Buff name
            item.buffTime = 1;  

			
		}
		public override bool UseItem(Player player)
		{
		int rnd = Main.rand.Next(1, 205);
		if(rnd != 160 && rnd != 156 && rnd != 149 &&  rnd != 47 && rnd != 169 && rnd != 199 && rnd != 163 && rnd != 35 && rnd != 23)
		{
					player.AddBuff(rnd, 20000);
		}
		return true;
		
		}
	}
}