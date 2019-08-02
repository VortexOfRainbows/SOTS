using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.Potions
{
	public class DemonBlood : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Demon Blood");
			
			Tooltip.SetDefault("Death in a bottle");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = 2500;
			item.rare = 3;
			item.maxStack = 999;
            item.buffType = mod.BuffType("Sacrifice");    //this is where you put your Buff name
            item.buffTime = 3600;  
            item.UseSound = SoundID.Item3;                //this is the sound that plays when you use the item
            item.useStyle = 2;                 //this is how the item is holded when used
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.consumable = true;       
			
		}
	}
}