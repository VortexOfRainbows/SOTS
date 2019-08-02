using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;


namespace SOTS.Items.Challenges
{
	public class EternalFragment : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eternal Fragment");
			Tooltip.SetDefault("Increases max health, does not cap out\nA strange form of matter, nigh unobtainable without the correct conditions");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(7, 4));
		}
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 22;
			item.useAnimation = 12;
			item.useTime = 12;
			item.useStyle = 2;
			item.value = 0;
			item.rare = 9;
			item.maxStack = 999;
			item.autoReuse = true;
			item.consumable = true;
			item.noUseGraphic = true;
			 ItemID.Sets.ItemNoGravity[item.type] = true; 
		}
		public override bool UseItem(Player player)
		{
			Main.PlaySound(4, (int)(player.Center.X), (int)(player.Center.Y), 39);
			player.statLifeMax++;
			return true;
		}
	}
}