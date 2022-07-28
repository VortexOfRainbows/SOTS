using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using SOTS.Items.Fragments;
using SOTS.Items.Otherworld.Furniture;
using SOTS.Projectiles.Minions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace SOTS.Items.Otherworld.FromChests
{
	[AutoloadEquip(EquipType.Back)]
	public class PlatformGenerator : ModItem	
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platform Generator");
			Tooltip.SetDefault("10% increased minion damage\nGenerates 2 platforms to the left and right of you\nYou can right click to drag the platforms, but they will always remain symmetrical\nSentries can be summoned on top of the platforms\n'Bring your sentries with you!'");
			this.SetResearchCost(1);
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/PlatformGeneratorBase").Value;
			Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/PlatformGeneratorBase").Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture2, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, lightColor * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/PlatformGeneratorOutline").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/PlatformGeneratorFill").Value;
			Color color = new Color(110, 110, 110, 0);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, color * 0.5f, 0f, origin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2(position.X + x, position.Y + y), null, color * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/PlatformGeneratorOutline").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/PlatformGeneratorFill").Value;
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color * 0.5f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 34;     
            Item.height = 34;
			Item.knockBack = 1f;
            Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Summon) += 0.1f;
			PlatformPlayer modPlayer = player.GetModPlayer<PlatformPlayer>();
			modPlayer.platformPairs++;
			if(hideVisual)
				modPlayer.hideChains = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DissolvingAether>(), 1).AddIngredient(ModContent.ItemType<HardlightAlloy>(), 8).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}
	public class PlatformPlayer : ModPlayer
	{
		public bool fortress = false;
		public bool hideChains = false;
		public int platformPairs = 0;
		public int[] platforms1 = { -1, -1, -1, -1, -1, -1, -1, -1 };
		public int[] platforms2 = { -1, -1, -1, -1, -1, -1, -1, -1 };
		public override void ResetEffects()
		{
			int type = ModContent.ProjectileType<HoloPlatform>();
			if(Main.myPlayer == Player.whoAmI)
			{
				for (int i = 0; i < platformPairs; i++)
				{
					if (platforms1[i] == -1)
					{
						platforms1[i] = Projectile.NewProjectile(Player.GetSource_Misc("SOTS:Pets"), Player.Center.X - 160 - (80 * (i + 1)), Player.position.Y, 0, 0, type, 0, 0, Player.whoAmI, i, -1);
					}
					if (!Main.projectile[platforms1[i]].active || Main.projectile[platforms1[i]].type != type || Main.projectile[platforms1[i]].ai[0] != i || Main.projectile[platforms1[i]].ai[1] != -1)
					{
						platforms1[i] = Projectile.NewProjectile(Player.GetSource_Misc("SOTS:Pets"), Player.Center.X - 160 - (80 * (i + 1)), Player.position.Y, 0, 0, type, 0, 0, Player.whoAmI, i, -1);
					}
					Main.projectile[platforms1[i]].timeLeft = 6;

					if (platforms2[i] == -1)
					{
						platforms2[i] = Projectile.NewProjectile(Player.GetSource_Misc("SOTS:Pets"), Player.Center.X + 160 + (80 * (i + 1)), Player.position.Y, 0, 0, type, 0, 0, Player.whoAmI, i, 1);
					}
					if (!Main.projectile[platforms2[i]].active || Main.projectile[platforms2[i]].type != type || Main.projectile[platforms2[i]].ai[0] != i || Main.projectile[platforms2[i]].ai[1] != 1)
					{
						platforms2[i] = Projectile.NewProjectile(Player.GetSource_Misc("SOTS:Pets"), Player.Center.X + 160 + (80 * (i + 1)), Player.position.Y, 0, 0, type, 0, 0, Player.whoAmI, i, 1);
					}
					Main.projectile[platforms2[i]].timeLeft = 6;
				}
			}
			platformPairs = 0;
			hideChains = false;
			fortress = false;
		}
	}
}