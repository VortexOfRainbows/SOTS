using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chess
{
	public class CheckeredBall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Checkered Ball Of Souls");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.width = 24;
			item.height = 24;
			item.value = 10000000;
			item.rare = 6;
			item.maxStack = 1;
			item.useStyle = 4;
			item.useTime = 220;
			item.useAnimation = 220;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SoulSingularity", 10);
			recipe.AddIngredient(null, "CheckeredBoard", 4);
			recipe.AddIngredient(ItemID.Ectoplasm, 20);
			recipe.AddIngredient(ItemID.TitaniumBar, 60);
			recipe.AddIngredient(ItemID.PumpkingTrophy, 1);
			recipe.AddIngredient(ItemID.IceQueenTrophy, 1);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
		public override bool CanUseItem(Player player)
		{
		return !NPC.AnyNPCs(mod.NPCType("ChessPortal"));
	
		}
		public override bool UseItem(Player player)
		{
		Projectile.NewProjectile(player.Center.X, player.Center.Y +40, 3, 0, mod.ProjectileType("ChessSpawn"), 0, 0, player.whoAmI);	
		Projectile.NewProjectile(player.Center.X, player.Center.Y +40, -3, 0, mod.ProjectileType("ChessSpawn2"), 0, 0, player.whoAmI);		
		Projectile.NewProjectile(player.Center.X, player.Center.Y -800, -3, 0, mod.ProjectileType("ChessSpawn3"), 0, 0, player.whoAmI);		
		Projectile.NewProjectile(player.Center.X, player.Center.Y -800, 3, 0, mod.ProjectileType("ChessSpawn3"), 0, 0, player.whoAmI);		
		Projectile.NewProjectile(player.Center.X + 1080, player.Center.Y +40 ,0, -2.3f, mod.ProjectileType("ChessSpawn3"), 0, 0, player.whoAmI);		
		Projectile.NewProjectile(player.Center.X - 1080, player.Center.Y +40 ,0, -2.3f, mod.ProjectileType("ChessSpawn3"), 0, 0, player.whoAmI);		
		Main.PlaySound(0, (int)player.position.X, (int)player.position.Y, 0);
		
		return true;
		
		}
	}
}