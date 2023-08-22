using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Planetarium;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Planetarium.FromChests
{
	public class CodeCorrupter : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.noMelee = true;
			Item.damage = 36;  
            Item.DamageType = DamageClass.Magic;    
            Item.width = 46;  
            Item.height = 30;   
            Item.useTime = 24;  
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 6f;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item96;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CodeBurst>(); 
            Item.shootSpeed = 16.5f;
			Item.reuseDelay = 8;
			Item.mana = 14;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Planetarium/FromChests/CodeCorrupterGlow").Value;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -1;
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Planetarium/FromChests/CodeCorrupterGlow").Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			position += velocity * 3;
			Projectile.NewProjectile(source, position, velocity * 0.75f, ModContent.ProjectileType<CodeVolley>(), damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			return false;
		}
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<HardlightAlloy>(16).AddTile(ModContent.TileType<Furniture.HardlightFabricatorTile>()).Register();
		}
	}
}
