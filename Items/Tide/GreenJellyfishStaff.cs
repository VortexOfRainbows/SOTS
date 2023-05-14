using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Projectiles.Lightning;
using Terraria.DataStructures;

namespace SOTS.Items.Tide
{
	public class GreenJellyfishStaff : VoidItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
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
			if (Item.favorited)
				Lighting.AddLight(player.Center, 0.9f, 1.1f, 0.9f);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PurpleJellyfishStaff>(), 1).AddIngredient(ItemID.SoulofLight, 15).AddIngredient(ItemID.SoulofSight, 15).AddTile(TileID.MythrilAnvil).Register();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(180)); 
            Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			
            Vector2 perturbedSpeed2 = perturbedSpeed.RotatedBy(MathHelper.ToRadians(180)); 
            Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed2.X, perturbedSpeed2.Y, type, damage, knockback, player.whoAmI);
            return false;
		}
	}
}
