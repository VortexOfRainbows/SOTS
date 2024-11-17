using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	public class PintOPunch : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.DamageType = DamageClass.Ranged;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.width = 44;
			Item.height = 28;
			Item.maxStack = 1;     
			Item.shoot = ModContent.ProjectileType<Projectiles.AbandonedVillage.PintOPunch>(); 
            Item.shootSpeed = 12.0f;
			Item.knockBack = 10.0f;
			Item.consumable = false;
			Item.noUseGraphic = Item.noMelee = Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
			Item.crit = 6;
		}
	}
}