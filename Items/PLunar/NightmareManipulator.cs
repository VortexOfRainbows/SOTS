using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.PLunar
{
	public class NightmareManipulator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightmare Manipulator");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 4));
		}
		public override void SetDefaults()
		{

			item.width = 21;
			item.height = 21;
			item.value = 0;
			item.rare = 12;
			item.maxStack = 999;
			item.expert = true;
			 ItemID.Sets.ItemNoGravity[item.type] = true; 
			 ItemID.Sets.AnimatesAsSoul[item.type] = false;


			
		}
	}
}