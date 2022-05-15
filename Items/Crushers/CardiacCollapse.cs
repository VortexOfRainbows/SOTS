using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Crushers;
using SOTS.Items.Fragments;

namespace SOTS.Items.Crushers
{
	public class CardiacCollapse : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cardiac Collapse");
			Tooltip.SetDefault("Charge to increase damage up to 800%\nTakes 3.5 seconds to reach max charge\nKilled enemies regenerate health");
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 23;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 48;
            Item.height = 48;  
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 8.5f;
            Item.value = Item.sellPrice(0, 0, 33, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item22;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CardiacCollapseCrusher>(); 
            Item.shootSpeed = 18f;
			Item.channel = true;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
			Item.crit = 6;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return player.ownedProjectileCounts[type] <= 0;
		}
		public override int GetVoid(Player player)
		{
			return 6;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.CrimtaneBar, 12).AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 2).AddTile(TileID.Anvils).Register();
		}
	}
}