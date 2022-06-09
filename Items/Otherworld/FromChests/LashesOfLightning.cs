using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Otherworld;
using SOTS.Dusts;
using SOTS.Items.Otherworld.Furniture;

namespace SOTS.Items.Otherworld.FromChests
{
	public class LashesOfLightning : ModItem
	{ 	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lashes of Lightning");
			Tooltip.SetDefault("Unleash rapid strikes of lightning in a small area in front of you\nDeals slightly more damage and can hit through walls at the tip");
		}
		public override void SetDefaults()
		{
            Item.damage = 26;  
            Item.DamageType = DamageClass.Magic; 
            Item.width = 30;    
            Item.height = 46;  
            Item.useTime = 8;
            Item.useAnimation = 24;
			Item.reuseDelay = 18;
            Item.useStyle = ItemUseStyleID.HoldUp;   
            Item.autoReuse = true; 
            Item.knockBack = 9f;
			Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/LashesOfLightningGlow").Value;
			}

			Item.shoot = ModContent.ProjectileType<LightningLash>();
			Item.shootSpeed = 15f;
			Item.noMelee = true;
			Item.mana = 20;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/LashesOfLightningGlow").Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(10))
			{
				Dust dust = Dust.NewDustDirect(hitbox.Location.ToVector2() - new Vector2(5f), hitbox.Width, hitbox.Height, ModContent.DustType<CopyDust4>(), 0, -2, 200, new Color(), 1f);
				dust.velocity *= 0.4f;
				dust.color = new Color(100, 100, 255, 120);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.5f;
			}
		}
        /*public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			knockback *= 3;
        }*/
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<StarlightAlloy>(), 16).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}
}
