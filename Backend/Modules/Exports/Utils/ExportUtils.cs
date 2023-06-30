// ------------------------------------------------------------------------
// Copyright (C) 2023 dadyarri
// This file is part of ChocoManager <https://github.com/choco-manager/Backend>.
// 
// ChocoManager is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// ChocoManager is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with ChocoManager.  If not, see <http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------
// 


#region

using Backend.Modules.Products.Contract;

using SkiaSharp;

#endregion


namespace Backend.Modules.Exports.Utils;

public class ExportUtils : IExportUtils {
  private const float XPadding = 25.0f;
  private const float YPadding = 25.0f;
  private const float TextSize = 20.0f;
  private const int NameXCoord = 20;
  private const int PriceXCoord = 425;
  private const int LeftoverXCoord = 525;

  public SKData GenerateImage(List<ProductDto> products) {
    var ySize = YPadding + 20 + 30 * products.Count;
    var imageInfo = new SKImageInfo(700, (int)ySize);

    using var surface = SKSurface.Create(imageInfo);
    var canvas = surface.Canvas;
    canvas.Clear(SKColors.White);

    using var paint = new SKPaint {
      IsAntialias = true,
      Color = new SKColor(0, 0, 0),
      TextSize = TextSize,
      Style = SKPaintStyle.Fill,
      Typeface = SKTypeface.FromFamilyName("Roboto"),
    };
    if (products.Count > 0)
    {
      var yCoord = 0;

      DrawHeader(canvas, paint);
      foreach (var product in products)
      {
        yCoord += 30;
        DrawProductInfo(canvas, paint, product, yCoord);
      }
    }
    else
    {
      canvas.DrawText("Товара нет в наличии", XPadding, YPadding, paint);
    }

    var image = surface.Snapshot();
    return image.Encode(SKEncodedImageFormat.Jpeg, 100);
  }

  private void DrawHeader(SKCanvas canvas, SKPaint paint) {
    canvas.DrawText("Название", NameXCoord + XPadding + 5, YPadding, paint);
    canvas.DrawText("Цена", PriceXCoord + XPadding + 5, YPadding, paint);
    canvas.DrawText("В наличии", LeftoverXCoord + XPadding + 5, YPadding, paint);

    DrawLine(
      canvas,
      paint,
      new SKPoint(NameXCoord + XPadding, YPadding - 20),
      new SKPoint(LeftoverXCoord + XPadding + 106, YPadding - 20)
    );
    DrawLine(
      canvas,
      paint,
      new SKPoint(NameXCoord + XPadding, YPadding + 10),
      new SKPoint(LeftoverXCoord + XPadding + 105, YPadding + 10)
    );
    DrawLine(
      canvas,
      paint,
      new SKPoint(NameXCoord + XPadding, YPadding - 20),
      new SKPoint(NameXCoord + XPadding, YPadding + 5)
    );
    DrawLine(
      canvas,
      paint,
      new SKPoint(PriceXCoord + XPadding, YPadding - 20),
      new SKPoint(PriceXCoord + XPadding, YPadding + 5)
    );
    DrawLine(
      canvas,
      paint,
      new SKPoint(LeftoverXCoord + XPadding, YPadding - 20),
      new SKPoint(LeftoverXCoord + XPadding, YPadding + 5)
    );
    DrawLine(
      canvas,
      paint,
      new SKPoint(LeftoverXCoord + XPadding + 105, YPadding - 20),
      new SKPoint(LeftoverXCoord + XPadding + 105, YPadding + 5)
    );
  }

  private void DrawProductInfo(SKCanvas canvas, SKPaint paint, ProductDto product, int yCoord) {
    var unit = product.IsByWeight ? "кг." : "шт.";
    var postfixAtPrice = product.IsByWeight ? "/кг" : "";

    DrawLine(
      canvas,
      paint,
      new SKPoint(NameXCoord + XPadding, yCoord + YPadding + 5),
      new SKPoint(LeftoverXCoord + XPadding + 105, yCoord + YPadding + 5)
    );
    canvas.DrawText(product.Name, NameXCoord + XPadding + 5, yCoord + YPadding, paint);
    canvas.DrawText($"{product.RetailPrice}₽{postfixAtPrice}", PriceXCoord + XPadding + 5, yCoord + YPadding, paint);
    canvas.DrawText($"{product.Leftover} {unit}", LeftoverXCoord + XPadding + 5, yCoord + YPadding, paint);
  }

  private void DrawLine(SKCanvas canvas, SKPaint paint, SKPoint startPoint, SKPoint endPoint) {
    canvas.DrawLine(startPoint, endPoint, paint);
  }
}