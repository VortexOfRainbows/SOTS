using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Earth;
using SOTS.Items.Permafrost;
using SOTS.Projectiles.Tide;

namespace SOTS.Items.Tide
{
	public class Atlantis : VoidItem
	{
        public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
        {
            Item.damage = 35;  
            Item.DamageType = DamageClass.Magic;  
            Item.width = 66;    
            Item.height = 62;
			Item.useAnimation = 36;
			Item.useTime = 36;
			Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 5.25f;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Lime;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Tide.AtlantisProj>(); 
            Item.shootSpeed = 16f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.crit = 16;
			Item.channel = true;
			Item.useTurn = true;
		}
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if(player.altFunctionUse == 0)
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, Main.MouseWorld.X, Main.MouseWorld.Y);
            }
			else if(player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position, velocity * 0.25f, ModContent.ProjectileType<AtlantisGlaive>(), damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override bool BeforeDrainMana(Player player)
		{
			return true;
		}
		public override int GetVoid(Player player)
		{
			return 15;
		}
		public override float UseTimeMultiplier(Player player)
		{
			return 1f;
		}
	}
}
