using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Crushers;
using SOTS.Items.Fragments;

namespace SOTS.Items.Crushers
{
	public class CrabClaw : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crab Claw");
			Tooltip.SetDefault("Charge to increase damage up to 800%\nTakes 4 seconds to reach max charge");
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 18;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 32;
            Item.height = 26;  
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 10f;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item22;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CrabCrusher>(); 
            Item.shootSpeed = 18f;
			Item.channel = true;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Topaz, 15).AddIngredient(ModContent.ItemType<FragmentOfTide>(), 4).AddTile(TileID.Anvils).Register();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return player.ownedProjectileCounts[type] <= 0;
		}
		public override int GetVoid(Player player)
		{
			return 3;
		}
	}
}