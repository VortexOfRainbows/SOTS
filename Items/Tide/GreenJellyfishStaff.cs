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
            item.damage = 70;
			item.magic = true;
            item.width = 38;    
            item.height = 38; 
            item.useTime = 18; 
            item.useAnimation = 18;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.knockBack = 3.5f;
			item.value = Item.sellPrice(0, 7, 50, 0);
            item.rare = ItemRarityID.Lime;
			item.UseSound = SoundID.Item92;
            item.noMelee = true; 
            item.autoReuse = true;
            item.shootSpeed = 6.25f; 
			item.shoot = ModContent.ProjectileType<GreenThunderCluster>();
			Item.staff[item.type] = true; 
		}
		public override void GetVoid(Player player)
		{
			voidMana = 5;
		}
		public override void UpdateInventory(Player player)
		{
			Lighting.AddLight(player.Center, 1f, 1f, 1f);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
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
