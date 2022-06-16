using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Ores;
using Terraria.DataStructures;

namespace SOTS.Items.OreItems
{
	public class GoldGlaive : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Glaive");
			Tooltip.SetDefault("Fires 3 bolts at your cursor, each dealing 75% damage");
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 28;
			Item.DamageType = DamageClass.Melee;
			Item.width = 42;
			Item.height = 40;
			Item.useTime = 31;
			Item.useAnimation = 31;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;       
			Item.shoot = ModContent.ProjectileType<GoldSpear>(); 
            Item.shootSpeed = 3.8f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
		public override int GetVoid(Player player)
		{
			return 5;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.GoldBar, 15).AddTile(TileID.Anvils).Register();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
			    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(160 + (i * 20)));
			    Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<GoldBolt>(), (int)(damage * 0.75f) + 1, knockback, player.whoAmI);
			}
			return true; 
		}
	}
}
	
