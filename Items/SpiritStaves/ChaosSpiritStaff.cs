using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Buffs.MinionBuffs;
using SOTS.Projectiles.Minions;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpiritStaves
{
	public class ChaosSpiritStaff : VoidItem
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Summons a Chaos Spirit to fight for you\nLocks onto one enemy at a time");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}
		public override void SafeSetDefaults() 
		{
			Item.damage = 60;
			Item.knockBack = 4f;
			Item.width = 38;
			Item.height = 38;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.SwingThrow;
			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.summon = true;
			Item.buffType = ModContent.BuffType<ChaosSpiritAid>();
			Item.shoot = ModContent.ProjectileType<ChaosSpirit>();
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
			recipe.AddIngredient(ModContent.ItemType<Fragments.DissolvingBrilliance>(), 1);
			recipe.AddIngredient(ModContent.ItemType<Chaos.PhaseBar>(), 10);
			recipe.AddIngredient(ItemID.SoulofLight, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}