using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Lightning;
using SOTS.Items.Fragments;

namespace SOTS.Items.Tide
{
	public class PinkJellyfishStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pink Jellyfish Staff");
			Tooltip.SetDefault("Fires pink lightning which chains off enemies for 60% damage");
		}
		public override void SetDefaults()
		{
            Item.damage = 14;
            Item.DamageType = DamageClass.Magic; 
            Item.width = 34;    
            Item.height = 32; 
            Item.useTime = 20; 
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 3;
			Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item93;
            Item.noMelee = true; 
            Item.autoReuse = false;
            Item.shootSpeed = 5.5f; 
			Item.shoot = ModContent.ProjectileType<PinkLightning>();
			Item.staff[Item.type] = true; 
			Item.mana = 13;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Amethyst, 15).AddIngredient(ModContent.ItemType<FragmentOfTide>(), 4).AddTile(TileID.Anvils).Register();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int i = 0; i < 2; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX * .66f, speedY * .66f).RotatedByRandom(MathHelper.ToRadians(25));
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI, 2.5f);
			}
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 2.5f);
            return false;
		}
	}
}
