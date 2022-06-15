using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using Terraria.DataStructures;
using SOTS.Projectiles.Permafrost;
using SOTS.Items.Fragments;

namespace SOTS.Items.Permafrost
{
	public class IcicleImpale : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Icicle Impale");
			Tooltip.SetDefault("Launches large icicles\nRegenerates void upon hit");
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 75;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 78;
            Item.height = 30;
            Item.useTime = 17; 
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
			Item.knockBack = 1f;  
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<HypericeRocket>(); 
            Item.shootSpeed = 19f;
		}
		public override int GetVoid(Player player)
		{
			return  7;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, -1);
		}
		int shot = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			shot++;
			if(shot % 3 == 0)
			{
				for(int i = 0; i < 2; i ++)
				{
					Vector2 angle = velocity.RotatedBy(MathHelper.ToRadians(2.5f - (5 * i)));
					Projectile.NewProjectile(source, position.X, position.Y, angle.X, angle.Y, ModContent.ProjectileType<HypericeRocket>(), damage, knockback, player.whoAmI);
				}
			}
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X * 1.6f, velocity.Y * 1.6f, ModContent.ProjectileType<IceImpale>(), damage, knockback, player.whoAmI);
			return false; 
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<HypericeClusterCannon>(1).AddIngredient(ModContent.ItemType<HelicopterParts>(), 1).AddIngredient<DissolvingAurora>(1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
