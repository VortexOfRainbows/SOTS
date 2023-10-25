using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.ChestItems
{
	public class WingedKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.ThrowingKnife);
			Item.damage = 12;
			Item.DamageType = DamageClass.Ranged;
			Item.rare = ItemRarityID.Blue;
			Item.width = 42;
			Item.height = 42;
			Item.maxStack = 1;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<Projectiles.WingedKnife>(); 
            Item.shootSpeed = 12f;
			Item.consumable = false;
			Item.knockBack = 1.5f;
		}
	}
}