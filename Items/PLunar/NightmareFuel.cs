using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.PLunar
{
	public class NightmareFuel : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightmare Fuel");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 8));
			Tooltip.SetDefault("The pure concentraition of your nightmares\nThis powerful material can only make normal users happy...\nCrazier users will always seek more...\nAlthough the material can be used for the strongest of items, too much usage can be your downfall...");
		}
		public override void SetDefaults()
		{

			item.width = 60;
			item.height = 60;
			item.value = 0;
			item.rare = 12;
			item.maxStack = 999;
			 ItemID.Sets.ItemNoGravity[item.type] = true; 
			 ItemID.Sets.AnimatesAsSoul[item.type] = false;


			
		}
	}
}