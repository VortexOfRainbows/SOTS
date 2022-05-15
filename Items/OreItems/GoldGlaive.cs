using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Ores;

namespace SOTS.Items.OreItems
{
	public class GoldGlaive : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Glaive");
			Tooltip.SetDefault("Fires 3 bolts at your cursor, each dealing 75% damage");
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
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			int numberProjectiles = 3;
			for (int i = 0; i < numberProjectiles; i++)
			{
			    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(160 + (i * 20)));
			    Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, Mod.Find<ModProjectile>("GoldBolt").Type, (int)(damage * 0.75f) + 1, knockBack, player.whoAmI);
			}
			return true; 
		}
	}
}
	
