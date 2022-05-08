using Microsoft.Xna.Framework;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Inferno;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Inferno
{
	public class Sharanga : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sharanga");
			Tooltip.SetDefault("Fires supercharged hellfire arrows");
		}
		public override void SetDefaults()
		{
            Item.damage = 25; 
            Item.ranged = true;  
            Item.width = 36;   
            Item.height = 54; 
            Item.useTime = 25; 
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.HoldingOut;    
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(0, 2, 25, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SharangaBolt>(); 
            Item.shootSpeed = 21.5f;
			Item.useAmmo = ItemID.WoodenArrow;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<SharangaBolt>(), damage, knockBack, player.whoAmI);
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HellwingBow, 1);
			recipe.AddIngredient(ItemID.MoltenFury, 1);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfInferno>(), 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
