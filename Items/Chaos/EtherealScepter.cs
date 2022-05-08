using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Buffs.MinionBuffs;
using SOTS.Projectiles.Minions;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chaos
{
	public class EtherealScepter : ModItem
	{
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Chaos/EtherealScepterEffect");
			Vector2 drawOrigin = new Vector2(Main.itemTexture[Item.type].Width * 0.5f, Item.height * 0.5f);
			Color color = Color.White;
			for (int k = 0; k < 6; k++)
			{
				Vector2 circular = new Vector2(4 * scale, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount * 2));
				color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60));
				color.A = 0;
				Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2) + circular, null, color * 0.3f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Chaos/EtherealScepterEffect");
			Vector2 drawOrigin = new Vector2(Main.itemTexture[Item.type].Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			texture = mod.GetTexture("Items/Chaos/EtherealScepterGlow");
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Ethereal Scepter");
			Tooltip.SetDefault("Summons an Ethereal Flame to fight for you\nEthereal Flames attack enemies by rapidly dashing through them");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults() 
		{
			Item.damage = 75;
			Item.knockBack = 4.5f;
			Item.width = 66;
			Item.height = 74;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.SwingThrow;
			Item.value = Item.sellPrice(0, 12, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.summon = true;
			Item.buffType = ModContent.BuffType<Ethereal>();
			Item.shoot = ModContent.ProjectileType<EtherealFlame>();
			Item.mana = 16;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Chaos/EtherealScepterGlow");
			}
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
			recipe.AddIngredient(ModContent.ItemType<PhaseBar>(), 18);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}