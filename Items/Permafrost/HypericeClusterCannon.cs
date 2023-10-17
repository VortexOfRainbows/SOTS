using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Permafrost;
using Terraria.DataStructures;

namespace SOTS.Items.Permafrost
{
	public class HypericeClusterCannon : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 38;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 64;
            Item.height = 24;
            Item.useTime = 26; 
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
			Item.knockBack = 1f;  
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<HypericeRocket>(); 
            Item.shootSpeed = 8;
			Item.useAmmo = ItemID.Snowball;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, -1);
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<HypericeRocket>(), damage, knockback, player.whoAmI);
			return false; 
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<AbsoluteBar>(16).AddIngredient<CryoCannon>(1).AddIngredient(ItemID.SnowballCannon, 1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
