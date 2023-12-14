using SOTS.Items.Pyramid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SoldStuff
{
	public class BoreBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(99);
		}
		public override void SetDefaults()
		{
			Item.damage = 11;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 30;
			Item.maxStack = 9999;
			Item.consumable = true;           
			Item.knockBack = 1f;
			Item.value = Item.buyPrice(0, 0, 1, 50);
			Item.rare = ItemRarityID.Pink;
			Item.shoot = ModContent.ProjectileType<Projectiles.BoreBullet>(); 
			Item.shootSpeed = 0.5f;             
			Item.ammo = AmmoID.Bullet;   
            Item.UseSound = SoundID.Item23;
		}
		public override void AddRecipes()
		{
			CreateRecipe(100).AddIngredient(ItemID.EmptyBullet, 100).AddIngredient(ModContent.ItemType<JuryRiggedDrill>(), 1).AddTile(TileID.WorkBenches).Register();
		}
	}
}