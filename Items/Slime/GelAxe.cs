using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
	public class GelAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(99);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Shuriken);
			Item.damage = 11;
			Item.alpha = 25;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 32;
			Item.height = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 1.75f;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;            
			Item.shoot = ModContent.ProjectileType<Projectiles.GelAxe>(); 
            Item.shootSpeed *= 1.2f;
			Item.consumable = true;
			Item.maxStack = 9999;
		}
		public override void AddRecipes()
		{
			CreateRecipe(100).AddIngredient(ItemID.Gel, 20).AddIngredient(ItemID.IronBar, 1).AddTile(TileID.Solidifier).Register();
		}
	}
}