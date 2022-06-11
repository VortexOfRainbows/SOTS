using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Pyramid
{
	public class SnakeBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snake Bow");
			Tooltip.SetDefault("Launches snakes that latch on to enemies");
		}
		public override void SetDefaults()
		{
			Item.damage = 19;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 30;
			Item.height = 48;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2.5f;
			Item.value = Item.sellPrice(0, 0, 75, 0);
			Item.rare = ItemRarityID.Orange;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = false;            
			Item.shoot = 1; 
            Item.shootSpeed = 20;
			Item.useAmmo = AmmoID.Arrow;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<Snakeskin>(18).AddIngredient(ItemID.Leather, 4).AddIngredient(ItemID.WoodenBow, 1).AddTile(TileID.Anvils).Register();
		}
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			type = ModContent.ProjectileType<Projectiles.Pyramid.Snake>();
        }
	}
}