using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
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
			item.damage = 80;
			item.magic = true;
			item.width = 64;
			item.height = 60;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 5;
			item.knockBack = 2.5f;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item42;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("FloweringBud"); 
            item.shootSpeed = 20f;
			item.noMelee = true;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff
			item.mana = 22;
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