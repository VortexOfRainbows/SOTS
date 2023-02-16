using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles;
using SOTS.Items.Fragments;
using Terraria.DataStructures;

namespace SOTS.Items.Nature
{
	public class NatureSpell : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
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
			CreateRecipe(1).AddRecipeGroup(RecipeGroupID.Wood, 20).AddIngredient(ModContent.ItemType<FragmentOfNature>(), 4).AddIngredient(ItemID.YellowMarigold, 1).AddTile(TileID.WorkBenches).Register();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, 5);
			return false; 
		}
	}
}
