using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Buffs.MinionBuffs;
using SOTS.Projectiles.Minions;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chaos
{
	public class EtherealScepter : ModItem
	{
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Chaos/EtherealScepterEffect").Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Color color = Color.White;
			for (int k = 0; k < 6; k++)
			{
				Vector2 circular = new Vector2(4 * scale, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount * 2));
				color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60));
				color.A = 0;
				Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)) + circular, null, color * 0.3f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Chaos/EtherealScepterEffect").Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			texture = Mod.Assets.Request<Texture2D>("Items/Chaos/EtherealScepterGlow").Value;
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Ethereal Scepter");
			Tooltip.SetDefault("Summons an Ethereal Flame to fight for you\nEthereal Flames attack enemies by rapidly dashing through them");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
			this.SetResearchCost(1);
		}

		public override void SetDefaults() 
		{
			Item.damage = 75;
			Item.knockBack = 4.5f;
			Item.width = 66;
			Item.height = 74;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(0, 12, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = ModContent.BuffType<Ethereal>();
			Item.shoot = ModContent.ProjectileType<EtherealFlame>();
			Item.mana = 16;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Chaos/EtherealScepterGlow").Value;
			}
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse != 2)
			{
				player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
				player.AddBuff(Item.buffType, 2);
			}
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PhaseBar>(), 18).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}