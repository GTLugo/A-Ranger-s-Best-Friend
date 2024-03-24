using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace BestFriend.Content.Items.Weapons;

public class StandardIssue : BaseGun {
  public override string Texture => BestFriend.ASSET_PATH + "Textures/Items/Weapons/Rifle";

  public override Dictionary<FireMode, GunStats> Stats { get; } = new() {
    {
      FireMode.Primary, new GunStats {
        damage = 16,
        crit = 5,
        knockback = 3,
        use_time = 26,
        fire_rate = 0
      }
    }
  };

  public override void SetStaticDefaults() {
    CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
  }

  public override void SetDefaults() {
    Item.width = 64;
    Item.height = 24;
    Item.scale = 0.8f;
    Item.rare = ItemRarityID.White;
    Item.value = Item.sellPrice(0, 0, 1, 69);

    base.SetDefaults();
  }

  public override Vector2? HoldoutOffset() {
    return new Vector2(-9, 2);
  }

  public override void AddRecipes() {
    CreateRecipe()
      .AddRecipeGroup(RecipeGroupID.IronBar, 8)
      .AddTile(TileID.Anvils)
      .Register();
  }
}