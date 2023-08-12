using Microsoft.Xna.Framework;
using SOTS.Projectiles.Evil;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	public class AncientSteelSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
        }
		public override void SetDefaults()
		{
            Item.damage = 16; 
            Item.DamageType = DamageClass.Melee;  
            Item.width = 60;   
            Item.height = 60;
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;    
            Item.knockBack = 5.4f;
            Item.value = Item.sellPrice(0, 0, 60, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.crit = 4;
		}
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(player.whoAmI == Main.myPlayer)
            {
                float ToPlayer = (player.Center - target.Center).ToRotation();
                Vector2 position = target.Hitbox.TopLeft() + EdgeOfRect(target.Hitbox, ToPlayer);
                Projectile.NewProjectile(player.GetSource_OnHit(target), position, new Vector2(player.direction, 0), ModContent.ProjectileType<BloodSpark>(), (int)(damage * 0.8f), knockBack, Main.myPlayer, (ToPlayer + MathHelper.PiOver2) * 180f / MathHelper.Pi, crit ? -1 : 0) ;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<AncientSteelBar>(), 18).AddTile(TileID.Anvils).Register();
        }
        public Vector2 EdgeOfRect(Rectangle rect, float rad)
        {
            float theta = MathHelper.WrapAngle(rad);
            float rectAtan = (float)Math.Atan2(rect.Width, rect.Height);
            float tanTheta = (float)Math.Tan(theta);
            Vector2 edgePoint = new Vector2(rect.Width / 2, rect.Height / 2);
            int xFactor = 1;
            int yFactor = -1;
            bool flip = false; 
            if ((theta > -rectAtan) && (theta <= rectAtan))
            {
                flip = true;
                yFactor = 1;
            }
            else if ((theta > rectAtan) && (theta <= (Math.PI - rectAtan)))
            {
                yFactor = 1;
            }
            else if ((theta > (Math.PI - rectAtan)) || (theta <= -(Math.PI - rectAtan)))
            {
                flip = true;
                xFactor = -1;
            }
            else
            {
                xFactor = -1;
            }
            if (flip) 
            {
                edgePoint.X += xFactor* (rect.Width / 2);
                edgePoint.Y += yFactor* (rect.Width / 2) * tanTheta;
            } 
            else
            {
                edgePoint.X += xFactor * (rect.Height / (2 * tanTheta));
                edgePoint.Y += yFactor * (rect.Height / 2);
            }
            return edgePoint;
        }
    }
}
