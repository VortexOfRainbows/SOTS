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
			DisplayName.SetDefault("Bore Bullet");
			Tooltip.SetDefault("20% of damage done ignores defense completely\nIn addition, a flat 10 damage will also completely ignore defense"); //\nCan also drill through blocks");
		}
		public override void SetDefaults()
		{
			Item.damage = 11;
			Item.ranged = true;
			Item.width = 14;
			Item.height = 30;
			Item.maxStack = 999;
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.EmptyBullet, 100);
			recipe.AddIngredient(ModContent.ItemType<JuryRiggedDrill>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}
}