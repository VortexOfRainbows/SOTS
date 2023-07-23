using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using SOTS.Void;
using Terraria.ModLoader;
using Terraria.DataStructures;
using SOTS.Projectiles.Pyramid;

namespace SOTS.Items.Pyramid
{
	public class SpiritTracer : VoidItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 33;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 30;
			Item.height = 68;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(0, 7, 25, 0);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;            
			Item.shoot = ProjectileID.WoodenArrowFriendly; 
            Item.shootSpeed = 16.5f;
			Item.useAmmo = AmmoID.Arrow;
			Item.noMelee = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<CursedMatter>(10).AddIngredient(ItemID.Amethyst, 8).AddTile(TileID.Anvils).Register();
		}
		public override int GetVoid(Player player)
		{
			return  6;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 0);
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<TracerArrow>(), damage, knockback, player.whoAmI, 0, type);
			return false; 
		}
	}
}