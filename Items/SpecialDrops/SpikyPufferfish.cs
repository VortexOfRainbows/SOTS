using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class SpikyPufferfish : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spiky Pufferfish");
			Tooltip.SetDefault("Can be used as a bullet");
		}
		public override void SetDefaults()
		{
			item.damage = 1;
			item.ranged = true;
			item.width = 20;
			item.height = 20;
			item.maxStack = 999;
			item.consumable = true;
			item.knockBack = 1f;
			item.value = 125;
			item.rare = ItemRarityID.Green;
			item.shoot = mod.ProjectileType("SpikyPufferfish");
			item.shootSpeed = 1.24f;
			item.ammo = AmmoID.Bullet;   
            item.UseSound = SoundID.Item23;
		}
		public override void CaughtFishStack(ref int stack)
		{
			stack = Main.rand.Next(99, 297);
		}
	}
}