using SOTS.Items.Pyramid;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Temple
{
	public class SolarBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Bullet");
			Tooltip.SetDefault("50% of damage done ignores defense completely\nIncreases in speed after bouncing off walls");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
			ItemID.Sets.AnimatesAsSoul[Type] = true;
			this.SetResearchCost(99);
		}
		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 22;
			Item.height = 40;
			Item.maxStack = 999;
			Item.consumable = true;           
			Item.knockBack = 1f;
			Item.value = Item.buyPrice(0, 0, 2, 0);
			Item.rare = ItemRarityID.Lime;
			Item.shoot = ModContent.ProjectileType<Projectiles.Temple.SolarBullet>(); 
			Item.shootSpeed = 3f;             
			Item.ammo = AmmoID.Bullet;   
            Item.UseSound = SoundID.Item23;
		}
		public override void AddRecipes()
		{
			CreateRecipe(200).AddIngredient(ItemID.EmptyBullet, 200).AddIngredient(ItemID.LunarTabletFragment, 1).AddTile(TileID.WorkBenches).Register();
		}
	}
}