using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SOTS.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Projectiles.Inferno;
using SOTS.Items.Fragments;

namespace SOTS.Items.Inferno
{
	public class PlasmaAccelerator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Accelerator");
			Tooltip.SetDefault("Fires supercharged plasma arrows");
		}
		public override void SetDefaults()
		{
            Item.damage = 44; 
            Item.DamageType = DamageClass.Ranged;  
            Item.width = 28;   
            Item.height = 64; 
            Item.useTime = 10; 
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(0, 7, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item91;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PlasmaphobiaBolt>(); 
            Item.shootSpeed = 12.5f;
			Item.useAmmo = ItemID.WoodenArrow;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position += new Vector2(speedX, speedY).SafeNormalize(Vector2.Zero) * 24;
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<PlasmaphobiaBolt>(), damage, knockBack, player.whoAmI);
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Sharanga>(), 1);
			recipe.AddIngredient(ModContent.ItemType<DissolvingAether>(), 1);
			recipe.AddIngredient(ModContent.ItemType<DissolvingNether>(), 1);
			recipe.AddIngredient(ItemID.Ectoplasm, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
