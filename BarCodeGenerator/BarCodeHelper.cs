using IronBarCode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace BarCodeGenerator
{
    public class BarCodeHelper
    {
        public string CreateBarCode(string barCodeNumber)
        {
            var MyBarCode = IronBarCode.QRCodeWriter.CreateQrCode(barCodeNumber);
            MyBarCode.SaveAsImage(@"D://ImagesFolder//BarCode//"+barCodeNumber+".png");
            return "/ImagesFolder//BarCode//" + barCodeNumber+".png";
        }

        public string ReadQRCode()
        {
            BarcodeResult QRResult = BarcodeReader.QuicklyReadOneBarcode(@"D://ImagesFolder//BarCode//MyBarCode1.png");
            // Work with the results
            if (QRResult != null)
            {
                string Value = QRResult.Value;
                Bitmap Img = QRResult.BarcodeImage;
                BarcodeEncoding BarcodeType = QRResult.BarcodeType;
                byte[] Binary = QRResult.BinaryValue;
                Console.WriteLine(QRResult.Value);
            }
            return string.Empty;

        }
    }
}
