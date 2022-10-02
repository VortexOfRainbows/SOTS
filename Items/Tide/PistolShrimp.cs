using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Lightning;
using SOTS.Items.Fragments;
using Terraria.DataStructures;

namespace SOTS.Items.Tide
{
	public class PistolShrimp : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pistol Shrimp");
			Tooltip.SetDefault("Fires hot, shrimpy plasma that homes on close enemies\n'Teamwork makes the dream work'");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 11;
            Item.DamageType = DamageClass.Magic; 
            Item.width = 34;    
            Item.height = 32; 
            Item.useTime = 8; 
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 3;
			Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item11;
            Item.noMelee = true; 
            Item.autoReuse = true;
            Item.shootSpeed = 3.5f; 
			Item.shoot = ModContent.ProjectileType<Projectiles.Tide.ShrimpLaser>();
			Item.mana = 4;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0); 
        }
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Shrimp, 10).AddIngredient(ItemID.Topaz, 5).AddIngredient(ModContent.ItemType<FragmentOfTide>(), 4).AddTile(TileID.Anvils).Register();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position + velocity.SafeNormalize(Vector2.Zero) * 24 + new Vector2(0, -10 * player.direction).RotatedBy(velocity.ToRotation()), velocity, type, damage, knockback, player.whoAmI);
            return false;
		}
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-18, 18)));
        }
    }
}
