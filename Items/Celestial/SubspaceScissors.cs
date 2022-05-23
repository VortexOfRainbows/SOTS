using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using SOTS.Void;
using Terraria.ModLoader;
using SOTS.Projectiles.Crushers;
using Terraria.DataStructures;

namespace SOTS.Items.Celestial
{
	public class SubspaceScissors : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Scissors");
			Tooltip.SetDefault("'Assistance from purgatory'");
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 190;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 46;
            Item.height = 46;  
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(0, 15, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item22;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SubspaceCrusher>(); 
            Item.shootSpeed = 18f;
			Item.channel = true;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			return player.ownedProjectileCounts[type] <= 0; 
		}
		public override int GetVoid(Player player)
		{
			return 17;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<SanguiteBar>(), 15).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
