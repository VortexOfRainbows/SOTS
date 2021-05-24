using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{	
	public class HeartInAJar : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heart in a Jar");
			Tooltip.SetDefault("Periodically applies a stacking curse to enemies within a small radius around you, draining their health overtime\nCursed enemies release Curse Fragments upon death, which seek out other enemies\nIncreases max life by 20");
		}
		public override void SetDefaults()
		{
            item.width = 38;     
            item.height = 48;   
            item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
			item.expert = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			modPlayer.rippleBonusDamage += 16;
			player.statLifeMax2 += 20;
		}
	}
}