using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{
	public class SupernovaHammer : ModItem
	{
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/SupernovaHammerGlow").Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Supernova Hammer");
			Tooltip.SetDefault("Critical strikes release 3 seekers that do 50% critical damage\nKilling enemies releases 3 seekers that do 70% damage\nEnemies killed by seekers release 2 more seekers, each doing 75% damage\nSeekers home onto enemies, but do not attack the enemies they are released from");
		}
		public override void SetDefaults()
		{
			Item.damage = 30;
			Item.DamageType = DamageClass.Melee;
			Item.width = 50;
			Item.height = 50;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 5f;
			Item.value = Item.sellPrice(0, 6, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.DD2_MonkStaffSwing;
			Item.autoReuse = true;
			Item.shoot = Mod.Find<ModProjectile>("SupernovaHammer").Type;
			Item.shootSpeed = 24f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Diamond, 5).AddIngredient(null, "StarlightAlloy", 15).AddTile(Mod.Find<ModTile>("HardlightFabricatorTile").Type).Register();
		}
	}
}