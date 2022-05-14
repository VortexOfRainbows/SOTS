using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Projectiles.Lightning;

namespace SOTS.Items.Tide
{
	public class GreenJellyfishStaff : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Green Jellyfish Staff");
			Tooltip.SetDefault("Fires 2 green orbs that, upon detonation, release green thunder towards your cursor\nGreen thunder chains off enemies for 90% damage\nProvides a light source while in the inventory");
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 70;
			Item.DamageType = DamageClass.Magic;
            Item.width = 38;    
            Item.height = 38; 
            Item.useTime = 18; 
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 3.5f;
			Item.value = Item.sellPrice(0, 7, 50, 0);
            Item.rare = ItemRarityID.Lime;
			Item.UseSound = SoundID.Item92;
            Item.noMelee = true; 
            Item.autoReuse = true;
            Item.shootSpeed = 6.25f; 
			Item.shoot = ModContent.ProjectileType<GreenThunderCluster>();
			Item.staff[Item.type] = true; 
		}
		public override int GetVoid(Player player)
		{
			return  5;
		}
		public override void UpdateInventory(Player player)
		{
			Lighting.AddLight(player.Center, 1f, 1f, 1f);
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PurpleJellyfishStaff>(), 1);
			recipe.AddIngredient(ItemID.SoulofLight, 15);
			recipe.AddIngredient(ItemID.SoulofSight, 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(180)); 
            Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			
            Vector2 perturbedSpeed2 = perturbedSpeed.RotatedBy(MathHelper.ToRadians(180)); 
            Projectile.NewProjectile(position.X, position.Y, perturbedSpeed2.X, perturbedSpeed2.Y, type, damage, knockBack, player.whoAmI);
            return false;
		}
	}
}
