using Microsoft.Xna.Framework;
using SOTS.Items.Pyramid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Conduit
{
	public class SkipBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(999);
		}
		public override void SetDefaults()
		{
			Item.damage = 8;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 30;
			Item.maxStack = 999;
			Item.consumable = true;           
			Item.knockBack = 1f;
			Item.value = Item.sellPrice(copper: 7);
			Item.rare = ItemRarityID.Green;
			Item.shoot = ModContent.ProjectileType<Projectiles.Anomaly.SkipBullet>(); 
			Item.shootSpeed = 4f;             
			Item.ammo = AmmoID.Bullet;   
            Item.UseSound = SoundID.Item23;
		}
		public override void AddRecipes()
		{
			CreateRecipe(200).AddIngredient(ModContent.ItemType<SkipSoul>(), 5).AddIngredient(ModContent.ItemType<SkipShard>(), 1).AddTile(TileID.Anvils).Register();
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * ((255 - Item.alpha) / 255f);
		}
	}
	public class SkipArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(999);
		}
		public override void SetDefaults()
		{
			Item.damage = 6;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 40;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.knockBack = 1f;
			Item.value = Item.sellPrice(copper: 7);
			Item.rare = ItemRarityID.Green;
			Item.shoot = ModContent.ProjectileType<Projectiles.Anomaly.SkipArrow>();
			Item.shootSpeed = 5f;
			Item.ammo = AmmoID.Arrow;
			Item.UseSound = SoundID.Item23;
		}
		public override void AddRecipes()
		{
			CreateRecipe(200).AddIngredient(ModContent.ItemType<SkipSoul>(), 5).AddIngredient(ModContent.ItemType<SkipShard>(), 1).AddTile(TileID.Anvils).Register();
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * ((255 - Item.alpha) / 255f);
		}
	}
}