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
            Item.width = 66;    
            Item.height = 62;
			Item.useAnimation = 36;
			Item.useTime = 36;
			Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 5.25f;
			Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ModContent.RarityType<AnomalyRarity>();
            Item.UseSound = null;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Bubble>(); 
            Item.shootSpeed = 0f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.useTurn = true;
		}
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, Main.MouseWorld.X, Main.MouseWorld.Y);
			return false;
        }
        public override bool BeforeDrainVoid(Player player)
		{
			return true;
		}
		
		public override float UseTimeMultiplier(Player player)
		{
			return 1f;
		}
	}
}
