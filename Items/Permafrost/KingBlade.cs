using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.DataStructures;
using SOTS.Projectiles.Blades;
using SOTS.Items.Evil;

namespace SOTS.Items.Permafrost
{
	public class KingBlade : VoidItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SafeSetDefaults()
		{
            Item.damage = 61;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 74;
            Item.height = 74;  
            Item.useTime = 14; 
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;		
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(0, 25, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<KingSlash>(); 
            Item.shootSpeed = 16f;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 8 * SOTSUtils.SignNoZero(velocity.X) * player.gravDir, Main.rand.NextFloat(0.98f, 1.02f));
			return false;
		}
        public override int GetVoid(Player player)
        {
            return 24;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<AbsoluteBar>(20).AddIngredient<AbyssalFury>(1).AddIngredient<ShatterBlade>(1).AddIngredient<ToothAche>(1).AddIngredient(ItemID.BrokenHeroSword, 1).AddTile(TileID.MythrilAnvil).Register();
            CreateRecipe(1).AddIngredient<AbsoluteBar>(20).AddIngredient<AbyssalFury>(1).AddIngredient<ShatterBlade>(1).AddIngredient<Vertebraeker>(1).AddIngredient(ItemID.BrokenHeroSword, 1).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}
