using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles;
using SOTS.Items.Fragments;

namespace SOTS.Items.Nature
{
	public class NatureSpell : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flower Spell");
			Tooltip.SetDefault("Launches a seed that latches onto enemies\nWhen the seed blooms, it does 500% damage");
		}
		public override void SetDefaults()
		{
            Item.damage = 5; 
            Item.DamageType = DamageClass.Magic; 
            Item.width = 24;   
            Item.height = 28;   
            Item.useTime = 30;   
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;  
            Item.knockBack = 2.5f;
			Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item8;
            Item.shoot = ModContent.ProjectileType<FlowerSeed>(); 
            Item.shootSpeed = 10f;
			Item.mana = 5;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup(RecipeGroupID.Wood, 20);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfNature>(), 4);
			recipe.AddIngredient(ItemID.YellowMarigold, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0, 5);
			return false; 
		}
	}
}
