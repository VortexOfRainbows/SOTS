using Microsoft.Xna.Framework;
using SOTS.Projectiles.BiomeChest;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.ChestItems
{
	public class TangleStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tangle Staff");
			Tooltip.SetDefault("Fire a flower that ensnares enemies\nEnsnared enemies are slowed and have their life drained\nIncreases life regeneration for each ensnared enemy");
		}
		public override void SetDefaults()
		{
			Item.damage = 80;
			Item.magic = true;
			Item.width = 64;
			Item.height = 60;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.HoldingOut;
			Item.knockBack = 2.5f;
			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item42;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<FloweringBud>(); 
            Item.shootSpeed = 20f;
			Item.noMelee = true;
			Item.staff[Item.type] = true; //this makes the useStyle animate as a staff
			Item.mana = 22;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 1);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			position += new Vector2(speedX, speedY) * 3f;
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
    }
}