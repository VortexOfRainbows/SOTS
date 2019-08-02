using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class BoreBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bore Bullet");
			Tooltip.SetDefault("Increases in damage as it pierces\nBusts bunkers effectively");
		}public override void SetDefaults()
		{
			item.damage = 18;
			item.ranged = true;
			item.width = 18;
			item.height = 26;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 0.12f;
			item.value = 250;
			item.rare = 7;
			item.shoot = mod.ProjectileType("BoreBullet");   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 0.56f;                  //The speed of the projectile
			item.ammo = AmmoID.Bullet;   
            item.UseSound = SoundID.Item23;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CobaltDrill, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
			
			ModRecipe recipe2 = new ModRecipe(mod);
			recipe2.AddIngredient(ItemID.PalladiumDrill, 1);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.SetResult(this, 100);
			recipe2.AddRecipe();
			
			ModRecipe recipe3 = new ModRecipe(mod);
			recipe3.AddIngredient(ItemID.MythrilDrill, 1);
			recipe3.AddTile(TileID.MythrilAnvil);
			recipe3.SetResult(this, 325);
			recipe3.AddRecipe();
			
			ModRecipe recipe4 = new ModRecipe(mod);
			recipe4.AddIngredient(ItemID.OrichalcumDrill, 1);
			recipe4.AddTile(TileID.MythrilAnvil);
			recipe4.SetResult(this, 325);
			recipe4.AddRecipe();
			
			ModRecipe recipe5 = new ModRecipe(mod);
			recipe5.AddIngredient(ItemID.AdamantiteDrill, 1);
			recipe5.AddTile(TileID.MythrilAnvil);
			recipe5.SetResult(this, 750);
			recipe5.AddRecipe();
			
			ModRecipe recipe6 = new ModRecipe(mod);
			recipe6.AddIngredient(ItemID.TitaniumDrill, 1);
			recipe6.AddTile(TileID.MythrilAnvil);
			recipe6.SetResult(this, 750);
			recipe6.AddRecipe();
		}
	}
}