using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.Chess
{
	public class KingTrinity : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Trinity");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 7));
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.width = 38;
			item.height = 38;
			item.value = 125000;
			item.rare = 7;
			item.maxStack = 99;
			 ItemID.Sets.ItemNoGravity[item.type] = true; 
			 ItemID.Sets.AnimatesAsSoul[item.type] = false;


			
		}
	}
}