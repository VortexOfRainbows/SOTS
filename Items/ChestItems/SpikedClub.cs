using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles;
using Terraria.DataStructures;

namespace SOTS.Items.ChestItems
{
	public class SpikedClub : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spiked Club");
			Tooltip.SetDefault("Lays down spike traps\nLays down more traps when wearing climbing related accessories");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = DamageClass.Melee;
			Item.width = 52;
			Item.height = 52;
			Item.useTime = 38;
			Item.useAnimation = 38;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<SpikeTrap>(); 
            Item.shootSpeed = 3.5f;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			for(int i = 0; i < player.spikedBoots; i++)
			{
				float speedMult = 1.25f + i * 0.25f;
				int totalDamage = damage - 2 - i; //slight damage fall of for farther spikes (bigger quantities)
				if (totalDamage < damage / 2)
					totalDamage = damage / 2;
				Projectile.NewProjectile(source, position, velocity * speedMult, type, totalDamage, knockback, player.whoAmI);
			}
			return true; 
		}
	}
}