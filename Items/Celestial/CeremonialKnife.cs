using Terraria;
using SOTS.Void;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Celestial
{
	public class CeremonialKnife : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Servant Knife");
			Tooltip.SetDefault("Jebaited");
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.DamageType = DamageClass.Melee;
			Item.damage = 100;
			Item.width = 50;
			Item.height = 48;
            Item.value = Item.sellPrice(0, 69, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.useTime = 7;
			Item.useAnimation = 14;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.knockBack = 2f;
		}
	}
}