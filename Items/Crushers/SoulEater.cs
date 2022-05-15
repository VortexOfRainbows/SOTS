using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Crushers;
using SOTS.Items.Fragments;

namespace SOTS.Items.Crushers
{
	public class SoulEater : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Eater");
			Tooltip.SetDefault("Charge to increase damage up to 1000%\nTakes 2.5 seconds to reach max charge\nKilled enemies regenerate mana and void");
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 17;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 40;
            Item.height = 40;  
            Item.useTime = 24; 
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 7.5f;
            Item.value = Item.sellPrice(0, 0, 33, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item22;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SoulEaterCrusher>(); 
            Item.shootSpeed = 18f;
			Item.channel = true;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return player.ownedProjectileCounts[type] <= 0;
		}
		public override int GetVoid(Player player)
		{
			return 5;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.DemoniteBar, 12).AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 2).AddTile(TileID.Anvils).Register();
		}
	}
}