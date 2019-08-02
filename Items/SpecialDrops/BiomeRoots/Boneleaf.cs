using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.SpecialDrops.BiomeRoots
{
	public class Boneleaf : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bone Leaf");
			
			Tooltip.SetDefault("Gives control of gravity\nUse the buff key for buff");
		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 30;
			item.value = 0;
			item.rare = 4;
			item.maxStack = 1;
			item.consumable = false;
            item.buffType = BuffID.Gravitation;    //this is where you put your Buff name
            item.buffTime = 20000;  

			
		}
	}
}