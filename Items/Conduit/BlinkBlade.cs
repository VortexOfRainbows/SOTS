using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.DataStructures;
using SOTS.Projectiles.Blades;

namespace SOTS.Items.Conduit
{
	public class BlinkBlade : VoidItem
	{
		public override void SetStaticDefaults()
		{
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 15;
            Item.DamageType = DamageClass.Melee;  
            Item.width = Item.height = 42;  
            Item.useTime = 12; 
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;		
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ModContent.RarityType<AnomalyRarity>();
            Item.UseSound = null;
            Item.shoot = ModContent.ProjectileType<BlinkBladeSlash>(); 
            Item.shootSpeed = 16f;
            Item.noUseGraphic = Item.autoReuse = Item.noMelee = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position, velocity, type, damage * 3, knockback * 2.5f, player.whoAmI, -2 * SOTSUtils.SignNoZero(velocity.X) * player.gravDir, 0.25f);
            }
            else
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 1 * SOTSUtils.SignNoZero(velocity.X) * player.gravDir, Main.rand.NextFloat(0.9f, 1.1f));
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<SkipSoul>(20).AddIngredient<SkipShard>(5).AddIngredient(ItemID.GoldShortsword).AddTile(TileID.Anvils).Register();
            CreateRecipe(1).AddIngredient<SkipSoul>(20).AddIngredient<SkipShard>(5).AddIngredient(ItemID.PlatinumShortsword).AddTile(TileID.Anvils).Register();
        }
        public override int GetVoid(Player player)
        {
            return 3 * (player.altFunctionUse == 2 ? 3 : 1);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}
