using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Projectiles.BiomeChest;

namespace SOTS.Items.ChestItems
{
	public class Blongus : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 7;
			Item.DamageType = DamageClass.Magic;
			Item.width = 54;
			Item.height = 64;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 2.5f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
			Item.autoReuse = Item.noMelee = true;       
			Item.shoot = ModContent.ProjectileType<BlongusProj>(); 
            Item.shootSpeed = 5f;
			Item.mana = 20;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for(int i = 0; i < 3; i++)
				Projectile.NewProjectile(source, position, velocity * Main.rand.NextFloat(0.9f, 1.1f) + Main.rand.NextVector2Circular(2, 2), type, damage, knockback, player.whoAmI, -1);
			return false; 
		}
	}
}