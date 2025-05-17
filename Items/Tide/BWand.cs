using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.DataStructures;
using SOTS.Projectiles.Tide;
using SOTS.Buffs;

namespace SOTS.Items.Tide
{
	public class BWand : VoidItem
	{
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
        {
            Item.width = 48;    
            Item.height = 48;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 5.25f;
			Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ModContent.RarityType<AnomalyRarity>();
            Item.UseSound = SoundID.Item85;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<Bubble>(); 
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 8;
		}
        public override bool AltFunctionUse(Player player)
        {
            return false;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			velocity += player.velocity;
			if (velocity.Y < 0)
				velocity *= 0.5f;
			Projectile.NewProjectile(source, position + new Vector2(0, 10), velocity, type, damage, knockback, player.whoAmI, 0, velocity.X > 0 ? 180 : 0);
			return false;
        }
        public override int GetVoid(Player player)
        {
            return 5;
        }
    }
}
