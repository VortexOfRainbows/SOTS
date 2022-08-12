using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Permafrost
{
	public class FrigidJavelin : VoidItem
	{
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(Main.LocalPlayer);
			Texture2D texture = ModContent.Request<Texture2D>(!modPlayer.frigidJavelinNoCost ? "SOTS/Items/Permafrost/FrigidJavelin" : "SOTS/Items/Permafrost/FrigidJavelinAlt").Value;
			Main.spriteBatch.Draw(texture, position, null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Javelin");
			Tooltip.SetDefault("Throw a powerful, fast traveling javelin that ricochets of surfaces");
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 42;  
            Item.DamageType = DamageClass.Magic;  
            Item.width = 46;    
            Item.height = 46;
			Item.useAnimation = 44;
			Item.useTime = 44;
			Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 5.25f;
            Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Permafrost.FrigidJavelin>(); 
            Item.shootSpeed = 12f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.crit = 16;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, modPlayer.frigidJavelinNoCost ? -1 : 0);
            return false;
        }
        public override bool BeforeDrainMana(Player player)
		{
			return true;
		}
		public override int GetVoid(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (modPlayer.frigidJavelinNoCost)
			{
				return 0;
			}
			return 5;
		}
		public override float UseTimeMultiplier(Player player)
		{
			return 1f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<FrigidBar>(12).AddTile(TileID.Anvils).Register();
		}
	}
}
