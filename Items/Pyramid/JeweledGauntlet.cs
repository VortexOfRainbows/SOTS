using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Projectiles.Pyramid;

namespace SOTS.Items.Pyramid
{
	public class JeweledGauntlet : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Gauntlet");
			Tooltip.SetDefault("");
		}
		public override void SafeSetDefaults()
		{
			Item.DamageType = DamageClass.Melee;
			Item.width = 24;
			Item.height = 30;
			Item.damage = 54;
            Item.useTime = 12;
            Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 7.0f;
            Item.value = Item.sellPrice(0, 4, 50, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item19;
            Item.autoReuse = true;       
			Item.shoot = ModContent.ProjectileType<PhantomFist>(); 
            Item.shootSpeed = 9f;
			Item.consumable = false;
			Item.noMelee = true;
			Item.noUseGraphic = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 1;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(12)); // This defines the projectiles random spread . 30 degree spread.
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
        public override int GetVoid(Player player)
        {
			return 4;
        }
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<SandstoneWarhammer>(), 1).AddIngredient(ModContent.ItemType<SpiritGlove>(), 1).AddIngredient(ModContent.ItemType<CursedMatter>(), 4).AddIngredient(ItemID.SoulofNight, 15).AddIngredient(ItemID.Ruby, 1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}