using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.Buffs.MinionBuffs;
using SOTS.Projectiles.Minions;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpiritStaves
{
	public class EarthenSpiritStaff : VoidItem
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Summons an Earthen Spirit to fight for you");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}
		public override void SafeSetDefaults() 
		{
			Item.damage = 14;
			Item.knockBack = 4f;
			Item.width = 38;
			Item.height = 36;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.SwingThrow;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.summon = true;
			Item.buffType = ModContent.BuffType<EarthenSpiritAid>();
			Item.shoot = ModContent.ProjectileType<EarthenSpirit>();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) 
		{
			player.AddBuff(Item.buffType, 2);
			position = Main.MouseWorld;
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Fragments.DissolvingEarth>(), 1);
			recipe.AddIngredient(ItemID.MeteoriteBar, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}