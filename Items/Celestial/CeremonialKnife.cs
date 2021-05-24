using Terraria;
using SOTS.Void;
using Terraria.ID;

namespace SOTS.Items.Celestial
{
	public class CeremonialKnife : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Servant Knife");
			Tooltip.SetDefault("Jebaited");
		}
		public override void SafeSetDefaults()
		{
			item.melee = true;
			item.damage = 100;
			item.width = 28;
			item.height = 32;
            item.value = Item.sellPrice(0, 69, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.useTime = 7;
			item.useAnimation = 14;
			item.useStyle = 1;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.knockBack = 2f;
		}
	}
}