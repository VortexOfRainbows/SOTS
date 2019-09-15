using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.Potions
{
	public class ThiccPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thicc Potion");
			
			Tooltip.SetDefault("Heart disease, diabetes, cancer\nAll in one lovely bottle!\nDrastically boosts defensive capabilities, but drastically lowers speed ");
		}
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 36;
			item.value = 27500;
			item.rare = 4;
			item.maxStack = 30;
            item.buffType = mod.BuffType("Fat");    //this is where you put your Buff name
            item.buffTime = 21600;  
            item.UseSound = SoundID.Item3;                //this is the sound that plays when you use the item
            item.useStyle = 2;                 //this is how the item is holded when used
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.consumable = true;       
			
		}
	}
}