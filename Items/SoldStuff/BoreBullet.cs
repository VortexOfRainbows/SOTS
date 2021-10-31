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
			item.damage = 9;
			item.ranged = true;
			item.width = 14;
			item.height = 30;
			item.maxStack = 999;
			item.consumable = true;           
			item.knockBack = 1f;
			item.value = Item.buyPrice(0, 0, 1, 50);
			item.rare = ItemRarityID.Pink;
			item.shoot = ModContent.ProjectileType<Projectiles.BoreBullet>(); 
			item.shootSpeed = 0.5f;             
			item.ammo = AmmoID.Bullet;   
            item.UseSound = SoundID.Item23;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.EmptyBullet, 200);
			recipe.AddIngredient(ModContent.ItemType<JuryRiggedDrill>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 200);
			recipe.AddRecipe();
		}
	}
}