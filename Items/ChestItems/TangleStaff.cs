using Microsoft.Xna.Framework;
using SOTS.Projectiles.BiomeChest;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
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
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.damage = 80;
			Item.DamageType = DamageClass.Magic;
			Item.width = 64;
			Item.height = 60;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.Shoot;
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
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			position += velocity * 3f;
        }
    }
}