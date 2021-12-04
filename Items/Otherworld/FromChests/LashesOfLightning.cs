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
            item.damage = 26;  
            item.magic = true; 
            item.width = 30;    
            item.height = 46;  
            item.useTime = 8;
            item.useAnimation = 24;
			item.reuseDelay = 18;
            item.useStyle = ItemUseStyleID.HoldingUp;   
            item.autoReuse = true; 
            item.knockBack = 3f;
			item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.LightPurple;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Otherworld/FromChests/LashesOfLightningGlow");
			}

			item.shoot = ModContent.ProjectileType<LightningLash>();
			item.shootSpeed = 15f;
			item.noMelee = true;
			item.mana = 20;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/LashesOfLightningGlow");
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
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
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			knockBack *= 3f;
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<StarlightAlloy>(), 16);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
