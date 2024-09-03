using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using Terraria.DataStructures;

namespace SOTS.Items.ChestItems
{
	public class SandstormPouch : VoidItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 28;
            Item.height = 40;
            Item.useTime = 50; 
            Item.useAnimation = 50;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 2f;  
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Orange;
            //Item.UseSound = SoundID.Item61;
            Item.shoot = ModContent.ProjectileType<Projectiles.BiomeChest.SandstormPouch>(); 
            Item.shootSpeed = 9.5f;
            Item.noMelee = Item.channel = Item.noUseGraphic = true;
        }
		public override int GetVoid(Player player)
		{
			return 10;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, -2);
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, -2f);
			return false; 
		}
        public override bool BeforeDrainVoid(Player player)
        {
            return false;
        }
    }
}
