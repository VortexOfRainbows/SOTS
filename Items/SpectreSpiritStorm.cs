using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Items.Pyramid;
using SOTS.Items.Fragments;
using Terraria.DataStructures;
using SOTS.Projectiles.Pyramid;

namespace SOTS.Items
{
	public class SpectreSpiritStorm : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectre Spirit Storm");
			Tooltip.SetDefault("Fires phantom arrows\nCan hit up to 4 enemies at a time");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 70;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 36;
			Item.height = 74;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;            
			Item.shoot = 10; 
            Item.shootSpeed = 21.5f;
			Item.useAmmo = AmmoID.Arrow;
			Item.noMelee = true;
		}
		public override int GetVoid(Player player)
		{
			return  6;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 0);
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<StormArrow>(), damage, knockback, player.whoAmI, 0, type);
			return false; 
		}
		public override void AddRecipes()	
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<SpiritTracer>(), 1).AddIngredient(ItemID.DaedalusStormbow, 1).AddIngredient(ItemID.SpectreBar, 10).AddIngredient(ModContent.ItemType<DissolvingDeluge>(), 1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}