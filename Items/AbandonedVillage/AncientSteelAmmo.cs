using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	public class AncientSteelBullet : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(99);
		public override void SetDefaults()
		{
			Item.damage = 4;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 10;
			Item.height = 24;
			Item.maxStack = 9999;
			Item.consumable = true;           
			Item.knockBack = 1f;
			Item.value = Item.sellPrice(copper: 5);
			Item.rare = ItemRarityID.Blue;
			Item.shoot = ModContent.ProjectileType<Projectiles.AbandonedVillage.AncientSteelBullet>(); 
			Item.shootSpeed = 7f;             
			Item.ammo = AmmoID.Bullet;   
            Item.UseSound = SoundID.Item23;
		}
		public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient<AncientSteelBar>(1).AddIngredient<CharredWood>(1).AddTile(TileID.Anvils).Register();
        }
	}
	public class AncientSteelArrow : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(99);
        public override void SetDefaults()
		{
			Item.damage = 5;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 32;
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.knockBack = 1f;
			Item.value = Item.sellPrice(copper: 5);
			Item.rare = ItemRarityID.Blue;
			Item.shoot = ModContent.ProjectileType<Projectiles.AbandonedVillage.AncientSteelArrow>();
			Item.shootSpeed = 8f;
			Item.ammo = AmmoID.Arrow;
			Item.UseSound = SoundID.Item23;
		}
		public override void AddRecipes()
		{
			CreateRecipe(100).AddIngredient<AncientSteelBar>(1).AddIngredient<CharredWood>(1).AddTile(TileID.Anvils).Register();
		}
	}
}