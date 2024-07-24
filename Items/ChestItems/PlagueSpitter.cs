using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.ChestItems
{
	public class PlagueSpitter : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 15;
			Item.DamageType = DamageClass.Magic;
			Item.width = 26;
			Item.height = 36;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4f;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = null;
			Item.autoReuse = false;
			Item.channel = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.BiomeChest.PlagueSpitter>();
			Item.shootSpeed = 8f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.mana = 5;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.BiomeChest.PlagueSpitter>(), damage, knockback, player.whoAmI, -1, -1);
			return false;
		}
        public override void OnConsumeMana(Player player, int manaConsumed)
        {
			player.statMana += manaConsumed;
        }
    }
}