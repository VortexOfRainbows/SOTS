using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.SpecialDrops.BiomeRoots
{
	public class Waterweed : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Water Weed");
			
			Tooltip.SetDefault("Gives fishy power\nUse the buff key for buff");
		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 30;
			item.value = 0;
			item.rare = 4;
			item.maxStack = 1;
			item.consumable = false;
            item.buffType = BuffID.Gills;    //this is where you put your Buff name
            item.buffTime = 20000;  

			
		}
	}
}