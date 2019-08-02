using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;


namespace SOTS.Items.Blood
{
	public class SoulFragment: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Fragment");
			Tooltip.SetDefault("A piece of someone's very own soul\nCan be added to your soul or used for crafting");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(7, 4));
		}
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 22;
			item.useAnimation = 24;
			item.useTime = 24;
			item.useStyle = 2;
			item.value = 250000;
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
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
            modPlayer.soulAmount += 1;
			return true;
		}
	}
}