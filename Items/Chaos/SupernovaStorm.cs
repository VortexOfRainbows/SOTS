using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Items.Permafrost;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Chaos;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Chaos
{
	public class SupernovaStorm : ModItem
	{
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Chaos/SupernovaStormGlow");
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Color color = Color.White;
			for (int k = 0; k < 6; k++)
			{
				Vector2 circular = new Vector2(3 * scale, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount * 2));
				color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60)) * 0.33f;
				color.A = 0;
				Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2) + circular, null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Chaos/SupernovaStormGlow");
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Supernova Storm");
			Tooltip.SetDefault("Calls down a Supernova Beam from the sky\nCauses enemies to rupture into homing bolts for 3x140% damage");
		}
		public override void SetDefaults()
		{
            item.damage = 60;
			item.magic = true;
            item.width = 40;    
            item.height = 52; 
            item.useTime = 14; 
            item.useAnimation = 14;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.knockBack = 1.5f;
			item.value = Item.sellPrice(0, 12, 0, 0);
            item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item92;
            item.noMelee = true; 
            item.autoReuse = true;
            item.shootSpeed = 3f; 
			item.shoot = ModContent.ProjectileType<SupernovaLaser>();
			item.mana = 7;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Chaos/SupernovaStormGlow");
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PhaseBar>(), 15);
			recipe.AddIngredient(ModContent.ItemType<ShardstormSpell>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Vector2 cursorPos = Main.MouseWorld;
			Vector2 skyPosition = new Vector2(MathHelper.Lerp(position.X, cursorPos.X, 0.44f), position.Y - 960);
            int numberProjectiles = 1; 
            for (int i = 0; i < numberProjectiles; i++)
            {
				Vector2 rotateArea = new Vector2(160, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
				skyPosition += rotateArea;
				Vector2 speed = (cursorPos - skyPosition).SafeNormalize(Vector2.Zero) * new Vector2(speedX, speedY).Length();
                Projectile.NewProjectile(skyPosition, speed, type, damage, knockBack, player.whoAmI);
            }
            return false;
		}
	}
}
