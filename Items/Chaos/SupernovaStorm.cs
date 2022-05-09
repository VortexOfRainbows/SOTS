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
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Chaos/SupernovaStormGlow").Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Color color = Color.White;
			for (int k = 0; k < 6; k++)
			{
				Vector2 circular = new Vector2(3 * scale, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount * 2));
				color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60)) * 0.33f;
				color.A = 0;
				Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2) + circular, null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Chaos/SupernovaStormGlow").Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Supernova Storm");
			Tooltip.SetDefault("Calls down a Supernova Beam from the sky\nCauses enemies to rupture into homing bolts for 3x140% damage");
		}
		public override void SetDefaults()
		{
            Item.damage = 60;
			Item.DamageType = DamageClass.Magic;
            Item.width = 40;    
            Item.height = 52; 
            Item.useTime = 14; 
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item92;
            Item.noMelee = true; 
            Item.autoReuse = true;
            Item.shootSpeed = 3f; 
			Item.shoot = ModContent.ProjectileType<SupernovaLaser>();
			Item.mana = 7;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Chaos/SupernovaStormGlow").Value;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PhaseBar>(), 18);
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
