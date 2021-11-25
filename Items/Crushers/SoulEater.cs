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
            item.damage = 17;
            item.melee = true;  
            item.width = 40;
            item.height = 40;  
            item.useTime = 24; 
            item.useAnimation = 24;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.knockBack = 7.5f;
            item.value = Item.sellPrice(0, 0, 33, 0);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item22;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<SoulEaterCrusher>(); 
            item.shootSpeed = 18f;
			item.channel = true;
            item.noUseGraphic = true; 
            item.noMelee = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return player.ownedProjectileCounts[type] <= 0;
		}
		public override void GetVoid(Player player)
		{
			voidMana = 5;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DemoniteBar, 12);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}