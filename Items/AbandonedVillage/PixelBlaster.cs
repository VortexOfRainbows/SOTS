using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Earth;
using Terraria.DataStructures;

namespace SOTS.Items.AbandonedVillage
{
	public class PixelBlaster : VoidItem
	{
		public Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/AbandonedVillage/PixelBlasterGlow").Value;
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(glowTexture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 1;
            Item.DamageType = DamageClass.Generic;
            Item.width = 44;
            Item.height = 22;
            Item.useTime = 20; 
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
			Item.knockBack = 0.1f;  
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = null;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<Projectiles.Earth.PixelBlaster>(); 
            Item.shootSpeed = 24f;
			Item.channel = true;
			Item.noUseGraphic = true;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = glowTexture;
			}
		}
        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
			crit *= 0.2f;
			crit += 1f;
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
			if (damage.Multiplicative < 1)
				damage *= 1 / damage.Multiplicative;
			if (damage.Additive < 1)
				damage += 1 - damage.Additive;
        }
        public override int GetVoid(Player player)
		{
			return 20;
		}
        public override bool BeforeDrainVoid(Player player)
        {
            return false;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			return player.ownedProjectileCounts[type] <= 0; 
		}
	}
}
