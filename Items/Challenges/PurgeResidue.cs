using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;


namespace SOTS.Items.Challenges
{
	public class PurgeResidue: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purge Residue");
			Tooltip.SetDefault("A chunk of strange matter created by a purge\nSeems completely useless, unless you want to remove part of your soul");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(7, 4));
		}
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 22;
			item.useAnimation = 24;
			item.useTime = 24;
			item.useStyle = 2;
			item.value = 0;
			item.rare = -1;
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
            modPlayer.soulAmount -= 1;
			return true;
		}
	}
}